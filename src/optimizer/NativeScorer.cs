using Microsoft.VisualBasic;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class NativeScorer
    {
        public static void Score(Map map, Dictionary<Personality, PersonalitySpecification> personalities, Dictionary<AwardType, AwardSpecification> awards, GameInput input)
        {
            //var budget = map.Budget;
            //var totalScore = 0.0;
            //var totalHappiness = 0.0;

            //for (int iterationIndex = 0; iterationIndex < iterations.Count; ++iterationIndex)
            //{

            //    foreach (Customer customer in customers)
            //    {
            //        CustomerAction customerAction = iteration.CustomerActions[customer.Name];
            //        customer.Payday();
            //        // ISSUE: reference to a compiler-generated field
            //        customer.PayBills(iterationIndex, this.\u003CconfigService\u003EP.Personalities);
            //        if (customer.CanPayLoan())
            //            map.Budget += customer.PayLoan();
            //        else
            //            customer.IncrementMark();
            //        if (customerAction.Type == CustomerActionType.Award)
            //            map.Budget -= this.Award(customer, customerAction.Award);
            //    }
            //}

        }
    }
}
