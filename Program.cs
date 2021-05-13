using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using VaccineSlotter;
using System.Media;

using System.Net.Mail;

namespace VaccineSlotter
{
    //======================================================================================
    class Program
    {
        static private string LoginStatus = "";

        //======================================================================================

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

        //======================================================================================

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

        //======================================================================================

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

        //======================================================================================

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

        //======================================================================================

        static string  DisplayCenterData(ref CenterData data)
        {
            string result = "";

            result += "===========================================\n";
            Console.WriteLine("===========================================");

            result += "Name \t" + data.Name + "\n";
            Console.WriteLine("Name \t" + data.Name);

            result += "PinCode \t" + data.PinCode +"\n";
            Console.WriteLine("PinCode \t" + data.PinCode);

            List<SessonData> pSessionData = data.SessonsDataArray;

            for (int sCount = 0; pSessionData != null && sCount < pSessionData.Count; ++sCount)
            {
                SessonData pObj = pSessionData[sCount];
                int minAgeInData = pObj.Min_age_limit;
                int slotAvailable = pObj.Available_Capacity;

                result += "Min Age Limit \t" + minAgeInData.ToString() + "\n";
                Console.WriteLine("Min Age Limit \t" + minAgeInData.ToString());

                result += "Slot Available \t" + slotAvailable.ToString() + "\n";
                Console.WriteLine("Slot Available \t" + slotAvailable.ToString());

            }
            result += "===========================================" + "\n";
            Console.WriteLine("===========================================\n");

            return result;
        }

        //=======================================================================

        static void ReadDataByDistrictName(int minAge, int distCode, ref string emailId)
        {
            DateTime date = DateTime.Now;
            // converting to string format
            string date_str = date.ToString("dd-MM-yyyy");

            string URL = "https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/calendarByDistrict?district_id=" + distCode.ToString() + 
                "&date=" + date_str;
            HttpWebResponse response = Program.GET(URL);
            HttpStatusCode statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.OK)
            {
                //var responseVal = new StreamReader(stream: response.GetResponseStream()).ReadToEnd();
                ResponseHandler pResponseHandler = new ResponseHandler();
                string converted = pResponseHandler.ConvertResponseStreamToString(response);

                List<CenterData> centerData = null;
                ParseStringByDist.GetCentersData(converted, out centerData);

                List<CenterData> shortedData = new List<CenterData>();

                for (int count = 0; count < centerData.Count; ++count )
                {
                    CenterData pData = centerData[count];
                    List<SessonData> pSessionData = pData.SessonsDataArray;

                    for (int sCount = 0; pSessionData != null && sCount < pSessionData.Count; ++sCount)
                    {
                        SessonData pSessonDataInArray = pSessionData[sCount];
                        int minAgeInData = pSessonDataInArray.Min_age_limit;
                        int slotAvailable = pSessonDataInArray.Available_Capacity;

                        if (minAgeInData == minAge)
                        {
                            if (slotAvailable > 0)
                            {
                                shortedData.Add(pData);
                            }
                        }
                    }
                }// Data 

                if (shortedData.Count > 0)
                {
                    string mailBody = "";
                    for (int count = 0; count < shortedData.Count; ++count)
                    {
                        Console.Beep();
                        Console.Beep();

                        CenterData pData = shortedData[count];
                        string result = DisplayCenterData(ref pData);
                        mailBody += result;

                    }// Vaild data

                    Email(ref mailBody, ref emailId);
                }// checker

            }
        }


        //======================================================================================

        public static void Email(ref string htmlString, ref string emailIdOfReceiver)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("akshaypadghan567@gmail.com");
                message.To.Add(new MailAddress(emailIdOfReceiver));
                message.Subject = "Vaccine slot available in your area";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("akshaypadghan567@gmail.com", "A8379820735@a");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception)            
            {
            
            }
        }

        //======================================================================================

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome!! I can alert you when slot is available.");

            Console.WriteLine("Enter min age to set an alert");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Give me your email ID to alert");
            string emailID = Console.ReadLine();

            Console.WriteLine("Give me your District code, ex 367 for Buldhana, 376 for Satara");
            int distCode = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Vaccine-Spotter is Running");

            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            TimeSpan ts = endTime - startTime;
            int sec = 3;

            DateTime messageTime = DateTime.Now;

            while(true)
            {                
                if (sec >= 3) // 3 Sec
                {
                    startTime = DateTime.Now;
                    endTime = DateTime.Now;

                    ReadDataByDistrictName(age, distCode, ref emailID);
                }                

                endTime = DateTime.Now;
                ts = endTime - startTime;
                sec = ts.Seconds;

                TimeSpan tempTS = endTime - messageTime;
                if (tempTS.Seconds / 60 > 3) // 3 Min
                {
                    Console.WriteLine("Vaccine-Spotter is Running to check Available Slot");
                    messageTime = DateTime.Now;
                }
            }
        }

        //======================================================================================

    }
}
