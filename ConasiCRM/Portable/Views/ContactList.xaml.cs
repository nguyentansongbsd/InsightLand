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
    public partial class ContactList : ContentPage
    {
        private readonly ContactListViewModel viewModel;
        public ContactList()
        {
            InitializeComponent();
            BindingContext = viewModel = new ContactListViewModel();
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) => viewModel.RefreshCommand.Execute(null);
            dataGrid.Commands.Add(new GridCellTapCommand<ContactListModel, ContactForm>("contactid"));
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
                        <condition attribute='fullname' operator='like' value='%" + viewModel.Keyword + @"%' />
                        <condition attribute='mobilephone' operator='like' value='%" + viewModel.Keyword + @"%' />
                        <condition attribute='bsd_identitycardnumber' operator='like' value='%" + viewModel.Keyword + @"%' />
                    </filter>";
            }
            string xml = @"<fetch version='1.0' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='bsd_fullname' />
                    <attribute name='mobilephone' />
                    <attribute name='birthdate' />
                    <attribute name='emailaddress1' />
                    <attribute name='bsd_diachithuongtru' />
                    <attribute name='createdon' />
                    <attribute name='contactid' />
                    <order attribute='fullname' descending='false' />
                    " + searchCondition + @"
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactListModel>>("contacts", xml);

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
            await Navigation.PushAsync(new ContactForm());
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