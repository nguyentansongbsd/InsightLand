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
        public FollowDetailModel FollowDetail { get => _followDetail; set { _followDetail = value; OnPropertyChanged(nameof(FollowDetail)); } }

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
                                  <condition attribute='bsd_followuplistid' operator='eq' value='" + id + @"'/>
                                </filter>
                                <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='quote'>
                                    <attribute name='totalamount'  alias='totalamount_quote'/>  
                                    <attribute name='name' alias='name_quote' />  
                                    <attribute name='totallineitemamount_base'  alias='totallineitemamount_base_quote'/>
                                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='af'>
                                      <attribute name='bsd_fullname' alias='customer_name_contact' />
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='acc'>
                                        <attribute name='bsd_name' alias='customer_name_account_quote'/>
                                    </link-entity>
                                </link-entity>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='project'>
                                    <attribute name='bsd_name'  alias='bsd_name_project'/>  
                                    <attribute name='bsd_projectcode'  alias='bsd_projectcode_project'/>  
                                    <attribute name='bsd_bookingfee'  alias='bsd_bookingfee_project'/>   
                                </link-entity>
                                <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='a_2ec267f5064be61180ea3863bb367d40'>
                                    <attribute name='name' alias='name_salesorder'/>
                                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a_2e8b11fe7b72e911a9ac000d3a828574'>
                                      <attribute name='bsd_name' alias='customer_name_account'/>
                                    </link-entity>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<FollowDetailModel>>("bsd_followuplists", fetchXml);
            if (result != null)
            {
                FollowDetail = result.value.FirstOrDefault();
            }
        }
    }
}
