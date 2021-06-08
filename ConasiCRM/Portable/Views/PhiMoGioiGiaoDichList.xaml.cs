using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhiMoGioiGiaoDichList : ContentPage
    {
        public PhiMoGioiGiaoDichListViewModel viewModel;
        public PhiMoGioiGiaoDichList()
        {
            InitializeComponent();
            BindingContext = viewModel = new PhiMoGioiGiaoDichListViewModel();
            this.init();
            dataGrid.Commands.Add(new GridCellTapCommand<PhiMoGioiGiaoDichListModel, PhiMoGioiGiaoDichForm>("bsd_brokeragetransactionid")); // event detail PhiMoGioi
            dataGrid.LoadOnDemand += async (sender, e) =>
            {
                viewModel.Page += 1;
                await loadData();
                e.IsDataLoaded = true;
            };
            //Task.Run(loadTotalAmount);
        }

        public async void init()
        {
            await loadTotalAmount();
            await loadTotalAmountReceived();
        }
        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"'>
                <entity name='bsd_brokeragetransaction'>
                    <all-attributes/>
                    <order attribute='createdon' descending='false' />
                    <filter type='and'>
                    </filter>
                    <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='quote'>
                      <attribute name='name' alias='quote_name'/>
                    </link-entity>
                    <link-entity name='bsd_brokeragefees' from='bsd_brokeragefeesid' to='bsd_brokeragefees' visible='false' link-type='outer' alias='brokeragefees'>
                      <attribute name='bsd_name' alias='brokeragefees_name'/>
                    </link-entity>
                  <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='product'>
                  <attribute name='name' alias='product_name'/>
                </link-entity>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer'>
                <attribute name='bsd_name' alias='project_bsd_name'/> 
                </link-entity>
                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' >
                <attribute name='bsd_name' alias='account_bsd_name'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' >
                <attribute name='bsd_fullname' alias='contact_bsd_fullname'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='bsd_collaborator' visible='false' link-type='outer' > 
                <attribute name='bsd_fullname' alias='sales_name'/>
                </link-entity>
                </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhiMoGioiGiaoDichListModel>>("bsd_brokeragetransactions", xml);
            if (result == null) // 
            {
                viewModel.Data.Clear();
                viewModel.LoadOnDemandMode = LoadOnDemandMode.Manual;
                return;
            }

            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    viewModel.Data.Add(item);
                }
            }
            else
            {
                viewModel.LoadOnDemandMode = LoadOnDemandMode.Manual;
            }

        }

        public async Task loadTotalAmount()
        {

            string xml_total = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                                    <entity name='bsd_brokeragetransaction'>
                                        <attribute name='bsd_totalamount' alias='totalPMG' aggregate='sum' />
                                        <filter type='and'>
                                        </filter>
                                    </entity>
                                  </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhiMoGioiGiaoDichListViewModel>>("bsd_brokeragetransactions", xml_total);
            if (result != null)
            {
                viewModel.totalPMG = result.value[0].totalPMG;
                this.totalPMGConLai();
            }
        }
        public async Task loadTotalAmountReceived()
        {
            string xml_total = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                                    <entity name='bsd_brokeragetransaction'>
                                        <attribute name='bsd_totalamount' alias='totalPMGNhan' aggregate='sum' />
                                        <filter type='and'>
                                          <condition attribute='statuscode' operator='eq' value='100000001' />
                                        </filter>
                                    </entity>
                                  </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhiMoGioiGiaoDichListViewModel>>("bsd_brokeragetransactions", xml_total);

            if (result != null)
            {
                viewModel.totalPMGNhan = result.value[0].totalPMGNhan;
                this.totalPMGConLai();
            }

        }
        public void totalPMGConLai()
        {
            viewModel.totalPMGConLai = viewModel.totalPMG - viewModel.totalPMGNhan;
        }
    }
}