using Newtonsoft.Json;
using optimizer;
using optimizer.Models.Simulation;
using optimizer.Strategies;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

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

        string gameUrlRemote = "https://api.considition.com/";
        string gameUrlLocal = "http://localhost:8080/";

        string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
        //string mapFile = "Config/Maps/Map-Nottingham.json";
        string mapFile = "Config/Maps/Map-Gothenburg.json";
        //string mapFile = "Config/map_100.json";
        //string mapFile = "Config/map_10000.json";
        string awardsFile = "Config/awards.json";
        string personalitiesFile = "Config/personalities.json";
        string personalitiesFileCompetition = "Config/personalitiesCompetition.json";
        bool useCompetitionPersonalities = false;
        /*
        ///////////////////////////////////////////////////////////////////
        //Here comes the meat.
        ///////////////////////////////////////////////////////////////////
        */

        var serverUtilsLocal = new ServerUtils(gameUrlLocal, apiKey);
        var serverUtilsRemote = new ServerUtils(gameUrlRemote, apiKey);

        Console.WriteLine("-----------------------------------------------------------");

        var map = GameUtils.GetMap(mapFile);
        Console.WriteLine("Map name: " + map.Name);
        Console.WriteLine("Customer count: " + map.Customers.Count.ToString());

        if (!GameUtils.IsCustomerNamesUnique(map))
            throw new Exception("Customer names are not unique. This was promised during training.");



        Dictionary<Personality, PersonalitySpecification> personalities;

        if (useCompetitionPersonalities)
        {
            if (!File.Exists(personalitiesFileCompetition))
            {
                Console.WriteLine("Inferring personalities from api.");
                personalities = PersonalityInferenceHelper.InferPersonalityInterestRatesBounds(serverUtilsLocal, map, personalitiesFile);
            }
            else
            {
                Console.WriteLine("Reading personalities file: " + personalitiesFileCompetition);
                personalities = PersonalityUtils.ReadPersonalitiesFile(personalitiesFileCompetition);
            }
        }
        else
        {
            Console.WriteLine("Reading personalities file: " + personalitiesFile);
            personalities = PersonalityUtils.ReadPersonalitiesFile(personalitiesFile);
        }

        Console.WriteLine("-----------------------------------------------------------");

        if (!PersonalityUtils.HasKnownPersonalities(map, personalities))
            throw new Exception("Map contains unknown personalities.");

        if (!PersonalityUtils.HasKnownInterestRates(personalities))
            throw new Exception("Some personalities have unknown interest rates.");

        var awards = GameUtils.GetAwards(awardsFile);

        if (awards.Count <= 0)
            throw new Exception("No awards found in file.");



        /*
         * SIMULATE
         */
        //var bruteForceDetails = new BruteForce().Run(serverUtilsLocal, map, personalities);
        Console.WriteLine("-----------------------------------------------------------");
        var customerDetails = IndividualScoreSimulatedAnnealingFacade.Run(map, personalities, awards);


        /*
         * REMOVE CUSTOMERS WITH NEGATIVE SCORE CONTRIBUTION
         */
        Console.WriteLine("-----------------------------------------------------------");
        int cntBefore = customerDetails.Count;
        customerDetails = customerDetails.Where(c => c.ScoreContribution > 0).ToList();
        int cntAfter = customerDetails.Count;
        Console.WriteLine($"Removed {cntBefore - cntAfter} customers with negative ScoreContribution.");


        /*
         * SELECT best customers for our budget.
         */
        Console.WriteLine("-----------------------------------------------------------");
        //var selectedCustomers = SelectCustomersDp.Select(map, customerDetails); // DP breaks down for 100 customers. 
        List<CustomerPropositionDetails> selectedCustomers = SelectCustomersGreedy.Select(map, customerDetails);
        //var selectedCustomers = SelectCustomersBranchAndBound.Select(map, customerDetails);
        //var selectedCustomers = SelectCustomersGeneticElitism.Select(map, customerDetails);


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

        var predictedScore = selectedCustomers.Sum(c => c.ScoreContribution);
        Console.WriteLine("Predicted score from selection process: ");
        Console.WriteLine(predictedScore);

        var gameInput = LoanUtils.CreateGameInput(map.Name, map.GameLengthInMonths, selectedCustomers);

        //Log input 
        var inputJson = JsonConvert.SerializeObject(gameInput, Formatting.Indented);
        File.WriteAllText("finalGameInput.json", inputJson);
        //Console.WriteLine("Final game input:");
        //Console.WriteLine(inputJson);

        //Score the game locally.
        var gameResponse = serverUtilsLocal.SubmitGameAsync(gameInput).Result;
        var totalScore = GameUtils.LogGameResponse(gameResponse, "finalGameOutput.json");

        if (totalScore != predictedScore)
        {
            throw new Exception("Predicted score and actual score do not match.");
        }

        //Score the game remotely.
        var gameResponseRemote = serverUtilsRemote.SubmitGameAsync(gameInput).Result;
        var totalScoreRemote = GameUtils.LogGameResponse(gameResponseRemote, "finalGameOutputRemote.json");

        if (totalScore != totalScoreRemote)
        {
            throw new Exception("Local and remote server gave different scores.");
        }

        Console.WriteLine("Done.");
    }
}
//TODO passa in map som cmdline argument.
//TODO passa in personalities som cmdline argument. Borde heta mapname_personalities.json


//TODO GetTotalMonthlyPayment kan optimeras med en iterativ approach, för små n. 
//TODO LivingStandardMultiplier kommer inte vara känd!!
//TODO Kanske logga ut i en databas vilka betalningar som kommer in och hur man kan förutspå bankrupt. 
//TODO undersök hur många som blir bankrupt. Ge award för att hindra att det händer. Jämför kostnaden för award vs Happiness -500 bankrupt. 
//TODO selection: Do greedy first and use that as seed for genetic approach.
//TODO make as high interest as possible that is both acceptable and not going bankrupt. Perhaps just binary search until interest rate is found. <-------------------
//TODO find out if any of the proposals result in someone going bankrupt.
//TODO labba med Award.NoInterestRate 
//TODO inspect the scoring code for performance bottlenecks. Extract it if necessary. Ask copilot for help. 
//TODO calculate how much budget we have left after granting all the loans. So we know how much we can spend on awards.
//TODO where does the final OptimalInterestRate get rounded to nice decial places? 
//TODO Extract docker image dlls to run scoring on native code. 
//TODO infer minAcceptedInterestRate, maxAcceptedInterestRate
//TODO maybe use TotalProfit instead of TotalScore
//TODO perhaps a genetic algorithm to explore awards. 

//Approaches to Solve the Knapsack Problem
//1.	Dynamic Programming (DP): Provides an exact solution but can be memory-intensive.
//2.	Greedy Algorithm: Provides an approximate solution and is more memory-efficient.
//3.	Branch and Bound: Another exact method that can be more efficient than DP in some cases.
//4.	Heuristic and Metaheuristic Algorithms: Such as Genetic Algorithms, Simulated Annealing, etc., which provide good approximations.

/*
 * Changelog
Score               Action
After Simulation Map+Customer:
284599,9999999999   SA maxIterations: 1000, coolingRate: 0.95, initialTemp: 1000.0, maxMonthsToPayBackLoan = 50 * 12, Time: 31s, Customers processed per second: 0,15703386989419763

Baseline:
284599,9999999999   SA maxIterations: 1000, coolingRate: 0.95, initialTemp: 1000.0, maxMonthsToPayBackLoan = 50 * 12, Time: 18s, Customers processed per second: 0,27689783951679064 




*/
