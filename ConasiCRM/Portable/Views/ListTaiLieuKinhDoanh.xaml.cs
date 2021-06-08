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
    public partial class ListTaiLieuKinhDoanh : ContentPage
    {
        private readonly ListTaiLieuKinhDoanhViewModel viewModel;
        public ListTaiLieuKinhDoanh()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListTaiLieuKinhDoanhViewModel();
            searchBar.TextChanged += (sender, textChangedEventArgs) =>
            {
                if (string.IsNullOrWhiteSpace(textChangedEventArgs.NewTextValue))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            };
            searchBar.SearchButtonPressed += (sender, a) => viewModel.RefreshCommand.Execute(null);
            dataGrid.Commands.Add(new GridCellTapCommand<ListTaiLieuKinhDoanhModel, TaiLieuKinhDoanhForm>("salesliteratureid")); // event detail TaiLieuKinhDoanh
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
                            <entity name='salesliterature'>
                                <all-attributes/>
                            <order attribute='name' descending='true' />
                            <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer'>
                                <attribute name='title' alias='subjecttitle'/>
                            </link-entity>
                            <filter type='and'>
                              <condition attribute='name' operator='like' value='%" + viewModel.Keyword + @"%' />
                            </filter>
                            </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListTaiLieuKinhDoanhModel>>("salesliteratures", xml);
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
