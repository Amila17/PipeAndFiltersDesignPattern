using System.Collections.Generic;

namespace PipeAndFiltersDesignPattern.Filters.Withdrawal
{
    public class BelowMinimumAllowed : IFinancialRule<DomainObjects.Withdrawal>
    {
        public IEnumerable<string> Errors { get; set; }

        public BelowMinimumAllowed()
        {
            Errors = new List<string>();
        }
        public void Execute(DomainObjects.Withdrawal item)
        {
            const int minimumAllowed = 100;

            if (item.Amount < minimumAllowed)
                Errors = new List<string> {"Withdrawal amount is less than minimum amount."};
        }
    }
}
