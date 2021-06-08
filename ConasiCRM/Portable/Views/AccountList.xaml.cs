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
    public partial class AccountList : ContentPage
    {
        private readonly AccountListViewModel viewModel;
        public AccountList()
        {
            InitializeComponent();
            BindingContext = viewModel = new AccountListViewModel();
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) => viewModel.RefreshCommand.Execute(null);
            dataGrid.Commands.Add(new GridCellTapCommand<AccountListModel, AccountForm>("accountid"));
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
                        <condition attribute='bsd_name' operator='like' value='%" + viewModel.Keyword + @"%' />
                        <condition attribute='bsd_registrationcode' operator='like' value='%" + viewModel.Keyword + @"%' />
                    </filter>";
            }
            string xml = @"<fetch version='1.0' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='account'>
                    <attribute name='name' />
                    <attribute name='telephone1' />
                    <attribute name='accountid' />
                    <attribute name='bsd_address' />
                    <attribute name='bsd_companycode' />
                    <attribute name='bsd_registrationcode' />
                    <attribute name='bsd_vatregistrationnumber' />
                    <order attribute='createdon' descending='true' />
                    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='a'>
                         <attribute name='bsd_fullname' alias='primarycontact_name' />
                    </link-entity>
                    " + searchCondition + @"
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountListModel>>("accounts", xml);

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

        private async void ViewMenu_Clicked(object sender, EventArgs e)
        {
            AccountListModel model = dataGrid.SelectedItem as AccountListModel;
            var form = new AccountForm(model.accountid);
            await Navigation.PushAsync(form);

        }
    }
    //public class AccountTapComand : DataGridCommand
    //{
    //    public AccountTapComand()
    //    {
    //        Id = DataGridCommandId.CellTap;
    //    }
    //    public override bool CanExecute(object parameter)
    //    {
    //        return true;
    //    }
    //    public override void Execute(object parameter)
    //    {
    //        var context = parameter as DataGridCellInfo;
    //        Account contact = (Account)context.Item;
    //        Application.Current.MainPage.Navigation.PushAsync(new AccountForm(contact.accountid));
    //        this.Owner.CommandService.ExecuteDefaultCommand(DataGridCommandId.CellTap, parameter);
    //    }
    //}
}