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
    public partial class ListTaiLieuKinhDoanh : ContentPage
    {
        private readonly ListTaiLieuKinhDoanhViewModel viewModel;
        public ListTaiLieuKinhDoanh()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListTaiLieuKinhDoanhViewModel();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }
        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListTaiLieuKinhDoanhModel val = e.Item as ListTaiLieuKinhDoanhModel;
            viewModel.IsBusy = true;
            TaiLieuKinhDoanhForm newPage = new TaiLieuKinhDoanhForm(val.salesliteratureid);
            newPage.CheckTaiLieuKinhDoanh = async (CheckTaiLieuKinhDoanh) =>
            {
                if (CheckTaiLieuKinhDoanh == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                viewModel.IsBusy = false;
            };
        }
    }
}
