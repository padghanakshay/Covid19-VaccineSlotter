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
     public class DriveTextData
    {
         private string m_emailID;
         private int m_distCode;
         private int m_minAge;

         //==================================================

         public DriveTextData()
         {

         }
         //==================================================

         public string EmailID
         {
             get { return m_emailID; }   // get method
             set { m_emailID = value; }  // set method
         }
        //==================================================

         public int DistCode 
         {
             get { return m_distCode; }   // get method
             set { m_distCode = value; }  // set method
         }
        //==================================================
         public int MinAge
         {
             get { return m_minAge; }   // get method
             set { m_minAge = value; }  // set method
         }
        //==================================================

     }


}