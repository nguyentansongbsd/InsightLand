using System;
using System.Collections.Generic;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichLamViecTheoNgay : ContentPage
    {
        LichLamViecViewModel viewModel;
        public LichLamViecTheoNgay()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LichLamViecViewModel();
            this.loadData();
        }
        public async void loadData()
        {
            await viewModel.loadAllActivities();
            viewModel.selectedDate = DateTime.Today;
        }

        async void Handle_AppointmentTapped(object sender, Telerik.XamarinForms.Input.AppointmentTappedEventArgs e)
        {
            var item = e.Appointment as CalendarEvent;

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

        async void AddButton_Clicked(object sender, System.EventArgs e)
        {
            var choice = await DisplayActionSheet("Chọn Activity để thêm", "Huỷ", null, new String[] { "Cuộc gọi", "Công việc", "Cuộc họp" });
            switch (choice)
            {
                case "Cuộc gọi":
                    await Navigation.PushAsync(new PhoneCallForm(viewModel.selectedDate.Value));
                    break;
                case "Công việc":
                    await Navigation.PushAsync(new TaskForm(viewModel.selectedDate.Value));
                    break;
                case "Cuộc họp":
                    await Navigation.PushAsync(new MeetingForm(viewModel.selectedDate.Value));
                    break;
            }
        }

        void Handle_TimeSlotTapped(object sender, Telerik.XamarinForms.Input.TimeSlotTapEventArgs e)
        {
            viewModel.selectedDate = e.StartTime;
        }

        void Handle_DisplayDateChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            viewModel.selectedDate = (DateTime)e.NewValue;
        }
    }
}
