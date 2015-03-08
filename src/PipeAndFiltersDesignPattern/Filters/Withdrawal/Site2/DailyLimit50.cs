
using PipeAndFiltersDesignPattern.Pipeline;

namespace PipeAndFiltersDesignPattern.Filters.Withdrawal.Site2
{
    public class DailyLimit50 : IFinancialRule<DomainObjects.Withdrawal>
    {
        public void Execute(Context<DomainObjects.Withdrawal> context)
        {
            const int dailyLimit = 50;

            if (context.item.Amount < dailyLimit)
                context.Errors.Add("Withdrawal amount if less than the daily limit.");
        }
    }
}
