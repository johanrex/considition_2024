// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.LocalGameService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC78B9DA-9821-4404-BDBA-C98E63F84698
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Interfaces;
using LocalHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace LocalHost.Services
{
    public class LocalGameService : IGameService
    {
        public LocalGameService(
          ICustomerService customerService,
          IIterationService iterationService,
          ISaveGameService saveGameService,
          IConfigService configService)
        {
            // ISSUE: reference to a compiler-generated field
            this.\u003CcustomerService\u003EP = customerService;
            // ISSUE: reference to a compiler-generated field
            this.\u003CiterationService\u003EP = iterationService;
            // ISSUE: reference to a compiler-generated field
            this.\u003CsaveGameService\u003EP = saveGameService;
            // ISSUE: reference to a compiler-generated field
            this.\u003CconfigService\u003EP = configService;
            // ISSUE: explicit constructor call
            base.\u002Ector();
        }

        public async Task<GameResponse> RunGame(GameInput gameInput, Guid apiKey)
        {
            Guid gameId = Guid.NewGuid();
            // ISSUE: reference to a compiler-generated field
            Map map = this.\u003CconfigService\u003EP.GetMap(gameInput.MapName);
            if ((object)map == null)
                return new GameResponse()
                {
                    Message = "Map with name " + gameInput.MapName + " not found!"
                };
            string str = LocalGameService.ValidateGameInput(gameInput, map);
            if (str != null)
                return new GameResponse() { Message = str };
            // ISSUE: reference to a compiler-generated field
            List<Customer> customerList = this.\u003CcustomerService\u003EP.RequestCustomers(gameInput, map);
            double num = customerList.Sum<Customer>((Func<Customer, double>)(c => c.Loan.Amount));
            map.Budget -= num;
            this.HandleIterations(gameInput.Iterations, customerList, map);
            // ISSUE: reference to a compiler-generated field
            GameResult gameResult = await this.\u003CsaveGameService\u003EP.SaveGame(gameInput, customerList, apiKey, gameId);
            return new GameResponse()
            {
                GameId = new Guid?(gameId),
                Score = gameResult
            };
        }

        public async Task<SaveGame?> GetGame(Guid gameId, Guid apiKey)
        {
            // ISSUE: reference to a compiler-generated field
            return await this.\u003CsaveGameService\u003EP.GetGame(gameId, apiKey);
        }

        private static string? ValidateGameInput(GameInput gameInput, Map map)
        {
            if (gameInput.Proposals.Count == 0)
                return "You must choose at least one customer to play!";
            if (gameInput.Iterations.Count > map.GameLengthInMonths)
                return "You can not exceed amount of months in 'iterations' then described in map config";
            if (map.GameLengthInMonths != gameInput.Iterations.Count)
                return "You must provide customer actions for each month of the designated game lenght!";
            if (gameInput.Iterations.Any<CustomerActionIteration>((Func<CustomerActionIteration, bool>)(iteration => iteration.CustomerActions.Count != gameInput.Proposals.Count)))
                return "Each iterations must have an action for each customer!";
            if (gameInput.Proposals.Any<CustomerLoanRequestProposal>((Func<CustomerLoanRequestProposal, bool>)(proposal => proposal.MonthsToPayBackLoan < 0)))
                return "Customers need at least one month to pay back loan";
            IEnumerable<string> mapCustomerNames = map.Customers.Select<Customer, string>((Func<Customer, string>)(c => c.Name));
            if (gameInput.Proposals.Any<CustomerLoanRequestProposal>((Func<CustomerLoanRequestProposal, bool>)(proposal => !mapCustomerNames.Contains<string>(proposal.CustomerName))))
                return "All requested customers must exist on the chosen map!";
            if (gameInput.Proposals.Sum<CustomerLoanRequestProposal>((Func<CustomerLoanRequestProposal, double>)(x =>
            {
                Customer customer = map.Customers.FirstOrDefault<Customer>((Func<Customer, bool>)(y => y.Name == x.CustomerName));
                return (object)customer == null ? 0.0 : customer.Loan.Amount;
            })) > map.Budget)
            {
                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
                interpolatedStringHandler.AppendLiteral("Tried starting game without sufficient funds, budget: ");
                interpolatedStringHandler.AppendFormatted<double>(map.Budget);
                return interpolatedStringHandler.ToStringAndClear();
            }
            HashSet<string> stringSet = new HashSet<string>();
            foreach (CustomerLoanRequestProposal proposal in gameInput.Proposals)
            {
                if (!stringSet.Add(proposal.CustomerName))
                    return "Customer '" + proposal.CustomerName + "' is already on the chosen map!";
            }
            return (string)null;
        }

        private void HandleIterations(
          List<CustomerActionIteration> iterations,
          List<Customer> customers,
          Map map)
        {
            for (int index = 0; index < iterations.Count; ++index)
            {
                // ISSUE: reference to a compiler-generated field
                this.\u003CiterationService\u003EP.CollectPayments(iterations[index], index, customers, map);
            }
        }
    }
}
