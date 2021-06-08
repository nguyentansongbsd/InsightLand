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
    public partial class LeadList : ContentPage
    {
        private readonly LeadListViewModel viewModel;
        public LeadList()
        {
            InitializeComponent();
            BindingContext = viewModel = new LeadListViewModel();
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) => viewModel.RefreshCommand.Execute(null);
            dataGrid.Commands.Add(new GridCellTapCommand<LeadListModel, LeadForm>("leadid"));
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
                searchCondition = @"<filter type='or'>
                        <condition attribute='fullname' operator='like' value='%" + viewModel.Keyword + @"%' />
                        <condition attribute='subject' operator='like' value='%" + viewModel.Keyword + @"%' />
                        <condition attribute='mobilephone' operator='like' value='%" + viewModel.Keyword + @"%' />
                        <condition attribute='telephone1' operator='like' value='%" + viewModel.Keyword + @"%' />
                    </filter>";
            }
            string xml = @"<fetch version='1.0' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='lead'>
                <attribute name='fullname' />
                <attribute name='createdon' />
                <attribute name='statuscode' />
                <attribute name='mobilephone' />
                <attribute name='telephone1' />
                <attribute name='emailaddress1' />
                <attribute name='bsd_contactaddress' />
                <attribute name='leadid' />
                <order attribute='createdon' descending='true' />
                 " + searchCondition + @"
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadListModel>>("leads", xml);

            if (result == null) // looix
            {
                viewModel.Data.Clear();
                viewModel.LoadOnDemandMode = LoadOnDemandMode.Manual;
                return;
            }

            var data = result.value;
            //var data = await service.RetrieveMultiple("leads", xml);
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
            await Navigation.PushAsync(new LeadForm());
        }

        protected override void OnAppearing()
        {
            if (App.Current.Properties.ContainsKey("update") && App.Current.Properties["update"] as string == "1")
            {
                App.Current.Properties["update"] = "0";
                viewModel.RefreshCommand.Execute(null);
            }
        }
    }
}