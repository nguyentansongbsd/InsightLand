using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.DataControls;
using Telerik.XamarinForms.DataControls.ListView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectSaleDetail : ContentPage
    {
        public Action<bool> OnComplete;
        private DirectSaleDetailViewModel viewModel;
        public Unit CurrentUnit;

        public DirectSaleDetail()
        {
            InitializeComponent();
        }

        public DirectSaleDetail(DirectSaleSearchModel model)
        {
            InitializeComponent();
            BindingContext = viewModel = new DirectSaleDetailViewModel(model);
            Init();
            viewModel.IsBusy = false;
        }

        public async void Init()
        {
            await viewModel.LoadData();
            fillterStatus.PreOpenAsync = viewModel.LoadStatusReason;
            fillterBlock.PreOpenAsync = LoadBlockAsync;
            fillterFloor.PreOpenAsync = LoadFloorAsync;
            if (viewModel.Data != null && viewModel.Data.Count > 0)
            {
                OnComplete?.Invoke(true);
            }
            else
            {
                OnComplete?.Invoke(false);
            }
        }

        private async Task LoadBlockAsync()
        {
            viewModel.IsBusy = true;
            await viewModel.LoadBlocks();
            viewModel.IsBusy = false;
        }

        private async Task LoadFloorAsync()
        {
            viewModel.IsBusy = true;
            await viewModel.LoadFloors();
            viewModel.IsBusy = false;
        }

        private async void OpenUnitPopUp_Tapped(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            CurrentUnit = ((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter as Unit;
            string action = string.Empty;
            if (CurrentUnit.statuscode == 1 || CurrentUnit.statuscode == 100000006) // preparing (vàng) + reserve(lá cây đậm)
            {
                action = await DisplayActionSheet("", "Đóng", null, "Xem thông tin căn hộ");
            }
            else if (CurrentUnit.statuscode == 100000000) // xanh la cây nhạt - Available 
            {
                if (CurrentUnit.event_id != Guid.Empty)
                {
                    action = await DisplayActionSheet("", "Đóng", null, "Xem thông tin căn hộ", "Tạo giữ chỗ", "Tạo đặt cọc");
                }
                else
                {
                    action = await DisplayActionSheet("", "Đóng", null, "Xem thông tin căn hộ", "Tạo giữ chỗ");
                }
            }
            else if (CurrentUnit.statuscode == 100000005)
            {
                action = await DisplayActionSheet("", "Đóng", null, "Xem thông tin căn hộ");
            }
            else if (CurrentUnit.statuscode == 100000003)
            {
                action = await DisplayActionSheet("", "Đóng", null, "Xem thông tin căn hộ");
            }
            else if (CurrentUnit.statuscode == 100000002)
            {
                action = await DisplayActionSheet("", "Đóng", null, "Xem thông tin căn hộ");
            }
            else if (CurrentUnit.statuscode == 100000004) //hoặc Queuing - xanh da trời
            {
                action = await DisplayActionSheet("", "Đóng", null, "Xem thông tin căn hộ", "Tạo giữ chỗ", "Xem danh sách giữ chỗ");
            }
            else
            {
                action = await DisplayActionSheet("", "Đóng", null, "Xem thông tin căn hộ");
            }

            if (action == "Xem thông tin căn hộ")
            {
                await Navigation.PushAsync(new UnitInfo(CurrentUnit.productid));
            }
            else if (action == "Tạo giữ chỗ")
            {
                await Navigation.PushAsync(new QueueForm(CurrentUnit.productid, true));
            }
            else if (action == "Xem danh sách giữ chỗ")
            {
                var QueueList_Result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueListModel_DirectSale>>("opportunities", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='opportunity'>
                    <attribute name='statuscode' />
                    <attribute name='createdon' />
                    <attribute name='bsd_queuenumber' />
                    <attribute name='opportunityid' />
                    <attribute name='bsd_bookingtime' />
                    <attribute name='bsd_queuingexpired' />
                    <order attribute='bsd_bookingtime' descending='false' />
                    <filter type='and'>
                      <condition attribute='statuscode' operator='in'>
                        <value>100000000</value>
                        <value>100000002</value>
                      </condition>
                      <condition attribute='bsd_units' operator='eq' uitype='product' value='" + CurrentUnit.productid.ToString() + @"' />
                    </filter>
                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a'>
                      <attribute name='bsd_name' alias='account_name' />
                    </link-entity>
                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='b'>
                      <attribute name='bsd_fullname'  alias='contact_name'/>
                    </link-entity>
                    <link-entity name='systemuser' from='systemuserid' to='owninguser' visible='false' link-type='outer' alias='a_092526258704e911a98b000d3aa2e890'>
                          <attribute name='fullname' alias='salesman' />
                     </link-entity>
                     <link-entity name='account' from='accountid' to='bsd_salesagentcompany' visible='false' link-type='outer' alias='a_1f25f6d5b214e911a97f000d3aa04914'>
                          <attribute name='bsd_name' alias='salesagentcompany' />
                      </link-entity>
                  </entity>
                </fetch>");
                if (QueueList_Result != null && QueueList_Result.value.Any())
                {
                    QueueListGrid.ItemsSource = QueueList_Result.value;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("", "Lỗi. Vui lòng thử lại", "Đóng");
                }
                modalQueueList.IsVisible = true;
            }
            else if (action == "Tạo đặt cọc")
            {
                bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn tạo báo giá không ?", "Đồng ý", "Hủy");
                if (confirm)
                {
                    var res = await CrmHelper.PostData($"/products({CurrentUnit.productid})//Microsoft.Dynamics.CRM.bsd_Action_DirectSale", new
                    {
                        Command = "Reservation"
                    });
                    if (res.IsSuccess)
                    {
                        DirectSaleActionResponse directSaleActionResponse = JsonConvert.DeserializeObject<DirectSaleActionResponse>(res.Content);
                        DirectSaleActionSubResponse subResponse = directSaleActionResponse.GetSubResponse();
                        if (subResponse.type == "Success")
                        {
                            await Navigation.PushAsync(new ReservationForm(Guid.Parse(subResponse.content)));
                        }
                        else await DisplayAlert("Thông báo", "Tạo báo giá thất bạn. " + subResponse.content, "Đóng");
                    }
                    else
                    {
                        await DisplayAlert("Thông báo", res.GetErrorMessage(), "Đóng");
                    }
                }
            }
            viewModel.IsBusy = false;
        }

        private void CloseQueseList_Modal(object sender, EventArgs e)
        {
            this.modalQueueList.IsVisible = false;
        }

        private async void ViewQueue_Clicked(object sender, EventArgs e)
        {
            var model = QueueListGrid.SelectedItem as QueueListModel_DirectSale;
            if (model == null || model.opportunityid == Guid.Empty)
            {
                await DisplayAlert("Thông báo", "Chọn Đặt chỗ muốn xem", "Đóng");
                return;
            }
            //await Navigation.PushAsync(new QueueForm(model.opportunityid));
        }

        private void listView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            viewModel.IsBusy = true;
            var item = (Unit)e.Item;
            UnitInfo unitInfo = new UnitInfo(item.productid);
            unitInfo.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    viewModel.IsBusy = false;
                    await Navigation.PushAsync(unitInfo);
                }
                else
                {
                    viewModel.IsBusy = false;
                    await DisplayAlert("", "Không tìm thấy thông tin", "Đóng");
                }
            };
        }

        private async void MainSearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.ResetXml();
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.IsBusy = false;
        }

        private void MainSearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Keyword))
            {
                MainSearchBar_SearchButtonPressed(null, EventArgs.Empty);
            }
        }

        private async void fillterStatus_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            viewModel.IsBusy = true;
            if (viewModel.StatusReason.Val == "-1")
            {
                viewModel.StatusReason = null;
            }
            viewModel.ResetXml();
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.IsBusy = false;
        }

        private async void fillterBlock_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            viewModel.IsBusy = true;
            if (viewModel.Block.Val == "-1")
            {
                viewModel.Block = null;
            }
            viewModel.ResetXml();
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.IsBusy = false;
        }

        private async void fillterFloor_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            viewModel.IsBusy = true;
            if (viewModel.Floor.Val =="-1")
            {
                viewModel.Floor = null;
            }
            viewModel.ResetXml();
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.IsBusy = false;
        }
    }
}