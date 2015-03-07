

using System.Collections.Generic;

namespace PipeAndFiltersDesignPattern.Filters.Withdrawal
{
    public class AboveMaximum : IFinancialRule<DomainObjects.Withdrawal>
    {
        public IEnumerable<string> Errors { get; set; }

        public AboveMaximum()
        {
            Errors = new List<string>();
        }
        public void Execute(DomainObjects.Withdrawal item)
        {
            const int maximum = 1000;
            if (item.Amount > maximum)
                Errors = new List<string>() {"Withdrawal amount is greater than maximum"};
        }
    }
}
