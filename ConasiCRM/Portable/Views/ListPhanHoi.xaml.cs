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
    public partial class ListPhanHoi : ContentPage
    {
        private readonly ListPhanHoiViewModel viewModel;
        public ListPhanHoi()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListPhanHoiViewModel();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhanHoiForm());
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListPhanHoiModel val = e.Item as ListPhanHoiModel;
            viewModel.IsBusy = true;
            PhanHoiForm newPage = new PhanHoiForm(val.incidentid);
            newPage.CheckPhanHoi = async (CheckPhanHoi) =>
            {
                if (CheckPhanHoi == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                viewModel.IsBusy = false;
            };
        }
    }
}
