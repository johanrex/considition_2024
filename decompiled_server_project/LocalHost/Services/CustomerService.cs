// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.CustomerService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D1B7BF3C-328E-422C-8A9F-0E1266BF8FE0
// Assembly location: C:\temp\app\LocalHost.dll

using LocalHost.Interfaces;
using LocalHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace LocalHost.Services
{
    public class CustomerService : ICustomerService
    {
        public CustomerService(IConfigService configService)
        {
            // ISSUE: reference to a compiler-generated field
            this.\u003CconfigService\u003EP = configService;
            // ISSUE: explicit constructor call
            base.\u002Ector();
        }

        public List<Customer> RequestCustomers(GameInput gameInput, Map map)
        {
            List<Customer> customerList = new List<Customer>();
            // ISSUE: reference to a compiler-generated field
            Dictionary<Personality, PersonalitySpecification> personalitySpecifications = this.\u003CconfigService\u003EP.GetPersonalitySpecifications(map.Name);
            foreach (CustomerLoanRequestProposal proposal1 in gameInput.Proposals)
            {
                CustomerLoanRequestProposal proposal = proposal1;
                Customer customer = map.Customers.FirstOrDefault<Customer>((Func<Customer, bool>)(c => c.Name.Equals(proposal.CustomerName)));
                if ((object)customer != null && customer.Propose(proposal.YearlyInterestRate, proposal.MonthsToPayBackLoan, personalitySpecifications, map.GameLengthInMonths))
                    customerList.Add(customer);
            }
            return customerList;
        }
    }
}
