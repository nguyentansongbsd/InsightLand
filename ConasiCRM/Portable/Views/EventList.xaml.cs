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
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) => viewModel.RefreshCommand.Execute(null);
            dataGrid.Commands.Add(new GridCellTapCommand<EventListModel, EventForm>("bsd_eventid"));
            dataGrid.LoadOnDemand += async (sender, e) =>
            {
                viewModel.Page += 1;
                await LoadData();
                e.IsDataLoaded = true;
            };

        }
        public async Task LoadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"' >
                <entity name='bsd_event'>
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='statuscode' />
                <attribute name='bsd_startdate' />
                <attribute name='bsd_project' />
                <attribute name='bsd_phaselaunch' />
                <attribute name='bsd_eventcode' />
                <attribute name='bsd_enddate' />
                <attribute name='bsd_description' />
                <attribute name='bsd_projectname' />
                <attribute name='bsd_eventid' />
                <order attribute='createdon' descending='true' />
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' visible='false' link-type='outer' alias='phaseslaunch'>
                    <attribute name='bsd_name' alias='bsd_phaseslaunch_name'/>
                </link-entity>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                    <attribute name='bsd_name' alias='bsd_project_name'/>
                </link-entity>
                <filter type='or'>
                    <condition attribute='bsd_projectname' operator='like' value='%" + viewModel.Keyword + @"%' />
                    <condition attribute='bsd_phaselaunchname' operator='like' value='%" + viewModel.Keyword + @"%' />
                    <condition attribute='bsd_eventcode' operator='like' value='%" + viewModel.Keyword + @"%' />
                    </filter>
                </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EventListModel>>("bsd_events", xml);

            if (result == null) // 
            {
                viewModel.Data.Clear();
                viewModel.LoadOnDemandMode = LoadOnDemandMode.Manual;
                return;
            }

            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.bsd_description == null) item.bsd_description = " ";
                    viewModel.Data.Add(item);
                }
            }
            else
            {
                viewModel.LoadOnDemandMode = LoadOnDemandMode.Manual;
            }


        }
    }
}