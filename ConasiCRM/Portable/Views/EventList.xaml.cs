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
	public partial class EventList : ContentPage
	{
        private readonly EventListViewModel viewModel;
		public EventList ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new EventListViewModel();
            Init();           
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }      

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.IsBusy = true;
            EventListModel val = e.Item as EventListModel;          
            EventForm newPage = new EventForm(val.bsd_eventid);
            newPage.CheckEventData = async (CheckEventData) =>
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