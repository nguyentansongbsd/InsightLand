using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Controls;
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
        LeadFormViewModel viewModel;
        //ProjectViewModel viewModelProject;

        List<MyNewCheckBox> checkBoxes;

        List<MyNewCheckBox> multiselectCheckBoxes;
        List<ImageButton> multiselectClearimageButton;

        private bool isShowingPopup;

        public ListView listViewCountry { get; set; }
        public ListView listViewProvince { get; set; }
        public ListView listViewDistrict { get; set; }

        public LeadForm()
        {
            InitializeComponent();
            this.Title = "Tạo mới khách hàng tiềm năng";
            this.BindingContext = viewModel = new LeadFormViewModel();
            this.constructor();
            this.loadData(null);
        }
        public LeadForm(Guid Id)
        {
            InitializeComponent();
            this.Title = "Cập nhật thông tin khách hàng tiềm năng";
            this.BindingContext = viewModel = new LeadFormViewModel();
            this.constructor();
            Init(Id);
        }

        public async void Init(Guid Id)
        {
            await loadData(Id.ToString());
            if (viewModel.singleLead != null)
                CheckSingleLead(true);
            else
                CheckSingleLead(false);
        }

        public void constructor()
        {
            isShowingPopup = false;

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

                if (viewModel.singleLead.new_gender != null) { await viewModel.loadOneGender(viewModel.singleLead.new_gender); }
                if (viewModel.singleLead.industrycode != null) { await viewModel.loadOneIndustrycode(viewModel.singleLead.industrycode); }
                //if (leadid != null) { await viewModelProject.LoadProjectsForLeadForm(); }

                if (viewModel.list_nhucauvediadiem.Count < 3)
                {
                    for (int i = viewModel.list_nhucauvediadiem.Count; i < 3; i++)
                    {
                        viewModel.list_nhucauvediadiem.Add(new Provinces());
                    }
                }

                if (viewModel.list_Duanquantam.Count < 3)
                {
                    for (int i = viewModel.list_Duanquantam.Count; i < 3; i++)
                    {
                        viewModel.list_Duanquantam.Add(new ProjectList());
                    }
                }
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
                btn_save_lead.Text = "Tạo mới";
                btn_save_lead.Clicked += AddLead_Clicked;

                label_nhu_cau_ve_dia_diem.IsVisible = false;
                stacklayout_nhucauvediadiem.IsVisible = false;
                label_du_an_quan_tam.IsVisible = false;
                //block_danh_sach_du_an_quan_tam.IsVisible = false;
            }
            else
            {
                viewModel.singleLead.showQualifyButton = viewModel.singleLead.bsd_diemdanhgia >= viewModel.Dudiemdanhgia.bsd_totalleadsratingpoint ? GridLength.Star : 0;
                btn_save_lead.Text = "Cập nhật";
                btn_save_lead.Clicked += UpdateLead_Clicked;
                btn_qualify_lead.Text = "Qualify";



                label_nhu_cau_ve_dia_diem.IsVisible = true;
                stacklayout_nhucauvediadiem.IsVisible = true;
                label_du_an_quan_tam.IsVisible = true;
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

            grid_leadsrating.Children.Clear();
            this.loadData(Id.ToString());
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////</summary>

        /////////////////////  LEADRATING                                         /////


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
                };
                tmp.changeChecked += Tmp_IsCheckedChanged;
                checkBoxes.Add(tmp);

                grid_leadsrating.Children.Add(tmp, 0, i);
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
                };
                tmp.changeChecked += Tmp_IsCheckedChanged;
                checkBoxes.Add(tmp);

                grid_leadsrating.Children.Add(tmp, 1, i - viewModel.list_leadrating.Count / 2);
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
                bsd_diemdanhgia_text.Text = viewModel.singleLead.bsd_diemdanhgia_format;
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
                bsd_diemdanhgia_text.Text = viewModel.singleLead.bsd_diemdanhgia_format;
            }
        }

        //////////////////// GENDER Picker
        private void New_gender_picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewModel.singleLead.new_gender = viewModel.singleGender == null ? null : viewModel.singleGender.Val;
            if (viewModel.singleGender.Val != null)
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

        ////////////////// INDUSTRYID Picker
        private void Industrycode_picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewModel.singleLead.industrycode = viewModel.singleIndustrycode == null ? null : viewModel.singleIndustrycode.Val;
        }

        //////////////////// FULLNAME Popup
        private void save_fullname(object sender, EventArgs e)
        {
            viewModel.singleLead.firstname = popupfullname_firstname.Text;
            viewModel.singleLead.lastname = popupfullname_lastname.Text;
            viewModel.singleLead.fullname = string.IsNullOrWhiteSpace(viewModel.singleLead.firstname) ? "" : (viewModel.singleLead.firstname + " ") + viewModel.singleLead.lastname;
            this.hide_popup_fullname(null, null);
        }

        private void hide_popup_fullname(object sender, EventArgs e)
        {
            isShowingPopup = false;

            popup_fullname.IsVisible = false;
        }

        private void show_popup_fullname(object sender, EventArgs e)
        {
            isShowingPopup = true;

            popupfullname_firstname.Text = viewModel.singleLead.firstname;
            popupfullname_lastname.Text = viewModel.singleLead.lastname;
            popup_fullname.IsVisible = true;
        }

        /////////////////// ADRESSS1_COMPOSITE Popup
        private void save_address1_composite(object sender, EventArgs e)
        {
            viewModel.singleLead.address1_line1 = popupaddress1composite_address1_line1.Text;
            viewModel.singleLead.address1_line2 = popupaddress1composite_address1_line2.Text;
            viewModel.singleLead.address1_line3 = popupaddress1composite_address1_line3.Text;
            viewModel.singleLead.address1_city = popupaddress1composite_address1_city.Text;
            viewModel.singleLead.address1_stateorprovince = popupaddress1composite_address1_stateorprovince.Text;
            viewModel.singleLead.address1_country = popupaddress1composite_address1_country.Text;
            viewModel.singleLead.address1_postalcode = popupaddress1composite_address1_postalcode.Text;

            viewModel.singleLead.address1_composite = (string.IsNullOrWhiteSpace(viewModel.singleLead.address1_line1) ? "" : (viewModel.singleLead.address1_line1.Trim() + "\r\n"))
                + (string.IsNullOrWhiteSpace(viewModel.singleLead.address1_line2) ? "" : (viewModel.singleLead.address1_line2.Trim() + "\r\n"))
                + (string.IsNullOrWhiteSpace(viewModel.singleLead.address1_line3) ? "" : (viewModel.singleLead.address1_line3.Trim() + "\r\n"))
                + (string.IsNullOrWhiteSpace(viewModel.singleLead.address1_city) ? "" : (viewModel.singleLead.address1_city.Trim() + ", "))
                + (string.IsNullOrWhiteSpace(viewModel.singleLead.address1_stateorprovince) ? "" : (viewModel.singleLead.address1_stateorprovince.Trim() + " "))
                + (string.IsNullOrWhiteSpace(viewModel.singleLead.address1_postalcode)
                    ? ((string.IsNullOrWhiteSpace(viewModel.singleLead.address1_stateorprovince) && string.IsNullOrWhiteSpace(viewModel.singleLead.address1_city)) ? "" : "\r\n")
                    : (viewModel.singleLead.address1_postalcode.Trim() + "\r\n"))
                + (viewModel.singleLead.address1_country);

            this.hide_popup_address1_composite(null, null);
        }

        private void hide_popup_address1_composite(object sender, EventArgs e)
        {
            isShowingPopup = false;
            popup_address1_composite.IsVisible = false;
        }

        private void show_popup_address1_composite(object sender, EventArgs e)
        {
            isShowingPopup = true;

            popupaddress1composite_address1_line1.Text = viewModel.singleLead.address1_line1;
            popupaddress1composite_address1_line2.Text = viewModel.singleLead.address1_line2;
            popupaddress1composite_address1_line3.Text = viewModel.singleLead.address1_line3;
            popupaddress1composite_address1_country.Text = viewModel.singleLead.address1_country;
            popupaddress1composite_address1_stateorprovince.Text = viewModel.singleLead.address1_stateorprovince;
            popupaddress1composite_address1_postalcode.Text = viewModel.singleLead.address1_postalcode;
            popupaddress1composite_address1_city.Text = viewModel.singleLead.address1_city;
            popup_address1_composite.IsVisible = true;
        }

        /////////////////// ADDNhuCauDiaDiem ListView Popup
        private async void BtnAddNhuCauDiaDiem_Clicked(object sender, EventArgs e)
        {
            isShowingPopup = true;

            viewModel.IsBusy = true;

            if (viewModel.list_provinces_lookup.Count == 0)
            {
                await viewModel.LoadAllProvinces();
            }

            listviewProvinces.SetBinding(ListView.ItemsSourceProperty, new Binding("list_provinces_lookup", source: viewModel));
            popup_province.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private async void popupprovinces_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Provinces selected = e.Item as Provinces;
            if (viewModel.list_nhucauvediadiem.ToList().FirstOrDefault(x => x.new_provinceid == selected.new_provinceid) == null)
            {
                this.hide_popup_province(null, null);
                viewModel.IsBusy = true;
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
                viewModel.IsBusy = false;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Địa điểm đã tồn tại", "OK");
            }
        }

        private async void BtnRemoveNhuCauDiaDiem_Clicked(object sender, EventArgs e)
        {
            var tmp = datagrid_nhucavediadiem.SelectedItem as Provinces;
            if (tmp != null)
            {
                if (tmp.new_provinceid != null)
                {
                    bool x = await App.Current.MainPage.DisplayAlert("", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                    if (x)
                    {
                        viewModel.IsBusy = true;
                        await viewModel.Delete_NhuCauDiaDiem(tmp.new_provinceid, viewModel.singleLead.leadid);
                        viewModel.list_nhucauvediadiem.Remove(tmp);
                        if (viewModel.list_nhucauvediadiem.Count < 3)
                        {
                            viewModel.list_nhucauvediadiem.Add(new Provinces());
                        }
                        viewModel.IsBusy = false;
                    }
                }
            }

        }

        private void hide_popup_province(object sender, EventArgs e)
        {
            isShowingPopup = false;

            popup_province.IsVisible = false;
        }

        private void SearchBarProvince_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
            {
                listviewProvinces.ItemsSource = viewModel.list_provinces_lookup;
                return;
            }
            listviewProvinces.ItemsSource = viewModel.list_provinces_lookup.Where(x => x.new_name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        ///////////////////// ADDDanhSachDuAnQuanTam Popup
        //private void BtnAddDanhSachDuAnQuanTam_Clicked(object sender, EventArgs e)
        //{
        //    App.Current.MainPage.Navigation.PushAsync(new ContentPageScreenCreateProject());
        //}

        ///////////////////// Clear Picker and EntryPopup value (set to null)
        private void Clear_new_gender_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleGender = null;
            viewModel.PhongThuy.gioi_tinh = 0;
            viewModel.PhongThuy.nam_sinh = viewModel.singleLead.new_birthday.HasValue ? viewModel.singleLead.new_birthday.Value.Year : 0;
        }

        private void Clear_new_birthday_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleLead.new_birthday = null;
        }

        private void Clear_industrycode_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleIndustrycode = null;
        }

        private void Clear_transactioncurrencyid_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleLead._transactioncurrencyid_value = null;
            viewModel.singleLead.transactioncurrencyid_label = null;
        }

        private void Clear_campaign_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleLead._campaignid_value = null;
            viewModel.singleLead.campaignid_label = null;
        }

        //////////////////// Hide PopoupList
        private void hide_listview_popup(object sender, EventArgs e)
        {
            isShowingPopup = false;

            popup_list_view.IsVisible = false;
        }
        private void SearchBarListView_TextChanged(object sender, TextChangedEventArgs e)
        {
            ///listviewProvinces.ItemsSource = viewModelProvinces.Items.Where(x => x.new_name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
            if (source_listviewpopup.Children.FirstOrDefault() != null)
            {
                if (e.NewTextValue == null)
                {
                    (source_listviewpopup.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup;
                    return;
                }
                (source_listviewpopup.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup.Where(x => x.Name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }


        /////////////////// TOPIC ListView Popup
        private async void show_popup_topic(object sender, EventArgs e)
        {
            isShowingPopup = true;

            viewModel.IsBusy = true;
            source_listviewpopup.Children.Clear();
            title_popuplistview.Text = "Toipc - Tiêu đề";

            ListView tmp = new ListView();

            //// LoadData Topic if Empty
            if (viewModel.list_topic_lookup.Count == 0)
            {
                await viewModel.LoadTopicsForLookup();
            }
            viewModel.list_lookup = viewModel.list_topic_lookup;

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = null;
            tmp.ItemTemplate = new DataTemplate(() =>
            {
                // Create views with bindings for displaying each property.
                Label nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, "Name");
                // Return an assembled ViewCell.
                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Children = { nameLabel }
                    }
                };
            });
            tmp.ItemTapped += popuptopic_ItemTapped;

            source_listviewpopup.Children.Add(tmp);

            ////show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private void popuptopic_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LookUp tmp = e.Item as LookUp;
            viewModel.singleLead._bsd_topic_value = tmp.Id.ToString();
            viewModel.singleLead.bsd_topic_label = tmp.Name;

            this.hide_listview_popup(null, null);
        }

        ///////////////////////////TRANSACTIONCURRENCY ListView Popup
        private async void show_popup_transactioncurrencyid(object sender, EventArgs e)
        {
            isShowingPopup = true;

            viewModel.IsBusy = true;
            source_listviewpopup.Children.Clear();
            title_popuplistview.Text = "Transaction Currency";

            ListView tmp = new ListView();

            //// LoadData Topic if Empty
            if (viewModel.list_currency_lookup.Count == 0)
            {
                await viewModel.LoadCurrenciesForLookup();
            }
            viewModel.list_lookup = viewModel.list_currency_lookup;

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = new DataTemplate(() =>
            {
                // Create views with bindings for displaying each property.
                Label nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, "Name");
                // Return an assembled ViewCell.
                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Children = { nameLabel }
                    }
                };
            });
            tmp.ItemTapped += popuptransctioncurrency_ItemTapped;
            source_listviewpopup.Children.Add(tmp);

            ////show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private void popuptransctioncurrency_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LookUp tmp = e.Item as LookUp;
            viewModel.singleLead._transactioncurrencyid_value = tmp.Id.ToString();
            viewModel.singleLead.transactioncurrencyid_label = tmp.Name;

            this.hide_listview_popup(null, null);
        }


        /////////////////////////////////// CAMPAIGNS ListView Popup
        private async void show_popup_campaignid(object sender, EventArgs e)
        {
            isShowingPopup = true;

            viewModel.IsBusy = true;
            source_listviewpopup.Children.Clear();
            title_popuplistview.Text = "Campaign";

            ListView tmp = new ListView();

            //// LoadData Topic if Empty
            if (viewModel.list_campaign_lookup.Count == 0)
            {
                await viewModel.LoadCampainsForLookup();
            }
            viewModel.list_lookup = viewModel.list_campaign_lookup;

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = new DataTemplate(() =>
            {
                // Create views with bindings for displaying each property.
                Label nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, "Name");
                // Return an assembled ViewCell.
                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Children = { nameLabel }
                    }
                };
            });
            tmp.ItemTapped += popupcampaignid_ItemTapped;
            source_listviewpopup.Children.Add(tmp);

            ////show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private void popupcampaignid_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LookUp tmp = e.Item as LookUp;
            viewModel.singleLead._campaignid_value = tmp.Id.ToString();
            viewModel.singleLead.campaignid_label = tmp.Name;

            this.hide_listview_popup(null, null);
        }

        ////////////////////////////// SEND FORM TO CRM
        private async void AddLead_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            var check = await checkData();
            if (check == "Sucesses")
            {
                var created = await viewModel.createLead(viewModel.singleLead);

                if (created != new Guid())
                {
                    Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Đã tạo khách hàng tiềm năng thành công", "OK");
                    Xamarin.Forms.Application.Current.Properties["update"] = "1";

                    this.reload(created);
                    //Xamarin.Forms.Application.Current.MainPage.Navigation.InsertPageBefore(new LeadForm(created), Xamarin.Forms.Application.Current.MainPage);
                    //Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo khách hàng tiềm năng thất bại", "OK");
                }
            }
            else
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", check, "OK");
            }

            viewModel.IsBusy = false;
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
            if (viewModel.singleLead._bsd_topic_value == null || viewModel.singleLead.fullname == null || viewModel.singleLead.mobilephone == null)
            {
                return "Vui lòng nhập các trường bắt buộc";
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

        private async void Qualifylead_Clicked(object sender, System.EventArgs e)
        {
            viewModel.IsBusy = true;
            //Buộc nhập CMND trước khi qualityLead
            if (viewModel.singleLead.bsd_identitycardnumber != null && await viewModel.Check_Quatify(viewModel.singleLead.bsd_identitycardnumber))
            {

                await viewModel.Qualify(viewModel.singleLead.leadid);

                Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Đã Qualify thành công", "OK");
                Xamarin.Forms.Application.Current.Properties["update"] = "1";

                this.reload(viewModel.singleLead.leadid);
            }
            else if (viewModel.singleLead.bsd_identitycardnumber == null)
            {
                Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "CMND bắt buộc nhập", "OK");

            }

            viewModel.IsBusy = false;
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

        protected override bool OnBackButtonPressed()
        {
            if (this.isShowingPopup)
            {
                this.hide_popup_fullname(null, null);
                this.hide_listview_popup(null, null);
                this.hide_popup_province(null, null);
                this.hide_popup_address1_composite(null, null);
                this.hide_popup_project(null, null);
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private async void BtnRemoveDuanquantam_Clicked(object sender, EventArgs e)
        {
            var tmp = datagrid_duanquantam.SelectedItem as ProjectList;

            if (tmp != null)
            {
                if (tmp.bsd_projectid != null)
                {
                    bool x = await App.Current.MainPage.DisplayAlert("", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                    if (x)
                    {
                        viewModel.IsBusy = true;
                        await viewModel.Delete_DuAnQuanTam(tmp.bsd_projectid, viewModel.singleLead.leadid);
                        viewModel.list_Duanquantam.Remove(tmp);
                        if (viewModel.list_Duanquantam.Count < 3)
                        {
                            viewModel.list_Duanquantam.Add(new ProjectList());
                        }
                        viewModel.IsBusy = false;
                    }
                }
            }
        }

        private async void BtnAddDuanquantam_Clicked(object sender, EventArgs e)
        {
            isShowingPopup = true;

            viewModel.IsBusy = true;

            if (viewModel.list_project_lookup.Count == 0)
            {
                await viewModel.LoadAllProject();
            }

            listviewProject.SetBinding(ListView.ItemsSourceProperty, new Binding("list_project_lookup", source: viewModel));
            popup_project.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private async void project_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ProjectList selected = e.Item as ProjectList;
            if (viewModel.list_Duanquantam.ToList().FirstOrDefault(x => x.bsd_projectid == selected.bsd_projectid) == null)
            {
                this.hide_popup_project(null, null);
                viewModel.IsBusy = true;
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
                viewModel.IsBusy = false;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Dự án đã tồn tại", "OK");
            }
        }

        private void hide_popup_project(object sender, EventArgs e)
        {
            isShowingPopup = false;

            popup_project.IsVisible = false;
        }

        private void SearchBarProject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
            {
                listviewProject.ItemsSource = viewModel.list_project_lookup;
                return;
            }
            listviewProject.ItemsSource = viewModel.list_project_lookup.Where(x => x.bsd_name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void DiaChiVN_Clicked(object sender, EventArgs e)
        {
            popup_contact_address.IsVisible = true;
        }

        private async void show_popup_listview_country(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            title_popuplistview.Text = "Quốc gia";
            if (viewModel.list_country_lookup.Count == 0)
            {
                await viewModel.LoadCountryForLookup();
            }
            source_listviewpopup.Children.Clear();
            if (listViewCountry == null)
            {
                listViewCountry = new ListView() { HasUnevenRows = true, SelectionMode = ListViewSelectionMode.None };
                listViewCountry.SetBinding(ListView.ItemsSourceProperty, new Binding("list_country_lookup", source: viewModel));

                listViewCountry.ItemTemplate = new DataTemplate(() =>
                {
                    Label nameLabel = new Label() { FontSize = 15, TextColor = Color.FromHex("#444444") };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(10),
                            VerticalOptions = LayoutOptions.Center,
                            Children = { nameLabel }
                        }
                    };
                });

                listViewCountry.ItemTapped += popupCountry_ItemTapped;
                listViewCountry.ItemAppearing += loadMoreCountryLookup;
            }
            source_listviewpopup.Children.Add(listViewCountry);
            popup_list_view.IsVisible = true;
            viewModel.IsBusy = false;
        }

        private void popupCountry_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as LookUp;

            this.clear_entry_province(null, null);
            this.clear_entry_district(null, null);

            viewModel.Country = selected.Name;
            viewModel.CountryId = selected.Id.ToString();
            viewModel.CountryEn = selected.Detail;
            popupcontactaddress_country.HasClearButton = true;

            this.hide_listview_popup(null, null);
        }

        private async void loadMoreCountryLookup(object sender, ItemVisibilityEventArgs e)
        {
            viewModel.IsBusy = true;
            if ((LookUp)e.Item == viewModel.list_country_lookup[viewModel.list_country_lookup.Count - 1])
            {
                viewModel.pageLookup_country++;
                await viewModel.LoadCountryForLookup();
            }
            viewModel.IsBusy = false;
        }

        private void clear_entry_country(object sender, EventArgs e)
        {
            viewModel.Country = null;
            viewModel.CountryId = null;
            viewModel.CountryEn = null;
            popupcontactaddress_country.HasClearButton = false;
            viewModel.list_province_lookup.Clear();
            viewModel.list_district_lookup.Clear();
            this.clear_entry_province(null, null);
            this.clear_entry_district(null, null);
        }

        private async void show_popup_listview_province(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            if (viewModel.Country == null)
            {
                await DisplayAlert("", "Vui lòng chọn quốc gia", "Ok");
                viewModel.IsBusy = false;
                return;
            }

            title_popuplistview.Text = "Tỉnh/Thành";
            if (viewModel.list_province_lookup.Count == 0)
            {
                await viewModel.loadProvincesForLookup(viewModel.CountryId);
            }
            source_listviewpopup.Children.Clear();
            if (listViewProvince == null)
            {
                listViewProvince = new ListView() { HasUnevenRows = true, SelectionMode = ListViewSelectionMode.None };
                listViewProvince.SetBinding(ListView.ItemsSourceProperty, new Binding("list_province_lookup", source: viewModel));

                listViewProvince.ItemTemplate = new DataTemplate(() =>
                {
                    Label nameLabel = new Label() { FontSize = 15, TextColor = Color.FromHex("#444444") };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(10),
                            VerticalOptions = LayoutOptions.Center,
                            Children = { nameLabel }
                        }
                    };
                });

                listViewProvince.ItemTapped += popupProvince_ItemTapped;
                listViewProvince.ItemAppearing += loadMoreProvinceLookup;
            }
            source_listviewpopup.Children.Add(listViewProvince);
            popup_list_view.IsVisible = true;
            viewModel.IsBusy = false;
        }

        private void popupProvince_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as LookUp;

            this.clear_entry_district(null, null);
            viewModel.list_district_lookup.Clear();

            viewModel.Province = selected.Name;
            viewModel.ProvinceId = selected.Id.ToString();
            viewModel.ProvinceEn = selected.Detail;
            popupcontactaddress_province.HasClearButton = true;

            this.hide_listview_popup(null, null);
        }

        private async void loadMoreProvinceLookup(object sender, ItemVisibilityEventArgs e)
        {
            viewModel.IsBusy = true;
            if ( (LookUp)e.Item == viewModel.list_province_lookup[viewModel.list_province_lookup.Count -1])
            {
                viewModel.pageLookup_province++;
                await viewModel.loadProvincesForLookup(viewModel.CountryId);
            }
            viewModel.IsBusy = false;
        }

        private void clear_entry_province(object sender, EventArgs e)
        {
            viewModel.Province = null;
            viewModel.ProvinceId = null;
            viewModel.ProvinceEn = null;
            popupcontactaddress_province.HasClearButton = false;
            viewModel.pageLookup_province = 1;
            this.clear_entry_district(null, null);
            viewModel.list_district_lookup.Clear();
        }

        private async void show_popup_listview_district(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            if (viewModel.Province == null)
            {
                await DisplayAlert("", "Vui lòng chọn Tỉnh/Thành", "Ok");
                viewModel.IsBusy = false;
                return;
            }

            title_popuplistview.Text = "Quận/Huyện";
            if (viewModel.list_district_lookup.Count == 0)
            {
                await viewModel.loadDistrictForLookup(viewModel.ProvinceId);
            }

            source_listviewpopup.Children.Clear();
            if (listViewDistrict == null)
            {
                listViewDistrict = new ListView() { HasUnevenRows = true, SelectionMode = ListViewSelectionMode.None };
                listViewDistrict.SetBinding(ListView.ItemsSourceProperty, new Binding("list_district_lookup", source: viewModel));

                listViewDistrict.ItemTemplate = new DataTemplate(() =>
                {
                    Label nameLabel = new Label() { FontSize = 15, TextColor = Color.FromHex("#444444") };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(10),
                            VerticalOptions = LayoutOptions.Center,
                            Children = { nameLabel }
                        }
                    };
                });

                listViewDistrict.ItemTapped += popupDistrict_ItemTapped;
                listViewDistrict.ItemAppearing += loadMoreDistrictLookup;
            }
            source_listviewpopup.Children.Add(listViewDistrict);
            popup_list_view.IsVisible = true;
            viewModel.IsBusy = false;
        }

        private void popupDistrict_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as LookUp;

            viewModel.District = selected.Name;
            viewModel.DistrictId = selected.Id.ToString();
            viewModel.DistrictEn = selected.Detail;
            popupcontactaddress_district.HasClearButton = true;

            this.hide_listview_popup(null, null);
        }

        private async void loadMoreDistrictLookup(object sender, ItemVisibilityEventArgs e)
        {
            viewModel.IsBusy = true;
            if ((LookUp)e.Item == viewModel.list_district_lookup[viewModel.list_district_lookup.Count - 1])
            {
                viewModel.pageLookup_district++;
                await viewModel.loadDistrictForLookup(viewModel.ProvinceId);
            }
            viewModel.IsBusy = false;
        }

        private void clear_entry_district(object sender, EventArgs e)
        {
            viewModel.District = null;
            viewModel.DistrictId = null;
            viewModel.DistrictEn = null;
            popupcontactaddress_district.HasClearButton = false;
            viewModel.pageLookup_district = 1;
        }

        private void hide_popup_contactaddress(object sender, EventArgs e)
        {
            popup_contact_address.IsVisible = false;
        }

        private void btnConfirm_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            List<string> listAddressVn = new List<string>();
            if (viewModel.StreetVn != null) listAddressVn.Add(viewModel.StreetVn);
            if (viewModel.District != null) listAddressVn.Add(viewModel.District);
            if (viewModel.Province != null) listAddressVn.Add(viewModel.Province);
            if (viewModel.Country != null) listAddressVn.Add(viewModel.Country);

            List<string> listAddressEn = new List<string>();
            if (viewModel.StreetEn != null) listAddressEn.Add(viewModel.StreetEn);
            if (viewModel.DistrictEn != null) listAddressEn.Add(viewModel.DistrictEn);
            if (viewModel.ProvinceEn != null) listAddressEn.Add(viewModel.ProvinceEn);
            if (viewModel.CountryEn != null) listAddressEn.Add(viewModel.CountryEn);

            viewModel.AddressVn = viewModel.singleLead.bsd_contactaddress = string.Join(", ", listAddressVn);
            viewModel.AddressEn = viewModel.singleLead.bsd_diachi = string.Join(", ", listAddressEn);

            viewModel.singleLead.bsd_housenumber = viewModel.StreetEn;
            viewModel.singleLead.bsd_housenumberstreet = viewModel.StreetVn;

            viewModel.singleLead._bsd_country_value = viewModel.CountryId;
            viewModel.singleLead.bsd_country_label = viewModel.Country;
            viewModel.singleLead.bsd_country_en = viewModel.CountryEn;

            viewModel.singleLead._bsd_province_value = viewModel.ProvinceId;
            viewModel.singleLead.bsd_province_label = viewModel.Province;
            viewModel.singleLead.bsd_province_en = viewModel.ProvinceEn;

            viewModel.singleLead._bsd_district_value = viewModel.DistrictId;
            viewModel.singleLead.bsd_district_label = viewModel.District;
            viewModel.singleLead.bsd_district_en = viewModel.DistrictEn;

            popup_contact_address.IsVisible = false;
            viewModel.IsBusy = false;
        }
    }
}