using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class ContactDetailPageViewModel : FormViewModal
    {
        private ContactFormModel _singleContact;
        public ContactFormModel singleContact { get { return _singleContact; } set { _singleContact = value; OnPropertyChanged(nameof(singleContact)); } }

        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }
        public ObservableCollection<OptionSet> list_gender_optionset { get; set; }
        private string _singleContactgroup;
        public string SingleContactgroup { get => _singleContactgroup; set { _singleContactgroup = value; OnPropertyChanged(nameof(SingleContactgroup)); } }
        private string _singleType;
        public string SingleType { get => _singleType; set { _singleType = value; OnPropertyChanged(nameof(SingleType)); } }
        public ObservableCollection<Provinces> list_nhucauvediadiem { get; set; }
        public ObservableCollection<Provinces> list_provinces_lookup { get; set; }

        private ObservableCollection<ProjectList> _list_Duanquantam;
        public ObservableCollection<ProjectList> list_Duanquantam { get { return _list_Duanquantam; } set { _list_Duanquantam = value; OnPropertyChanged(nameof(_list_Duanquantam)); } }
        public ObservableCollection<ProjectList> list_project_lookup { set; get; }

        private ObservableCollection<TieuChi> _list_TieuChiChonMua;
        public ObservableCollection<TieuChi> list_TieuChiChonMua { get { return _list_TieuChiChonMua; } set { _list_TieuChiChonMua = value; OnPropertyChanged(nameof(list_TieuChiChonMua)); } }
        public ObservableCollection<TieuChi> list_tieuchichonmua_lookup { set; get; }

        private ObservableCollection<TieuChi> _list_NhuCauVeDienTichCanHo;
        public ObservableCollection<TieuChi> list_NhuCauVeDienTichCanHo { get { return _list_NhuCauVeDienTichCanHo; } set { _list_NhuCauVeDienTichCanHo = value; OnPropertyChanged(nameof(list_NhuCauVeDienTichCanHo)); } }
        public ObservableCollection<TieuChi> list_nhucauvedientichcanho_lookup { set; get; }

        private ObservableCollection<TieuChi> _list_LoaiBatDongSanQuanTam;
        public ObservableCollection<TieuChi> list_LoaiBatDongSanQuanTam { get { return _list_LoaiBatDongSanQuanTam; } set { _list_LoaiBatDongSanQuanTam = value; OnPropertyChanged(nameof(list_LoaiBatDongSanQuanTam)); } }
        public ObservableCollection<TieuChi> list_loaibatdongsanquantam_lookup { set; get; }

        public ObservableCollection<QueueListModel> list_danhsachdatcho { get; set; }
        private bool _showMoreDanhSachDatCho;
        public bool ShowMoreDanhSachDatCho { get => _showMoreDanhSachDatCho; set { _showMoreDanhSachDatCho = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCho)); } }
        public int PageDanhSachDatCho { get; set; } = 1;

        public ObservableCollection<QuotationReseravtion> list_danhsachdatcoc { get; set; }
        private bool _showMoreDanhSachDatCoc;
        public bool ShowMoreDanhSachDatCoc { get => _showMoreDanhSachDatCoc; set { _showMoreDanhSachDatCoc = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCoc)); } }
        public int PageDanhSachDatCoc { get; set; } = 1;

        public ObservableCollection<OptionEntry> list_danhsachhopdong { get; set; }
        private bool _showMoreDanhSachHopDong;
        public bool ShowMoreDanhSachHopDong { get => _showMoreDanhSachHopDong; set { _showMoreDanhSachHopDong = value; OnPropertyChanged(nameof(ShowMoreDanhSachHopDong)); } }
        public int PageDanhSachHopDong { get; set; } = 1;

        public ObservableCollection<Case> list_chamsockhachhang { get; set; }
        private bool _showMoreChamSocKhachHang;
        public bool ShowMoreChamSocKhachHang { get => _showMoreChamSocKhachHang; set { _showMoreChamSocKhachHang = value; OnPropertyChanged(nameof(ShowMoreChamSocKhachHang)); } }
        public int PageChamSocKhachHang { get; set; } = 1;
        private bool _optionEntryHasOnlyTerminatedStatus;
        public bool optionEntryHasOnlyTerminatedStatus { get => _optionEntryHasOnlyTerminatedStatus; set { _optionEntryHasOnlyTerminatedStatus = value; OnPropertyChanged(nameof(optionEntryHasOnlyTerminatedStatus)); } }

        private PhongThuyModel _PhongThuy;
        public PhongThuyModel PhongThuy { get => _PhongThuy; set { _PhongThuy = value; OnPropertyChanged(nameof(PhongThuy)); } }
        public ObservableCollection<HuongPhongThuy> list_HuongTot { set; get; }
        public ObservableCollection<HuongPhongThuy> list_HuongXau { set; get; }

        string frontImage_name;
        string behindImage_name;

        public ContactDetailPageViewModel()
        {
            singleGender = new OptionSet();
            list_gender_optionset = new ObservableCollection<OptionSet>();
            list_nhucauvediadiem = new ObservableCollection<Provinces>();
            list_provinces_lookup = new ObservableCollection<Provinces>();
            list_project_lookup = new ObservableCollection<ProjectList>();
            list_Duanquantam = new ObservableCollection<ProjectList>();            
            list_TieuChiChonMua = new ObservableCollection<TieuChi>();
            list_tieuchichonmua_lookup = new ObservableCollection<TieuChi>();
            list_LoaiBatDongSanQuanTam = new ObservableCollection<TieuChi>();
            list_loaibatdongsanquantam_lookup = new ObservableCollection<TieuChi>();
            list_NhuCauVeDienTichCanHo = new ObservableCollection<TieuChi>();
            list_nhucauvedientichcanho_lookup = new ObservableCollection<TieuChi>();
            list_danhsachdatcho = new ObservableCollection<QueueListModel>();
            list_danhsachdatcoc = new ObservableCollection<QuotationReseravtion>();
            list_danhsachhopdong = new ObservableCollection<OptionEntry>();
            list_chamsockhachhang = new ObservableCollection<Case>();
            list_HuongTot = new ObservableCollection<HuongPhongThuy>();
            list_HuongXau = new ObservableCollection<HuongPhongThuy>();            
            optionEntryHasOnlyTerminatedStatus = true;
            LoadGender();          
        }

        // load one contat
        public async Task loadOneContact(String id)
        {
            singleContact = new ContactFormModel();
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='contact'>
                                    <all-attributes />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='contactid' operator='eq' value='" + id + @"' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            }

            this.singleContact = tmp;
            //if (tmp.bsd_loingysinh == false)
            //{
            //    checkbirth = true;
            //    checkbirthy = false;
            //}
            //else { checkbirth = false; checkbirthy = true; }
            frontImage_name = tmp.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_front.jpg";
            behindImage_name = tmp.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_behind.jpg";
        }
        //Gender
        public void LoadGender()
        {
            list_gender_optionset.Add(new OptionSet() { Val = ("1"), Label = "Nam", });
            list_gender_optionset.Add(new OptionSet() { Val = ("2"), Label = "Nữ", });
            list_gender_optionset.Add(new OptionSet() { Val = ("100000000"), Label = "Khác", });
        }

        public OptionSet LoadOneGender(string id)
        {
            this.singleGender = list_gender_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleGender;
        }

        // Nhu cau ve dia diem
        public async Task Load_NhuCauVeDiaDiem(string contactid)
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='new_province'>
                                    <attribute name='new_name' />
                                    <attribute name='createdon' />
                                    <attribute name='new_id' />
                                    <attribute name='bsd_country' />
                                    <attribute name='bsd_priority' />
                                    <attribute name='bsd_provincename' />
                                    <attribute name='new_provinceid' />
                                    <order attribute='bsd_priority' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statecode' operator='eq' value='0' />
                                    </filter>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer'>
                                        <attribute name='bsd_name'  alias='bsd_countries'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Provinces>>("contacts(" + contactid + @")/bsd_contact_new_province", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            foreach (var x in result.value)
            {
                list_nhucauvediadiem.Add(x);
            }
        }
        public async Task<Boolean> Add_NhuCauDiaDiem(string id, Guid contactid)
        {
            string path = $"/contacts({contactid})/bsd_contact_new_province/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/new_provinces(" + id + ")";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                return false;
            }
        }
        public async Task<Boolean> Delete_NhuCauDiaDiem(string id, Guid contactid)
        {
            string Token = App.Current.Properties["Token"] as string;

            var request = $"{OrgConfig.ApiUrl}/contacts(" + contactid + ")/bsd_contact_new_province(" + id + ")/$ref";

            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            using (HttpClient Client = new HttpClient(ClientHandler))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                using (HttpRequestMessage RequestMessage = new HttpRequestMessage(new HttpMethod("DELETE"), request))
                {
                    using (HttpResponseMessage ResponseMessage = await Client.SendAsync(RequestMessage))
                    {
                        string result = await ResponseMessage.Content.ReadAsStringAsync();

                        if (ResponseMessage.StatusCode == HttpStatusCode.NoContent)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        public async Task LoadAllProvinces()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='new_province'>
                                    <attribute name='new_name' />
                                    <attribute name='createdon' />
                                    <attribute name='new_id' />
                                    <attribute name='bsd_country' />
                                    <attribute name='bsd_priority' />
                                    <attribute name='bsd_provincename' />
                                    <attribute name='new_provinceid' />
                                    <order attribute='bsd_priority' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statecode' operator='eq' value='0' />
                                    </filter>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer' alias='bsd_countries'>
                                        <attribute name='bsd_name' alias='bsd_countries' />
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Provinces>>("new_provinces", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            foreach (var x in result.value)
            {
                list_provinces_lookup.Add(x);
            }
        }

        // du an quan tam
        public async Task Load_DanhSachDuAn(string contactid)
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_project'>
                                <attribute name='bsd_name' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='bsd_landvalueofproject' />
                                <attribute name='bsd_esttopdate' />
                                <attribute name='bsd_acttopdate' />
                                <attribute name='bsd_projectid' />
                                <order attribute='bsd_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("contacts(" + contactid + @")/bsd_contact_bsd_project", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            } 
            foreach (var x in result.value)
            {
                list_Duanquantam.Add(x);
            }
        }
        public async Task<Boolean> Add_DuAnQuanTam(string id, Guid Contactid)
        {
            string path = $"/contacts({Contactid})/bsd_contact_bsd_project/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_projects(" + id + ")";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                return false;
            }
        }
        public async Task<Boolean> Delete_DuAnQuanTam(string id, Guid Contactid)
        {
            string Token = App.Current.Properties["Token"] as string;

            var request = $"{OrgConfig.ApiUrl}/contacts(" + Contactid + ")/bsd_contact_bsd_project(" + id + ")/$ref";

            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            using (HttpClient Client = new HttpClient(ClientHandler))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                using (HttpRequestMessage RequestMessage = new HttpRequestMessage(new HttpMethod("DELETE"), request))
                {
                    using (HttpResponseMessage ResponseMessage = await Client.SendAsync(RequestMessage))
                    {
                        string result = await ResponseMessage.Content.ReadAsStringAsync();

                        if (ResponseMessage.StatusCode == HttpStatusCode.NoContent)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        public async Task LoadAllProject()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_project'>
                                <attribute name='bsd_name' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='bsd_landvalueofproject' />
                                <attribute name='bsd_esttopdate' />
                                <attribute name='bsd_acttopdate' />
                                <attribute name='bsd_projectid' />
                                <order attribute='bsd_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("bsd_projects", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            foreach (var x in result.value)
            {
                list_project_lookup.Add(x);
            }
        }

        // tieu chi
        public async void UpdateTieuChi(int id, bool val)
        {
            if (id == 1)
                singleContact.bsd_tieuchi_vitri = val;
            if (id == 2)
                singleContact.bsd_tieuchi_phuongthucthanhtoan = val;
            if (id == 3)
                singleContact.bsd_tieuchi_giacanho = val;
            if (id == 4)
                singleContact.bsd_tieuchi_nhadautuuytin = val;
            if (id == 5)
                singleContact.bsd_tieuchi_moitruongsong = val;
            if (id == 6)
                singleContact.bsd_tieuchi_hethonganninh = val;
            if (id == 7)
                singleContact.bsd_tieuchi_baidauxe = val;
            if (id == 8)
                singleContact.bsd_tieuchi_huongcanho = val;
            if (id == 9)
                singleContact.bsd_tieuchi_hethongcuuhoa = val;
            if (id == 10)
                singleContact.bsd_tieuchi_nhieutienich = val;
            if (id == 11)
                singleContact.bsd_tieuchi_ganchosieuthi = val;
            if (id == 12)
                singleContact.bsd_tieuchi_gantruonghoc = val;
            if (id == 13)
                singleContact.bsd_tieuchi_ganbenhvien = val;
            if (id == 14)
                singleContact.bsd_tieuchi_dientichcanho = val;
            if (id == 15)
                singleContact.bsd_tieuchi_thietkenoithatcanho = val;
            if (id == 16)
                singleContact.bsd_tieuchi_tangdepcanhodep = val;
            if (id == 17)
                singleContact.bsd_dientich_3060m2 = val;
            if (id == 18)
                singleContact.bsd_dientich_6080m2 = val;
            if (id == 19)
                singleContact.bsd_dientich_80100m2 = val;
            if (id == 20)
                singleContact.bsd_dientich_100120m2 = val;
            if (id == 21)
                singleContact.bsd_dientich_lonhon120m2 = val;
            if (id == 22)
                singleContact.bsd_quantam_datnen = val;
            if (id == 23)
                singleContact.bsd_quantam_canho = val;
            if (id == 24)
                singleContact.bsd_quantam_bietthu = val;
            if (id == 25)
                singleContact.bsd_quantam_khuthuongmai = val;
            if (id == 26)
                singleContact.bsd_quantam_nhapho = val;
            await updateContact(singleContact);
        }
        public void LoadTieuChi()
        {
            
            if (singleContact.bsd_tieuchi_vitri == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 1, Name = "Tiêu chí - Vị trí" });
            if (singleContact.bsd_tieuchi_phuongthucthanhtoan == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 2, Name = "Tiêu chí - Phương thức thanh toán" });
            if (singleContact.bsd_tieuchi_giacanho == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 3, Name = "Tiêu chí - Giá căn hộ" });
            if (singleContact.bsd_tieuchi_nhadautuuytin == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 4, Name = "Tiêu chí - Nhà đầu tư uy tính" });
            if (singleContact.bsd_tieuchi_moitruongsong == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 5, Name = "Tiêu chí - Môi trường sống" });
            if (singleContact.bsd_tieuchi_hethonganninh == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 6, Name = "Tiêu chí - Hệ thống an ninh" });
            if (singleContact.bsd_tieuchi_baidauxe == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 7, Name = "Tiêu chí - Bãi đậu xe" });
            if (singleContact.bsd_tieuchi_huongcanho == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 8, Name = "Tiêu chí - Hướng căn hộ" });
            if (singleContact.bsd_tieuchi_hethongcuuhoa == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 9, Name = "Tiêu chí - Hệ thống cứu hoả" });
            if (singleContact.bsd_tieuchi_nhieutienich == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 10, Name = "Tiêu chí - Nhiều tiện ích" });
            if (singleContact.bsd_tieuchi_ganchosieuthi == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 11, Name = "Tiêu chí - Gần chợ - Siêu thị" });
            if (singleContact.bsd_tieuchi_gantruonghoc == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 12, Name = "Tiêu chí - Gần trường học" });
            if (singleContact.bsd_tieuchi_ganbenhvien == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 13, Name = "Tiêu chí - Gần bệnh viện" });
            if (singleContact.bsd_tieuchi_dientichcanho == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 14, Name = "Tiêu chí - Diện tích căn hộ" });
            if (singleContact.bsd_tieuchi_thietkenoithatcanho == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 15, Name = "Tiêu chí - Thiết kế nội thất căn hộ" });
            if (singleContact.bsd_tieuchi_tangdepcanhodep == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 16, Name = "Tiêu chí - Tầng/Căn hộ đẹp" });
            if (singleContact.bsd_dientich_3060m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 17, Name = "Diện tích 30 - 60 m2" });
            if (singleContact.bsd_dientich_6080m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 18, Name = "Diện tích 60 - 80 m2" });
            if (singleContact.bsd_dientich_80100m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 19, Name = "Diện tích 80 - 100 m2" });
            if (singleContact.bsd_dientich_100120m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 20, Name = "Diện tích 100 - 120 m2" });
            if (singleContact.bsd_dientich_lonhon120m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 21, Name = "Diện tích > 120 m2" });
            if (singleContact.bsd_quantam_datnen == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 22, Name = "Quan tâm - Đất nền" });
            if (singleContact.bsd_quantam_canho == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 23, Name = "Quan tâm - Căn hộ" });
            if (singleContact.bsd_quantam_bietthu == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 24, Name = "Quan tâm - Biệt thự" });
            if (singleContact.bsd_quantam_khuthuongmai == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 25, Name = "Quan tâm - Khu thương mại" });
            if (singleContact.bsd_quantam_nhapho == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 26, Name = "Quan tâm - Nhà phố" });
        }
        public void LoadAllTieuChiChonMua()
        {
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 1, Name = "Tiêu chí - Vị trí" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 2, Name = "Tiêu chí - Phương thức thanh toán" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 3, Name = "Tiêu chí - Giá căn hộ" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 4, Name = "Tiêu chí - Nhà đầu tư uy tính" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 5, Name = "Tiêu chí - Môi trường sống" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 6, Name = "Tiêu chí - Hệ thống an ninh" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 7, Name = "Tiêu chí - Bãi đậu xe" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 8, Name = "Tiêu chí - Hướng căn hộ" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 9, Name = "Tiêu chí - hệ thống cứu hoả" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 10, Name = "Tiêu chí - Nhiều tiện ích" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 11, Name = "Tiêu chí - Gần chợ - Siêu thị" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 12, Name = "Tiêu chí - Gần trường học" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 13, Name = "Tiêu chí - Gần bệnh viện" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 14, Name = "Tiêu chí - Diện tích căn hộ" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 15, Name = "Tiêu chí - Thiết kế nội thất căn hộ" });
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 16, Name = "Tiêu chí - Tầng/Căn hộ đẹp" });
        }

        public void LoadAllNhuCauVeDienTichCanHo()
        {
            list_nhucauvedientichcanho_lookup.Add(new TieuChi { Id = 17, Name = "Diện tích 30 - 60 m2" });
            list_nhucauvedientichcanho_lookup.Add(new TieuChi { Id = 18, Name = "Diện tích 60 - 80 m2" });
            list_nhucauvedientichcanho_lookup.Add(new TieuChi { Id = 19, Name = "Diện tích 80 - 100 m2" });
            list_nhucauvedientichcanho_lookup.Add(new TieuChi { Id = 20, Name = "Diện tích 100 - 120 m2" });
            list_nhucauvedientichcanho_lookup.Add(new TieuChi { Id = 21, Name = "Diện tích > 120 m2" });
        }

        public void LoadAllLoaiBatDongSanQuanTam()
        {
            list_loaibatdongsanquantam_lookup.Add(new TieuChi { Id = 22, Name = "Quan tâm - Đất nền" });
            list_loaibatdongsanquantam_lookup.Add(new TieuChi { Id = 23, Name = "Quan tâm - Căn hộ" });
            list_loaibatdongsanquantam_lookup.Add(new TieuChi { Id = 24, Name = "Quan tâm - Biệt thự" });
            list_loaibatdongsanquantam_lookup.Add(new TieuChi { Id = 25, Name = "Quan tâm - Khu thương mại" });
            list_loaibatdongsanquantam_lookup.Add(new TieuChi { Id = 26, Name = "Quan tâm - Nhà phố" });
        }

        public async Task<Boolean> updateContact(ContactFormModel contact)
        {
            string path = "/contacts(" + contact.contactid + ")";
            var content = await this.getContent(contact);
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", mess, "OK");
                return false;
            }
        }

        private async Task<object> getContent(ContactFormModel contact)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_loingysinh"] = contact.bsd_loingysinh;
            data["bsd_nmsinh"] = contact.bsd_nmsinh;
          //  contact.bsd_type = SingleType;
            data["bsd_lastname"] = contact.bsd_lastname;
            data["lastname"] = contact.lastname;
            data["bsd_fullname"] = contact.bsd_fullname;

            if (!string.IsNullOrEmpty(contact.bsd_firstname))
            {
                data["bsd_firstname"] = contact.bsd_firstname;
                data["firstname"] = contact.firstname;
            }

            data["emailaddress1"] = contact.emailaddress1;
            data["jobtitle"] = contact.jobtitle;
            data["birthdate"] = contact.birthdate.HasValue ? (DateTime.Parse(contact.birthdate.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;
            data["mobilephone"] = contact.mobilephone;
            data["gendercode"] = contact.gendercode;
            data["bsd_identitycardnumber"] = contact.bsd_identitycardnumber;
            data["contactid"] = contact.contactid;
            data["bsd_type"] = contact.bsd_type;
            data["bsd_localization"] = contact.bsd_localization;
            data["bsd_haveprotector"] = contact.bsd_haveprotector;
            data["bsd_dategrant"] = contact.bsd_dategrant.HasValue ? (DateTime.Parse(contact.bsd_dategrant.ToString()).ToLocalTime()).ToString("yyy-MM-dd") : null;  //null
            data["bsd_placeofissue"] = contact.bsd_placeofissue;
            data["bsd_passport"] = contact.bsd_passport;
            data["bsd_issuedonpassport"] = contact.bsd_issuedonpassport.HasValue ? (DateTime.Parse(contact.bsd_issuedonpassport.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;  //null
            data["bsd_placeofissuepassport"] = contact.bsd_placeofissuepassport;
            data["bsd_idcard"] = contact.bsd_idcard;
            data["bsd_issuedonpassport"] = contact.bsd_issuedateidcard.HasValue ? (DateTime.Parse(contact.bsd_issuedateidcard.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null; //null
            data["bsd_placeofissuepassport"] = contact.bsd_placeofissuepassport;
            data["bsd_jobtitlevn"] = contact.bsd_jobtitlevn;
            data["bsd_taxcode"] = contact.bsd_taxcode;  //null
            data["bsd_email2"] = contact.bsd_email2;  //null
            data["telephone1"] = contact.telephone1;
            data["fax"] = contact.fax; //null
            data["bsd_totaltransaction"] = contact.bsd_totaltransaction; //null
            data["bsd_customergroup"] = contact.bsd_customergroup;
            data["bsd_housenumberstreet"] = contact.bsd_housenumberstreet;
            data["bsd_housenumber"] = contact.bsd_housenumber; //null
            data["bsd_permanentaddress"] = contact.bsd_permanentaddress;
            data["bsd_permanenthousenumber"] = contact.bsd_permanenthousenumber; //null
            data["bsd_contactaddress"] = contact.bsd_contactaddress;
            data["bsd_diachi"] = contact.bsd_diachi;
            data["bsd_permanentaddress1"] = contact.bsd_permanentaddress1;
            data["bsd_diachithuongtru"] = contact.bsd_diachithuongtru;
            data["bsd_tieuchi_vitri"] = contact.bsd_tieuchi_vitri;
            data["bsd_tieuchi_phuongthucthanhtoan"] = contact.bsd_tieuchi_phuongthucthanhtoan;
            data["bsd_tieuchi_giacanho"] = contact.bsd_tieuchi_giacanho;
            data["bsd_tieuchi_nhadautuuytin"] = contact.bsd_tieuchi_nhadautuuytin;
            data["bsd_tieuchi_moitruongsong"] = contact.bsd_tieuchi_moitruongsong;
            data["bsd_tieuchi_baidauxe"] = contact.bsd_tieuchi_baidauxe;
            data["bsd_tieuchi_hethonganninh"] = contact.bsd_tieuchi_hethonganninh;
            data["bsd_tieuchi_huongcanho"] = contact.bsd_tieuchi_huongcanho;
            data["bsd_tieuchi_hethongcuuhoa"] = contact.bsd_tieuchi_hethongcuuhoa;
            data["bsd_tieuchi_nhieutienich"] = contact.bsd_tieuchi_nhieutienich;
            data["bsd_tieuchi_ganchosieuthi"] = contact.bsd_tieuchi_ganchosieuthi;
            data["bsd_tieuchi_gantruonghoc"] = contact.bsd_tieuchi_gantruonghoc;
            data["bsd_tieuchi_ganbenhvien"] = contact.bsd_tieuchi_ganbenhvien;
            data["bsd_tieuchi_dientichcanho"] = contact.bsd_tieuchi_dientichcanho;
            data["bsd_tieuchi_thietkenoithatcanho"] = contact.bsd_tieuchi_thietkenoithatcanho;
            data["bsd_tieuchi_tangdepcanhodep"] = contact.bsd_tieuchi_tangdepcanhodep;
            data["bsd_dientich_3060m2"] = contact.bsd_dientich_3060m2;
            data["bsd_dientich_6080m2"] = contact.bsd_dientich_6080m2;
            data["bsd_dientich_80100m2"] = contact.bsd_dientich_80100m2;
            data["bsd_dientich_100120m2"] = contact.bsd_dientich_100120m2;
            data["bsd_dientich_lonhon120m2"] = contact.bsd_dientich_lonhon120m2;
            data["bsd_quantam_datnen"] = contact.bsd_quantam_datnen;
            data["bsd_quantam_canho"] = contact.bsd_quantam_canho;
            data["bsd_quantam_bietthu"] = contact.bsd_quantam_bietthu;
            data["bsd_quantam_khuthuongmai"] = contact.bsd_quantam_khuthuongmai;
            data["bsd_quantam_nhapho"] = contact.bsd_quantam_nhapho;
            data["bsd_birthyear"] = contact.birthdate.HasValue ? contact.birthdate.Value.Year : 0;
            data["bsd_birthmonth"] = contact.birthdate.HasValue ? contact.birthdate.Value.Month : 0;
            data["bsd_birthdate"] = contact.birthdate.HasValue ? contact.birthdate.Value.Day : 0;
            //data["bsd_mattruoccmnd"] = contact.bsd_mattruoccmnd;
            //data["bsd_matsaucmnd"] = contact.bsd_matsaucmnd;


            if (contact._bsd_protecter_value == null || !contact.bsd_haveprotector)
            {
                await DeletLookup("bsd_protecter", contact.contactid);
            }
            else
            {
                data["bsd_protecter@odata.bind"] = "/contacts(" + contact._bsd_protecter_value + ")"; /////Lookup Field
            }

            if (contact._parentcustomerid_value == null)
            {
                await DeletLookup("parentcustomerid", contact.contactid);
            }
            else
            {
                if (contact.parentcustomerid_label_account != null)
                {
                    data["parentcustomerid_account@odata.bind"] = "/accounts(" + contact._parentcustomerid_value + ")"; /////Lookup Field
                }
                else
                {
                    data["parentcustomerid_contact@odata.bind"] = "/contacts(" + contact._parentcustomerid_value + ")"; /////Lookup Field
                }
            }

            if (contact._bsd_country_value == null)
            {
                await DeletLookup("bsd_country", contact.contactid);
            }
            else
            {
                data["bsd_country@odata.bind"] = "/bsd_countries(" + contact._bsd_country_value + ")"; /////Lookup Field
            }

            if (contact._bsd_province_value == null)
            {
                await DeletLookup("bsd_province", contact.contactid);
            }
            else
            {
                data["bsd_province@odata.bind"] = "/new_provinces(" + contact._bsd_province_value + ")"; /////Lookup Field
            }

            if (contact._bsd_district_value == null)
            {
                await DeletLookup("bsd_district", contact.contactid);
            }
            else
            {
                data["bsd_district@odata.bind"] = "/new_districts(" + contact._bsd_district_value + ")"; /////Lookup Field
            }

            if (contact._bsd_permanentcountry_value == null)
            {
                await DeletLookup("bsd_permanentcountry", contact.contactid);
            }
            else
            {
                data["bsd_permanentcountry@odata.bind"] = "/bsd_countries(" + contact._bsd_permanentcountry_value + ")"; /////Lookup Field
            }

            if (contact._bsd_permanentprovince_value == null)
            {
                await DeletLookup("bsd_permanentprovince", contact.contactid);
            }
            else
            {
                data["bsd_permanentprovince@odata.bind"] = "/new_provinces(" + contact._bsd_permanentprovince_value + ")"; /////Lookup Field
            }

            if (contact._bsd_permanentdistrict_value == null)
            {
                await DeletLookup("bsd_permanentdistrict", contact.contactid);
            }
            else
            {
                data["bsd_permanentdistrict@odata.bind"] = "/new_districts(" + contact._bsd_permanentdistrict_value + ")"; /////Lookup Field
            }

            return data;
        }
        public async Task<Boolean> DeletLookup(string fieldName, Guid contactId)
        {
            var result = await CrmHelper.SetNullLookupField("contacts", contactId, fieldName);
            return result.IsSuccess;
        }

        // giao dich

        //DANH SACH DAT COC
        public async Task LoadQueuesForContactForm(string customerId)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachDatCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='opportunityid' />
                                <attribute name='customerid' alias='customer_id' />
                                <attribute name='name' alias='unit_name'/>
                                <attribute name='bsd_queuenumber' />
                                <attribute name='bsd_project' alias='project_id' />
                                <attribute name='estimatedvalue' />
                                <attribute name='statuscode' />
                                <attribute name='actualclosedate' />
                                <attribute name='createdon' />
                                <order attribute='actualclosedate' descending='true' />
                                <filter type='and'>
                                  <condition attribute='customerid' operator='eq' value='{customerId}' />
                                </filter>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                                    <attribute name='fullname'  alias='contact_name'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                    <attribute name='name'  alias='account_name'/>
                                </link-entity>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer'>
                                    <attribute name='bsd_name'  alias='project_name'/>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueListModel>>("opportunities", fetch);
            if (result == null)
            {
                // await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreDanhSachDatCho = false;
            }
            else
            {
                ShowMoreDanhSachDatCho = true;
            }

            foreach (var x in data)
            {
                list_danhsachdatcho.Add(x);
            }
        }
        // DANH SACH DAT COC
        public async Task LoadReservationForContactForm(string customerId)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachDatCoc}' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='quote'>
                            <attribute name='quoteid' />
                            <attribute name='bsd_projectid' />
                            <attribute name='bsd_unitno' />
                            <attribute name='bsd_reservationno' />
                            <attribute name='customerid' />
                            <attribute name='statuscode' />
                            <attribute name='totalamount' />
                            <attribute name='createdon' />
                            <order attribute='createdon' descending='true' />
                            <filter type='and'>
                              <condition attribute='customerid' operator='eq' value='{customerId}' />
                            </filter>
                            <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='fullname'  alias='customerid_label_contact'/>
                            </link-entity>
                            <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='name'  alias='customerid_label_account'/>
                            </link-entity>
                            <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer'>
                                <attribute name='bsd_name'  alias='bsd_projectid_label'/>
                            </link-entity>
                            <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer'>
                                <attribute name='name'  alias='bsd_unitno_label'/>
                            </link-entity>
                            <link-entity name='transactioncurrency' from='transactioncurrencyid' to='transactioncurrencyid' visible='false' link-type='outer'>
                                <attribute name='currencysymbol'  alias='transaction_currency'/>
                            </link-entity>
                          </entity>
                        </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuotationReseravtion>>("quotes", fetch);
            if (result == null)
            {
                // await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreDanhSachDatCoc = false;
            }
            else
            {
                ShowMoreDanhSachDatCoc = true;
            }

            foreach (var x in data)
            {
                list_danhsachdatcoc.Add(x);
            }
        }

        // DANH SACH HOP DONG
        public async Task LoadOptoinEntryForContactForm(string customerId)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachHopDong}' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='salesorder'>
                            <attribute name='salesorderid' />
                            <attribute name='bsd_optionno' />
                            <attribute name='statuscode' />
                            <attribute name='totalamount' />
                            <attribute name='bsd_signingexpired' />
                            <attribute name='createdon' />
                            <order attribute='bsd_signingexpired' descending='true' />
                            <filter type='and'>
                              <condition attribute='customerid' operator='eq' value='{customerId}' />
                            </filter>
                            <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='fullname'  alias='customerid_label_contact'/>
                            </link-entity>
                            <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='name'  alias='customerid_label_account'/>
                            </link-entity>
                            <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer'>
                                <attribute name='bsd_name'  alias='bsd_project_label'/>
                            </link-entity>
                            <link-entity name='transactioncurrency' from='transactioncurrencyid' to='transactioncurrencyid' visible='false' link-type='outer'>
                                <attribute name='currencysymbol'  alias='transactioncurrency'/>
                            </link-entity>
                            <link-entity name='product' from='productid' to='bsd_unitnumber' visible='false' link-type='outer'>
                                <attribute name='name'  alias='bsd_unitnumber_label'/>
                            </link-entity>
                          </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionEntry>>("salesorders", fetch);
            if (result == null)
            {
                // await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreDanhSachHopDong = false;
            }
            else
            {
                ShowMoreDanhSachHopDong = true;
            }

            foreach (var x in data)
            {
                if (x.statuscode != "100000006") { optionEntryHasOnlyTerminatedStatus = false; }
                list_danhsachhopdong.Add(x);
            }
        }

        // CHAM SOC KHACH HANG
        public async Task LoadCaseForContactForm(string customerId)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageChamSocKhachHang}' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='incident'>
                            <attribute name='title' alias='title_label'/>
                            <attribute name='ticketnumber' />
                            <attribute name='createdon' />
                            <attribute name='incidentid' />
                            <attribute name='caseorigincode' />
                            <attribute name='statuscode' />
                            <attribute name='prioritycode' />
                            <order attribute='title' descending='false' />
                            <filter type='and'>
                              <condition attribute='customerid' operator='eq' value='{customerId}' />
                            </filter>
                            <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='fullname'  alias='customerid_label_contact'/>
                            </link-entity>
                            <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='name'  alias='customerid_label_account'/>
                            </link-entity>
                          </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Case>>("incidents", fetch);
            if (result == null)
            {
                //await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreChamSocKhachHang = false;
            }
            else
            {
                ShowMoreChamSocKhachHang = true;
            }

            foreach (var x in data)
            {
                list_chamsockhachhang.Add(x);
            }
        }

        // phon thuy
        public void LoadPhongThuy()
        {
            PhongThuy = new PhongThuyModel();
            LoadOneGender(singleContact.gendercode);
            if (list_HuongTot != null || list_HuongXau != null)
            {
                list_HuongTot.Clear();
                list_HuongXau.Clear();
                if (singleContact != null && singleContact.gendercode != null && singleGender != null && singleGender.Val != null)
                {
                    PhongThuy.gioi_tinh = Int32.Parse(singleContact.gendercode);
                    PhongThuy.nam_sinh = singleContact.birthdate.HasValue ? singleContact.birthdate.Value.Year : 0;
                    if (PhongThuy.huong_tot != null && PhongThuy.huong_tot != null)
                    {
                        string[] huongtot = PhongThuy.huong_tot.Split('\n');
                        string[] huongxau = PhongThuy.huong_xau.Split('\n');
                        int i = 1;
                        foreach (var x in huongtot)
                        {
                            string[] huong = x.Split(':');
                            string name_huong = i + ". " + huong[0];
                            string detail_huong = huong[1].Remove(0, 1);
                            list_HuongTot.Add(new HuongPhongThuy { Name = name_huong, Detail = detail_huong });
                            i++;
                        }
                        int j = 1;
                        foreach (var x in huongxau)
                        {
                            string[] huong = x.Split(':');
                            string name_huong = j + ". " + huong[0];
                            string detail_huong = huong[1].Remove(0, 1);
                            list_HuongXau.Add(new HuongPhongThuy { Name = name_huong, Detail = detail_huong });
                            j++;
                        }
                    }
                }
                else
                {
                    PhongThuy.gioi_tinh = 0;
                    PhongThuy.nam_sinh = 0;
                }
            }
        }
    }
}
