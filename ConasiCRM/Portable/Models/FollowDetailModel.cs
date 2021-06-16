using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class FollowDetailModel 
    {
        public string bsd_followuplistcode { get; set; }
        public string bsd_followuplistid { get; set; }
        public int statuscode { get; set; }
        public DateTime bsd_date { get; set; }
        public int bsd_type { get; set; }
        public DateTime bsd_expiredate { get; set; }
        public string bsd_name { get; set; }
        public string _bsd_project_value { get; set; }
        public int bsd_group { get; set; }
        public string _bsd_reservation_value { get; set; }
        public DateTime createdon { get; set; }
        public string _bsd_units_value { get; set; }
        public string name_product { get; set; }
        public string bsd_name_bsd_project { get; set; }
        public string customerid_quote { get; set; }
    }
}
