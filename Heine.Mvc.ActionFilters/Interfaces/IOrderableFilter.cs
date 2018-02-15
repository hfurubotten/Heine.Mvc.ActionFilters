using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters.Interfaces 
{
    public interface IOrderableFilter : IFilter
    {
        int Order { get; set; }
    }
}