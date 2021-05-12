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
     public class Slot
    {
         private DateTime m_Start;
         private DateTime m_End;

         public DateTime Start
         {
             get { return m_Start; }   // get method
             set { m_Start = value; }  // set method
         }
        //==================================================

         public DateTime End 
         {
             get { return m_End; }   // get method
             set { m_End = value; }  // set method
         }
        //==================================================

     }

    //=========================================

    //==========================================
    public class SessonData
    {
        private string m_Session_id;
        private string m_date;
        private int m_Available_Capacity;
        private int m_Min_age_limit;
        private string m_vaccine;
        private List<Slot> m_Slots = new List<Slot>();

        public void AddSlot(ref Slot slot)
        {
            m_Slots.Add(slot);
        }

        public List<Slot> Slots
        {
            get { return m_Slots; }   // get method
            set { m_Slots = value; }  // set method
        }

        public string Vaccine
        {
            get { return m_vaccine; }   // get method
            set { m_vaccine = value; }  // set method
        }
        //==================================================
        public int Min_age_limit
        {
            get { return m_Min_age_limit; }   // get method
            set { m_Min_age_limit = value; }  // set method
        }
        //==================================================

        public int Available_Capacity
        {
            get { return m_Available_Capacity; }   // get method
            set { m_Available_Capacity = value; }  // set method
        }
        //==================================================

        public string Date
        {
            get { return m_date; }   // get method
            set { m_date = value; }  // set method
        }

        //==================================================

        public string Session_id
        {
            get { return m_Session_id; }   // get method
            set { m_Session_id = value; }  // set method
        }

        //==================================================

    }

}