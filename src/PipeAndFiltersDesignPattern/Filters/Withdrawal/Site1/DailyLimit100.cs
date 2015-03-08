
using PipeAndFiltersDesignPattern.Pipeline;

namespace PipeAndFiltersDesignPattern.Filters.Withdrawal.Site1
{
    public class DailyLimit100 : IFinancialRule<DomainObjects.Withdrawal>
    {
        public void Execute(Context<DomainObjects.Withdrawal> context)
        {
            const int dailyLimit = 100;

            if (context.item.Amount < dailyLimit)
                context.Errors.Add("Withdrawal amount if less than the daily limit.");
        }
    }
}
