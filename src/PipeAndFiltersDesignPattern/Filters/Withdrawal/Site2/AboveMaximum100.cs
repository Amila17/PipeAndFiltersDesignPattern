
using PipeAndFiltersDesignPattern.Pipeline;

namespace PipeAndFiltersDesignPattern.Filters.Withdrawal.Site2
{
    public class AboveMaximum100 : IFinancialRule<DomainObjects.Withdrawal>
    {
        public void Execute(Context<DomainObjects.Withdrawal> context)
        {
            const int maximum = 100;
            if (context.item.Amount > maximum)
                context.Errors.Add("Withdrawal amount is greater than maximum");
        }
    }
}
