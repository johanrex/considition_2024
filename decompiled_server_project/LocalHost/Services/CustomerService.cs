﻿// Decompiled with JetBrains decompiler
// Type: LocalHost.Services.CustomerService
// Assembly: LocalHost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC78B9DA-9821-4404-BDBA-C98E63F84698
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
            foreach (CustomerLoanRequestProposal proposal1 in gameInput.Proposals)
            {
                CustomerLoanRequestProposal proposal = proposal1;
                Customer customer = map.Customers.FirstOrDefault<Customer>((Func<Customer, bool>)(c => c.Name.Equals(proposal.CustomerName)));
                // ISSUE: reference to a compiler-generated field
                if (customer?.Propose(proposal.YearlyInterestRate, proposal.MonthsToPayBackLoan, this.\u003CconfigService\u003EP.Personalities).HasValue)
                    customerList.Add(customer);
            }
            return customerList;
        }
    }
}
