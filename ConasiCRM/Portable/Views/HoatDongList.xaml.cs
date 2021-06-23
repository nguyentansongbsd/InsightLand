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
            dataGrid.Commands.Add(new HoatDongCellTapCommand()); // activity detail PhiMoGioi
            dataGrid.LoadOnDemand += async (sender, e) =>
            {
                viewModel.Page += 1;
                await loadData();
                e.IsDataLoaded = true;
            };
        }
        protected override void OnAppearing()
        {
            if (viewModel != null && a ==1) viewModel.RefreshCommand.Execute(null);
        }
        protected override void OnDisappearing()
        {
            a = 1;
        }
        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"'>
              <entity name='activitypointer'>
                <attribute name='subject' />
                <attribute name='scheduledstart' />
                <attribute name='regardingobjectid' />
                <attribute name='prioritycode' />
                <attribute name='scheduledend' />
                <attribute name='activitytypecode' />
                <attribute name='instancetypecode' />
                <attribute name='community' />
                <attribute name='createdon' />
                <attribute name='statecode' />
                <attribute name='ownerid' />
                <attribute name='activityid' />
                <order attribute='createdon' descending='true' />
                <order attribute='scheduledend' descending='true' />
                <filter type='and'>
                  <condition attribute='statecode' operator='in'>
                    <value>0</value>
                    <value>3</value>
                    <value>1</value>
                    <value>2</value>
                  </condition>
                  <condition attribute='isregularactivity' operator='eq' value='1' />
                </filter>
                <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='aa'>
                  <filter type='and'>
                    <condition attribute='partyid' operator='eq-userid' />
                  </filter>
                </link-entity>
                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='accounts'>
                  <attribute name='bsd_name' alias='accounts_bsd_name'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='contacts'>
                  <attribute name='fullname' alias='contact_bsd_fullname'/>
                </link-entity>
                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='leads'>
                  <attribute name='fullname' alias='lead_fullname'/>
                </link-entity>
                <link-entity name='bsd_systemsetup' from='bsd_systemsetupid' to='regardingobjectid' visible='false' link-type='outer' alias='users'>
                    <attribute name='bsd_name' alias='systemsetup_bsd_name'/>
                </link-entity>
                <link-entity name='systemuser' from='systemuserid' to='owninguser' visible='false' link-type='outer' alias='owners'>
                  <attribute name='fullname' alias='owners_fullname'/>
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>("activitypointers", xml);
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
                    //if (item.activitytypecode == "appointment")
                    //{
                    //    dataGrid.Commands.Add(new GridCellTapCommand<HoatDongListModel, MeetingForm>("activityid")); // activity detail PhiMoGioi

                    //}
                    //else if (item.activitytypecode == "phonecall")
                    //{
                    //    dataGrid.Commands.Clear();
                    //    dataGrid.Commands.Add(new GridCellTapCommand<HoatDongListModel, PhoneCallForm>("activityid")); // event detail PhiMoGioi
                    //}

                    viewModel.Data.Add(item);
                }
            }
            else
            {
                viewModel.LoadOnDemandMode = LoadOnDemandMode.Manual;
            }
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
    }

    public class HoatDongCellTapCommand : DataGridCommand
    {
        public HoatDongCellTapCommand()
        {
            Id = DataGridCommandId.CellTap;
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override void Execute(object parameter)
        {
            var context = parameter as DataGridCellInfo;
            HoatDongListModel model = (HoatDongListModel)context.Item;
            if (model.activitytypecode == "appointment")
            {
                Application.Current.MainPage.Navigation.PushAsync(new MeetingForm(model.activityid));
            }
            else if (model.activitytypecode == "phonecall")
            {
                Application.Current.MainPage.Navigation.PushAsync(new PhoneCallForm(model.activityid));
            }
            else if (model.activitytypecode == "task")
            {
                Application.Current.MainPage.Navigation.PushAsync(new TaskForm(model.activityid));
            }
        }
    }
}