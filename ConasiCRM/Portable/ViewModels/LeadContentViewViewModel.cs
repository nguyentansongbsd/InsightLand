using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{   
    public class LeadContentViewViewModel : ListViewBaseViewModel2<LeadListModel>
    {
        public string Keyword { get; set; }

        public LeadContentViewViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "leads";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='lead'>
                            <attribute name='fullname' />
                            <attribute name='createdon' />
                            <attribute name='statuscode' />
                            <attribute name='subject' />
                            <attribute name='mobilephone' />
                            <attribute name='telephone1' />
                            <attribute name='emailaddress1' />
                            <attribute name='bsd_contactaddress' />
                            <attribute name='leadid' />
                            <attribute name='leadqualitycode' />
                            <order attribute='createdon' descending='true' />
                            <filter type='and'>
                                <condition attribute='fullname' operator='like' value='%{Keyword}%' />
                            </filter>
                          </entity>
                        </fetch>";
            });
        }
    }
}
