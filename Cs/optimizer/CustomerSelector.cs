using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimizer.Models.Pocos;

namespace optimizer
{
    internal class CustomerSelector
    {
        public static List<CustomerPropositionDetails> Select(MapData map, List<CustomerPropositionDetails> customerDetails)
        {
            //TODO verify that customer names are unique when reading the map.

            //TODO how do we select the most profitable customers within our budget?
            //Is this a knapsack problem? Are there other approaches?

            //The bank's budget
            budget = map.budget;

        }
    }
