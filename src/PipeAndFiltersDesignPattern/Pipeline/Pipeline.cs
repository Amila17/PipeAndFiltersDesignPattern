using System.Collections.Generic;
using System.Linq;
using PipeAndFiltersDesignPattern.Filters;

namespace PipeAndFiltersDesignPattern.Pipeline
{
    public class Pipeline<T> : IPipeline<T>
    {
        private readonly List<IFinancialRule<T>> _financialRules;

        public Pipeline()
        {
            _financialRules = new List<IFinancialRule<T>>();
        }
        
        //This line is not required if we are using an IoC container;
        public IPipeline<T> Register(IFinancialRule<T> financialRule)
        {
            _financialRules.Add(financialRule);
            return this;
        }

        public bool Execute(Context<T> item)
        {
            _financialRules.ForEach(x => x.Execute(item));

            return !item.Errors.Any();
        }
    }
}
