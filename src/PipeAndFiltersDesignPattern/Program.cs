using System;
using PipeAndFiltersDesignPattern.DomainObjects;
using PipeAndFiltersDesignPattern.Filters;
using PipeAndFiltersDesignPattern.Filters.Withdrawal;
using PipeAndFiltersDesignPattern.Pipeline;

namespace PipeAndFiltersDesignPattern
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Valid withdrawal limit is amount > 100 and amount < 1000.

            var validWithdrawal = new Withdrawal() {Amount = 150};
            var inValidWithdrawalLimit = new Withdrawal() {Amount = 50};

            IFinancialRule<Withdrawal> dailyLimit = new DailyLimit();
            IFinancialRule<Withdrawal> belowMinimumAllowed = new BelowMinimumAllowed();
            IFinancialRule<Withdrawal> aboveMaximumAllowed = new AboveMaximum();

            IPipeline<Withdrawal> withdrawalPipeline = new Pipeline<Withdrawal>();
            withdrawalPipeline.Register(dailyLimit)
                .Register(belowMinimumAllowed)
                .Register(aboveMaximumAllowed);

            var result = withdrawalPipeline.Execute(validWithdrawal);
            Console.WriteLine(string.Format("The result for the valid Withdrawal filter is: {0}", result));


            result = withdrawalPipeline.Execute(inValidWithdrawalLimit);
            Console.WriteLine(string.Format("The result for the invalid Withdrawal filter is: {0}", result));

            Console.ReadKey();
        }
    }
}
