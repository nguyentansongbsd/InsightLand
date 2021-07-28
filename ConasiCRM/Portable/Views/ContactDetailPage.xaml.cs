using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
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
    public partial class ContactDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        private ContactDetailPageViewModel viewModel;
        private Guid Id;
        public ContactDetailPage(Guid contactId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ContactDetailPageViewModel();
            LoadingHelper.Show();
            Tab_Tapped(1);
            Id = contactId;
            Init();
        }
        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());
            if (viewModel.singleContact != null)
                OnCompleted(true);
            else
                OnCompleted(false);
            LoadingHelper.Hide();
        }

        // tab thong tin
        private async Task LoadDataThongTin(string Id)
        {
            if (Id != null && viewModel.singleContact == null)
            {
                await viewModel.loadOneContact(Id);
                if (viewModel.singleContact.gendercode != null)
                { 
                   viewModel.LoadOneGender(viewModel.singleContact.gendercode); 
                }
                if (viewModel.singleContact.bsd_customergroup != null)
                {
                    viewModel.SingleContactgroup = ContactGroup.GetContactGroupById(viewModel.singleContact.bsd_customergroup);
                }
                if(viewModel.singleContact.bsd_type !=null)
                {
                    viewModel.SingleType = ContactType.GetTypeById(viewModel.singleContact.bsd_type);
                }
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
                if (viewModel.list_Duanquantam.Count == 0)
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
                ListView LookUpNhuCauDiaDiem = CreateListView(viewModel.list_provinces_lookup.Cast<object>().ToList(), "new_name");
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
                await viewModel.Add_NhuCauDiaDiem(selected.new_provinceid.ToString(), viewModel.singleContact.contactid);
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
            Provinces tmp = item.CommandParameter as Provinces;
            if (tmp != null && tmp.new_id != null)
            {
                bool x = await App.Current.MainPage.DisplayAlert("Thông Báo", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                if (x)
                {
                    LoadingHelper.Show();
                    await viewModel.Delete_NhuCauDiaDiem(tmp.new_provinceid.ToString(), viewModel.singleContact.contactid);
                    viewModel.list_nhucauvediadiem.Remove(tmp);
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
                ListView LookUpDuAnQuanTam = CreateListView(viewModel.list_project_lookup.Cast<object>().ToList(), "bsd_name");
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
                await viewModel.Add_DuAnQuanTam(selected.bsd_projectid, viewModel.singleContact.contactid);
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
                        await viewModel.Delete_DuAnQuanTam(tmp.bsd_projectid, viewModel.singleContact.contactid);
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
                ListView LookUpTieuChiChonMua = CreateListView(viewModel.list_tieuchichonmua_lookup.Cast<object>().ToList(), "Name");
                LookUpTieuChiChonMua.ItemTapped += LookUpTieuChiChonMua_ItemTapped;
                LookUpModal.ModalContent = LookUpTieuChiChonMua;
                LookUpModal.Title = "Thêm tiêu chí chọn mua";
                LoadingHelper.Hide();
                await LookUpModal.Show();
                //await  LookUpMultipleOptions.Show();
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
                        viewModel.UpdateTieuChi(tmp.Id, false);
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
                ListView LookUpNhuCauVeDienTichCanHo = CreateListView(viewModel.list_nhucauvedientichcanho_lookup.Cast<object>().ToList(), "Name");
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
                        viewModel.UpdateTieuChi(tmp.Id, false);
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
                ListView LookUpLoaiBatDongSanQuanTam = CreateListView(viewModel.list_loaibatdongsanquantam_lookup.Cast<object>().ToList(), "Name");
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
                        viewModel.UpdateTieuChi(tmp.Id, false);
                        viewModel.list_LoaiBatDongSanQuanTam.Remove(tmp);
                        LoadingHelper.Hide();
                    }
                }
            }
        }

        #endregion

        #region Tab giao dich
        private async Task LoadDataGiaoDich(string Id)
        {
            if (viewModel.list_danhsachdatcho.Count == 0)
            {
                viewModel.PageDanhSachDatCho = 1;
                await viewModel.LoadQueuesForContactForm(Id);
            }
            if (viewModel.list_danhsachdatcoc.Count == 0)
            {
                viewModel.PageDanhSachDatCoc = 1;
                await viewModel.LoadReservationForContactForm(Id);
            }
            if (viewModel.list_danhsachhopdong.Count == 0)
            {
                viewModel.PageDanhSachHopDong = 1;
                await viewModel.LoadOptoinEntryForContactForm(Id);
            }
            if (viewModel.list_chamsockhachhang.Count == 0)
            {
                viewModel.PageChamSocKhachHang = 1;
                await viewModel.LoadCaseForContactForm(Id);
            }            
        }
        // danh sach dat cho
        private async void ShowMoreDanhSachDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCho++;
            await viewModel.LoadQueuesForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        // danh sach dat coc
        private async void ShowMoreDanhSachDatCoc_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadReservationForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        // danh sach hop dong
        private async void ShowMoreDanhSachHopDong_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadOptoinEntryForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        //Cham soc khach hang
        private async void ShowMoreChamSocKhachHang_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageChamSocKhachHang++;
            await viewModel.LoadCaseForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        #endregion

        #region TabPhongThuy
        private void LoadDataPhongThuy()
        {
            if(viewModel.PhongThuy == null)
            {
                viewModel.LoadPhongThuy();
            }
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

        #endregion

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {           
            string phone = viewModel.singleContact.mobilephone;
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
            string phone = viewModel.singleContact.mobilephone;
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

        private async void GiaoDich_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(3);
            await LoadDataGiaoDich(Id.ToString());
        }

        private void PhongThuy_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(4);
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
                VisualStateManager.GoToState(radBorderGiaoDich, "Selected");
                VisualStateManager.GoToState(lbGiaoDich, "Selected");
                TabGiaoDich.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderGiaoDich, "Normal");
                VisualStateManager.GoToState(lbGiaoDich, "Normal");
                TabGiaoDich.IsVisible = false;
            }
            if (tab == 4)
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

        private void ThongTinCongTy_Tapped(object sender, EventArgs e)
        {            
            if (viewModel.singleContact._parentcustomerid_value != string.Empty)
            {
                LoadingHelper.Show();
                AccountForm newPage = new AccountForm(Guid.Parse(viewModel.singleContact._parentcustomerid_value));
                newPage.CheckSingleAccount = async (CheckSingleAccount) =>
                {
                    if (CheckSingleAccount == true)
                    {
                        await Navigation.PushAsync(newPage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        await DisplayAlert("Thông Báo", "Không tìm thấy thông tin công ty", "Đóng");
                    }
                };
            }
        }

        private ListView CreateListView<T>(List<T> list, string Name) where T : class
        {
            ListView bsdListView = new ListView();
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
        
    }
}