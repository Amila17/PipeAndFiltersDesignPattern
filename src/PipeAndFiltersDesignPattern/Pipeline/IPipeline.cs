
using PipeAndFiltersDesignPattern.Filters;

namespace PipeAndFiltersDesignPattern.Pipeline
{
    public interface IPipeline<T>
    {
        //This method is only if there is no IoC container usage.
        IPipeline<T> Register(IFinancialRule<T> financialRule);

        bool Execute(Context<T> item);
    }
}
