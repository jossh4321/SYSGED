using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Client.Repo
{
    interface IRepository
    {
        Task<HttpResponseWrapper<object>> Delete(string url);
        Task<HttpResponseWrapper<T>> Get<T>(string url);
        Task<HttpResponseWrapper<object>> Post<T>(string url, T send);
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T send);
        Task<HttpResponseWrapper<TResponse>> Put<TResponse, T>(string url, T send);
        Task<HttpResponseWrapper<object>> Put<T>(string url, T requestBody);

    }
}
