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
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) => viewModel.RefreshCommand.Execute(null);
            dataGrid.Commands.Add(new GridCellTapCommand<ListPhanHoiModel, PhanHoiForm>("incidentid")); // event detail PhanHoi
            dataGrid.LoadOnDemand += async (sender, e) =>
            {
                viewModel.Page += 1;
                await loadData();
                e.IsDataLoaded = true;
            };
        }

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhanHoiForm());
        }

        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"'>
                            <entity name='incident'>
                                <all-attributes/>
                                <order attribute='createdon' descending='true' />
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='bsd_name' alias='case_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' >
                                  <attribute name='bsd_fullname' alias='case_namecontact'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='productid' visible='false' link-type='outer' >
                                  <attribute name='name' alias='productname'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer'>
                                  <attribute name='fullname' alias='contactname'/>
                                </link-entity>
                                <link-entity name='contract' from='contractid' to='contractid' visible='false' link-type='outer' alias='contracts'>
                                  <attribute name='title' alias='contractname'/>
                                </link-entity>
                                <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' >
                                  <attribute name='title' alias='subjecttitle'/>
                                </link-entity>
                                <filter type='and'>
                                  <condition attribute='title' operator='like' value='%" + viewModel.Keyword + @"%' />
                                </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListPhanHoiModel>>("incidents", xml);
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
                    if (item.case_nameaccount != null)
                    {
                        item.customerid = item.case_nameaccount;
                    }
                    else
                    {
                        item.customerid = item.case_namecontact;
                    }
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
