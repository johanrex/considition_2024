using Common.Models;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer.Strategies
{
    internal class IterationAwardsSimulatedAnnealingFacade
    {
        /*
        public static List<CustomerLoanRequestProposalEx> Run(
            Map map, 
            List<CustomerLoanRequestProposalEx> proposalExs,
            ConfigService configService,
            Dictionary<string, Customer> mapCustomerLookup,
            Dictionary<Personality, PersonalitySpecification> personalities,
            Dictionary<AwardType, AwardSpecification> awards
            )
        {
            double temperature = 1.0;
            double coolingRate = 0.003;

            int maxIterations = 1000; // Set your desired number of iterations here
            int iteration = 0;

            List<CustomerLoanRequestProposalEx> bestProposalExs = new();
            for(int i = 0; i < 0; i++)
            {
                var proposalEx = proposalExs[i];
                IterationAwardsSimulatedAnnealing annealing = new IterationAwardsSimulatedAnnealing(
                    map,
                    proposalEx,
                    configService,
                    mapCustomerLookup,
                    personalities,
                    awards
                    );

                var bestProposal = annealing.Run();
                bestProposalExs.Add(bestProposal);
            }

            //TODO should the output of the facade perhaps be a GameInput object instead?
            //TODO perhaps CustomerLoanRequestProposalEx isn't even needed, I can just extend GameInput.

            return bestProposals;
        }
        */
    }
}
