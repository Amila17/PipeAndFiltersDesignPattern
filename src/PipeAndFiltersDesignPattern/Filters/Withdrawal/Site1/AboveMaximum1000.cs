
using PipeAndFiltersDesignPattern.Pipeline;

namespace PipeAndFiltersDesignPattern.Filters.Withdrawal.Site1
{
    public class AboveMaximum1000 : IFinancialRule<DomainObjects.Withdrawal>
    {
        public void Execute(Context<DomainObjects.Withdrawal> context)
        {
            const int maximum = 1000;
            if (context.item.Amount > maximum)
                context.Errors.Add("Withdrawal amount is greater than maximum");
        }
    }
}
