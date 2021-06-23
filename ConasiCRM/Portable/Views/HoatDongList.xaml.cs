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
using Telerik.XamarinForms.DataGrid;
using Telerik.XamarinForms.DataGrid.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HoatDongList : ContentPage
    {
        int a = 0;
        public HoatDongListViewModel viewModel;
        public HoatDongList()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new HoatDongListViewModel();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }
        protected override void OnAppearing()
        {
            if (viewModel != null && a ==1) viewModel.RefreshCommand.Execute(null);
        }
        protected override void OnDisappearing()
        {
            a = 1;
        }

        private async void NewTaskMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaskForm());
        }
        private async void PhoneMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhoneCallForm());
        }

        private async void MeetingMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MeetingForm());
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.IsBusy = true;
            HoatDongListModel val = e.Item as HoatDongListModel;           
            if (val.activitytypecode == "task")
            {
                TaskForm newPage = new TaskForm(val.activityid);
                newPage.CheckTaskForm = async (CheckEventData) =>
                {
                    if (CheckEventData == true)
                    {
                        await Navigation.PushAsync(newPage);
                    }
                    viewModel.IsBusy = false;
                };
            }
            else if (val.activitytypecode == "phonecall")
            {
                PhoneCallForm newPage = new PhoneCallForm(val.activityid);
                newPage.CheckPhoneCell = async (CheckEventData) =>
                {
                    if (CheckEventData == true)
                    {
                        await Navigation.PushAsync(newPage);
                    }
                    viewModel.IsBusy = false;
                };
            }
            else if (val.activitytypecode == "appointment")
            {
                MeetingForm newPage = new MeetingForm(val.activityid);
                newPage.CheckMeeting = async (CheckEventData) =>
                {
                    if (CheckEventData == true)
                    {
                        await Navigation.PushAsync(newPage);
                    }
                    viewModel.IsBusy = false;
                };
            }          
        }
    }
}