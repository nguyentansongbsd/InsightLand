using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Telerik.XamarinForms.DataGrid;
using Telerik.XamarinForms.DataGrid.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationList : ContentPage
    {
        private readonly ReservationListViewModel viewModel;
        public ReservationList()
        {
            InitializeComponent();
            BindingContext = viewModel = new ReservationListViewModel();
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) => viewModel.RefreshCommand.Execute(null);
            dataGrid.Commands.Add(new GridCellTapCommand<ReservationListModel, ReservationForm>("quoteid"));
            dataGrid.LoadOnDemand += async (sender, e) =>
            {
                viewModel.Page += 1;
                await LoadData();
                e.IsDataLoaded = true;
            };
        }

        public async Task LoadData()
        {
            string xml = @"<fetch version='1.0' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='quote'>
                <attribute name='name' />
                <attribute name='quotenumber' />
                <attribute name='totalamount' />
                <attribute name='statecode' />
                <attribute name='customerid' />
                <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                <attribute name='statuscode' />
                <attribute name='bsd_reservationno' />
                <attribute name='bsd_projectid' alias='bsd_project_id' />
                <attribute name='bsd_quotationnumber' />
                <attribute name='quoteid' />
                <order attribute='createdon' descending='true' />
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a'>
                  <attribute name='bsd_name' alias='bsd_projectid_name' />
                </link-entity>
                <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='b'>
                  <attribute name='name' alias='bsd_unitno_name' />
                </link-entity>
                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='c'>
                  <attribute name='bsd_name' alias='purchaser_accountname' />
                </link-entity>
                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='d'>
                  <attribute name='bsd_fullname' alias='purchaser_contactname' />
                </link-entity>
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='e'>
                  <attribute name='bsd_name' alias='phaseslaunch_name' />
                </link-entity>
                <link-entity name='bsd_paymentscheme' from='bsd_paymentschemeid' to='bsd_paymentscheme' visible='false' link-type='outer' alias='a_8524eae1b214e911a97f000d3aa04914'>
                      <attribute name='bsd_name'  alias='paymentscheme_name' />
                    </link-entity>
                <filter type='or'>
                      <condition attribute='name' operator='like' value='%" + viewModel.Keyword + @"%' />
                      <condition attribute='bsd_reservationno' operator='like' value='%" + viewModel.Keyword + @"%' />
                 </filter>
              </entity>
            </fetch>";


            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationListModel>>("quotes", xml);

            if (result == null) // looix
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
    }
}