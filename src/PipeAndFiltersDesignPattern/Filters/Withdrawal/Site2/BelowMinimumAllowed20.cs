
using PipeAndFiltersDesignPattern.Pipeline;

namespace PipeAndFiltersDesignPattern.Filters.Withdrawal.Site2
{
    public class BelowMinimumAllowed20 : IFinancialRule<DomainObjects.Withdrawal>
    {
        public void Execute(Context<DomainObjects.Withdrawal> context)
        {
            const int minimumAllowed = 20;

            if (context.item.Amount < minimumAllowed)
                context.Errors.Add("Withdrawal amount is less than minimum amount.");
        }
    }
}
