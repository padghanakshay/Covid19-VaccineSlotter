using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Globalization;

namespace VaccineSlotter
{
    public class ParseStringByDist
    {


        //==================================================================================

        static private SessonData parseSessionData(ref string centerDataString)
        {
            SessonData data = new SessonData();

            String value = "\"session_id\":";
            data.Session_id = parseData(ref centerDataString, ref value, true);

            value = "\"date\":";
            data.Date = parseData(ref centerDataString, ref value, true);
            //DateTime dateTime = DateTime.Parse(dateString);
           // data.Date = dateTime;

            value = "\"available_capacity\":";
            string availableCapacity = parseData(ref centerDataString, ref value, false);
            data.Available_Capacity = int.Parse(availableCapacity);

            value = "\"min_age_limit\":";
            string ageLimitMin = parseData(ref centerDataString, ref value, false);
            data.Min_age_limit = int.Parse(ageLimitMin);

            value = "\"vaccine\":";
            data.Vaccine = parseData(ref centerDataString, ref value, true);

            // TODO
            // Slot Reading need to impliment
            return data;

        }

        //==================================================================================

        static private string parseData(ref string key, ref string value, bool removeQuotationMarks)
        {
            int keyStartIndex = key.IndexOf(value);
            if (keyStartIndex < 0)
                return null;

            key = key.Remove(keyStartIndex, value.Length);

            int endCharIndex = key.IndexOf(",");
            string idString = key.Substring(keyStartIndex, endCharIndex - keyStartIndex);

            key = key.Remove(keyStartIndex, endCharIndex - keyStartIndex + 1);

            if (removeQuotationMarks)
            {
                char charToRemove = '"';
                idString = idString.Replace(charToRemove.ToString(), "");
                //idString = idString.Replace(charToRemove.ToString(), "");
            }
            return idString;
        }

        //==================================================================================

        static private CenterData ConvertStringToCenterData(ref string centerDataString)
        {
            CenterData data = new CenterData();

            // Now  center data in {---} brace remove it
            centerDataString = centerDataString.Remove(0, 1);
            centerDataString = centerDataString.Remove(centerDataString.Length - 1, 1);

            String value = "\"center_id\":";
            data.CenterID = parseData(ref centerDataString, ref value, false);

            value = "\"name\":";
            data.Name = parseData(ref centerDataString, ref value, true);

            value = "\"address\":";
            data.Address = parseData(ref centerDataString, ref value, true);

            value = "\"state_name\":";
            data.StateName = parseData(ref centerDataString, ref value, true);

            value = "\"district_name\":";
            data.District = parseData(ref centerDataString, ref value, true);

            value = "\"block_name\":";
            data.BlockName = parseData(ref centerDataString, ref value, true);

            value = "\"pincode\":";
            data.PinCode = parseData(ref centerDataString, ref value, false);

            value = "\"lat\":";
            data.Lat = parseData(ref centerDataString, ref value, false);

            value = "\"long\":";
            data.Long = parseData(ref centerDataString, ref value, false);

            value = "\"from\":";
            data.From_ = parseData(ref centerDataString, ref value, true);
            value = "\"to\":";
            data.To_ = parseData(ref centerDataString, ref value, true);

            value = "\"fee_type\":";
            string fees = parseData(ref centerDataString, ref value, true);
            data.IsFree = fees == "Free";

            data.SessonsData = parseSessionData(ref centerDataString);           
            return data;
        }


        //==================================================================================

        static private void getStringCenterData(ref string response, out List<String> centerDataStringArray)
        {
            // Now a Center inside the {--}      
            centerDataStringArray = new List<String>();

            while (true)
            {
                int centerStartIndex = response.IndexOf("{");
                if (centerStartIndex < 0)
                    break;

                int centerEndIndexIndex = response.IndexOf("},");
                bool isLastCenter = false;
                int commaAtLast = 1;

                if (centerEndIndexIndex < 0)
                {
                    isLastCenter = true;
                    centerEndIndexIndex = response.IndexOf("}");
                    commaAtLast = 0;
                }

                // Get substring of center data
                int lengthOfSubString_ = centerEndIndexIndex - centerStartIndex + 1;
                string centerDataString = response.Substring(centerStartIndex, lengthOfSubString_);
                centerDataStringArray.Add(centerDataString);

                // Remove substring from response
                response = response.Remove(centerStartIndex, lengthOfSubString_ + commaAtLast);

                if (isLastCenter)
                {
                    break;
                }
            }

            if (centerDataStringArray.Count == 0)
                centerDataStringArray = null;
        }
        
        //==================================================================================

        static public void GetCentersData(string response, out List<CenterData> data)
        {
            data = null;

            // {"centers":[ -----------------]}
            //{"center_id"----------------------}, in not last else comma NOT AVAILABLE

            int frontBrace = response.IndexOf("{");
            if (frontBrace < 0)
                return;

            int lastBrace = response.LastIndexOf("}");
            if (lastBrace < 0)
                return;

            int totalLengthInBrace = lastBrace - frontBrace - 1;
            response = response.Substring(frontBrace + 1, totalLengthInBrace);

            char[] charV = {'"'};
            string tempCharV = new string(charV);

            string tempCentersString = tempCharV + "centers" + tempCharV + ":";
            int centersIndex = response.IndexOf(tempCentersString);

            response = response.Remove(centersIndex, tempCentersString.Length);

            // Now All center in [---] brace remove it
            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);

             List<String> centerDataStringArray = null;
             getStringCenterData(ref response, out centerDataStringArray);

             if (centerDataStringArray == null || centerDataStringArray.Count <=0 )
                 return;

             data = new List<CenterData>();
             for (int count = 0; count < centerDataStringArray.Count; ++count )
             {
                 string stringData = centerDataStringArray[count];
                 CenterData centerDataConverted = ConvertStringToCenterData(ref stringData);
                 data.Add(centerDataConverted);
             }

        }

        //==========================================
    }

}