using Microsoft.VisualBasic;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;

namespace optimizer
{
    internal class LoanUtils
    {

        public static GameInput CreateSingleCustomerGameInput(string mapName, int gameLengthInMonths, string customerName, double yearlyInterestRate, int monthsToPayBackLoan)
        {
            //Create proposal
            var proposal = CreateCustomerProposal(customerName, yearlyInterestRate, monthsToPayBackLoan, gameLengthInMonths);

            //Create actions.
            //Must create action for all customers during the whole game length 
            var custActions = new List<Dictionary<string, CustomerAction>>();
            for (int i = 0; i < gameLengthInMonths; i++)
            {
                var custAction = new Dictionary<string, CustomerAction>();
                custAction[customerName] = new CustomerAction
                {
                    Type = "Skip",
                    Award = "None"
                };

                custActions.Add(custAction);
            }

            GameInput input = new()
            {
                MapName = mapName,
                Proposals = [proposal],
                Iterations = custActions
            };

            return input;
        }

        public static CustomerLoanRequestProposal CreateCustomerProposal(string customerName, double yearlyInterestRate, int monthsToPayBackLoan, int gameLengthInMonths)
        {
            var proposal = new CustomerLoanRequestProposal()
            {
                CustomerName = customerName,
                YearlyInterestRate = yearlyInterestRate,
                MonthsToPayBackLoan = monthsToPayBackLoan
            };

            return proposal;
        }

        public static double testTotalLoanPayments(double interestRate, double loanAmount, int monthsToPayBack)
        {
            Models.Simulation.Customer sCust = new Models.Simulation.Customer
            {
                Name = "Test",
                Capital = 100000,
                Income = 1000,
                MonthlyExpenses = 500,
                NumberOfKids = 2,
                Mortgage = 1000,
                HasStudentLoans = false,
                Personality = Personality.Conservative,
                Loan = new Models.Simulation.Loan
                {
                    Amount = loanAmount,
                    Product = "Test",
                    EnvironmentalImpact = 0.5,
                    YearlyInterestRate = interestRate,
                    MonthsToPayBack = monthsToPayBack
                }
            };

            for (int month = 0; month < monthsToPayBack; month++)
            {
                sCust.Payday();
                sCust.PayLoan();
            }

            return sCust.Profit;
        }

        public static (double, double) findAcceptedInterestRates(Personality personality)
        {
            //infer interest rates using one customer per personality.
            double minInterest = 0.0;
            double maxInterest = 0.0;
            double currentInterest = 0.0;

            //TODO must use remote version of the .Propose() function since that's the one that has the unknown personality interest rates. 

            return (minInterest, maxInterest);
        }

        public static double TotalPayments(
            double loanAmount,
            double initialCapital,
            double monthlyIncome, 
            double monthlyExpenses, 
            int numberOfKids, 
            double mortgage, 
            bool hasStudentLoans, 
            double interestRate, 
            int monthsToPayBack,
            double livingStandardMultiplier)
        {
            //----------------------------
            //total capital for the period
            //----------------------------
            double totalCapital = initialCapital + monthlyIncome * monthsToPayBack;

            //----------------------------
            //pay bills
            //----------------------------
            //adapted from the PayBills function:
            //TODO this formula is wrong, but it's in accordance with the original code.
            //TODO keep a close look on this. It seems to be implemented wrong in the docker container. 
            //this.Capital -= this.MonthlyExpenses * personalityDict[this.Personality].LivingStandardMultiplier - (this.HasStudentLoans & flag ? 2000.0 : 0.0) - (double)(this.NumberOfKids * 2000) - this.Mortgage * 0.01;
            double totalBills =
                (monthsToPayBack * monthlyExpenses * livingStandardMultiplier)
                - (hasStudentLoans ? ((monthsToPayBack/3)+1) * 2000.0 : 0.0)
                - (monthsToPayBack * (double)(numberOfKids * 2000))
                - (monthsToPayBack * (mortgage * 0.01));

            //----------------------------
            //pay loan
            //----------------------------
            //TODO check the PayLoan function in the Customer class. 
            //GetTotalMonthlyPayment
            //TODO profit är bara cumulativ interest. Principalpayment skall inte inkluderas. 

            // Calculate the monthly interest rate
            double monthlyInterestRate = interestRate / 12.0;

            // Calculate the monthly payment using the amortizing loan formula
            double monthlyPayment = loanAmount * (monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, monthsToPayBack)) / (Math.Pow(1 + monthlyInterestRate, monthsToPayBack) - 1);

