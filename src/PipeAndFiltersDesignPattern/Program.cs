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

            var validContext = new Context<Withdrawal>() {item = validWithdrawal};
            var result = withdrawalPipeline.Execute(validContext);
            Console.WriteLine(string.Format("The request for withdrawal is valid? {0}", result));

            var invalidContext = new Context<Withdrawal> {item = inValidWithdrawalLimit};
            result = withdrawalPipeline.Execute(invalidContext);
            Console.WriteLine(string.Format("The request for the withdrawal is valid? {0}", result));

            Console.ReadKey();
        }
    }
}
