using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Interfaces;

namespace Heine.Mvc.ActionFilters.Providers
{
    public class OrderedFilterProvider : IFilterProvider
    {
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            // controller-specific
            var controllerSpecificFilters = OrderFilters(actionDescriptor.ControllerDescriptor.GetFilters(), FilterScope.Controller);

            // action-specific
            var actionSpecificFilters = OrderFilters(actionDescriptor.GetFilters(), FilterScope.Action);

            return controllerSpecificFilters.Concat(actionSpecificFilters);
        }

        private IEnumerable<FilterInfo> OrderFilters(IEnumerable<IFilter> filters, FilterScope scope)
        {
            var filterList = filters.ToList();

            // get all filter that dont implement IOrderedFilter and give them order number of 0
            var notOrderableFilter = filterList.Where(filter => !(filter is IOrderableFilter)).ToDictionary(filter => 0, filter => new FilterInfo(filter, scope));

            // get all filter that implement IOrderFilter and give them order number from the instance
            var orderableFilter = filterList.OfType<IOrderableFilter>().OrderBy(filter => filter.Order).ToDictionary(filter => filter.Order, filter => new FilterInfo(filter, scope));

            // concat lists => order => return
            return notOrderableFilter.Concat(orderableFilter).OrderBy(x => x.Key).Select(y => y.Value);
        }
    }
}