            // Calculate the total loan payment
            double totalLoanPayment = monthlyPayment * monthsToPayBack;


            if (totalCapital >= loanAmount)
            {

            }
            else
            {
                //TODO mark or bankcrupt
            }

            var profit = totalLoanPayment - loanAmount;
            return profit;

        }

        public static (double, CustomerLoanRequestProposal) FindOptimalLoanProposal(MapData map, Models.Pocos.Customer mCust, Dictionary<Personality, PersonalitySpecification> personalities)
        {
            // can we afford this loan?
            if (mCust.loan.amount > map.budget)
            {
                return (0.0, null);
            }

            double maxTotalPayment = 0.0;
            int maxMonthsToPayBack = 0;
            double maxYearlyInterestRate = 0.0;

            //TODO this could be verified earlier when reading the map and inferring the acceptedmininterest and acceptedmaxinterest.
            Personality p = (Personality)Enum.Parse(typeof(Personality), mCust.personality);
            if (personalities[p].AcceptedMinInterest == null || personalities[p].AcceptedMaxInterest == null)
                throw new Exception($"Personality {p} have missing AcceptedMinInterest or AcceptedMaxInterest.");

            if (personalities[p].AcceptedMinInterest > personalities[p].AcceptedMaxInterest)
                throw new Exception($"Personality {p} have AcceptedMinInterest greater than AcceptedMaxInterest.");

            double acceptedMinInterest = personalities[p].AcceptedMinInterest ?? 0.0;
            double acceptedMaxInterest = personalities[p].AcceptedMaxInterest ?? 0.0;

            //TODO if we already know the AcceptedMaxInterest at this point we don't need to iterate over it. Higher interest rate generally mean higher profitability. 
            //TODO cost of going bankrupt. 
            //TODO calculate risk of going bankrupt. It's a bad deal if the customer goes bankrupt BEFORE paying back the loan.
            //TODO hur länge kan vi mjölka kunden och vad kostar det?
            for (var yearlyInterestRate = acceptedMinInterest; yearlyInterestRate <= acceptedMaxInterest; yearlyInterestRate += 0.001)
            {
                for (int monthsToPayBack = 1; monthsToPayBack < map.gameLengthInMonths; monthsToPayBack++)
                {
                    Models.Simulation.Customer sCust = new Models.Simulation.Customer
                    {
                        Name = mCust.name,
                        Capital = (double)mCust.capital,
                        Income = (double)mCust.income,
                        MonthlyExpenses = (double)mCust.monthlyExpenses,
                        NumberOfKids = mCust.numberOfKids,
                        Mortgage = (double)mCust.homeMortgage,
                        HasStudentLoans = mCust.hasStudentLoan,
                        Personality = (Personality)Enum.Parse(typeof(Personality), mCust.personality),
                        Loan = new Models.Simulation.Loan
                        {
                            Amount = (double)mCust.loan.amount,
                            Product = mCust.loan.product,
                            EnvironmentalImpact = (double)mCust.loan.environmentalImpact
                        }
                    };

                    //this does the same as the Propose function in the Customer class
                    sCust.Loan.YearlyInterestRate = yearlyInterestRate;
                    sCust.Loan.MonthsToPayBack = map.gameLengthInMonths;

                    var totalPayment = 0.0;


                    for (int month = 0; month < monthsToPayBack; month++)
                    {
                        //code copied from IterationService.CollectPayments
                        sCust.Payday();
                        sCust.PayBills(month, personalities);
                        if (sCust.CanPayLoan())
                            totalPayment += sCust.PayLoan();
                        else
                            sCust.IncrementMark();
                    }

                    if (totalPayment > maxTotalPayment)
                    {
                        maxTotalPayment = totalPayment;
                        maxYearlyInterestRate = yearlyInterestRate;
                    }
                }
            }

            if (maxTotalPayment == 0)
            {
                return (0.0, null);
            }
            else
            {
                var retProposal = new CustomerLoanRequestProposal
                {
                    CustomerName = mCust.name,
                    YearlyInterestRate = maxYearlyInterestRate,
                    MonthsToPayBackLoan = maxMonthsToPayBack,
                };

                return (maxTotalPayment, retProposal);
            }
        }
    }
}
