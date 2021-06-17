using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public partial class QueueList : ContentPage
    {
        private readonly QueuListViewModel viewModel;
        public ICRMService<QueueListModel> service;
        public QueueList()
        {
            InitializeComponent();
            BindingContext = viewModel = new QueuListViewModel();
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) =>
            {
                viewModel.RefreshCommand.Execute(null);
            };
            service = new CRMService<QueueListModel>();
            dataGrid.Commands.Add(new GridCellQueueListTapCommand());
            dataGrid.LoadOnDemand += async (sender, e) =>
            {
                viewModel.Page += 1;
                await LoadData();
                e.IsDataLoaded = true;
            };
        }

        public async Task LoadData()
        {
            string searchCondition = "";
            if (!string.IsNullOrWhiteSpace(viewModel.Keyword))
            {
                searchCondition = @" <filter type='or'>
                        <condition attribute='name' operator='like' value='%" + viewModel.Keyword + @"%' />
                    </filter>";
            }
            string xml = @"<fetch version='1.0' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='opportunity'>
                    <attribute name='name' />
                    <attribute name='statecode' />
                    <attribute name='customerid' alias='customer_id'/>
                    <attribute name='emailaddress' />
                    <attribute name='bsd_queuenumber' />
                    <attribute name='statuscode' />
                    <attribute name='bsd_queuingfee' />
                    <attribute name='bsd_project' alias='project_id' />
                    <attribute name='bsd_phaselaunch' />
                    <attribute name='createdon' />
                    <attribute name='bsd_queuingexpired' />
                    <attribute name='statuscode' />
                    <attribute name='opportunityid' />                    
                    <order attribute='createdon' descending='true' />                    
                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a'>
                      <attribute name='bsd_name' alias='account_name' />
                      <attribute name='telephone1' alias='telephone' />
                    </link-entity>
                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='b'>
                      <attribute name='bsd_fullname'  alias='contact_name'/>
                    </link-entity>
                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='c'>
                      <attribute name='bsd_name' alias='project_name' />
                    </link-entity>
                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_b088efffb214e911a97f000d3aa04914'>
                        <attribute name='name' alias='unit_name' />
                    </link-entity>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueListModel>>("opportunities", xml);

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

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountForm());
        }
    }
}