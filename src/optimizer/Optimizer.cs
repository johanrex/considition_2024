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

class Program
{
    static void parseCommandLineArguments(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No command line arguments found.");
            return;
        }

        for (int i = 0; i < args.Length; i++)
        {
            Console.WriteLine($"Argument {i}: {args[i]}");
        }
    }

    static void Main(string[] args)
    {
        parseCommandLineArguments(args);


        int maxDegreesOfParallelism = -1;
        string gameUrlRemote = "https://api.considition.com/";
        string gameUrlLocal = "http://localhost:8080/";
        string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
        string mapName = "Almhult";
        //string mapName = "Gothenburg";
        //string mapName = "Nottingham";

        /*
        ///////////////////////////////////////////////////////////////////
        //Here comes the meat.
        ///////////////////////////////////////////////////////////////////
        */

        ConfigService configService = new();
        var map = configService.GetMap(mapName);

        var personalities = configService.GetPersonalitySpecifications(mapName);
        var awards = configService.GetAwardSpecifications(mapName);

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

        if (!PersonalityUtils.HasKnownPersonalities(map, personalities))
            throw new Exception("Map contains unknown personalities.");

        if (!PersonalityUtils.HasKnownInterestRates(personalities))
            throw new Exception("Some personalities have unknown interest rates.");

        if (awards.Count <= 0)
            throw new Exception("No awards found in file.");

        Console.WriteLine("Done.");

        while (true)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<CustomerLoanRequestProposalEx> customerDetails;

            /*
             * SIMULATE: INDIVIDUAL CUSTOMERS WITH NO AWARDS
             */
            Console.WriteLine("-----------------------------------------------------------");
            customerDetails = IndividualScoreSimulatedAnnealingFacade.Run(scoreUtils, map, personalities, maxDegreesOfParallelism);


            /*
             * REMOVE USELESS CUSTOMERS 
             */
            Console.WriteLine("-----------------------------------------------------------");
            int cntBefore = customerDetails.Count;
            //keep only customers with a positive total score
            customerDetails = customerDetails.Where(c => c.TotalScore > 0).ToList();
            int cntAfter = customerDetails.Count;
            Console.WriteLine($"Removed {cntBefore - cntAfter} customers with negative ScoreContribution.");
            cntBefore = cntAfter;
            customerDetails = customerDetails.Where(c => c.LoanAmount <= map.Budget / 2).ToList();
            cntAfter = customerDetails.Count;
            Console.WriteLine($"Removed {cntBefore - cntAfter} customers that wants to loan too much.");




            //var gameInput = GameUtils.CreateGameInput(map.Name, map.GameLengthInMonths, customerDetails);
            //var gameResponse = serverUtils.SubmitGameAsync(gameInput).Result;
            //Console.WriteLine("Score:");
            //Console.WriteLine(gameResponse.Score.TotalScore);

            /*
             * SIMULATE: INDIVIDUAL CUSTOMERS ONLY AWARDS
             */
            Console.WriteLine("-----------------------------------------------------------");
            customerDetails = IterationAwardsSimulatedAnnealingFacade.Run(scoreUtils, map, customerDetails, maxDegreesOfParallelism);


            /*
             * SELECT best customers for our budget.
             */
            Console.WriteLine("-----------------------------------------------------------");
            List<CustomerLoanRequestProposalEx> selectedCustomers = SelectCustomersGreedy.Select(configService, map, customerDetails);


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

            //TODO, score it againt and see if we have any bankrupcies. <-- Yes we have. 
            //var mapCustomerLookup = map.Customers.ToDictionary(c => c.Name);
            //var scorer = new NativeScorer.NativeScorer(configService, personalities, awards);
            //var gameResponse = scorer.RunGame(gameInput, mapCustomerLookup);

            //LOG INPUT TO FILE BEFORE SUBMITTING
            var inputJson = System.Text.Json.JsonSerializer.Serialize(gameInput);
            File.WriteAllText("finalGameInput.json", inputJson);
            //Console.WriteLine("Final game input:");
            //Console.WriteLine(inputJson);


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
