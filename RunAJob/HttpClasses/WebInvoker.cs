using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;

//using System.Xml;

namespace ICS.Framework.RESTful
{
    public class WebInvoker
    {
        public static bool Get(HttpGetRequest getrequest, out string result)
        
        {
            return Invoke(getrequest, out result);
        }

               
        //public static bool Post(HttpPostRequest request, out string result)
               
        //{
        //    return Invoke(request, out result);
        //}

        //public static bool Delete(HttpDeleteRequest request, out string result)
        //{
        //    return Invoke(request, out result);
        //}

        //public static bool Get(HttpGetRequest request, out byte[] result)
        
        //{
        //    return Invoke(request, out result);
        //}

        public static bool Post(HttpPostRequest request, out byte[] result)
        
        {
            return Invoke(request, out result);
        }
     
        // public static bool Invoke(IHttpRequestBase request, out string result)
        public static bool Invoke(IHttpRequestBase request, out string result)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            result = string.Empty;

            byte[] contents;
            bool requestState = Invoke(request, out contents);

            if (contents != null && contents.Length > 0)
            {
                result = request.Encoding.GetString(contents);
            }

            return requestState;
        }

        //public static bool Invoke(IHttpRequestBase request, out byte[] result)
        public static bool Invoke(IHttpRequestBase request, out byte[] result)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            result = null;

            try
            {
                var webRequest = WebRequest.Create(request.GetRequestUrl()) as HttpWebRequest;

                if (webRequest == null)
                {
                    return false;
                }

                SetHttpWebRequest(request, webRequest);
                SetRequestStream(request, webRequest);

                return GetResponse(webRequest, out result);
            }
            catch (Exception ex)
            {
                // log the exception...?? in most cases is a time out.
                //string aa = ex.ToString();
                //Console.WriteLine("ERROR: " + ex.Message + "Exiting!");
                return false;
            }
        }


        private static bool GetResponse(HttpWebRequest webRequest, out byte[] content)
        {
            content = null;

            using (var webResponse = webRequest.GetResponse() as HttpWebResponse)
            {
                if (webResponse == null || webResponse.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }

                using (var responseStream = webResponse.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        return false;
                    }

                    var memoryStream = new MemoryStream();
                    responseStream.CopyTo(memoryStream);
                    content = memoryStream.ToArray();

                    return true;
                }
            }
        }

        private static void SetRequestStream(IHttpRequestBase request, HttpWebRequest webRequest)
        //private static void SetRequestStream(IHttpRequestBase request, HttpWebRequest webRequest)
        {
            if (request.HttpVerbs != HttpVerbs.GET)
            {
                var requestBytes = request.GetRequestBytes();

                if (requestBytes != null && requestBytes.Length > 0)
                {
                    using (var requestStream = webRequest.GetRequestStream())
                    {
                        requestStream.Write(requestBytes, 0, requestBytes.Length);
                    }
                }
            }
        }

        //private static void SetHttpWebRequest(IHttpRequestBase request, HttpWebRequest webRequest)
        private static void SetHttpWebRequest(IHttpRequestBase request, HttpWebRequest webRequest)
        {
            webRequest.Timeout = request.Timeout;
            webRequest.AllowAutoRedirect = false;
            webRequest.Method = request.HttpVerbs.ToString().ToUpper();
            webRequest.ContentType = request.ContentType;
            webRequest.ProtocolVersion = request.ProtocolVersion;
            webRequest.Accept = request.Accept;

            if (request.postData != "")
            {
                Stream RequestStream = webRequest.GetRequestStream();
                //    byte[] RequestBytes = Encoding.GetEncoding("utf-8").GetBytes(xmlTask);
                RequestStream.Write(Encoding.GetEncoding("utf-8").GetBytes(request.postData), 0,
                    Encoding.GetEncoding("utf-8").GetBytes(request.postData).Length);
                RequestStream.Close();
            }
            
            if (request.Headers.Count !=0) 
            {
              for (int i = 0; i < request.Headers.Count; ++i)
                {
                    string header = request.Headers.GetKey(i);
                    
                    foreach (string value in request.Headers.GetValues(i))
                    {
                        webRequest.Headers.Add(header, value);
                    }
                }
                
            }
            
            
            if (request.Cookies.Count > 0)
            {
                webRequest.CookieContainer = new CookieContainer();
                webRequest.CookieContainer.Add(request.Cookies);
            }
        }



    }
}

