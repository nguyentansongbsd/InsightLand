using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConasiCRM.Portable.ViewModels;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Helper;
using Telerik.XamarinForms.Common;
using ConasiCRM.Portable.Config;

namespace ConasiCRM.Portable.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HoaHongGiaoDichList : ContentPage
	{
        public HoaHongGiaoDichListViewModel viewModel;
		public HoaHongGiaoDichList ()
		{
			InitializeComponent ();
            this.init();
            BindingContext = viewModel = new HoaHongGiaoDichListViewModel();
            dataGrid.Commands.Add(new GridCellTapCommand<HoaHongGiaoDichListModel, HoaHongGiaoDichForm>("bsd_commissiontransactionid")); // event detail PhiMoGioi
            dataGrid.LoadOnDemand += async (sender, e) =>
            {
                viewModel.Page += 1;
                await loadData();
                e.IsDataLoaded = true;
            };
        }
        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"'>
              <entity name='bsd_commissiontransaction'>
                <all-attributes/>
                <order attribute='bsd_name' descending='false' />
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                  <attribute name='bsd_name' alias='project_bsd_name'/>
                </link-entity>
                <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='products'>
                  <attribute name='name' alias='products_name'/>
                </link-entity>
                <link-entity name='account' from='accountid' to='bsd_saleagentcompany' visible='false' link-type='outer' alias='accounts'>
                  <attribute name='bsd_name' alias='accounts_bsd_name'/>
                </link-entity>
                <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='quotes'>
                  <attribute name='name' alias='quotes_name' />
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoaHongGiaoDichListModel>>("bsd_commissiontransactions", xml);
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

        public async void init()
        {
            await loadTongTienHoaHong();
            await loadTongTienHoaHongNhan();
        }
        public async Task loadTongTienHoaHong()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                      <entity name='bsd_commissiontransaction'>
                          <attribute name='bsd_totalcommission' alias='totalHoaHong' aggregate='sum' />
                          <filter type='and'>
                          </filter>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoaHongGiaoDichListViewModel>>("bsd_commissiontransactions", xml);
            if (result != null)
            {
                viewModel.totalHoaHong = result.value[0].totalHoaHong;
                this.totalHoaHongConLai();
            }
        }
        public async Task loadTongTienHoaHongNhan()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                            <entity name='bsd_commissiontransaction'>
                                <attribute name='bsd_totalcommission' alias='totalHoaHongNhan' aggregate='sum' />
                                <filter type='and'>
                                    <condition attribute='statuscode' operator='eq' value='100000001' />
                                </filter>
                            </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoaHongGiaoDichListViewModel>>("bsd_commissiontransactions", xml);
            if (result != null)
            {
                viewModel.totalHoaHongNhan = result.value[0].totalHoaHongNhan;
                this.totalHoaHongConLai();
            }
        }
        public void totalHoaHongConLai()
        {
            viewModel.totalHoaHongConLai = viewModel.totalHoaHong - viewModel.totalHoaHongNhan;
        }
    }
}