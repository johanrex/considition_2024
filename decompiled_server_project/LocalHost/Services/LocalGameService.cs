﻿// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.LocalGameService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 77EDA3FC-B32E-487F-8161-20E228F5089F
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
            string errorMessage = this.HandleIterations(gameInput.Iterations, customerList, map);
            return errorMessage != null ? await this.SaveFailedGame(gameInput, apiKey, customerList, gameId, errorMessage) : await this.CalculateScoreAndSaveGame(gameInput, apiKey, customerList, gameId);
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
            HashSet<string> customerNames = new HashSet<string>();
            foreach (CustomerLoanRequestProposal proposal in gameInput.Proposals)
            {
                if (!customerNames.Add(proposal.CustomerName))
                    return "Customer '" + proposal.CustomerName + "' is already on the chosen map!";
            }
            for (int index = 0; index < gameInput.Iterations.Count; ++index)
            {
                if (!LocalGameService.SequenceEqualsAnyOrder((ICollection<string>)gameInput.Iterations[index].CustomerActions.Keys.ToList<string>(), (ICollection<string>)customerNames))
                {
                    DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 1);
                    interpolatedStringHandler.AppendLiteral("Customer names in iteration ");
                    interpolatedStringHandler.AppendFormatted<int>(index);
                    interpolatedStringHandler.AppendLiteral(" doesn't match customers in proposal list");
                    return interpolatedStringHandler.ToStringAndClear();
                }
            }
            return (string)null;
        }

        private static bool SequenceEqualsAnyOrder(
          ICollection<string> iterationCustomerNames,
          ICollection<string> customerNames)
        {
            return iterationCustomerNames.Count == customerNames.Count && !iterationCustomerNames.Except<string>((IEnumerable<string>)customerNames).Any<string>();
        }

        private string? HandleIterations(
          List<CustomerActionIteration> iterations,
          List<Customer> customers,
          Map map)
        {
            for (int index = 0; index < iterations.Count; ++index)
            {
                // ISSUE: reference to a compiler-generated field
                string str = this.\u003CiterationService\u003EP.CollectPayments(iterations[index], index, customers, map);
                if (str != null)
                    return str;
            }
            return (string)null;
        }

        private async Task<GameResponse> SaveFailedGame(
          GameInput gameInput,
          Guid apiKey,
          List<Customer> acceptedCustomers,
          Guid gameId,
          string errorMessage)
        {
            GameResult gameResult = new GameResult()
            {
                MapName = gameInput.MapName
            };
            // ISSUE: reference to a compiler-generated field
            await this.\u003CsaveGameService\u003EP.SaveGame(gameInput, gameResult, acceptedCustomers, apiKey, gameId);
            return new GameResponse()
            {
                GameId = new Guid?(gameId),
                Message = errorMessage
            };
        }

        private async Task<GameResponse> CalculateScoreAndSaveGame(
          GameInput gameInput,
          Guid apiKey,
          List<Customer> acceptedCustomers,
          Guid gameId)
        {
            GameResult gameResult = new GameResult()
            {
                TotalProfit = (long)acceptedCustomers.Sum<Customer>((Func<Customer, double>)(x => x.Profit)),
                HappinessScore = (long)acceptedCustomers.Sum<Customer>((Func<Customer, double>)(x => x.Happiness)),
                EnvironmentalImpact = (long)acceptedCustomers.Sum<Customer>((Func<Customer, double>)(x => x.Loan.EnvironmentalImpact)),
                MapName = gameInput.MapName
            };
            // ISSUE: reference to a compiler-generated field
            await this.\u003CsaveGameService\u003EP.SaveGame(gameInput, gameResult, acceptedCustomers, apiKey, gameId);
            GameResponse scoreAndSaveGame = new GameResponse()
            {
                GameId = new Guid?(gameId),
                Score = gameResult
            };
            gameResult = (GameResult)null;
            return scoreAndSaveGame;
        }
    }
}
