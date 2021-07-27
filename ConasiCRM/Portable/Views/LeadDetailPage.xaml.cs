using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadDetailPage : ContentPage
    {
        public Action<bool> CheckSingleLead;
        private LeadDetailPageViewModel viewModel;
        private Guid Id;
        public LeadDetailPage(Guid id)
        {
            InitializeComponent();
            this.Title = "THÔNG TIN KHÁCH HÀNG";
            this.Id = id;
            this.BindingContext = viewModel = new LeadDetailPageViewModel();
            Tab_Tapped(1);
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());

            if (viewModel.singleLead.statuscode == "3") // qualified
            {
                floatingButtonGroup.IsVisible = true;
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Chỉnh sửa", "FontAwesomeRegular", "\uf044", null, Update));
            }
            else
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Lead Qualify", "FontAwesomeSolid", "\uf12e", null, LeadQualify));
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Chỉnh sửa", "FontAwesomeRegular", "\uf044", null, Update));
            }

            if (viewModel.singleLead != null)
                CheckSingleLead?.Invoke(true);
            else
                CheckSingleLead?.Invoke(false);
        }

        private async void Update(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new LeadForm(viewModel.singleLead.leadid));
            LoadingHelper.Hide();
        }

        private async void LeadQualify(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            bool IsSuccess = await viewModel.Qualify(viewModel.singleLead.leadid);
            if (IsSuccess)
            {
                await viewModel.LoadOneLead(Id.ToString());
                LoadingHelper.Hide();
                await Shell.Current.DisplayAlert("", "Thành công", "OK");
            }
            else
            {
                LoadingHelper.Hide();
                await Shell.Current.DisplayAlert("", "Đã xảy ra lỗi. Vui lòng thử lại.", "OK");
            }
            
        }

        private async void ThongTin_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
            await LoadDataThongTin(Id.ToString());
        }

        private async void NhuCau_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(2);
            await LoadDataNhuCau(Id.ToString());
        }

        private async void PhongThuy_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(3);
           // await LoadDataNhuCau(Id.ToString());
        }

        private void Tab_Tapped(int tab)
        {
            if (tab == 1)
            {
                VisualStateManager.GoToState(radBorderThongTin, "Selected");
                VisualStateManager.GoToState(lbThongTin, "Selected");
                TabThongTin.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderThongTin, "Normal");
                VisualStateManager.GoToState(lbThongTin, "Normal");
                TabThongTin.IsVisible = false;
            }
            if (tab == 2)
            {
                VisualStateManager.GoToState(radBorderNhuCau, "Selected");
                VisualStateManager.GoToState(lbNhuCau, "Selected");
                TabNhuCau.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderNhuCau, "Normal");
                VisualStateManager.GoToState(lbNhuCau, "Normal");
                TabNhuCau.IsVisible = false;
            }
            if (tab == 3)
            {
                VisualStateManager.GoToState(radBorderPhongThuy, "Selected");
                VisualStateManager.GoToState(lbPhongThuy, "Selected");
            }
            else
            {
                VisualStateManager.GoToState(radBorderPhongThuy, "Normal");
                VisualStateManager.GoToState(lbPhongThuy, "Normal");
            }
        }

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleLead.mobilephone; // thêm sdt ở đây
            if (phone != string.Empty)
            {
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    SmsMessage sms = new SmsMessage(null, phone);
                    await Sms.ComposeAsync(sms);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Thông Báo", "Số điện thoại sai định dạng. Vui lòng kiểm tra lại", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Thông Báo", "Khách hàng không có số điện thoại. Vui lòng kiểm tra lại", "OK");
            }
        }

        private async void GoiDien_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleLead.mobilephone; // thêm sdt ở đây
            if (phone != string.Empty)
            {
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    await Launcher.OpenAsync($"tel:{phone}");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Thông Báo", "Số điện thoại sai định dạng. Vui lòng kiểm tra lại", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Thông Báo", "Khách hàng không có số điện thoại. Vui lòng kiểm tra lại", "OK");
            }
        }
        // Tab Thong tin
        private async Task LoadDataThongTin(string leadid)
        {
            if (leadid != null)
            {
                await viewModel.LoadOneLead(leadid);
                if (viewModel.singleLead.new_gender != null) { await viewModel.loadOneGender(viewModel.singleLead.new_gender); }
                if (viewModel.singleLead.industrycode != null) { await viewModel.loadOneIndustrycode(viewModel.singleLead.industrycode); }
            }
        }

        #region Tab Nhu cau
        //nhu cau dia diem
        private async Task LoadDataNhuCau(string leadid)
        {
            if (leadid != null)
            {
                viewModel.PageNhuCauDiaDiem = 1;
                viewModel.list_nhucauvediadiem.Clear();
                await viewModel.Load_NhuCauVeDiaDiem(leadid);               
            }
        }        

        private async void BtnAddNhuCauDiaDiem_Clicked(object sender, EventArgs e)
        {
            if (viewModel.list_provinces_lookup.Count == 0)
            {
                await viewModel.LoadAllProvinces();
            }
            if (viewModel.list_provinces_lookup.Count != 0)
            {
                LoadingHelper.Show();
                //Controls.LookUp lookUpNhuCauDiaDiem = new Controls.LookUp();
                //lookUpNhuCauDiaDiem.SetBinding(Controls.LookUp.ItemsSourceProperty, new Binding("list_provinces_lookup", source: viewModel));
                //lookUpNhuCauDiaDiem.SetBinding(Controls.LookUp.SelectedItemProperty, new Binding("provinces_selected", source: viewModel));
                //lookUpNhuCauDiaDiem.BottomModal = LookUpModal;
                //lookUpNhuCauDiaDiem.NameDisplay = "new_name";
                //lookUpNhuCauDiaDiem.SelectedItemChange += LookUpNhuCauDiaDiem_SelectedItemChange;
                //LoadingHelper.Hide();             
                //await lookUpNhuCauDiaDiem.OpenModal();
                //LookUpView lookUpView = new LookUpView();
                //lookUpView.SetList(viewModel.list_provinces_lookup.Cast<object>().ToList(), "new_name");
                //lookUpView.lookUpListView.ItemTapped += LookUpListView_ItemTapped;
                //LookUpModal.ModalContent = lookUpView;
                //LoadingHelper.Hide();
                //await LookUpModal.Show();
                BsdListView bsdListView = new BsdListView();
                bsdListView.ItemsSource = viewModel.list_provinces_lookup;
                bsdListView.ItemTapped += LookUpListView_ItemTapped;
                LookUpModal.ModalContent = bsdListView;
                LoadingHelper.Hide();
                await LookUpModal.Show();
            }
        }

        private async void LookUpListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Provinces selected = e.Item as Provinces;
            if (viewModel.list_nhucauvediadiem.ToList().FirstOrDefault(x => x.new_provinceid == selected.new_provinceid) == null)
            {
                LoadingHelper.Show();
                await viewModel.Add_NhuCauDiaDiem(selected.new_provinceid, viewModel.singleLead.leadid);
                if (viewModel.list_nhucauvediadiem.FirstOrDefault(x => x.new_id == null) != null)
                {
                    var index = viewModel.list_nhucauvediadiem.IndexOf(viewModel.list_nhucauvediadiem.FirstOrDefault(x => x.new_id == null));
                    viewModel.list_nhucauvediadiem[index] = selected;
                }
                else
                {
                    viewModel.list_nhucauvediadiem.Add(selected);
                }
                LoadingHelper.Hide();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Địa điểm đã tồn tại", "OK");
            }
        }

        private async void LookUpNhuCauDiaDiem_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            Provinces selected = viewModel.provinces_selected;
            if (viewModel.list_nhucauvediadiem.ToList().FirstOrDefault(x => x.new_provinceid == selected.new_provinceid) == null)
            {
                LoadingHelper.Show();
                await viewModel.Add_NhuCauDiaDiem(selected.new_provinceid, viewModel.singleLead.leadid);
                if (viewModel.list_nhucauvediadiem.FirstOrDefault(x => x.new_id == null) != null)
                {
                    var index = viewModel.list_nhucauvediadiem.IndexOf(viewModel.list_nhucauvediadiem.FirstOrDefault(x => x.new_id == null));
                    viewModel.list_nhucauvediadiem[index] = selected;
                }
                else
                {
                    viewModel.list_nhucauvediadiem.Add(selected);
                }
                LoadingHelper.Hide();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Địa điểm đã tồn tại", "OK");
            }
        }
        private async void DeleteNhuCauVeDiaDiem_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            Provinces provinces = item.CommandParameter as Provinces;
            if (provinces != null && provinces.new_provinceid != null)
            {
                bool x = await App.Current.MainPage.DisplayAlert("Thông Báo", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                if (x)
                {
                    LoadingHelper.Show();
                    await viewModel.Delete_NhuCauDiaDiem(provinces.new_provinceid, viewModel.singleLead.leadid);
                    viewModel.list_nhucauvediadiem.Remove(provinces);
                    LoadingHelper.Hide();
                }
            }
        }
        private async void ShowMoreNhuCauDiaDiem_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageNhuCauDiaDiem++;
            await viewModel.Load_NhuCauVeDiaDiem(viewModel.singleLead.leadid.ToString());
            LoadingHelper.Hide();
        }

        #endregion
    }
}