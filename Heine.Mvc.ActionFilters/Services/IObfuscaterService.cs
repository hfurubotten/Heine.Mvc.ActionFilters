using System;
using System.Net.Http.Headers;

namespace Heine.Mvc.ActionFilters.Services
{
    public interface IObfuscaterService
    {
        void SetObfuscateHeader(HttpHeaders httpHeaders, params Type[] types);
    }
}