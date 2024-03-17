using AWDBiuBiu.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AWDBiuBiu.Util
{
    public class NetWork
    {

        /// <summary>
        /// POST&&GET 的 HTTP 请求
        /// </summary>
        /// <param name="api">api地址，当fulluri为false时，不需要主机地址</param>
        /// <param name="paramList">POST时的参数</param>
        /// <param name="headerList">header参数</param>
        /// <param name="mode">请求类型</param>
        /// <param name="fulluri">是否为全地址</param>
        /// <param name="filePath">上传文件的路径</param>
        /// <param name="fileParam">上传文件的参数名</param>
        /// <param name="isNeedErrorReturn">是否需要不是200的返回</param>
        /// <returns></returns>
        public static async Task<string> getHttpWebRequestOld(string api, string content = "", List<KeyValuePair<string, string>> paramList = null, List<KeyValuePair<string, string>> headerList = null, string mode = "POST", string filePath = "", string fileParam = "", bool isNeedErrorReturn = false, string jsonParam = "")
        {
            HttpResponseMessage response = null;
            string responseReturn = "";
            return await Task.Run(() =>
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 0, 500);
                    if (headerList != null && headerList.Count != 0)
                    {
                        foreach (var item in headerList)
                        {
                            httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }
                    string uri;
                    uri = api;
                    //HttpRequestMessage requst;
                    switch (mode)
                    {
                        case "POST":
                            //requst = new HttpRequestMessage(HttpMethod.Post, new Uri(uri));
                            if (!string.IsNullOrEmpty(jsonParam))
                            {
                                response = httpClient.PostAsync(new Uri(uri), new StringContent(jsonParam, Encoding.UTF8, "application/json")).Result;
                            }
                            else if (string.IsNullOrEmpty(filePath))
                            {
                                response = httpClient.PostAsync(new Uri(uri), new System.Net.Http.FormUrlEncodedContent(paramList)).Result;
                            }
                            else
                            {
                                var multipartFormDataContent = new MultipartFormDataContent();
                                foreach (var keyValuePair in paramList)
                                {
                                    multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                                        String.Format("\"{0}\"", keyValuePair.Key));
                                }

                                multipartFormDataContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(filePath)),
                                    String.Format("\"{0}\"", fileParam),
                                    String.Format("\"{0}\"", filePath.Split('\\').Last()));

                                response = httpClient.PostAsync(new Uri(uri), multipartFormDataContent).Result;
                            }
                            break;
                        case "GET":
                            //requst = new HttpRequestMessage(HttpMethod.Get, new Uri(uri));
                            response = httpClient.GetAsync(new Uri(uri)).Result;
                            break;
                        case "PUT":
                            //requst = new HttpRequestMessage(HttpMethod.Put, new Uri(uri));
                            response = httpClient.PutAsync(new Uri(uri), new StringContent(content, Encoding.UTF8, "application/json")).Result;
                            break;
                        default:
                            break;
                    }
                    if (response.StatusCode == HttpStatusCode.OK || isNeedErrorReturn)
                    {
                        responseReturn = response.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message + "网络请求异常");
                }
                return responseReturn;
            });
        }


        public static async Task<string> getHttpWebRequest(string api, string content = "", List<KeyValuePair<string, string>> paramList = null, List<KeyValuePair<string, string>> headerList = null, HttpMode _HttpMode = HttpMode.POST, PostMode _PostMode = PostMode.键值对, string filePath = "", string fileParam = "", bool isNeedErrorReturn = false, string jsonParam = "")
        {
            HttpResponseMessage response = null;
            string responseReturn = "";
            return await Task.Run(() =>
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 0, 500);
                    if (headerList != null && headerList.Count != 0)
                    {
                        foreach (var item in headerList)
                        {
                            httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }
                    string uri;
                    uri = api;
                    //HttpRequestMessage requst;
                    switch (_HttpMode)
                    {
                        case HttpMode.POST:
                            switch (_PostMode)
                            {
                                case PostMode.键值对:
                                    response = httpClient.PostAsync(new Uri(uri), new System.Net.Http.FormUrlEncodedContent(paramList)).Result;
                                    break;
                                case PostMode.JSON:
                                    response = httpClient.PostAsync(new Uri(uri), new StringContent(jsonParam, Encoding.UTF8, "application/json")).Result;
                                    break;
                                case PostMode.FormData:
                                    var multipartFormDataContent = new MultipartFormDataContent();
                                    foreach (var keyValuePair in paramList)
                                    {
                                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                                            String.Format("\"{0}\"", keyValuePair.Key));
                                    }

                                    multipartFormDataContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(filePath)),
                                        String.Format("\"{0}\"", fileParam),
                                        String.Format("\"{0}\"", filePath.Split('\\').Last()));

                                    response = httpClient.PostAsync(new Uri(uri), multipartFormDataContent).Result;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case HttpMode.GET:
                            response = httpClient.GetAsync(new Uri(uri)).Result;
                            break;
                        //case HttpMode.PUT:
                        //    //requst = new HttpRequestMessage(HttpMethod.Put, new Uri(uri));
                        //    response = httpClient.PutAsync(new Uri(uri), new StringContent(content, Encoding.UTF8, "application/json")).Result;
                        //    break;
                        default:
                            break;
                    }
                    if (response.StatusCode == HttpStatusCode.OK || isNeedErrorReturn)
                    {
                        responseReturn = response.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message + "网络请求异常");
                }
                return responseReturn;
            });
        }


    }
}
