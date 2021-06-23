using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ListTaiLieuKinhDoanhViewModel : ListViewBaseViewModel2<ListTaiLieuKinhDoanhModel>
    {
        public ListTaiLieuKinhDoanhViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "salesliteratures";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='15' page='{Page}'>
                            <entity name='salesliterature'>
                                <all-attributes/>
                            <order attribute='name' descending='true' />
                            <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer'>
                                <attribute name='title' alias='subjecttitle'/>
                            </link-entity>                           
                            </entity>
                          </fetch>";
                //@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"'>
                //            <entity name='salesliterature'>
                //                <all-attributes/>
                //            <order attribute='name' descending='true' />
                //            <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer'>
                //                <attribute name='title' alias='subjecttitle'/>
                //            </link-entity>
                //            <filter type='and'>
                //              <condition attribute='name' operator='like' value='%" + viewModel.Keyword + @"%' />
                //            </filter>
                //            </entity>
                //          </fetch>";
            });
        }
    }
}

