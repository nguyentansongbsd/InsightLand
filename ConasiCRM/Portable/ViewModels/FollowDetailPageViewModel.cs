using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class FollowDetailPageViewModel : BaseViewModel
    {
        private FollowDetailModel _followDetail;
        public FollowDetailModel FollowDetail { get => _followDetail ; set { _followDetail= value; OnPropertyChanged(nameof(FollowDetail)); } }
       
        public FollowDetailPageViewModel()
        {
         
        }
        public async Task Load(string id)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_followuplist'>
                                <attribute name='bsd_name' />
                                <attribute name='createdon' />
                                <attribute name='bsd_units' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_reservation' />
                                <attribute name='bsd_optionentry' />
                                <attribute name='bsd_expiredate' />
                                <attribute name='bsd_date' />
                                <attribute name='bsd_type' />
                                <attribute name='bsd_installment' />
                                <attribute name='bsd_project' />
                                <attribute name='bsd_group' />
                                <attribute name='bsd_followuplistcode' />
                                <attribute name='bsd_followuplistid' />
                                <order attribute='createdon' descending='true' /> 
                                <filter type='and'>
                                  <condition attribute='bsd_followuplistid' operator='eq' value='" + id + @"'/> (50d6fae1-211f-eb11-a813-000d3a82fbcb)
                                </filter>
                                <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='a_9fe1c29b064be61180ea3863bb367d40'>
                                  <attribute name='customerid' alias='customerid_quote'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_units' link-type='inner' alias='ai'>
                                  <attribute name='name' alias='name_product'/>
                                </link-entity>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='aj'>
                                  <attribute name='bsd_name' alias='bsd_name_bsd_project'/>
                                </link-entity>
                                <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='a_2ec267f5064be61180ea3863bb367d40'>
                                  <attribute name='customerid' />
                                </link-entity>
                              </entity>
                            </fetch> ";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<FollowDetailModel>>("bsd_followuplists", fetchXml);
            if (result != null)
            {
                FollowDetail = result.value.FirstOrDefault();
            }
        }
    }
}
