using optimizer.Models.Simulation;
using optimizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
namespace optimizer.Strategies
{
    public class SelectCustomersGeneticElitism
    {
        private static Random random = new Random();

        public static List<CustomerPropositionDetails> Select(Map map, List<CustomerPropositionDetails> customerDetails, int populationSize=200, int generations=500, double mutationRate=0.1, int elitismCount = 5)
        {
            Console.WriteLine("Selecting customers: Genetic.");
            Stopwatch stopwatch = Stopwatch.StartNew();

            double budget = map.Budget;
            List<List<CustomerPropositionDetails>> population = InitializePopulation(customerDetails, populationSize, budget);

            for (int generation = 0; generation < generations; generation++)
            {
                population = EvolvePopulation(population, budget, mutationRate, elitismCount);
            }

            var selectedCustomers = population.OrderByDescending(ComputeFitness).First();

            stopwatch.Stop();
            Console.WriteLine($"Genetic selection took {stopwatch.ElapsedMilliseconds} ms.");

            return selectedCustomers;
        }

        private static List<List<CustomerPropositionDetails>> InitializePopulation(List<CustomerPropositionDetails> customerDetails, int populationSize, double budget)
        {
            List<List<CustomerPropositionDetails>> population = new List<List<CustomerPropositionDetails>>();

            for (int i = 0; i < populationSize; i++)
            {
                List<CustomerPropositionDetails> individual = new List<CustomerPropositionDetails>();
                double totalWeight = 0;

                foreach (var customer in customerDetails.OrderBy(x => random.Next()))
                {
                    if (totalWeight + customer.LoanAmount <= budget)
                    {
                        individual.Add(customer);
                        totalWeight += customer.LoanAmount;
                    }
                }

                population.Add(individual);
            }

            return population;
        }

        private static List<List<CustomerPropositionDetails>> EvolvePopulation(List<List<CustomerPropositionDetails>> population, double budget, double mutationRate, int elitismCount)
        {
            List<List<CustomerPropositionDetails>> newPopulation = new List<List<CustomerPropositionDetails>>();

            // Sort the population by fitness in descending order
            var sortedPopulation = population.OrderByDescending(ComputeFitness).ToList();

            // Add the top elites to the new population
            for (int i = 0; i < elitismCount; i++)
            {
                newPopulation.Add(sortedPopulation[i]);
            }

            // Fill the rest of the new population with offspring
            for (int i = elitismCount; i < population.Count; i++)
            {
                var parent1 = SelectParent(population);
                var parent2 = SelectParent(population);
                var offspring = Crossover(parent1, parent2, budget);

                if (random.NextDouble() < mutationRate)
                {
                    Mutate(offspring, budget);
                }

                newPopulation.Add(offspring);
            }

            return newPopulation;
        }


        private static List<CustomerPropositionDetails> SelectParent(List<List<CustomerPropositionDetails>> population)
        {
            return population[random.Next(population.Count)];
        }

        private static List<CustomerPropositionDetails> Crossover(List<CustomerPropositionDetails> parent1, List<CustomerPropositionDetails> parent2, double budget)
        {
            List<CustomerPropositionDetails> offspring = new List<CustomerPropositionDetails>();
            double totalWeight = 0;

            foreach (var customer in parent1.Concat(parent2).OrderBy(x => random.Next()))
            {
                if (totalWeight + customer.LoanAmount <= budget && !offspring.Contains(customer))
                {
                    offspring.Add(customer);
                    totalWeight += customer.LoanAmount;
                }
            }

            return offspring;
        }

        private static void Mutate(List<CustomerPropositionDetails> individual, double budget)
        {
            if (individual.Count == 0) return;

            int index = random.Next(individual.Count);
            individual.RemoveAt(index);

            // Add a random customer to maintain the budget constraint
            // This part can be improved to ensure the mutation is valid
        }

        private static double ComputeFitness(List<CustomerPropositionDetails> individual)
        {
            return individual.Sum(c => c.ScoreContribution);
        }
    }
}