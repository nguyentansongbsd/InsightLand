using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhiMoGioiForm : ContentPage
	{
        private Guid idPhiMoGioi;
        public PhiMoGioiFormViewModel viewModel;
		public PhiMoGioiForm (Guid idPMG)
		{
			InitializeComponent ();
            BindingContext = viewModel = new PhiMoGioiFormViewModel();
            this.idPhiMoGioi = idPMG;
            viewModel.IsBusy = true;
            Task.Run(loadData);
		}

        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_brokeragefees'>
                                  <all-attributes/>
                                  <order attribute='bsd_name' descending='false' />
                                  <filter type='and'>
                                      <condition attribute='bsd_brokeragefeesid' operator='eq' value='" + idPhiMoGioi + @"' />
                                  </filter>
                                  <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                                    <attribute name='bsd_name' alias='project_bsd_name'/>
                                    <attribute name='bsd_projectid' />
                                  </link-entity>
                              </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhiMoGioiFormModel>>("bsd_brokeragefeeses", xml);
            var data = result.value.FirstOrDefault();
            if (data == null)
            {
                await DisplayAlert("Thông báo", "Thông tin chi tiết của phí mô giới không tìm thấy !", "Đóng");
            }
            viewModel.PhiMoGioi = data;
            viewModel.IsBusy = false;
        }
    }
}