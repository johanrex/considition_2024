// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.CustomerService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDC2938F-C917-4854-87EA-D677106BD5FA
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
                if ((object)customer != null && customer.Propose(proposal.YearlyInterestRate, proposal.MonthsToPayBackLoan, personalitySpecifications))
                    customerList.Add(customer);
            }
            return customerList;
        }
    }
}
