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
        public string type
        {
            get
            {
                if (bsd_type == 100000007)
                {
                    return "Units";
                }
                else if (bsd_type == 100000000)
                {
                    return "Reservation - Sign off RF";
                }
                else if (bsd_type == 100000001)
                {
                    return "Reservation - Deposited";
                }
                else if (bsd_type == 100000005)
                {
                    return "Reservation - Terminate";
                }
                else if (bsd_type == 100000002)
                {
                    return "Option Entry - 1st installment";
                }
                else if (bsd_type == 100000003)
                {
                    return "Option Entry - Contract";
                }
                else if (bsd_type == 100000004)
                {
                    return "Option Entry - Installments";
                }
                else if (bsd_type == 100000006)
                {
                    return "Option Entry - Terminate";
                }
                else { return null; }
            }
        }

        public DateTime bsd_expiredate { get; set; }
        public string bsd_name { get; set; }
        public string _bsd_project_value { get; set; }
        public int bsd_group { get; set; }
        public string _bsd_reservation_value { get; set; }
        public DateTime createdon { get; set; }
        public string _bsd_units_value { get; set; }

        public string bsd_name_project { get; set; }
        public double bsd_bookingfee_project { get; set; }
        public string bsd_projectcode_project { get; set; }

        public double totalamount_quote { get; set; }
        public double totallineitemamount_base_quote { get; set; }
        public string name_quote { get; set; }
        public string customer_name_contact { get; set; }
        public string customer_name_account_quote { get; set; }

        public string name_salesorder { get; set; }
        public string customer_name_account { get; set; }

        public string name_work
        {
            get
            {
                if (this.name_quote != null)
                {
                    return this.name_quote;
                }
                else if (this.name_salesorder != null)
                {
                    return this.name_salesorder;
                }
                else
                {
                    return null;
                }
            }
        }

        public string customer
        {
            get
            {
                if (this.customer_name_account != null)
                {
                    return this.customer_name_account;
                }
                else if (this.customer_name_contact != null)
                {
                    return this.customer_name_contact;
                }
                else if (this.customer_name_account_quote != null)
                {
                    return this.customer_name_account_quote;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
