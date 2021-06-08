using System;
using System.Collections.Generic;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichLamViecTheoTuan : ContentPage
    {
        LichLamViecViewModel viewModel;
        public LichLamViecTheoTuan()
        {
            InitializeComponent();

            this.BindingContext = viewModel = new LichLamViecViewModel();
            this.loadData();
        }

        public async void loadData()
        {
            await viewModel.loadAllActivities();
            this.Handle_DateSelected(null, new DateChangedEventArgs(DateTime.Today, DateTime.Today));
        }

        void Handle_DateSelected(object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            viewModel.selectedDate = e.NewDate;
            viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);

            listView.ItemsSource = viewModel.selectedDateEventsGrouped;
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

        async void Event_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = e.Item as CalendarEvent;
            if(item.Title != null)
            {
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
        }
    }
}
