using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
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
            viewModel.IsBusy = true;
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            viewModel.IsBusy = false;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Thông báo!", "Bạn có thực sự muốn thoát?", "Đồng ý", "Huỹ");

                if (result)
                {
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); 
                }
            });
            return true;
        }

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;            
            await Navigation.PushAsync(new ContactForm());
            viewModel.IsBusy = false;
        }
        protected override void OnAppearing()
        {
            if (App.Current.Properties.ContainsKey("update") && App.Current.Properties["update"] as string == "1")
            {
                App.Current.Properties["update"] = "0";
                viewModel.RefreshCommand.Execute(null);
            }
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.IsBusy = true;
            var item = e.Item as ContactListModel;
            ContactForm newPage = new ContactForm(item.contactid);
            newPage.CheckSingleContact = async (CheckSingleContact) =>
            {
                if (CheckSingleContact == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                viewModel.IsBusy = false;
            };
        }

        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            viewModel.IsBusy = true;
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.IsBusy = false;
        }

        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                SearchBar_SearchButtonPressed(null, EventArgs.Empty);
            }
        }
    }
}