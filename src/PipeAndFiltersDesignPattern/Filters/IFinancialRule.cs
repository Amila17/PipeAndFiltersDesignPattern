
using System.Collections.Generic;

namespace PipeAndFiltersDesignPattern.Filters
{
    public interface IFinancialRule<T>
    {
        IEnumerable<string> Errors { get; set; } 
        void Execute(T item);
    }
}
