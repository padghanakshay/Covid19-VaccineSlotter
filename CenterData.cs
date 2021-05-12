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
    public class CenterData
    {
        private string m_CenterID;
        private string m_Name;
        private string m_Address;
        private string m_StateName;
        private string m_District;
        private string m_BlockName; // Like Taluka
        private string m_PinCode;
        private string m_Lat;
        private string m_Long;
        private string m_From_;
        private string m_To_;
        private bool m_IsFree;
        private SessonData m_SessonsData = new SessonData();

        //==================================================

        public SessonData SessonsData
        {
            get { return m_SessonsData; }   // get method
            set { m_SessonsData = value; }  // set method
        }
        //==================================================

        public bool IsFree 
        {
            get { return m_IsFree; }   // get method
            set { m_IsFree = value; }  // set method
        }

        //==================================================

        public string To_
        {
            get { return m_To_; }   // get method
            set { m_To_ = value; }  // set method
        }
        //==================================================

        public string From_
        {
            get { return m_From_; }   // get method
            set { m_From_ = value; }  // set method
        }
        //==================================================

        public string Long
        {
            get { return m_Long; }   // get method
            set { m_Long = value; }  // set method
        }
        //==================================================

        public string Lat
        {
            get { return m_Lat; }   // get method
            set { m_Lat = value; }  // set method
        }
        //==================================================

        public string PinCode
        {
            get { return m_PinCode; }   // get method
            set { m_PinCode = value; }  // set method
        }
        //==================================================

        public string BlockName
        {
            get { return m_BlockName; }   // get method
            set { m_BlockName = value; }  // set method
        }

        //==================================================

        public string District
        {
            get { return m_District; }   // get method
            set { m_District = value; }  // set method
        }

        //==================================================

        public string StateName
        {
            get { return m_StateName; }   // get method
            set { m_StateName = value; }  // set method
        }

        //==================================================

        public string Address
        {
            get { return m_Address; }   // get method
            set { m_Address = value; }  // set method
        }

        //==================================================

        public string CenterID
        {
            get { return m_CenterID; }   // get method
            set { m_CenterID = value; }  // set method
        }

        //==================================================

        public string Name
        {
            get { return m_Name; }   // get method
            set { m_Name = value; }  // set method
        }

        //==================================================

    }

}