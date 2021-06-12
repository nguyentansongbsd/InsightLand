using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectSale : ContentPage
    {
        private DirectSaleViewModel viewModel;
        public DirectSale()
        {
            InitializeComponent();
            BindingContext = viewModel = new DirectSaleViewModel();
            viewModel.IsCollapse = true;
            viewModel.ModalLookUp = modalLookUp;
            viewModel.InitializeModal();
            viewModel.IsBusy = false;
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            if (viewModel.Project == null || viewModel.Project.Id == Guid.Empty)
            {
                DisplayAlert("Thông báo", "Vui lòng chọn Dự án", "Đóng");
            }
            else
            {
                var model = new DirectSaleSearchModel(viewModel.Project.Id, viewModel.PhasesLanch?.Id ?? Guid.Empty, 
                    viewModel.IsEvent, 
                    viewModel.IsCollapse, "", 
                    viewModel.UnitCode,
                    viewModel.SelectedDirections, 
                    viewModel.SelectedViews, 
                    viewModel.SelectedUnitStatus, 
                    viewModel.minNetArea, viewModel.maxNetArea,
                    viewModel.minPrice, viewModel.maxPrice);
                Application.Current.MainPage.Navigation.PushAsync(new DirectSaleDetail(model));
            }
            viewModel.IsBusy = false;
        }

        private async void ShowInfo(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            await Navigation.PushAsync(new ProjectInfo(viewModel.Project.Id));
            viewModel.IsBusy = false;
        }

        private async void VideoList_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Project != null)
            {
                await Navigation.PushAsync(new UnitVideoGallery("Project", viewModel.Project.Id.ToString(), viewModel.Project.Name, "Video dự án"));
            }
        }

        private async void ImageList_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Project != null)
            {
                await Navigation.PushAsync(new UnitImageGallery("Project", viewModel.Project.Id.ToString(), viewModel.Project.Name, "Hình ảnh dự án"));
            }
        }

        private void Project_Focused(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.CurrentLookUpConfig = viewModel.ProjectConfig;
            viewModel.ProcessLookup(nameof(viewModel.ProjectConfig));
        }

        public void EntryPhasesLaunch_Focused(object sender, EventArgs e)
        {
            viewModel.CurrentLookUpConfig = viewModel.PhasesLanchConfig;
            if (viewModel.Project == null)
            {
                viewModel.ShowLookUpModal = true; ;
            }
            else
            {
                viewModel.IsBusy = true;
                viewModel.CurrentLookUpConfig.FetchXml = @"<fetch version='1.0' count='20' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                        <entity name='bsd_phaseslaunch'>
                        <attribute name='bsd_name' alias='Name' />
                        <attribute name='createdon' />
                        <attribute name='statuscode' />
                        <attribute name='bsd_projectid' />
                        <attribute name='bsd_phaseslaunchid' alias='Id' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                          <condition attribute='statecode' operator='eq' value='0' />
                          <condition attribute='statuscode' operator='eq' value='100000000' />
                          <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + viewModel.Project.Id.ToString() + @"' />
                          <filter type='or'>
                              <condition attribute='bsd_name' operator='like' value='%{1}%' />
                        </filter>
                        </filter>
                      </entity>
                    </fetch>";
                viewModel.ProcessLookup(nameof(viewModel.PhasesLanchConfig), true);
            }
        }

        private async void Refresh_CLicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            await Navigation.PushAsync(new MasterDetailPage1());
            //viewModel.Project = null;
            //viewModel.PhasesLanch = null;
            //viewModel.IsEvent = false;
            //viewModel.UnitCode = null;
            //viewModel.SelectedViews = null;
            //viewModel.SelectedDirections.Clear();
            //viewModel.SelectedUnitStatus.Clear();
            //viewModel.minNetArea = null;
            //viewModel.maxNetArea = null;
            //viewModel.minPrice = null;
            //viewModel.maxPrice = null;
            //viewModel.IsCollapse = false;
            viewModel.IsBusy = false;
        }
    }
}