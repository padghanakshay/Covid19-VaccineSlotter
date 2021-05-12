using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;

namespace VaccineSlotter
{
    public class ResponseHandler
    {
        public Stream unEncodedResponseStream;
        public StreamReader reader;
        //public JContainer jsonResponseContainer;
        public HttpWebResponse responsePassedIn;
        public string responseAsJsonString;

        //===============================================================================

        public Stream UnEncodeResponseStream()
        {
            // Unencode your response stream or, if it is not encoded, return it.
            var responseStream = responsePassedIn.GetResponseStream();
            if (responsePassedIn.ContentEncoding.ToLower().Contains("gzip"))
                unEncodedResponseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            else if (responsePassedIn.ContentEncoding.ToLower().Contains("deflate"))
                unEncodedResponseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
            else
                unEncodedResponseStream = responseStream;

            return unEncodedResponseStream;
        }

        //===============================================================================

        public string ConvertResponseStreamToString(HttpWebResponse webResponse)
        {
            // Unencode Response Stream.
            responsePassedIn = webResponse;
            var responseStream = UnEncodeResponseStream();

            // Convert the response to a JSON string.
            reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            return reader.ReadToEnd();
        }
        //===============================================================================

        //public JContainer ConvertResponseToJson(HttpWebResponse response)
        //{
        //    string localString;

        //    if (response.ContentEncoding.Contains("application/xml"))
        //    {
        //        // Convert the escaped Stream into an XML document.
        //        ConfigXmlDocument xmlDocument = new ConfigXmlDocument();
        //        xmlDocument.LoadXml(ConvertResponseStreamToString(response));

        //        // Now convert the properly-escaped JSON for the response into a JContainer
        //        localString = JsonConvert.SerializeXmlNode(xmlDocument);
        //    }
        //    else
        //        localString = ConvertResponseStreamToString(response);

        //    return JToken.Parse(localString) as JContainer;
        //}

        //===============================================================================

        static public void WriteStringInText(string textFileName, ref string dataToWrite)
        {
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter(textFileName);
            //Write a line of text
            sw.WriteLine(dataToWrite);
            sw.Close();
        }

        //===============================================================================
    }
}
