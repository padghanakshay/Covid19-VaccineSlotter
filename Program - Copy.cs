using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO; 

namespace VaccineSlotter
{
    class Program
    {
        static private string LoginStatus = "";

        private static string GetCookie()
        {
            string uri = string.Empty;
            string cookie = string.Empty;

            HttpWebResponse response = POST(uri);
            if (response == null)
            {
                LoginStatus = "Not connected";
                return "";
            }

            WebHeaderCollection headers = response.Headers;

            if ((response.StatusCode == HttpStatusCode.Found) ||
                    (response.StatusCode == HttpStatusCode.Redirect) ||
                    (response.StatusCode == HttpStatusCode.Moved) ||
                    (response.StatusCode == HttpStatusCode.MovedPermanently))
            {
                if (headers["Set-Cookie"] != null)
                {
                    string cookies = headers["Set-Cookie"];
                    String[] fields = Regex.Split(cookies, ";\\s*");
                    cookie = fields[0];

                }
            }
            return cookie;
        }
        public static HttpWebResponse POST(string url)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "POST";
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.0.5) Gecko/2008120122 Firefox/3.0.5";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch
            {
                return (HttpWebResponse)null;
            }
        }

        public static HttpWebResponse GET(string url)
        {
            //https://www.codeproject.com/Questions/621289/get-http-request-header-values-in-to-csharp
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls; 

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

                request.Method = "GET";
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch
            {
                return (HttpWebResponse)null;
            }
        }
        public static HttpWebResponse POST(string postData, string url, string referer, string cookie)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "POST";
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";
                request.ContentType = "application/x-www-form-urlencoded";
                if (!string.IsNullOrEmpty(cookie))
                    request.Headers.Add(HttpRequestHeader.Cookie, cookie);
                if (!string.IsNullOrEmpty(referer))
                    request.Referer = referer;
                request.ContentLength = byteArray.Length;
                request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.0.5) Gecko/2008120122 Firefox/3.0.5";
                //   request.Proxy = null;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                try
                {
                    return (HttpWebResponse)request.GetResponse();
                }
                catch
                {
                    return (HttpWebResponse)null;
                }
            }
            catch
            {
                return (HttpWebResponse)null;
            }
        }


        static void Main(string[] args)
        {
            string URL = "https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/calendarByDistrict?district_id=367&date=12-05-2021";
            HttpWebResponse response = Program.GET(URL);
            HttpStatusCode statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.OK)
            {
                var responseVal = new StreamReader(stream: response.GetResponseStream()).ReadToEnd();
                int s = 0;
                s = s +1;
                //URL;
            }

        }
    }
}
