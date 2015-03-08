
using PipeAndFiltersDesignPattern.Pipeline;

namespace PipeAndFiltersDesignPattern.Filters
{
    public interface IFinancialRule<T>
    {
        void Execute(Context<T> context);
    }
}
