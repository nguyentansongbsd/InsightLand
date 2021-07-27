using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Models;
using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadForm : ContentPage
    {
        public Action<bool> CheckSingleLead;
        private bool IsMaQuocGiaCaNhan;
        public LeadFormViewModel viewModel;

        List<MyNewCheckBox> checkBoxes;
        List<MyNewCheckBox> multiselectCheckBoxes;
        List<ImageButton> multiselectClearimageButton;

        public ListView listViewCountry { get; set; }
        public ListView listViewProvince { get; set; }
        public ListView listViewDistrict { get; set; }

        public LeadForm()
        {
            InitializeComponent();
            this.Title = "TẠO MỚI KHÁCH HÀNG";
            Init();
            //this.constructor();
        }
        public LeadForm(Guid Id)
        {
            InitializeComponent();
            this.Title = "CẬP NHẬT KHÁCH HÀNG";
            this.BindingContext = viewModel = new LeadFormViewModel();
            this.constructor();
        }

        public async void Init()
        {
            this.BindingContext = viewModel = new LeadFormViewModel();
            centerModalAddress.Body.BindingContext = viewModel;
            listMaQuocGia.ItemsSource = viewModel.MaQuocGiaList;
            SetPreOpen();



            //await loadData(Id.ToString());
            //if (viewModel.singleLead != null)
            //    CheckSingleLead?.Invoke(true);
            //else
            //    CheckSingleLead?.Invoke(false);
        }

        public void SetPreOpen()
        {
            lookUpCurrency.PreOpenAsync = async () => {
                await viewModel.LoadCurrenciesForLookup();
            };

            lookUpLinhVuc.PreOpenAsync = async () => {
                viewModel.loadIndustrycode();
            };

            lookUpChienDich.PreOpenAsync = async () => {
                await viewModel.LoadCampainsForLookup();
            };
        }

        public void constructor()
        {
            viewModel.singleLead = new LeadFormModel();
            //viewModelProject = new ProjectViewModel();
            checkBoxes = new List<MyNewCheckBox>();
            multiselectCheckBoxes = new List<MyNewCheckBox>();
            multiselectClearimageButton = new List<ImageButton>();
        }

        public async Task loadData(string leadid)
        {
            viewModel.IsBusy = true;
            if (leadid != null)
            {
                await viewModel.LoadOneLead(leadid);
                await viewModel.Load_NhuCauVeDiaDiem(leadid);
                await viewModel.dudiemdanhgia();
                await viewModel.Load_DanhSachDuAn(leadid);

                viewModel.AddressVn = viewModel.singleLead.bsd_contactaddress;
                viewModel.AddressEn = viewModel.singleLead.bsd_diachi;
                viewModel.StreetVn = viewModel.singleLead.bsd_housenumberstreet;
                viewModel.StreetEn = viewModel.singleLead.bsd_housenumber;
                viewModel.Country = viewModel.singleLead.bsd_country_label;
                viewModel.Province = viewModel.singleLead.bsd_province_label;
                viewModel.District = viewModel.singleLead.bsd_district_label;

                
                if (viewModel.singleLead.industrycode != null) { await viewModel.loadOneIndustrycode(viewModel.singleLead.industrycode); }
                //if (leadid != null) { await viewModelProject.LoadProjectsForLeadForm(); }
            
            }        
            await viewModel.LoadLeadsRating();
            this.render(leadid);
            viewModel.IsBusy = false;
        }

        private void render(string leadid)
        {
            if (leadid == null)
            {
                viewModel.singleLead.showQualifyButton = 0;
                btn_save_lead.Clicked += AddLead_Clicked;

                //label_nhu_cau_ve_dia_diem.IsVisible = false;
               // stacklayout_nhucauvediadiem.IsVisible = false;
                //label_du_an_quan_tam.IsVisible = false;
                //block_danh_sach_du_an_quan_tam.IsVisible = false;
            }
            else
            {
                viewModel.singleLead.showQualifyButton = viewModel.singleLead.bsd_diemdanhgia >= viewModel.Dudiemdanhgia.bsd_totalleadsratingpoint ? GridLength.Star : 0;
                btn_save_lead.Text = "Cập nhật";
                btn_save_lead.Clicked += UpdateLead_Clicked;
                //btn_qualify_lead.Text = "Qualify";



                //label_nhu_cau_ve_dia_diem.IsVisible = true;
                //stacklayout_nhucauvediadiem.IsVisible = true;
                //label_du_an_quan_tam.IsVisible = true;
                //block_danh_sach_du_an_quan_tam.IsVisible = true;

                //datagrid_danhsachduanquantam.SetBinding(RadDataGrid.ItemsSourceProperty, new Binding("Items", source: viewModelProject));
                //datagrid_danhsachduanquantam.Commands.Add(new ProjectRowTapped());
            }
            /// Render LeadRating
            this.renderLeadRating();
        }

        public void reload(Guid Id)
        {
            viewModel.reset();
            btn_save_lead.Clicked -= AddLead_Clicked;
            btn_save_lead.Clicked -= UpdateLead_Clicked;

            this.constructor();

            //grid_leadsrating.Children.Clear();
            this.loadData(Id.ToString());
        }

        private void renderLeadRating()
        {
            //source for grid leadsrating
            for (int i = 0; i < viewModel.list_leadrating.Count / 2; i++)
            {
                var item = viewModel.list_leadrating[i];
                bool isEnable = item.bsd_startdate == null ? true : (item.bsd_startdate.Value.CompareTo(DateTime.Today) > 0 ? false : (item.bsd_enddate.Value.CompareTo(DateTime.Today) < 0 ? false : true));

                MyNewCheckBox tmp = new MyNewCheckBox
                {
                    IsChecked = isEnable ? viewModel.singleLead.checkLeadsRating(item.bsd_leadsratingid) : false,
                    IsEnabled = isEnable ? true : false,
                    UncheckedColor = isEnable ? Color.Black : Color.LightGray,
                    //Title = item.bsd_name,
                    Title = isEnable ? item.bsd_name : item.bsd_name + " (Hết hạn)",
                    TitleColor = isEnable ? Color.Black : Color.Red,
                    VerticalOptions = LayoutOptions.Start
                };
                tmp.changeChecked += Tmp_IsCheckedChanged;
                checkBoxes.Add(tmp);

                //grid_leadsrating.Children.Add(tmp, 0, i);
            }
            for (int i = viewModel.list_leadrating.Count / 2; i < viewModel.list_leadrating.Count; i++)
            {
                var item = viewModel.list_leadrating[i];
                bool isEnable = item.bsd_startdate == null ? true : (item.bsd_startdate.Value.CompareTo(DateTime.Today) > 0 ? false : (item.bsd_enddate.Value.CompareTo(DateTime.Today) < 0 ? false : true));

                MyNewCheckBox tmp = new MyNewCheckBox
                {
                    IsChecked = isEnable ? viewModel.singleLead.checkLeadsRating(item.bsd_leadsratingid) : false,
                    IsEnabled = isEnable ? true : false,
                    UncheckedColor = isEnable ? Color.Black : Color.LightGray,
                    //Title = item.bsd_name,
                    Title = isEnable ? item.bsd_name : item.bsd_name + " (Hết hạn)",
                    TitleColor = isEnable ? Color.Black : Color.Red,
                    VerticalOptions = LayoutOptions.Start
                };
                tmp.changeChecked += Tmp_IsCheckedChanged;
                checkBoxes.Add(tmp);

                //grid_leadsrating.Children.Add(tmp, 1, i - viewModel.list_leadrating.Count / 2);
            }
        }

        private void Tmp_IsCheckedChanged(object sender, EventArgs e)
        {
            var checkedId = checkBoxes.IndexOf(sender as MyNewCheckBox);

            if (viewModel.singleLead.checkLeadsRating(viewModel.list_leadrating[checkedId].bsd_leadsratingid))
            {
                var itemsUnChecked = viewModel.list_leadrating[checkedId];

                List<String> lst_danhgiaid = viewModel.singleLead.getList(viewModel.singleLead.bsd_danhgiaid);
                List<String> lst_danhgiadiem = viewModel.singleLead.getList(viewModel.singleLead.bsd_danhgiadiem);

                lst_danhgiaid.RemoveAll(x => string.IsNullOrEmpty(x) && string.IsNullOrWhiteSpace(x));
                lst_danhgiadiem.RemoveAll(x => string.IsNullOrEmpty(x) && string.IsNullOrWhiteSpace(x));

                int index = lst_danhgiaid.IndexOf(itemsUnChecked.bsd_leadsratingid);

                lst_danhgiaid.RemoveAt(index);
                lst_danhgiadiem.RemoveAt(index);

                viewModel.singleLead.bsd_danhgiaid = string.Join(",", lst_danhgiaid);
                viewModel.singleLead.bsd_danhgiadiem = string.Join(",", lst_danhgiadiem);

                decimal tmp = 0;
                foreach (var item in lst_danhgiadiem)
                {
                    tmp += decimal.Parse(Device.RuntimePlatform == Device.Android ? item.Replace('.', ',') : item);
                }
                viewModel.singleLead.bsd_diemdanhgia = tmp;
                //bsd_diemdanhgia_text.Text = viewModel.singleLead.bsd_diemdanhgia_format;
            }
            else
            {
                var itemsChecked = viewModel.list_leadrating[checkedId];

                List<String> lst_danhgiaid = viewModel.singleLead.getList(viewModel.singleLead.bsd_danhgiaid);
                List<String> lst_danhgiadiem = viewModel.singleLead.getList(viewModel.singleLead.bsd_danhgiadiem);

                lst_danhgiaid.Add(itemsChecked.bsd_leadsratingid);
                lst_danhgiadiem.Add(itemsChecked.bsd_point.ToString("G29", CultureInfo.CreateSpecificCulture("en-US")));

                lst_danhgiaid.RemoveAll(x => string.IsNullOrEmpty(x) && string.IsNullOrWhiteSpace(x));
                lst_danhgiadiem.RemoveAll(x => string.IsNullOrEmpty(x) && string.IsNullOrWhiteSpace(x));

                viewModel.singleLead.bsd_danhgiaid = string.Join(",", lst_danhgiaid);
                viewModel.singleLead.bsd_danhgiadiem = string.Join(",", lst_danhgiadiem);

                decimal tmp = 0;
                foreach (var item in lst_danhgiadiem)
                {
                    tmp += decimal.Parse(Device.RuntimePlatform == Device.Android ? item.Replace('.', ',') : item);
                }
                viewModel.singleLead.bsd_diemdanhgia = tmp;
                //bsd_diemdanhgia_text.Text = viewModel.singleLead.bsd_diemdanhgia_format;
            }
        }

        //////////////////// GENDER Picker
        private void New_gender_picker_SelectedIndexChanged(object sender, EventArgs e)
        {          
           // viewModel.singleLead.new_gender = viewModel.singleGender == null ? null : viewModel.singleGender.Val;
            if (viewModel.singleGender != null && viewModel.singleGender.Val != null)
            {
                viewModel.singleLead.new_gender = viewModel.singleGender.Val;
                viewModel.PhongThuy.gioi_tinh = Int32.Parse(viewModel.singleLead.new_gender);
                viewModel.PhongThuy.nam_sinh = viewModel.singleLead.new_birthday.HasValue ? viewModel.singleLead.new_birthday.Value.Year : 0;
            }
            else
            {
                viewModel.singleLead.new_gender = null;
                viewModel.PhongThuy.gioi_tinh = 0;
                viewModel.PhongThuy.nam_sinh = 0;
            }
        }

        ////////////////////////////// SEND FORM TO CRM
        private async void AddLead_Clicked(object sender, EventArgs e)
        {
            //viewModel.IsBusy = true;
            //var check = await checkData();
            //if (check == "Sucesses")
            //{
            //    var created = await viewModel.createLead(viewModel.singleLead);

            //    if (created != new Guid())
            //    {
            //        Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Đã tạo khách hàng tiềm năng thành công", "OK");
            //        Xamarin.Forms.Application.Current.Properties["update"] = "1";

            //        this.reload(created);
            //        //Xamarin.Forms.Application.Current.MainPage.Navigation.InsertPageBefore(new LeadForm(created), Xamarin.Forms.Application.Current.MainPage);
            //        //Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //    }
            //    else
            //    {
            //        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo khách hàng tiềm năng thất bại", "OK");
            //    }
            //}
            //else
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", check, "OK");
            //}

            //viewModel.IsBusy = false;
        }

        private async void UpdateLead_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            var check = await checkData();
            if (check == "Sucesses")
            {
                var updated = await viewModel.updateLead(viewModel.singleLead);

                if (updated)
                {
                    Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Đã cập nhật thành công", "OK");
                    Xamarin.Forms.Application.Current.Properties["update"] = "1";
                    viewModel.PageDuAnQuanTam = 1;
                    viewModel.PageNhuCauDiaDiem = 1;
                    this.reload(viewModel.singleLead.leadid);

                    //Xamarin.Forms.Application.Current.MainPage.Navigation.InsertPageBefore(new LeadForm(viewModel.singleLead.leadid), Xamarin.Forms.Application.Current.MainPage);
                    //Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Cập nhật thất bại", "OK");
                }
            }
            else
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", check, "OK");
            }

            viewModel.IsBusy = false;
        }

        private async Task<String> checkData()
        {
            if (viewModel.singleLead._bsd_topic_value == null || string.IsNullOrWhiteSpace(viewModel.singleLead.fullname) || string.IsNullOrWhiteSpace(viewModel.singleLead.mobilephone))
            {
                return "Vui lòng nhập các trường bắt buộc";
            }

            if(!PhoneNumberFormatVNHelper.CheckValidate(viewModel.singleLead.mobilephone))
            {
                return "Số điện thoại sai định dạng";
            }

            if (viewModel.singleLead.new_birthday != null && (DateTime.Now.Year - DateTime.Parse(viewModel.singleLead.new_birthday.ToString()).Year < 18))
            {
                return "Khách hàng phải từ 18 tuổi";
            }

            //Kiem tra trùng tên - số điện thoại, tên - email
            await viewModel.Checkdata_identical_lock(viewModel.singleLead.fullname, viewModel.singleLead.mobilephone, viewModel.singleLead.emailaddress1, viewModel.singleLead.leadid);
            if (viewModel.single_Leadcheck != null)
            {
                if (viewModel.singleLead.fullname.Trim() == viewModel.single_Leadcheck.fullname && viewModel.singleLead.mobilephone == viewModel.single_Leadcheck.mobilephone)
                {
                    return "Khách hàng - Số điện thoại đã tồn tại";
                }
                else if (viewModel.singleLead.fullname.Trim() == viewModel.single_Leadcheck.fullname && viewModel.singleLead.emailaddress1 == viewModel.single_Leadcheck.emailaddress1)
                {
                    return "Khách hàng - Email đã tồn tại";
                }
            }
            return "Sucesses";
        }

        private void MyNewDatePicker_DateChanged(object sender, EventArgs e)
        {
            if (viewModel.singleLead.new_birthday != null && (DateTime.Now.Year - DateTime.Parse(viewModel.singleLead.new_birthday.ToString()).Year < 18))
            {
                Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Lỗi", "Khách hàng phải từ 18 tuổi", "OK");
                viewModel.singleLead.new_birthday = null;
            }
            viewModel.PhongThuy.gioi_tinh = viewModel.singleLead.new_gender != null ? Int32.Parse(viewModel.singleLead.new_gender) : 0;
            viewModel.PhongThuy.nam_sinh = viewModel.singleLead.new_birthday.HasValue ? viewModel.singleLead.new_birthday.Value.Year : 0;
        }

        private async void MaQuocGiaCaNhan_Tapped(object sender, EventArgs e)
        {
            IsMaQuocGiaCaNhan = true;
            await LookUpModalMaQuocGia.Show();
        }

        private async void MaQuocGiaCty_Tapped(object sender, EventArgs e)
        {
            IsMaQuocGiaCaNhan = false;
            await LookUpModalMaQuocGia.Show();
        }

        private async void listMaQuocGia_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = e.Item as MaQuocGia;
            if (IsMaQuocGiaCaNhan)
            {
                viewModel.MaQuocGiaCaNhan = item;
            }
            else
            {
                viewModel.MaQuocGiaCty = item;
            }
            await LookUpModalMaQuocGia.Hide();
        }

        private async void Address_Tapped(object sender, EventArgs e)
        {
            await centerModalAddress.Show();
        }

        private async void CloseAddress_Clicked(object sender, EventArgs e)
        {
            await centerModalAddress.Hide();
        }

        private async void ConfirmAddress_Clicked(object sender, EventArgs e)
        {
            List<string> address = new List<string>();
            if (!string.IsNullOrWhiteSpace(viewModel.AddressLine1))
            {
                viewModel.singleLead.address1_line1 = viewModel.AddressLine1;
                address.Add(viewModel.AddressLine1);
            }
            else
            {
                viewModel.singleLead.address1_line1 = null;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.AddressLine2))
            {
                viewModel.singleLead.address1_line2 = viewModel.AddressLine2;
                address.Add(viewModel.AddressLine2);
            }
            else
            {
                viewModel.singleLead.address1_line2 = null;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.AddressLine3))
            {
                viewModel.singleLead.address1_line3 = viewModel.AddressLine3;
                address.Add(viewModel.AddressLine3);
            }
            else
            {
                viewModel.singleLead.address1_line3 = null;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.AddressCity))
            {
                viewModel.singleLead.address1_city = viewModel.AddressCity;
                address.Add(viewModel.AddressCity);
            }
            else
            {
                viewModel.singleLead.address1_city = null;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.AddressStateProvince))
            {
                viewModel.singleLead.address1_stateorprovince = viewModel.AddressStateProvince;
                address.Add(viewModel.AddressStateProvince);
            }
            else
            {
                viewModel.singleLead.address1_stateorprovince = null;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.AddressPostalCode))
            {
                viewModel.singleLead.address1_postalcode = viewModel.AddressPostalCode;
                address.Add(viewModel.AddressPostalCode);
            }
            else
            {
                viewModel.singleLead.address1_postalcode = null;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.AddressCountry))
            {
                viewModel.singleLead.address1_country = viewModel.AddressCountry;
                address.Add(viewModel.AddressCountry);
            }
            else
            {
                viewModel.singleLead.address1_country = null;
            }
            viewModel.singleLead.address1_composite = viewModel.AddressComposite = string.Join(", ", address);
            await centerModalAddress.Hide();
        }

        private async void SaveLead_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleLead.bsd_topic_label))
            {
                await DisplayAlert("", "Vui lòng nhập tiêu đề", "Đóng");
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.singleLead.fullname))
            {
                await DisplayAlert("", "Vui lòng nhập họ tên", "Đóng");
                return;
            }

            if (string.IsNullOrWhiteSpace(mobilephone_text.Text))
            {
                await DisplayAlert("", "Vui lòng nhập số điện thoại", "Đóng");
                return;
            }

            if (!PhoneNumberFormatVNHelper.CheckValidate(mobilephone_text.Text))
            {
                await DisplayAlert("", "Số điện thoại sai định dạng", "Đóng");
                return ;
            }

            LoadingHelper.Show();
            viewModel.singleLead.industrycode = viewModel.IndustryCode != null ? viewModel.IndustryCode.Val : null;
            viewModel.singleLead._transactioncurrencyid_value = viewModel.SelectedCurrency != null ? viewModel.SelectedCurrency.Val : null;
            viewModel.singleLead._campaignid_value = viewModel.Campaign != null ? viewModel.Campaign.Val : null;

            viewModel.singleLead.mobilephone = viewModel.MaQuocGiaCaNhan.Value + mobilephone_text.Text;
            viewModel.singleLead.telephone1 = !string.IsNullOrWhiteSpace(telephone1_text.Text) ? viewModel.MaQuocGiaCty.Value + telephone1_text.Text : null;

            var result = await viewModel.createLead();
            if (result.IsSuccess)
            {
                LoadingHelper.Hide();
                await Navigation.PopAsync();
                await DisplayAlert("", "Thành công", "Đóng");
            }
            else
            {
                LoadingHelper.Hide();
                await DisplayAlert("Lỗi", "Vui lòng thử lại", "Đóng");
            }
        }
    }
}