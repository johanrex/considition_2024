using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace optimizer.Models.Simulation
{
    public record Customer
    {
        public string Name { get; init; }

        public Loan Loan { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Personality Personality { get; init; }

        public double Capital { get; set; }

        public double Income { get; init; }

        public double MonthlyExpenses { get; init; }

        public int NumberOfKids { get; init; }

        public double Mortgage { get; init; }

        public bool HasStudentLoans { get; init; }

        public double Happiness { get; set; }

        public bool IsBankrupt { get; private set; }

        public int Marks { get; private set; }

        public int AwardsInRow { get; set; }

        public double Profit { get; set; }

        private int MarkLimit { get; }

        public void Payday() => this.Capital += this.Income;

        public void PayBills(
          int iteration,
          Dictionary<Personality, PersonalitySpecification> personalityDict)
        {
            bool flag = iteration % 3 == 0;
            this.Capital -= this.MonthlyExpenses * personalityDict[this.Personality].LivingStandardMultiplier - (this.HasStudentLoans & flag ? 2000.0 : 0.0) - (double)(this.NumberOfKids * 2000) - this.Mortgage * 0.01;
        }

        public bool CanPayLoan() => this.Capital >= this.Loan.GetTotalMonthlyPayment();

        public double PayLoan()
        {
            this.Capital -= this.Loan.GetTotalMonthlyPayment();
            double interestPayment = this.Loan.GetInterestPayment();
            this.Loan.LowerRemainingBalance();
            this.Profit += interestPayment;
            return interestPayment;
        }

        public void IncrementMark()
        {
            ++this.Marks;
            this.Capital = 0.0;
            if (this.Marks >= this.MarkLimit)
            {
                this.IsBankrupt = true;
                this.Happiness -= 500.0;
            }
            else
                this.Happiness -= 50.0;
        }

        public bool Propose(
          double yearlyInterestRate,
          int monthsToPayBack,
          Dictionary<Personality, PersonalitySpecification> personalityDict)
        {
            double? acceptedMinInterest = personalityDict[this.Personality].AcceptedMinInterest;
            double? acceptedMaxInterest = personalityDict[this.Personality].AcceptedMaxInterest;
            double num1 = yearlyInterestRate;
            double? nullable1 = acceptedMinInterest;
            double valueOrDefault1 = nullable1.GetValueOrDefault();
            if (!(num1 < valueOrDefault1 & nullable1.HasValue))
            {
                double num2 = yearlyInterestRate;
                double? nullable2 = acceptedMaxInterest;
                double valueOrDefault2 = nullable2.GetValueOrDefault();
                if (!(num2 > valueOrDefault2 & nullable2.HasValue))
                {
                    this.Loan.YearlyInterestRate = yearlyInterestRate;
                    this.Loan.MonthsToPayBack = monthsToPayBack;
                    return true;
                }
            }
            return false;
        }

        public Customer()
        {
        }
    }
}
