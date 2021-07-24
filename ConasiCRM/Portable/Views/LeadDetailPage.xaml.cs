using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
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
            if (viewModel.singleLead != null)
                CheckSingleLead?.Invoke(true);
            else
                CheckSingleLead?.Invoke(false);
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
            LoadDataPhongThuy();
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
                TabPhongThuy.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderPhongThuy, "Normal");
                VisualStateManager.GoToState(lbPhongThuy, "Normal");
                TabPhongThuy.IsVisible = false;
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
        private async Task LoadDataNhuCau(string leadid)
        {
            if (leadid != null)
            {
                if (viewModel.list_nhucauvediadiem.Count == 0)
                {
                    await viewModel.Load_NhuCauVeDiaDiem(leadid); 
                }
                if(viewModel.list_Duanquantam.Count == 0)
                {
                    await viewModel.Load_DanhSachDuAn(leadid);
                }
                if (viewModel.list_TieuChiChonMua.Count == 0 || viewModel.list_LoaiBatDongSanQuanTam.Count == 0 || viewModel.list_NhuCauVeDienTichCanHo.Count == 0)
                {
                    viewModel.LoadTieuChi();
                }
            }
        }
        //nhu cau dia diem
        private async void BtnAddNhuCauDiaDiem_Clicked(object sender, EventArgs e)
        {
            if (viewModel.list_provinces_lookup.Count == 0)
            {
                await viewModel.LoadAllProvinces();
            }
            if (viewModel.list_provinces_lookup.Count != 0)
            {
                LoadingHelper.Show();                
                BsdListView LookUpNhuCauDiaDiem = CreateBsdListView(viewModel.list_provinces_lookup.Cast<object>().ToList(), "new_name");
                LookUpNhuCauDiaDiem.ItemTapped += LookUpNhuCauDiaDiem_ItemTapped;
                LookUpModal.ModalContent = LookUpNhuCauDiaDiem;
                LookUpModal.Title = "Thêm nhu cầu địa điểm";
                LoadingHelper.Hide();
                await LookUpModal.Show();
            }
        }
        private async void LookUpNhuCauDiaDiem_ItemTapped(object sender, ItemTappedEventArgs e)
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
                await LookUpModal.Hide();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông Báo", "Địa điểm đã tồn tại", "OK");
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

        // du an quan tam
        private async void BtnAddDuanquantam_Clicked(object sender, EventArgs e)
        {           
            if (viewModel.list_project_lookup.Count == 0)
            {
                await viewModel.LoadAllProject();
            }
            if (viewModel.list_project_lookup.Count != 0)
            {
                LoadingHelper.Show();
                BsdListView LookUpDuAnQuanTam = CreateBsdListView(viewModel.list_project_lookup.Cast<object>().ToList(), "bsd_name");
                LookUpDuAnQuanTam.ItemTapped += LookUpDuAnQuanTam_ItemTapped;
                LookUpModal.ModalContent = LookUpDuAnQuanTam;
                LookUpModal.Title = "Thêm dự án quan tâm";
                LoadingHelper.Hide();
                await LookUpModal.Show();
            }
        }
        private async void LookUpDuAnQuanTam_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ProjectList selected = e.Item as ProjectList;
            if (viewModel.list_Duanquantam.ToList().FirstOrDefault(x => x.bsd_projectid == selected.bsd_projectid) == null)
            {
                LoadingHelper.Show();
                await viewModel.Add_DuAnQuanTam(selected.bsd_projectid, viewModel.singleLead.leadid);
                if (viewModel.list_Duanquantam.FirstOrDefault(x => x.bsd_projectid == null) != null)
                {
                    var index = viewModel.list_Duanquantam.IndexOf(viewModel.list_Duanquantam.FirstOrDefault(x => x.bsd_projectid == null));
                    viewModel.list_Duanquantam[index] = selected;
                }
                else
                {
                    viewModel.list_Duanquantam.Add(selected);
                }
                LoadingHelper.Hide();
                await LookUpModal.Hide();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông Báo", "Dự án đã tồn tại", "OK");
            }
        }
        private async void DeleteDuAnQuanTam_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            ProjectList tmp = item.CommandParameter as ProjectList;
            if (tmp != null)
            {
                if (tmp.bsd_projectid != null)
                {
                    bool x = await App.Current.MainPage.DisplayAlert("Thông Báo", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                    if (x)
                    {
                        LoadingHelper.Show();
                        await viewModel.Delete_DuAnQuanTam(tmp.bsd_projectid, viewModel.singleLead.leadid);
                        viewModel.list_Duanquantam.Remove(tmp);
                        LoadingHelper.Hide();
                    }
                }
            }
        }

        // cac tieu chi
        private async void BtnAddTieuChiChonMua_Clicked(object sender, EventArgs e)
        {
            if (viewModel.list_tieuchichonmua_lookup.Count == 0)
            {
                viewModel.LoadAllTieuChiChonMua();
            }
            if (viewModel.list_tieuchichonmua_lookup.Count != 0)
            {
                LoadingHelper.Show();
                BsdListView LookUpTieuChiChonMua = CreateBsdListView(viewModel.list_tieuchichonmua_lookup.Cast<object>().ToList(), "Name");
                LookUpTieuChiChonMua.ItemTapped += LookUpTieuChiChonMua_ItemTapped;
                LookUpModal.ModalContent = LookUpTieuChiChonMua;
                LookUpModal.Title = "Thêm tiêu chí chọn mua";
                LoadingHelper.Hide();
                await LookUpModal.Show();
            }
        }

        private async void LookUpTieuChiChonMua_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            TieuChi selected = e.Item as TieuChi;
            if (viewModel.list_TieuChiChonMua.ToList().FirstOrDefault(x => x.Id == selected.Id) == null)
            {
                LoadingHelper.Show();
                viewModel.UpdateTieuChi(selected.Id, true);    
                viewModel.list_TieuChiChonMua.Add(selected);                
                LoadingHelper.Hide();
                await LookUpModal.Hide();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông Báo", "Tiêu chí chọn mua đã tồn tại", "OK");
            }
        }

        private async void DeleteTieuChiChonMua_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            TieuChi tmp = item.CommandParameter as TieuChi;
            if (tmp != null)
            {
                if (tmp.Id != null)
                {
                    bool x = await App.Current.MainPage.DisplayAlert("Thông Báo", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                    if (x)
                    {
                        LoadingHelper.Show();
                        viewModel.UpdateTieuChi(tmp.Id, true);
                        viewModel.list_TieuChiChonMua.Remove(tmp);
                        LoadingHelper.Hide();
                    }
                }
            }
        }

        private async void BtnAddNhuCauVeDienTichCanHo_Clicked(object sender, EventArgs e)
        {
            if (viewModel.list_nhucauvedientichcanho_lookup.Count == 0)
            {
                viewModel.LoadAllNhuCauVeDienTichCanHo();
            }
            if (viewModel.list_nhucauvedientichcanho_lookup.Count != 0)
            {
                LoadingHelper.Show();
                BsdListView LookUpNhuCauVeDienTichCanHo = CreateBsdListView(viewModel.list_nhucauvedientichcanho_lookup.Cast<object>().ToList(), "Name");
                LookUpNhuCauVeDienTichCanHo.ItemTapped += LookUpNhuCauVeDienTichCanHo_ItemTapped;
                LookUpModal.ModalContent = LookUpNhuCauVeDienTichCanHo;
                LookUpModal.Title = "Thêm nhu cầu về diện tích căn hộ";
                LoadingHelper.Hide();
                await LookUpModal.Show();
            }
        }

        private async void LookUpNhuCauVeDienTichCanHo_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            TieuChi selected = e.Item as TieuChi;
            if (viewModel.list_NhuCauVeDienTichCanHo.ToList().FirstOrDefault(x => x.Id == selected.Id) == null)
            {
                LoadingHelper.Show();
                viewModel.UpdateTieuChi(selected.Id, true);
                viewModel.list_NhuCauVeDienTichCanHo.Add(selected);
                LoadingHelper.Hide();
                await LookUpModal.Hide();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông Báo", "Nhu cầu về diện tích căn hộ đã tồn tại", "OK");
            }
        }

        private async void DeleteNhuCauVeDienTichCanHo_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            TieuChi tmp = item.CommandParameter as TieuChi;
            if (tmp != null)
            {
                if (tmp.Id != null)
                {
                    bool x = await App.Current.MainPage.DisplayAlert("Thông Báo", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                    if (x)
                    {
                        LoadingHelper.Show();
                        viewModel.UpdateTieuChi(tmp.Id, true);
                        viewModel.list_NhuCauVeDienTichCanHo.Remove(tmp);
                        LoadingHelper.Hide();
                    }
                }
            }
        }

        private async void BtnAddLoaiBatDongSanQuanTam_Clicked(object sender, EventArgs e)
        {
            if (viewModel.list_loaibatdongsanquantam_lookup.Count == 0)
            {
                viewModel.LoadAllLoaiBatDongSanQuanTam();
            }
            if (viewModel.list_loaibatdongsanquantam_lookup.Count != 0)
            {
                LoadingHelper.Show();
                BsdListView LookUpLoaiBatDongSanQuanTam = CreateBsdListView(viewModel.list_loaibatdongsanquantam_lookup.Cast<object>().ToList(), "Name");
                LookUpLoaiBatDongSanQuanTam.ItemTapped += LookUpLoaiBatDongSanQuanTam_ItemTapped;
                LookUpModal.ModalContent = LookUpLoaiBatDongSanQuanTam;
                LookUpModal.Title = "Thêm loại bất động sản quan tâm";
                LoadingHelper.Hide();
                await LookUpModal.Show();
            }
        }

        private async void LookUpLoaiBatDongSanQuanTam_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            TieuChi selected = e.Item as TieuChi;
            if (viewModel.list_LoaiBatDongSanQuanTam.ToList().FirstOrDefault(x => x.Id == selected.Id) == null)
            {
                LoadingHelper.Show();
                viewModel.UpdateTieuChi(selected.Id, true);
                viewModel.list_LoaiBatDongSanQuanTam.Add(selected);
                LoadingHelper.Hide();
                await LookUpModal.Hide();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông Báo", "Loại bất động sản quan tâm đã tồn tại", "OK");
            }
        }

        private async void DeleteLoaiBatDongSanQuanTam_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            TieuChi tmp = item.CommandParameter as TieuChi;
            if (tmp != null)
            {
                if (tmp.Id != null)
                {
                    bool x = await App.Current.MainPage.DisplayAlert("Thông Báo", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                    if (x)
                    {
                        LoadingHelper.Show();
                        viewModel.UpdateTieuChi(tmp.Id, true);
                        viewModel.list_LoaiBatDongSanQuanTam.Remove(tmp);
                        LoadingHelper.Hide();
                    }
                }
            }
        }
        #endregion

        #region TabPhongThuy
        private void LoadDataPhongThuy()
        {
            viewModel.LoadPhongThuy();                 
        }

        #endregion
        private BsdListView CreateBsdListView<T>(List<T> list, string Name) where T : class
        {
            BsdListView bsdListView = new BsdListView();
            bsdListView.BackgroundColor = Color.White;
            bsdListView.HasUnevenRows = true;
            bsdListView.SelectionMode = ListViewSelectionMode.None;
            bsdListView.SeparatorVisibility = SeparatorVisibility.None;
            var dataTemplate = new DataTemplate(() =>
            {
                RadBorder st = new RadBorder();
                st.BorderThickness = new Thickness(0, 0, 0, 1);
                st.BorderColor = Color.FromHex("#f2f2f2");
                st.Padding = new Thickness(15, 10);
                Label lb = new Label();
                lb.TextColor = Color.Black;
                lb.FontSize = 15;
                lb.SetBinding(Label.TextProperty, Name);
                st.Content = lb;
                return new ViewCell { View = st };
            });

            bsdListView.ItemTemplate = dataTemplate;
            bsdListView.ItemsSource = list;
            return bsdListView;
        }

        private void ShowImage_Tapped(object sender, EventArgs e)
        {
            LookUpImagePhongThuy.IsVisible = true;
        }
        
        protected override bool OnBackButtonPressed()
        {
            if (LookUpImagePhongThuy.IsVisible)
            {
                LookUpImagePhongThuy.IsVisible = false;
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private void Close_LookUpImagePhongThuy_Clicked(object sender, EventArgs e)
        {
            LookUpImagePhongThuy.IsVisible = false;
        }
    }
}