using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichLamViecTheoThang : ContentPage
    {
        LichLamViecViewModel viewModel;

        public LichLamViecTheoThang()
        {
            InitializeComponent();

            this.BindingContext = viewModel = new LichLamViecViewModel();
        }

        public void reset()
        {
            this.viewModel.reset();
            this.loadData();
        }

        public async void loadData()
        {
            await viewModel.loadAllActivities();
            viewModel.selectedDate = DateTime.Today;
            this.seletedDay(viewModel.selectedDate.Value);
        }

        void seletedDay(DateTime d)
        {
            viewModel.selectedDate = d;
            viewModel.UpdateSelectedEvents(d);
        }

        void Handle_SelectionChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            this.seletedDay((DateTime)e.NewValue);
        }

        async void AddButton_Clicked(object sender, System.EventArgs e)
        {
            var choice = await DisplayActionSheet("Chọn Activity để thêm", "Huỷ", null, new String[] { "Cuộc gọi", "Công việc", "Cuộc họp" });
            switch (choice)
            {
                case "Cuộc gọi": await Navigation.PushAsync(new PhoneCallForm(viewModel.selectedDate.Value));
                    break;
                case "Công việc":
                    await Navigation.PushAsync(new TaskForm(viewModel.selectedDate.Value));
                    break;
                case "Cuộc họp":
                    await Navigation.PushAsync(new MeetingForm(viewModel.selectedDate.Value));
                    break;
            }
        }

        async void Event_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = e.Item as CalendarEvent;

            switch (item.Activity.activitytypecode)
            {
                case "task":
                    await Navigation.PushAsync(new TaskForm(item.Activity.activityid));
                    break;
                case "phonecall":
                    await Navigation.PushAsync(new PhoneCallForm(item.Activity.activityid));
                    break;
                case "appointment":
                    await Navigation.PushAsync(new MeetingForm(item.Activity.activityid));
                    break;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.reset();
        }
    }
}
