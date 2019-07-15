using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VOD.Common.DTOModels;
using VOD.Common.Exceptions;
using VOD.Common.Extensions;

namespace VOD.Common.Services
{
    public class HttpClientFactoryService: IHttpClientFactoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        public async  Task<List<TResponse>> GetListAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound,
                        "Could not find the resource");
                }
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                var result = await httpClient.GetListAsync<TResponse>(uri.ToLower(), _cancellationToken, token);
                return result;
            }
            catch 
            {

                throw;
            }
        }

        public async  Task<TResponse> GetAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound,
                        "Could not find the resource");
                }
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                var result = await httpClient.GetAsync<TResponse,string>(
                    uri.ToLower(), _cancellationToken,null, token);
                return result;
            }
            catch
            {

                throw;
            }
        }

        public async  Task<TResponse> PostAsync<TRequest, TResponse>(TRequest content, string uri, string serviceName, string token = "")
            where TRequest : class
            where TResponse : class
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound,
                        "Could not find the resource");
                }
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                var result = await httpClient.PostAsync<TRequest, TResponse>(
                    uri.ToLower(),content, _cancellationToken,  token);
                return result;
            }
            catch
            {

                throw;
            }
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(TRequest content, string uri, string serviceName, string token = "")
            where TRequest : class
            where TResponse : class
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound,
                        "Could not find the resource");
                }
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                var result = await httpClient.PutAsync<TRequest, TResponse>(
                    uri.ToLower(), content, _cancellationToken, token);
                return result;
            }
            catch
            {

                throw;
            }
        }

        public async Task<string> DeleteAsync(string uri, string serviceName, string token = "")
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound,
                        "Could not find the resource");
                }
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                var result = await httpClient.DeleteAsync(uri.ToLower(), _cancellationToken, token);
                return result;
            }
            catch
            {

                throw;
            }
        }

        public async Task<TokenDTO> CreateTokenAsync(LoginUserDTO user, string uri, string serviceName, string token = "")
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound,
                        "Could not find the resource");
                }
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                var result = await httpClient.PostAsync<LoginUserDTO, TokenDTO>(
                    uri.ToLower(), user, _cancellationToken, token);
                return result;
            }
            catch
            {

                throw;
            }
        }

        public async Task<TokenDTO> GetTokenAsync(LoginUserDTO user, string uri, string serviceName, string token = "")
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound,
                        "Could not find the resource");
                }
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                var result = await httpClient.GetAsync<TokenDTO, LoginUserDTO>(
                    uri.ToLower(), _cancellationToken, null, token);
                return result;
            }
            catch
            {

                throw;
            }
        }
    }
}
