using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadContentView : ContentView
    {
        private readonly LeadContentViewViewModel viewModel;
        public LeadContentView()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LeadContentViewViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();          
            LoadingHelper.Hide();
        }
        //protected override void OnAppearing()
        //{
        //    if (App.Current.Properties.ContainsKey("update") && App.Current.Properties["update"] as string == "1")
        //    {
        //        App.Current.Properties["update"] = "0";
        //        viewModel.RefreshCommand.Execute(null);
        //    }
        //}
        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var item = e.Item as LeadListModel;
            LeadForm newPage = new LeadForm(item.leadid);
            newPage.CheckSingleLead = async (checkSingleLead) =>
            {
                if (checkSingleLead == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                LoadingHelper.Hide();
            };
        }

        private async void Search_Pressed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private async void Search_TextChanged(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                await viewModel.LoadOnRefreshCommandAsync();
            }
            LoadingHelper.Hide();
        }
        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new LeadForm());
            LoadingHelper.Hide();
        }
    }
}