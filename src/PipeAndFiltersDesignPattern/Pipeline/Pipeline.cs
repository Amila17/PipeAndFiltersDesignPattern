using System.Collections.Generic;
using System.Linq;
using PipeAndFiltersDesignPattern.Filters;

namespace PipeAndFiltersDesignPattern.Pipeline
{
    public class Pipeline<T> : IPipeline<T>
    {
        private List<IFinancialRule<T>> _financialRules;

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

        public bool Execute(T item)
        {
            var valid = true;
            _financialRules.ForEach(x =>
            {
                x.Execute(item);
                valid = !x.Errors.Any() && valid;
            });

            return valid;
        }
    }
}
