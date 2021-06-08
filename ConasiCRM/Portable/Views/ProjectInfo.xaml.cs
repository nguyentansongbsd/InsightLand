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
    public partial class ProjectInfo : ContentPage
    {
        public ProjectInfoViewModel viewModel;
        private Guid ProjectId;
        public ProjectInfo(Guid Id)
        {
            InitializeComponent();
            labeDuAnNghienCu.Text = "Dự án nghiên cứu (R&D)";
            ProjectId = Id;
            this.BindingContext = viewModel = new ProjectInfoViewModel();
            LoadData();

        }

        public async Task LoadData()
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_project'>
                <attribute name='bsd_projectid' />
                <attribute name='bsd_name' />
                <attribute name='bsd_loaiduan' />
                <attribute name='createdon' />
                <attribute name='bsd_projectcode' />
                <attribute name='bsd_address' />
                <attribute name='bsd_addressen' />
                <attribute name='bsd_depositpercentda' />
                <attribute name='bsd_esttopdate' />
                <attribute name='bsd_estimatehandoverdate' />
                <attribute name='bsd_landvalueofproject' />
                <attribute name='bsd_maintenancefeespercent' />
                <attribute name='bsd_numberofmonthspaidmf' />
                <attribute name='bsd_managementamount' />
                <attribute name='bsd_bookingfee' />
                <attribute name='bsd_depositamount' />
                <attribute name='bsd_description' />
                <filter type='and'>
                  <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                </filter>
                <link-entity name='account' from='accountid' to='bsd_investor' visible='false' link-type='outer' alias='a_8924f6d5b214e911a97f000d3aa04914'>
                  <attribute name='bsd_name' alias='bsd_investor_name' />
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectInfoModel>>("bsd_projects", FetchXml);
            if (result == null || result.value.Any() == false)
            {
                await DisplayAlert("Thông báo", "Không tìm thấy dự án.", "Đóng");
                return;
            }

            var project = result.value.FirstOrDefault();
            viewModel.Project = project;


            var tasks = new Task[2]
               {
                    LoadDuAnCanhTranh(),
                    LoadDoiThuCanhTrang()
               };
            await Task.WhenAll(tasks);
            viewModel.IsBusy = false;
        }

        public async Task LoadDuAnCanhTranh()
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
              <entity name='bsd_competitiveproject'>
                <attribute name='bsd_competitiveprojectid' />
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_projectcode' />
                <attribute name='bsd_weakness' />
                <attribute name='bsd_strength' />
                <order attribute='bsd_name' descending='false' />
                <link-entity name='bsd_bsd_competitiveproject_bsd_project' from='bsd_competitiveprojectid' to='bsd_competitiveprojectid' visible='false' intersect='true'>
                  <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' alias='ab'>
                    <filter type='and'>
                      <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                    </filter>
                  </link-entity>
                </link-entity>
                <link-entity name='account' from='accountid' to='bsd_investor' visible='false' link-type='outer' alias='a_0a24f6d5b214e911a97f000d3aa04914'>
                  <attribute name='bsd_name' alias='bsd_investor_name' />
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Project_DuAnCanhTranhModel>>("bsd_competitiveprojects", FetchXml);
            if (result == null)
            {
                await DisplayAlert("Lỗi", "Không load được danh sách dự án cạnh tranh", "Đóng");
            }
            var list = result.value;
            var count = list.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    viewModel.DuAnCanhTranh_List.Add(list[i]);
                }
            }
            for (int i = count + 1; i < 4; i++)
            {
                viewModel.DuAnCanhTranh_List.Add(new Project_DuAnCanhTranhModel());
            }
        }
        public async Task LoadDoiThuCanhTrang()
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
              <entity name='competitor'>
                <attribute name='name' />
                <attribute name='websiteurl' />
                <attribute name='competitorid' />
                <attribute name='weaknesses' />
                <attribute name='strengths' />
                <attribute name='createdon' />
                <order attribute='name' descending='false' />
                <link-entity name='bsd_competitor_bsd_project' from='competitorid' to='competitorid' visible='false' intersect='true'>
                  <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' alias='ac'>
                    <filter type='and'>
                      <condition attribute='bsd_projectid' operator='eq' uiname='ARIYANA' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                    </filter>
                  </link-entity>
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Project_DoiThuCanhTranhModel>>("competitors", FetchXml);
            if (result == null)
            {
                await DisplayAlert("Lỗi", "Không load được đối thủ cạnh tranh.", "Đóng");
            }
            var list = result.value;
            var count = list.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    viewModel.DoiThuCanhTranh_List.Add(list[i]);
                }
            }
            for (int i = count + 1; i < 4; i++)
            {
                viewModel.DoiThuCanhTranh_List.Add(new Project_DoiThuCanhTranhModel());
            }
        }
    }
}