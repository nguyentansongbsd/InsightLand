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
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }
        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;   
            await Navigation.PushAsync(new AccountForm());
            viewModel.IsBusy = false;
        }

        private async void ViewMenu_Clicked(object sender, EventArgs e)
        {
            //AccountListModel model = dataGrid.SelectedItem as AccountListModel;
            //var form = new AccountForm(model.accountid);
            //await Navigation.PushAsync(form);

        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.IsBusy = true;           
            var item = e.Item as AccountListModel;
            AccountForm newPage = new AccountForm(item.accountid);
            newPage.CheckSingleAccount = async (CheckSingleAccount) =>
            {
                if (CheckSingleAccount == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                viewModel.IsBusy = false;
            };
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