using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
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
            viewModel.IsBusy = true;
            viewModel.ModalLookUp = modalLookUp;
            viewModel.InitializeModal();
            viewModel.IsBusy = false;
        }

        private async void SearchClicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            if (viewModel.Project == null || viewModel.Project.Id == Guid.Empty)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn Dự án", "Đóng");
                viewModel.IsBusy = false;
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
                DirectSaleDetail directSaleDetail = new DirectSaleDetail(model);
                directSaleDetail.OnComplete = async (IsSuccess) =>
                {
                    if (IsSuccess)
                    {
                        await Navigation.PushAsync(directSaleDetail);
                        viewModel.IsBusy = false;
                    }
                    else
                    {
                        viewModel.IsBusy = false;
                        await DisplayAlert("Thông Báo", "Không tìm thấy thông tin", "Đồng ý");
                    }
                };
            }

        }

        private async void ShowInfo(object sender, EventArgs e)
        {
            if (viewModel.Project != null)
            {
                viewModel.IsBusy = true;
                ProjectInfo projectInfo = new ProjectInfo(viewModel.Project.Id);
                projectInfo.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess == true)
                    {
                        await Navigation.PushAsync(projectInfo);
                        viewModel.IsBusy = false;
                    }
                    else
                    {
                        await DisplayAlert("", "Không tìm thấy thông tin.", "Đóng");
                        viewModel.IsBusy = false;
                    }
                };                              
            }
        }

        private void VideoList_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Project != null)
            {
                viewModel.IsBusy = true;
                UnitVideoGallery unitVideoGallery = new UnitVideoGallery("Project", viewModel.Project.Id.ToString(), viewModel.Project.Name, "Video dự án");
                unitVideoGallery.OnCompleted = async (IsSuccess) =>
               {
                   if (IsSuccess)
                   {
                       await Navigation.PushAsync(unitVideoGallery);
                       viewModel.IsBusy = false;
                   }
                   else
                   {
                       await DisplayAlert("", "Không có video để hiển thị.", "Đóng");
                       viewModel.IsBusy = false;
                   }
               };

            }
        }

        private void ImageList_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Project != null)
            {
                viewModel.IsBusy = true;
                UnitImageGallery unitImageGallery = new UnitImageGallery("Project", viewModel.Project.Id.ToString(), viewModel.Project.Name, "Hình ảnh dự án");
                unitImageGallery.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess)
                    {
                        await Navigation.PushAsync(unitImageGallery);
                        viewModel.IsBusy = false;
                    }
                    else
                    {
                        await DisplayAlert("", "Không có image để hiển thị.", "Đóng");
                        viewModel.IsBusy = false;
                    }
                };

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
        private int CompareInt(string a, string b)
        {
            if (a != string.Empty && b != string.Empty)
            {
                if (Int32.TryParse(a, out int value1) && Int32.TryParse(b, out int value2))
                {
                    if (value1 > value2)
                        return 1;
                    if (value2 == value1)
                        return 0;
                    if (value1 < value2)
                        return -1;
                }
                if (!Int32.TryParse(a, out int i) || !Int32.TryParse(b, out int j))
                {
                    if (!Int32.TryParse(a, out int c))
                        return -1;
                    if (!Int32.TryParse(b, out int d))
                        return 1;
                    return 0;
                }
            }
            return 0;
        }

        private void MinPrice_Unfocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused == false)
            {
                if (CompareInt(viewModel.minPrice.ToString(), viewModel.maxPrice.ToString()) == 1)
                {
                    DisplayAlert("Thông Báo", "Giá trị không hợp lệ. Vui lòng thử lại!", "Đồng ý");
                    //  viewModel.minPrice = viewModel.maxPrice;
                }
            }
        }

        private void MaxPrice_Unfocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused == false)
            {
                if (CompareInt(viewModel.maxPrice.ToString(), viewModel.minPrice.ToString()) == -1)
                {
                    DisplayAlert("Thông Báo", "Giá trị không hợp lệ. Vui lòng thử lại!", "Đồng ý");
                    //  viewModel.maxPrice = viewModel.minPrice;
                }
            }
        }

        private void MinNetArea_Unfocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused == false)
            {
                if (CompareInt(viewModel.minNetArea.ToString(), viewModel.maxNetArea.ToString()) == 1)
                {
                    DisplayAlert("Thông Báo", "Giá trị không hợp lệ. Vui lòng thử lại!", "Đồng ý");
                    // viewModel.minNetArea = viewModel.maxNetArea;
                }
            }
        }

        private void MaxNetArea_Unfocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused == false)
            {
                if (CompareInt(viewModel.maxNetArea.ToString(), viewModel.minNetArea.ToString()) == -1)
                {
                    DisplayAlert("Thông Báo", "Giá trị không hợp lệ. Vui lòng thử lại!", "Đồng ý");
                    // viewModel.maxNetArea = viewModel.minNetArea;
                }
            }
        }
    }
}