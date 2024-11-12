using Newtonsoft.Json;
using optimizer;
using optimizer.Strategies;
using Common.Models;
using Common.Services;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using NativeScorer;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

class Program
{
    private static Dictionary<string, double> CustomerCosts(Dictionary<AwardType, AwardSpecification> awardSpecs, List<CustomerLoanRequestProposalEx> customerDetails)
    {
        //find out the total cost of each customer
        Dictionary<string, double> customerNameCosts = new();
        foreach (var customer in customerDetails)
        {
            double totalAwardCost = 0;
            foreach (CustomerActionIteration iteration in customer.Iterations)
            {
                var customerAction = iteration.CustomerActions[customer.CustomerName];
                if (customerAction.Award == AwardType.None)
                    continue;

                AwardSpecification spec = awardSpecs[customerAction.Award];
                totalAwardCost += spec.Cost;
            }

            double totalCustomerCost = customer.LoanAmount + totalAwardCost;
            customerNameCosts[customer.CustomerName] = totalCustomerCost;
        }
        return customerNameCosts;
    }


    static void Main(string[] args)
    {
        int maxDegreesOfParallelism = -1;
        string gameUrlRemote = "https://api.considition.com/";
        string gameUrlLocal = "http://localhost:8080/";
        string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
        string mapName = "Almhult";
        //string mapName = "Gothenburg";
        //string mapName = "Nottingham";

        ConfigService configService = new();
        var map = configService.GetMap(mapName);

        var personalitySpecs = configService.GetPersonalitySpecifications(mapName);
        var awardSpecs = configService.GetAwardSpecifications(mapName);

        var serverUtils = new ServerUtils(gameUrlLocal, apiKey);
        var serverUtilsRemote = new ServerUtils(gameUrlRemote, apiKey);

        ScoreUtils scoreUtils = new(serverUtils, configService);


        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Map name: " + map.Name);
        Console.WriteLine("Customer count: " + map.Customers.Count.ToString());

        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Doing sanity checks...");

        if (!GameUtils.IsCustomerNamesUnique(map))
            throw new Exception("Customer names are not unique. This was promised during training.");

        if (!PersonalityUtils.HasKnownPersonalities(map, personalitySpecs))
            throw new Exception("Map contains unknown personalities.");

        if (!PersonalityUtils.HasKnownInterestRates(personalitySpecs))
            throw new Exception("Some personalities have unknown interest rates.");

        if (awardSpecs.Count <= 0)
            throw new Exception("No awards found in file.");

        Console.WriteLine("Done.");

        while (true)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int cntBefore;
            int cntAfter;
            List<CustomerLoanRequestProposalEx> customerDetails;


            //Build a dictionary of customers 
            Dictionary<string, Customer> nameCustomers = map.Customers.Select(c => c).ToDictionary(c => c.Name);

            /*
             * FILTER OUT TOO EXPENSIVE CUSTOMERS
             */
            Console.WriteLine("-----------------------------------------------------------");
            cntBefore = nameCustomers.Count;
            nameCustomers = nameCustomers.Where(kvp => kvp.Value.Loan.Amount < map.Budget).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            cntAfter = nameCustomers.Count;
            Console.WriteLine($"Removed {cntBefore - cntAfter} customers that wanted to loan too much.");

            /*
             * SIMULATE: INDIVIDUAL CUSTOMERS WITH NO AWARDS
             */
            Console.WriteLine("-----------------------------------------------------------");
            customerDetails = IndividualScoreSimulatedAnnealingFacade.Run(scoreUtils, map, personalitySpecs, maxDegreesOfParallelism, nameCustomers);


            /*
             * SIMULATE: INDIVIDUAL CUSTOMERS ONLY AWARDS
             */
            Console.WriteLine("-----------------------------------------------------------");
            customerDetails = IterationAwardsSimulatedAnnealingFacade.Run(scoreUtils, map, customerDetails, maxDegreesOfParallelism);


            /*
             * REMOVE USELESS CUSTOMERS 
             */
            Console.WriteLine("-----------------------------------------------------------");
            cntBefore = customerDetails.Count;
            customerDetails = customerDetails.Where(c => c.TotalScore > 0).ToList();
            cntAfter = customerDetails.Count;
            Console.WriteLine($"Removed {cntBefore - cntAfter} customers with negative ScoreContribution.");
            cntBefore = cntAfter;


            var customerNameCosts = CustomerCosts(awardSpecs, customerDetails);
            var customerNameDetails = customerDetails.ToDictionary(c => c.CustomerName);

            /*
             * SELECT best customers for our budget.
             */
            Console.WriteLine("-----------------------------------------------------------");
            //List<CustomerLoanRequestProposalEx> selectedCustomers = SelectCustomersGreedy.Select(customerNameCosts, map, customerDetails);
            List<CustomerLoanRequestProposalEx> selectedCustomers = SelectCustomersSimulatedAnnealing.Select(customerNameDetails, customerNameCosts, map);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine($"These customers were selected ({selectedCustomers.Count}):");
            Console.WriteLine(DataFrameHelper.ToDataFrame(selectedCustomers).ToString());

            var notSelectedCustomers = customerDetails.Except(selectedCustomers).ToList();

            if (notSelectedCustomers.Count > 0)
            {
                var notSelectedDf = DataFrameHelper.ToDataFrame(notSelectedCustomers);
                Console.WriteLine($"These customers were NOT selected ({notSelectedCustomers.Count}):");
                Console.WriteLine(notSelectedDf.ToString());
            }

            Console.WriteLine("-----------------------------------------------------------");

            var predictedScore = selectedCustomers.Sum(c => c.TotalScore);
            Console.WriteLine("Predicted score from selection process: ");
            Console.WriteLine(predictedScore);

            var gameInput = GameUtils.CreateGameInput(map.Name, map.GameLengthInMonths, selectedCustomers);

            //LOG INPUT TO FILE BEFORE SUBMITTING
            var inputJson = System.Text.Json.JsonSerializer.Serialize(gameInput);
            File.WriteAllText("finalGameInput.json", inputJson);

            var gameResult = scoreUtils.SubmitGame(gameInput);

            //DOCKER SCORE
            var gameResponse = serverUtils.SubmitGameAsync(gameInput).Result;
            var totalScore = GameUtils.LogGameResponse(gameResponse, "finalGameOutput.json");
            Console.WriteLine($"Score from local docker:");
            Console.WriteLine(totalScore);

            //REMOTE SCORE
            //var gameResponseRemote = serverUtilsRemote.SubmitGameAsync(gameInput).Result;
            //var totalScoreRemote = GameUtils.LogGameResponse(gameResponseRemote, "finalGameOutputRemote.json");
            //Console.WriteLine($"Score from remote api:");
            //Console.WriteLine(totalScoreRemote);

            stopwatch.Stop();
            double totalTimeInSeconds = stopwatch.Elapsed.TotalSeconds;

            //log score to file
            using (StreamWriter writer = new StreamWriter("scores.txt", append: true))
            {
                writer.WriteLine($"{DateTime.Now}: {totalScore}. Iteration took: {totalTimeInSeconds}s.");
            }

            Console.WriteLine("Done.");
        }
    }
}
