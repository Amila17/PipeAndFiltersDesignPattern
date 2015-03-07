
using System.Collections.Generic;

namespace PipeAndFiltersDesignPattern.Filters.Withdrawal
{
    public class DailyLimit : IFinancialRule<DomainObjects.Withdrawal>
    {
        public IEnumerable<string> Errors { get; set; }

        public DailyLimit()
        {
            Errors = new List<string>();
        }
        public void Execute(DomainObjects.Withdrawal item)
        {
            const int dailyLimit = 100;

            if (item.Amount < dailyLimit)
                Errors = new List<string>() {"Withdrawal amount if less than the daily limit."};
        }
    }
}
