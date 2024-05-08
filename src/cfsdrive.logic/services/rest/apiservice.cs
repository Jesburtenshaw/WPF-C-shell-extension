#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : apiservice.cs 
 * 
 * Contents	: Implementation of rest api service
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using cfsdrive.logic.helpers;
using cfsdrive.logic.services.rest.core;
using cfsdrive.logic.services.rest.dto;
using cfsdrive.logic.services.rest.exceptions;
using cfsdrive.logic.services.rest.models;
using Newtonsoft.Json;

namespace cfsdrive.logic.services.rest
{
    internal class RestApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IApiAuthorization _apiAuthorization = null;
        private CookieContainer _cookies = null;
        protected const int HttpClientTimeOut = 120000; // 120 sec

        public RestApiService(string baseAddress) :
            this(baseAddress, null, null)
        {
        }

        public RestApiService(string baseAddress, string authCookieName, string authCookieValue)
        {
            Api.Instance.Logger.LogInfo("RestApiService created");

            _cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler
            {
                CookieContainer = _cookies
            };

            if (!string.IsNullOrEmpty(authCookieValue))
            {
                _cookies.Add(new Uri(baseAddress), new Cookie(authCookieName, authCookieValue));
            }

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseAddress)
            };

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public CookieContainer Cookies => _cookies;

        public async Task<TResult> Execute<TParams, TResult>(ApiActionAttributes<TParams, TResult> apiAction, TParams @params, object[] actionParams = null)
        {
            string action = apiAction.Action;
            if (actionParams != null)
            {
                action = string.Format(apiAction.Action, actionParams);
            }

            switch (apiAction.Method)
            {
                case HttpMethods.Get:
                    var getQueryParams = @params as IQueryStringParams;
                    return JsonConvert.DeserializeObject<TResult>(await Get(action, getQueryParams));
                case HttpMethods.Post:
                    var postQueryParams = @params as IJsonQueryParams;
                    return JsonConvert.DeserializeObject<TResult>(await Post(action, postQueryParams));
                case HttpMethods.Put:
                    var putQueryParams = @params as IJsonQueryParams;
                    return JsonConvert.DeserializeObject<TResult>(await Put(action, putQueryParams));
                case HttpMethods.Delete:
                    var deleteQueryParams = @params as IQueryStringParams;
                    return JsonConvert.DeserializeObject<TResult>(await Delete(action, deleteQueryParams));
                case HttpMethods.DeleteJson:
                    var deleteJsonParams = @params as IJsonQueryParams;
                    return JsonConvert.DeserializeObject<TResult>(await Delete(action, deleteJsonParams));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private const int ReadBufferSize = 16384;

        public bool DownloadFile<TParams>(ApiActionAttributes<TParams, bool> apiAction, TParams @params, object[] actionParams, string fileName, CancellationToken ct, Action<long, long> progressCallback)
        {
            bool isCancelled = false;
            string action = apiAction.Action;
            if (actionParams != null)
            {
                action = string.Format(apiAction.Action, actionParams);
            }

            var query = (@params as IQueryStringParams)?.ToQueryString() ?? string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Get, GetUrl(action) + (string.IsNullOrWhiteSpace(query) ? string.Empty : "?" + query));

            Sign(request, null);

            Api.Instance.Logger.LogInfo($"{request.Method} {request.RequestUri}");

            int totalReaded = 0;
            var task = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ContinueWith(taskwithresponse =>
            {
                if (taskwithresponse.IsCanceled)
                    return;
                try
                {
                    ct.ThrowIfCancellationRequested();

                    var response = taskwithresponse.Result;
                    long total = response.Content.Headers.ContentLength ?? -1L;

                    Api.Instance.Logger.LogInfo("ContentLength is {0}", total);
                    Api.Instance.Logger.LogInfo("Receive response with statuscode {0}", response.StatusCode);
                    response.EnsureSuccessStatusCode();
                    Api.Instance.Logger.LogInfo("Reading (async) the response's content");
                    var result = response.Content.ReadAsStreamAsync().ContinueWith(t =>
                    {
                        ct.ThrowIfCancellationRequested();

                        Api.Instance.Logger.LogInfo("Got async stream");
                        
                        Api.Instance.Logger.LogInfo("Creates a new file {0}", fileName);
                        using (FileStream fileStream = File.Create(fileName))
                        {
                            bool isMoreToRead = true;
                            do
                            {
                                ct.ThrowIfCancellationRequested();
                                //ctx.ThrowIfCancellationRequested();
                                byte[] buffer = new byte[ReadBufferSize];
                                t.Result.ReadAsync(buffer, 0, buffer.Length).ContinueWith(readed =>
                                {
                                    if (readed.Result == 0)
                                    {
                                        Api.Instance.Logger.LogInfo("Content is readed");
                                        isMoreToRead = false;
                                        progressCallback?.Invoke(total, total);
                                    }
                                    else
                                    {
                                        // resize the result to the size of the data that was returned
                                        Array.Resize(ref buffer, readed.Result);
                                        fileStream.Write(buffer, 0, buffer.Length);
                                        totalReaded += readed.Result;
                                        Api.Instance.Logger.LogInfo($"Received {totalReaded} bytes. Total is {total}.");
                                        progressCallback?.Invoke(totalReaded, total);
                                    }
                                }).Wait();

                            } while (isMoreToRead);
                        }
                    });

                    result.Wait();
                }
                catch (OperationCanceledException e)
                {
                    Api.Instance.Logger.LogInfo("DownloadFile: The operation is cancelled by user");
                }
                catch (AggregateException ex)
                {
                    Api.Instance.Logger.LogError(ex, $"Error occured in {GetType().Name}. Description: {ex.Message}");

                    if (ex.InnerException is OperationCanceledException)
                    {
                        Api.Instance.Logger.LogInfo("The operation is cancelled by user");
                        isCancelled = true;
                    }
                    else
                    {
                        throw new ApiException(ex.Message);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Api.Instance.Logger.LogError(ex, $"Error occured in {GetType().Name}. Description: {ex.Message}");
                    throw new ApiException(ex.Message);
                }
                catch (Exception ex)
                {
                    Api.Instance.Logger.LogError(ex, $"Error occured in {GetType().Name}. Description: {ex.Message}");

                    throw new ApiException(ex.Message);
                }
            });
            task.Wait();

            return !isCancelled;
        }

        public bool UploadFile<TParams>(ApiActionAttributes<TParams, bool> apiAction, TParams @params, object[] actionParams, string fileName, CancellationToken ct, Action<long, long> progressCallback)
        {
            bool isCancelled = false;
            string action = apiAction.Action;
            if (actionParams != null)
            {
                action = string.Format(apiAction.Action, actionParams);
            }

            var query = (@params as IQueryStringParams)?.ToQueryString() ?? string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Post, GetUrl(action) + (string.IsNullOrWhiteSpace(query) ? string.Empty : "?" + query));

            Sign(request, null);

            Api.Instance.Logger.LogInfo($"{request.Method} {request.RequestUri}");

            request.Content = GetFileStreamContent(fileName, ct, progressCallback);

            var task = _httpClient.SendAsync(request, ct).ContinueWith(taskwithresponse =>
            {
                try
                {
                    if (taskwithresponse.IsCanceled)
                        return;

                    HttpResponseMessage response = taskwithresponse.Result;
                    Api.Instance.Logger.LogInfo("Receive response with statuscode {0}", response.StatusCode);
                    response.EnsureSuccessStatusCode();
                    Api.Instance.Logger.LogInfo("Reading (async) the response's content");
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait(ct);
                    Api.Instance.Logger.LogInfo("Receive the response's content : {0}", jsonTask.Result);
                }
                catch (OperationCanceledException e)
                {
                    Api.Instance.Logger.LogInfo("PostFile: The operation is cancelled by user");
                }
                catch (Exception ex)
                {
                    Api.Instance.Logger.LogError(ex, $"Error occured in {GetType().Name}. Description: {ex.Message}");
                    throw new ApiException(ex.Message);
                }
            }, ct);
            task.Wait();

            return !isCancelled;
        }

        private HttpContent GetFileStreamContent(string filePath, CancellationToken ct, Action<long, long> progressCallback)
        {
            MultipartFormDataContent requestContent = new MultipartFormDataContent();
            string fileName = System.IO.Path.GetFileName(filePath);
            MemoryStream ms = new MemoryStream();

            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);

            ms.Seek(0, SeekOrigin.Begin);
            StreamContent fileContent = new StreamContent(ms, 4 * 1024);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"file\"",
                FileName = "\"" + fileName + "\""
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(PathHelper.GetMimeType(filePath));

            requestContent.Add(fileContent);

            return new StreamContentWithProgress(requestContent, progressCallback);
        }

        private Task<string> Get(string action, IQueryStringParams parameters)
        {
            var query = parameters?.ToQueryString() ?? string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Get, GetUrl(action) + (string.IsNullOrWhiteSpace(query) ? string.Empty : "?" + query));

            return SendAndGetResponseAsync(request);
        }

        private Task<string> Delete(string action, IQueryStringParams parameters)
        {
            var query = parameters?.ToQueryString() ?? string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Delete, GetUrl(action) + (string.IsNullOrWhiteSpace(query) ? string.Empty : "?" + query));

            return SendAndGetResponseAsync(request);
        }
        
        private Task<string> Delete(string action, IJsonQueryParams parameters) => SendAndGetResponseAsync(HttpMethod.Delete, action, parameters);
        
        private Task<string> Post(string action, IJsonQueryParams parameters) => SendAndGetResponseAsync(HttpMethod.Post, action, parameters);

        private Task<string> Put(string action, IJsonQueryParams parameters) => SendAndGetResponseAsync(HttpMethod.Put, action, parameters);
        
        private Task<string> SendAndGetResponseAsync(HttpMethod method, string action, IJsonQueryParams parameters)
        {
            var content = parameters?.ToJson() ?? string.Empty;
            var url = GetUrl(action);

            Api.Instance.Logger.LogInfo("{0} sending content:{1}", action, content);

            var request = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            return SendAndGetResponseAsync(request, content);
        }

        private async Task<string> SendAndGetResponseAsync(HttpRequestMessage request, string @params = null)
        {
            Sign(request, @params);

            Api.Instance.Logger.LogInfo($"{request.Method} {request.RequestUri}");

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            Api.Instance.Logger.LogInfo($"{request.Method} {request.RequestUri.PathAndQuery} {(response.IsSuccessStatusCode ? "Response is successfully" : "Response is wrong")}");
            Api.Instance.Logger.LogInfo($"Response is: {responseString}");

            if (!response.IsSuccessStatusCode)
            {
                // try to parse the error response
                ErrorDto error = new ErrorDto
                {
                    Message = "Unknown error",
                    Status = response.StatusCode
                };
                throw new ApiException(error);
            }
            return responseString;
        }

        private void Sign(HttpRequestMessage request, string @params)
        {
            if (_apiAuthorization != null)
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_apiAuthorization.Username}:{_apiAuthorization.ApiKey}")));
        }

        private static string GetUrl(string action) => "/" + action;
    }

}
