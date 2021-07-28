using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class LeadDetailPageViewModel : BaseViewModel
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();
        private LeadFormModel _singleLead;
        public LeadFormModel singleLead { get => _singleLead; set { _singleLead = value; OnPropertyChanged(nameof(singleLead)); } }
        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }
        private OptionSet _singleIndustrycode;
        public OptionSet singleIndustrycode { get => _singleIndustrycode; set { _singleIndustrycode = value; OnPropertyChanged(nameof(singleIndustrycode)); } }

        private PhongThuyModel _PhongThuy;
        public PhongThuyModel PhongThuy { get => _PhongThuy; set { _PhongThuy = value; OnPropertyChanged(nameof(PhongThuy)); } }       
        public ObservableCollection<Provinces> list_provinces_lookup { get; set; }           
        public ObservableCollection<OptionSet> list_gender_optionset { get; set; }
        public ObservableCollection<OptionSet> list_industrycode_optionset { get; set; }
        public ObservableCollection<Provinces> list_nhucauvediadiem { get; set; }

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
        
        public ObservableCollection<HuongPhongThuy> list_HuongTot { set; get; }

        public ObservableCollection<HuongPhongThuy> list_HuongXau { set; get; }

        public LeadCheckData single_Leadcheck;              

        public LeadDetailPageViewModel()
        {
            singleGender = new OptionSet();
            singleIndustrycode = new OptionSet();                      

            list_provinces_lookup = new ObservableCollection<Provinces>();            

            list_gender_optionset = new ObservableCollection<OptionSet>();
            list_industrycode_optionset = new ObservableCollection<OptionSet>();           

            list_nhucauvediadiem = new ObservableCollection<Provinces>();          
            list_Duanquantam = new ObservableCollection<ProjectList>();
            list_project_lookup = new ObservableCollection<ProjectList>();
            single_Leadcheck = new LeadCheckData();

            list_TieuChiChonMua = new ObservableCollection<TieuChi>();
            list_tieuchichonmua_lookup = new ObservableCollection<TieuChi>();
            list_LoaiBatDongSanQuanTam = new ObservableCollection<TieuChi>();
            list_loaibatdongsanquantam_lookup = new ObservableCollection<TieuChi>();
            list_NhuCauVeDienTichCanHo = new ObservableCollection<TieuChi>();
            list_nhucauvedientichcanho_lookup = new ObservableCollection<TieuChi>();
            list_HuongTot = new ObservableCollection<HuongPhongThuy>();
            list_HuongXau = new ObservableCollection<HuongPhongThuy>();

            this.loadGender();
            this.loadIndustrycode();
        }      

        public async Task LoadOneLead(String leadid)
        {
            singleLead = new LeadFormModel();
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='lead'>
                                    <attribute name='fullname' />
                                    <attribute name='subject' alias='bsd_topic_label'/>
                                    <attribute name='statuscode' />
                                    <attribute name='leadqualitycode' />
                                    <attribute name='mobilephone' />
                                    <attribute name='telephone1' />
                                    <attribute name='emailaddress1' />
                                    <attribute name='jobtitle' />  
                                    <attribute name='companyname' />
                                    <attribute name='websiteurl' />
                                    <attribute name='address1_composite' />
                                    <attribute name='description' />
                                    <attribute name='industrycode' />
                                    <attribute name='revenue' />
                                    <attribute name='numberofemployees' />
                                    <attribute name='sic' />
                                    <attribute name='donotsendmm' />
                                    <attribute name='lastusedincampaign' />
                                    <attribute name='createdon' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='leadid' operator='eq' value='{" + leadid + @"}' />
                                    </filter>
                                    <link-entity name='transactioncurrency' from='transactioncurrencyid' to='transactioncurrencyid' visible='false' link-type='outer'>
                                        <attribute name='currencyname'  alias='transactioncurrencyid_label'/>
                                    </link-entity>
                                    <link-entity name='campaign' from='campaignid' to='campaignid' visible='false' link-type='outer'>
                                        <attribute name='name'  alias='campaignid_label'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadFormModel>>("leads", fetch);
            var tmp = result.value.FirstOrDefault();
            this.singleLead = tmp;
        }

        public async Task<Boolean> updateLead(LeadFormModel lead)
        {
            string path = "/leads(" + lead.leadid + ")";
            var content = await this.getContent(lead);

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

        public async Task<Guid> createLead(LeadFormModel lead)
        {
            string path = "/leads";
            lead.leadid = Guid.NewGuid();
            var content = await this.getContent(lead);

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                var api_Response = JsonConvert.DeserializeObject(result.Content);
                return lead.leadid;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                return new Guid();
            }


        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid leadId)
        {
            var result = await CrmHelper.SetNullLookupField("leads", leadId, fieldName);
            return result.IsSuccess;
        }

        public async Task<bool> Qualify(Guid id)
        {
            string path = "/leads(" + id + ")//Microsoft.Dynamics.CRM.bsd_Action_Lead_QualifyLead";
            var content = new object();

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<object> getContent(LeadFormModel lead)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();

            data["bsd_topic@odata.bind"] = "/bsd_topics(" + lead._bsd_topic_value + ")"; /////Lookup Field (use Schema Name)
            data["subject"] = lead.bsd_topic_label;
            data["fullname"] = lead.fullname;
            data["leadid"] = lead.leadid;
            data["firstname"] = lead.firstname;
            data["lastname"] = lead.lastname;
            data["mobilephone"] = lead.mobilephone;
            data["telephone1"] = lead.telephone1;
            data["jobtitle"] = lead.jobtitle;
            data["emailaddress1"] = lead.emailaddress1;
            data["new_gender"] = lead.new_gender;
            data["bsd_identitycardnumber"] = lead.bsd_identitycardnumber;
            //data["new_birthday"] = "1994-02-11";
            data["new_birthday"] = lead.new_birthday.HasValue ? (DateTime.Parse(lead.new_birthday.ToString()).ToLocalTime()).ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"") : null;
            data["companyname"] = lead.companyname;
            data["websiteurl"] = lead.websiteurl;
            data["address1_composite"] = lead.address1_composite;
            data["address1_line1"] = lead.address1_line1;
            data["address1_line2"] = lead.address1_line2;
            data["address1_line3"] = lead.address1_line3;
            data["address1_city"] = lead.address1_city;
            data["address1_stateorprovince"] = lead.address1_stateorprovince;
            data["address1_postalcode"] = lead.address1_postalcode;
            data["address1_country"] = lead.address1_country;
            data["bsd_diemdanhgia"] = lead.bsd_diemdanhgia;
            data["bsd_danhgiaid"] = lead.bsd_danhgiaid;
            data["bsd_danhgiadiem"] = lead.bsd_danhgiadiem;
            data["description"] = lead.description;
            data["industrycode"] = lead.industrycode;
            data["revenue"] = decimal.Parse(lead.revenue); //bug
            data["numberofemployees"] = lead.numberofemployees;
            data["sic"] = lead.sic;
            data["donotsendmm"] = lead.donotsendmm.ToString();
            data["lastusedincampaign"] = lead.lastusedincampaign.HasValue ? (DateTime.Parse(lead.lastusedincampaign.ToString()).ToLocalTime()).ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"") : null;
            data["bsd_tieuchi_vitri"] = lead.bsd_tieuchi_vitri.ToString();
            data["bsd_tieuchi_phuongthucthanhtoan"] = lead.bsd_tieuchi_phuongthucthanhtoan.ToString();
            data["bsd_tieuchi_giacanho"] = lead.bsd_tieuchi_giacanho.ToString();
            data["bsd_tieuchi_nhadautuuytin"] = lead.bsd_tieuchi_nhadautuuytin.ToString();
            data["bsd_tieuchi_moitruongsong"] = lead.bsd_tieuchi_moitruongsong.ToString();
            data["bsd_tieuchi_baidauxe"] = lead.bsd_tieuchi_baidauxe.ToString();
            data["bsd_tieuchi_hethonganninh"] = lead.bsd_tieuchi_hethonganninh.ToString();
            data["bsd_tieuchi_huongcanho"] = lead.bsd_tieuchi_huongcanho.ToString();
            data["bsd_tieuchi_hethongcuuhoa"] = lead.bsd_tieuchi_hethongcuuhoa.ToString();
            data["bsd_tieuchi_nhieutienich"] = lead.bsd_tieuchi_nhieutienich.ToString();
            data["bsd_tieuchi_ganchosieuthi"] = lead.bsd_tieuchi_ganchosieuthi.ToString();
            data["bsd_tieuchi_gantruonghoc"] = lead.bsd_tieuchi_gantruonghoc.ToString();
            data["bsd_tieuchi_ganbenhvien"] = lead.bsd_tieuchi_ganbenhvien.ToString();
            data["bsd_tieuchi_dientichcanho"] = lead.bsd_tieuchi_dientichcanho.ToString();
            data["bsd_tieuchi_thietkenoithatcanho"] = lead.bsd_tieuchi_thietkenoithatcanho.ToString();
            data["bsd_tieuchi_tangcanhodep"] = lead.bsd_tieuchi_tangcanhodep.ToString();
            data["bsd_dientich_3060m2"] = lead.bsd_dientich_3060m2.ToString();
            data["bsd_dientich_6080m2"] = lead.bsd_dientich_6080m2.ToString();
            data["bsd_dientich_80100m2"] = lead.bsd_dientich_80100m2.ToString();
            data["bsd_dientich_100120m2"] = lead.bsd_dientich_100120m2.ToString();
            data["bsd_dientich_lonhon120m2"] = lead.bsd_dientich_lonhon120m2.ToString();
            data["bsd_quantam_datnen"] = lead.bsd_quantam_datnen.ToString();
            data["bsd_quantam_canho"] = lead.bsd_quantam_canho.ToString();
            data["bsd_quantam_bietthu"] = lead.bsd_quantam_bietthu.ToString();
            data["bsd_quantam_khuthuongmai"] = lead.bsd_quantam_khuthuongmai.ToString();
            data["bsd_quantam_nhapho"] = lead.bsd_quantam_nhapho.ToString();
            data["bsd_contactaddress"] = lead.bsd_contactaddress;
            data["bsd_diachi"] = lead.bsd_diachi;
            data["bsd_housenumberstreet"] = lead.bsd_housenumberstreet;
            data["bsd_housenumber"] = lead.bsd_housenumber;

            if (lead._transactioncurrencyid_value == null)
            {
                await DeletLookup("transactioncurrencyid", lead.leadid);
            }
            else
            {
                data["transactioncurrencyid@odata.bind"] = "/transactioncurrencies(" + lead._transactioncurrencyid_value + ")"; /////Lookup Field
            }
            if (lead._campaignid_value == null)
            {
                await DeletLookup("campaignid", lead.leadid);
            }
            else
            {
                data["campaignid@odata.bind"] = "/campaigns(" + lead._campaignid_value + ")"; /////Lookup Field
            }

            if (lead._bsd_country_value == null)
            {
                await DeletLookup("bsd_Country", lead.leadid);
            }
            else
            {
                data["bsd_Country@odata.bind"] = "/bsd_countries(" + lead._bsd_country_value + ")"; /////Lookup Field
            }

            if (lead._bsd_province_value == null)
            {
                await DeletLookup("bsd_province", lead.leadid);
            }
            else
            {
                data["bsd_province@odata.bind"] = "/new_provinces(" + lead._bsd_province_value + ")"; /////Lookup Field
            }

            if (lead._bsd_district_value == null)
            {
                await DeletLookup("bsd_District", lead.leadid);
            }
            else
            {
                data["bsd_District@odata.bind"] = "/new_districts(" + lead._bsd_district_value + ")"; /////Lookup Field
            }

            return data;
        }       

        public void loadGender()
        {
            list_gender_optionset.Add(new OptionSet() { Val = ("1"), Label = "Nam", });
            list_gender_optionset.Add(new OptionSet() { Val = ("2"), Label = "Nữ", });
            list_gender_optionset.Add(new OptionSet() { Val = ("100000000"), Label = "Khác", });
        }

        public async Task<OptionSet> loadOneGender(string id)
        {
            this.singleGender = list_gender_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleGender;
        }

        //////// INDUSTRYCODE OPTIONSET AREA
        /// ////
        public void loadIndustrycode()
        {
            list_industrycode_optionset.Add(new OptionSet() { Val = ("1"), Label = "Kế toán", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("2"), Label = "Nông nghiệp và Trích xuất Tài nguyên Thiên nhiên Không Dầu", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("3"), Label = "In và Xuất bản Truyền thông", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("4"), Label = "Nhà môi giới", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("5"), Label = "Bán lẻ Dịch vụ Cấp nước trong Tòa nhà", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("6"), Label = "Dịch vụ Kinh doanh", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("7"), Label = "Tư vấn", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("8"), Label = "Dịch vụ Tiêu dùng", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("9"), Label = "Quản lý Thiết kế, Chỉ đạo và Quảng cáo", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("10"), Label = "Nhà phân phối, Người điều vận và Nhà chế biến", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("11"), Label = "Văn phòng và Phòng khám Bác sĩ", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("12"), Label = "Sản xuất Lâu bền", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("13"), Label = "Địa điểm Ăn Uống", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("14"), Label = "Bán lẻ Dịch vụ Giải trí", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("15"), Label = "Thuê và Cho thuê Thiết bị", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("16"), Label = "Tài chính", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("17"), Label = "Chế biến Thực phẩm và Thuốc lá", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("18"), Label = "Xử lý Dựa vào Nhiều vốn Chuyển về", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("19"), Label = "Sửa chữa và Bảo dưỡng Chuyển đến", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("20"), Label = "Bảo hiểm", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("21"), Label = "Dịch vụ Pháp lý", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("22"), Label = "Bán lẻ Hàng hóa Không lâu bền", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("23"), Label = "Dịch vụ Tiêu dùng Bên ngoài", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("24"), Label = "Trích xuất và Phân phối Hóa dầu", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("25"), Label = "Bán lẻ Dịch vụ", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("26"), Label = "Chi nhánh SIG", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("27"), Label = "Dịch vụ Xã hội", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("28"), Label = "Nhà thầu Giao dịch Bên ngoài Đặc biệt", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("29"), Label = "Bất động sản Đặc biệt", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("30"), Label = "Vận tải", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("31"), Label = "Tạo và Phân phối Tiện ích", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("32"), Label = "Bán lẻ Phương tiện", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("33"), Label = "Bán buôn", });
        }

        public async Task<OptionSet> loadOneIndustrycode(string id)
        {
            this.singleIndustrycode = list_industrycode_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleIndustrycode;
        }
       
        // tieu chi chon mua lookup
        public void LoadAllTieuChiChonMua()
        {
            list_tieuchichonmua_lookup.Add(new TieuChi { Id = 1 ,Name= "Tiêu chí - Vị trí" });
            list_tieuchichonmua_lookup.Add(new TieuChi {Id = 2, Name = "Tiêu chí - Phương thức thanh toán"});
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

        ////////////// PROVINCE LOOKUP AREA
        ////

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

        //////////////////// NHUCAUVEDIADIEM AREA
        //////

        public async Task Load_NhuCauVeDiaDiem(string leadid)
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
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Provinces>>("leads(" + leadid + @")/bsd_lead_new_province", fetch);
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

        public async Task<Boolean> Add_NhuCauDiaDiem(string id, Guid leadid)
        {
            string path = $"/leads({leadid})/bsd_lead_new_province/$ref";

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

        public async Task<Boolean> Delete_NhuCauDiaDiem(string id, Guid leadid)
        {
            string Token = App.Current.Properties["Token"] as string;

            var request = $"{OrgConfig.ApiUrl}/leads(" + leadid + ")/bsd_lead_new_province(" + id + ")/$ref";

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

        public async Task Load_DanhSachDuAn(string leadid)
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
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("leads(" + leadid + @")/bsd_lead_bsd_project", fetch);
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

        public async Task<Boolean> Add_DuAnQuanTam(string id, Guid leadid)
        {
            string path = $"/leads({leadid})/bsd_lead_bsd_project/$ref";

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

        public async Task<Boolean> Delete_DuAnQuanTam(string id, Guid leadid)
        {
            string Token = App.Current.Properties["Token"] as string;

            var request = $"{OrgConfig.ApiUrl}/leads(" + leadid + ")/bsd_lead_bsd_project(" + id + ")/$ref";

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
        //get fullName, mobiphone, email da ton tai
        public async Task Checkdata_identical_lock(string Name, string PhoneTel, string Email, Guid leadid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='lead'>
                                <attribute name='fullname' />
                                <attribute name='mobilephone' />
                                <attribute name='emailaddress1' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <filter type='or'>
                                    <filter type='and'>
                                      <condition attribute='fullname' operator='eq' value='" + Name + @"' />
                                      <condition attribute='mobilephone' operator='eq' value='" + PhoneTel + @"' />
                                      <condition attribute='leadid' operator='ne' value='{" + leadid + @"}' />
                                    </filter>
                                    <filter type='and'>
                                      <condition attribute='fullname' operator='eq' value='" + Name + @"' />
                                      <condition attribute='emailaddress1' operator='eq' value='" + Email + @"' />
                                      <condition attribute='leadid' operator='ne' value='{" + leadid + @"}' />
                                    </filter>
                                  </filter>
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadCheckData>>("leads", fetch);
            var tmp = result.value.FirstOrDefault();

            this.single_Leadcheck = tmp;
        }

        public async Task<Boolean> Check_Quatify(string cardNumber)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='contact'>
                            <attribute name='fullname' />
                            <attribute name='bsd_identitycardnumber' />
                            <attribute name='contactid' />
                            <order attribute='createdon' descending='true' />
                            <filter type='and'>
                                <condition attribute='statecode' operator='eq' value='0' />
                                <condition attribute='bsd_identitycardnumber' operator='eq' value='" + cardNumber + @"' />
                            </filter>
                          </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            var tmp = result.value.FirstOrDefault();
            if (tmp != null)
            {
                if (tmp.bsd_identitycardnumber == cardNumber)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "CMND đã tồn tại.", "OK");
                    return false;
                }
            }
            return true;
        }      

        public async void UpdateTieuChi(int id, bool val)
        {
            if (id == 1)
                singleLead.bsd_tieuchi_vitri = val;
            if (id == 2)
                singleLead.bsd_tieuchi_phuongthucthanhtoan = val;
            if (id == 3)
                singleLead.bsd_tieuchi_giacanho = val;
            if (id == 4)
                singleLead.bsd_tieuchi_nhadautuuytin = val;
            if (id == 5)
                singleLead.bsd_tieuchi_moitruongsong = val;
            if (id == 6)
                singleLead.bsd_tieuchi_hethonganninh = val;
            if (id == 7)
                singleLead.bsd_tieuchi_baidauxe = val;
            if (id == 8)
                singleLead.bsd_tieuchi_huongcanho = val;
            if (id == 9)
                singleLead.bsd_tieuchi_hethongcuuhoa = val;
            if (id == 10)
                singleLead.bsd_tieuchi_nhieutienich = val;
            if (id == 11)
                singleLead.bsd_tieuchi_ganchosieuthi = val;
            if (id == 12)
                singleLead.bsd_tieuchi_gantruonghoc = val;
            if (id == 13)
                singleLead.bsd_tieuchi_ganbenhvien = val;
            if (id == 14)
                singleLead.bsd_tieuchi_dientichcanho = val;
            if (id == 15)
                singleLead.bsd_tieuchi_thietkenoithatcanho = val;
            if (id == 16)
                singleLead.bsd_tieuchi_tangcanhodep = val;
            if (id == 17)
                singleLead.bsd_dientich_3060m2 = val;
            if (id == 18)
                singleLead.bsd_dientich_6080m2 = val;
            if (id == 19)
                singleLead.bsd_dientich_80100m2 = val;
            if (id == 20)
                singleLead.bsd_dientich_100120m2 = val;
            if (id == 21)
                singleLead.bsd_dientich_lonhon120m2 = val;
            if (id == 22)
                singleLead.bsd_quantam_datnen = val;
            if (id == 23)
                singleLead.bsd_quantam_canho = val;
            if (id == 24)
                singleLead.bsd_quantam_bietthu = val;
            if (id == 25)
                singleLead.bsd_quantam_khuthuongmai = val;
            if (id == 26)
                singleLead.bsd_quantam_nhapho = val;
            await updateLead(singleLead);
        }
        public void LoadTieuChi()
        { 
            if (singleLead.bsd_tieuchi_vitri == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 1, Name = "Tiêu chí - Vị trí" });
            if (singleLead.bsd_tieuchi_phuongthucthanhtoan == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 2, Name = "Tiêu chí - Phương thức thanh toán" });
            if (singleLead.bsd_tieuchi_giacanho == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 3, Name = "Tiêu chí - Giá căn hộ" });
            if (singleLead.bsd_tieuchi_nhadautuuytin == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 4, Name = "Tiêu chí - Nhà đầu tư uy tính" });
            if (singleLead.bsd_tieuchi_moitruongsong == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 5, Name = "Tiêu chí - Môi trường sống" });
            if (singleLead.bsd_tieuchi_hethonganninh == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 6, Name = "Tiêu chí - Hệ thống an ninh" });
            if (singleLead.bsd_tieuchi_baidauxe == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 7, Name = "Tiêu chí - Bãi đậu xe" });
            if (singleLead.bsd_tieuchi_huongcanho == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 8, Name = "Tiêu chí - Hướng căn hộ" });
            if (singleLead.bsd_tieuchi_hethongcuuhoa == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 9, Name = "Tiêu chí - Hệ thống cứu hoả" });
            if (singleLead.bsd_tieuchi_nhieutienich == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 10, Name = "Tiêu chí - Nhiều tiện ích" });
            if (singleLead.bsd_tieuchi_ganchosieuthi == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 11, Name = "Tiêu chí - Gần chợ - Siêu thị" });
            if (singleLead.bsd_tieuchi_gantruonghoc == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 12, Name = "Tiêu chí - Gần trường học" });
            if (singleLead.bsd_tieuchi_ganbenhvien == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 13, Name = "Tiêu chí - Gần bệnh viện" });
            if (singleLead.bsd_tieuchi_dientichcanho == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 14, Name = "Tiêu chí - Diện tích căn hộ" });
            if (singleLead.bsd_tieuchi_thietkenoithatcanho == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 15, Name = "Tiêu chí - Thiết kế nội thất căn hộ" });
            if (singleLead.bsd_tieuchi_tangcanhodep == true)
                list_TieuChiChonMua.Add(new TieuChi { Id = 16, Name = "Tiêu chí - Tầng/Căn hộ đẹp" });
            if (singleLead.bsd_dientich_3060m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 17, Name = "Diện tích 30 - 60 m2" });
            if (singleLead.bsd_dientich_6080m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 18, Name = "Diện tích 60 - 80 m2" });
            if (singleLead.bsd_dientich_80100m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 19, Name = "Diện tích 80 - 100 m2" });
            if (singleLead.bsd_dientich_100120m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 20, Name = "Diện tích 100 - 120 m2" });
            if (singleLead.bsd_dientich_lonhon120m2 == true)
                list_NhuCauVeDienTichCanHo.Add(new TieuChi { Id = 21, Name = "Diện tích > 120 m2" });
            if (singleLead.bsd_quantam_datnen == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 22, Name = "Quan tâm - Đất nền" });
            if (singleLead.bsd_quantam_canho == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 23, Name = "Quan tâm - Căn hộ" });
            if (singleLead.bsd_quantam_bietthu == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 24, Name = "Quan tâm - Biệt thự" });
            if (singleLead.bsd_quantam_khuthuongmai == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 25, Name = "Quan tâm - Khu thương mại" });
            if (singleLead.bsd_quantam_nhapho == true)
                list_LoaiBatDongSanQuanTam.Add(new TieuChi { Id = 26, Name = "Quan tâm - Nhà phố" });           
        }
        public void LoadPhongThuy()
        {
            PhongThuy = new PhongThuyModel();
            _ = loadOneGender(singleLead.new_gender);
            if (list_HuongTot != null || list_HuongXau != null)
            {
                list_HuongTot.Clear();
                list_HuongXau.Clear();
                if (singleLead != null && singleLead.new_gender != null && singleGender != null && singleGender.Val != null)
                {
                    PhongThuy.gioi_tinh = Int32.Parse(singleLead.new_gender);
                    PhongThuy.nam_sinh = singleLead.new_birthday.HasValue ? singleLead.new_birthday.Value.Year : 0;
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
