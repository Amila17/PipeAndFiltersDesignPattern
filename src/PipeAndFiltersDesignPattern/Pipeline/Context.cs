
using System.Collections.Generic;

namespace PipeAndFiltersDesignPattern.Pipeline
{
    public class Context<T>
    {
        public Context()
        {
            Errors = new List<string>();
        }
        public T item { get; set; }

        public IList<string> Errors { get; set; } 
    }
}
