using Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Services;

//namespace optimizer.Strategies
//{
//    internal class SelectCustomersSimulatedAnnealing
//    {
//        public static List<CustomerLoanRequestProposalEx> Select(Dictionary<string, double> customerNameCosts, Map map, List<CustomerLoanRequestProposalEx> customerDetails)
//        {
//            Console.WriteLine("Selecting customers within budget: Simulated Annealing approach.");
//            Stopwatch stopwatch = Stopwatch.StartNew();

//            double budget = map.Budget;
//            List<CustomerLoanRequestProposalEx> selectedCustomer = new();

//            //TODO implement simulated annealing

//            stopwatch.Stop();
//            Console.WriteLine($"Selection took {stopwatch.ElapsedMilliseconds} ms.");

//            return selectedCustomers;
//        }

//        private double ScoreFunction(List<CustomerLoanRequestProposalEx> solution)
//        {
//            return solution.Sum(x => x.Value);
//        }

//    }
//}
