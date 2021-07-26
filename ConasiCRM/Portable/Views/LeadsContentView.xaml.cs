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
    public partial class LeadsContentView : ContentView
    {
        private readonly LeadsContentViewViewModel viewModel;
        public LeadsContentView()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LeadsContentViewViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();          
        }

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
    }
}