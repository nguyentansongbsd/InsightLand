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
	public partial class PhiMoGioiList : ContentPage
	{
        private readonly PhiMoGioiListViewModel viewModel;
		public PhiMoGioiList()
		{
			InitializeComponent ();
            BindingContext = viewModel = new PhiMoGioiListViewModel();
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) => viewModel.RefreshCommand.Execute(null);
            dataGrid.Commands.Add(new GridCellTapCommand<PhiMoGioiListModel, PhiMoGioiForm>("bsd_brokeragefeesid")); // event detail PhiMoGioi
            dataGrid.LoadOnDemand += async (sender, e) =>
            {
                viewModel.Page += 1;
                await loadData();
                e.IsDataLoaded = true;
            };
        }
        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='" + OrgConfig.RecordPerPage + @"' page='" + viewModel.Page + @"'>
                            <entity name='bsd_brokeragefees'>
                              <all-attributes/>
                              <order attribute='createdon' descending='false' />
                              <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                                <attribute name='bsd_name' alias='project_bsd_name'/>
                              </link-entity>
                            <filter type='and'>
                              <condition attribute='bsd_name' operator='like' value='%"+ viewModel.Keyword + @"%' />
                            </filter>
                            </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhiMoGioiListModel>>("bsd_brokeragefeeses", xml);
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