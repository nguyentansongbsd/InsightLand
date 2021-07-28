using System;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Helper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using ConasiCRM.Portable.Config;
using System.Net.Http.Headers;
using System.Net;
using System.Windows.Input;
using Xamarin.Forms;
using ConasiCRM.Models;

namespace ConasiCRM.Portable.ViewModels
{
    public class LeadFormViewModel : BaseViewModel
    {
        public ObservableCollection<OptionSet> list_currency_lookup { get; set; }
        
        private OptionSet _selectedCurrency;
        public OptionSet SelectedCurrency { get => _selectedCurrency; set { _selectedCurrency = value;OnPropertyChanged(nameof(SelectedCurrency)); } }

        public ObservableCollection<OptionSet> list_industrycode_optionset { get; set; }

        private OptionSet _industryCode;
        public OptionSet IndustryCode { get => _industryCode; set { _industryCode = value;OnPropertyChanged(nameof(IndustryCode)); } }

        public ObservableCollection<OptionSet> list_campaign_lookup { get; set; }

        private OptionSet _campaign;
        public OptionSet Campaign { get => _campaign; set { _campaign = value; OnPropertyChanged(nameof(Campaign)); } }

        private string _addressComposite;
        public string AddressComposite { get => _addressComposite; set { _addressComposite = value;OnPropertyChanged(nameof(AddressComposite)); } }

        private string _addressCountry;
        public string AddressCountry { get => _addressCountry; set { _addressCountry = value; OnPropertyChanged(nameof(AddressCountry)); } }

        private string _addressPostalCode;
        public string AddressPostalCode { get => _addressPostalCode; set { _addressPostalCode = value; OnPropertyChanged(nameof(AddressPostalCode)); } }

        private string _addressStateProvince;
        public string AddressStateProvince { get => _addressStateProvince; set { _addressStateProvince = value; OnPropertyChanged(nameof(AddressStateProvince)); } }

        private string _addressCity;
        public string AddressCity { get => _addressCity; set { _addressCity = value; OnPropertyChanged(nameof(AddressCity)); } }

        private string _addressLine3;
        public string AddressLine3 { get => _addressLine3; set { _addressLine3 = value; OnPropertyChanged(nameof(AddressLine3)); } }

        private string _addressLine2;
        public string AddressLine2 { get => _addressLine2; set { _addressLine2 = value; OnPropertyChanged(nameof(AddressLine2)); } }

        private string _addressLine1;
        public string AddressLine1 { get => _addressLine1; set { _addressLine1 = value; OnPropertyChanged(nameof(AddressLine1)); } }


        private LeadFormModel _singleLead;
        public LeadFormModel singleLead { get => _singleLead; set { _singleLead = value; OnPropertyChanged(nameof(singleLead)); } }
        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }
        private OptionSet _singleIndustrycode;
        public OptionSet singleIndustrycode { get => _singleIndustrycode; set { _singleIndustrycode = value; OnPropertyChanged(nameof(singleIndustrycode)); } }

        private PhongThuyModel _PhongThuy;
        public PhongThuyModel PhongThuy { get => _PhongThuy; set { _PhongThuy = value; OnPropertyChanged(nameof(PhongThuy)); } }

        private bool _looking_up;
        public bool looking_up { get => _looking_up; set { _looking_up = value; OnPropertyChanged(nameof(looking_up)); } }

        private string _country;
        public string Country { get => _country; set { _country = value; OnPropertyChanged(nameof(Country)); } }
        public string CountryId { get; set; }
        public string CountryEn { get; set; }

        private string _province;
        public string Province { get => _province; set { _province = value; OnPropertyChanged(nameof(Province)); } }
        public string ProvinceId { get; set; }
        public string ProvinceEn { get; set; }

        private string _district;
        public string District { get => _district; set { _district = value; OnPropertyChanged(nameof(District)); } }
        public string DistrictId { get; set; }
        public string DistrictEn { get; set; }

        private string _addressVn;
        public string AddressVn { get => _addressVn; set { _addressVn = value; OnPropertyChanged(nameof(AddressVn)); } }

        private string _addressEn;
        public string AddressEn { get => _addressEn; set { _addressEn = value; OnPropertyChanged(nameof(AddressEn)); } }

        private string _streetVn;
        public string StreetVn { get => _streetVn; set { _streetVn = value; OnPropertyChanged(nameof(StreetVn)); } }

        private string _streetEn;
        public string StreetEn { get => _streetEn; set { _streetEn = value; OnPropertyChanged(nameof(StreetEn)); } }

        public ObservableCollection<LookUp> list_lookup { get; set; }
        public ObservableCollection<LookUp> list_topic_lookup { get; set; }
        
        
        public ObservableCollection<Provinces> list_provinces_lookup { get; set; }

        public ObservableCollection<LookUp> list_country_lookup { get; set; }
        public ObservableCollection<LookUp> list_province_lookup { get; set; }
        public ObservableCollection<LookUp> list_district_lookup { get; set; }

        public ObservableCollection<OptionSet> list_gender_optionset { get; set; }
          
        public ObservableCollection<Provinces> list_nhucauvediadiem { get; set; }

        public ObservableCollection<LeadsRating> list_leadrating { get; set; }

        public bool morelookup_country;
        public int pageLookup_country;
        public int pageLookup_province;
        public bool morelookup_province;
        public int pageLookup_district;
        public bool morelookup_district;

        public Diemdanhgia Dudiemdanhgia { get; set; }

        private ObservableCollection<ProjectList> _list_Duanquantam;
        public ObservableCollection<ProjectList> list_Duanquantam { get { return _list_Duanquantam; } set { _list_Duanquantam = value; OnPropertyChanged(nameof(_list_Duanquantam)); } }

        public ObservableCollection<ProjectList> list_project_lookup { set; get; }

        public LeadCheckData single_Leadcheck;

        private bool _showMoreNhuCauDiaDiem;
        public bool ShowMoreNhuCauDiaDiem { get => _showMoreNhuCauDiaDiem; set { _showMoreNhuCauDiaDiem = value; OnPropertyChanged(nameof(ShowMoreNhuCauDiaDiem)); } }

        public int PageNhuCauDiaDiem { get; set; } = 1;

        private bool _showMoreDuAnQuanTam;
        public bool ShowMoreDuAnQuanTam { get => _showMoreDuAnQuanTam; set { _showMoreDuAnQuanTam = value; OnPropertyChanged(nameof(ShowMoreDuAnQuanTam)); } }

        public int PageDuAnQuanTam { get; set; } = 1;

        private List<MaQuocGia> _maQuocGiaList;
        public List<MaQuocGia> MaQuocGiaList { get=> _maQuocGiaList; set { _maQuocGiaList = value; OnPropertyChanged(nameof(MaQuocGiaList)); } }

        private MaQuocGia _maQuocGiaCaNha;
        public MaQuocGia MaQuocGiaCaNhan { get=>_maQuocGiaCaNha; set { _maQuocGiaCaNha = value;OnPropertyChanged(nameof(MaQuocGiaCaNhan)); } }
        

        private MaQuocGia _maQuocGiaCty;
        public MaQuocGia MaQuocGiaCty { get => _maQuocGiaCty; set { _maQuocGiaCty = value; OnPropertyChanged(nameof(MaQuocGiaCty)); } }

        public LeadFormViewModel()
        {
            MaQuocGiaList= MaQuocGiaData.GetList();
            MaQuocGiaCaNhan = MaQuocGiaList[0];
            MaQuocGiaCty = MaQuocGiaList[0];

            singleLead = new LeadFormModel();
            singleGender = new OptionSet();
            singleIndustrycode = new OptionSet();

            PhongThuy = new PhongThuyModel();

            looking_up = false;
            pageLookup_country = 1;
            morelookup_country = true;
            pageLookup_province = 1;
            morelookup_province = true;
            pageLookup_district = 1;
            morelookup_district = true;

            list_lookup = new ObservableCollection<LookUp>();
            list_topic_lookup = new ObservableCollection<LookUp>();
            list_currency_lookup = new ObservableCollection<OptionSet>();
            list_campaign_lookup = new ObservableCollection<OptionSet>();

            list_provinces_lookup = new ObservableCollection<Provinces>();

            list_country_lookup = new ObservableCollection<LookUp>();
            list_province_lookup = new ObservableCollection<LookUp>();
            list_district_lookup = new ObservableCollection<LookUp>();

            list_gender_optionset = new ObservableCollection<OptionSet>();
            list_industrycode_optionset = new ObservableCollection<OptionSet>();

            list_leadrating = new ObservableCollection<LeadsRating>();

            list_nhucauvediadiem = new ObservableCollection<Provinces>();

            Dudiemdanhgia = new Diemdanhgia();
            list_Duanquantam = new ObservableCollection<ProjectList>();
            list_project_lookup = new ObservableCollection<ProjectList>();
            single_Leadcheck = new LeadCheckData();         

            this.loadIndustrycode();
        }

        public void reset()
        {
            singleLead = new LeadFormModel();
            singleGender = new OptionSet();
            singleIndustrycode = new OptionSet();

            list_leadrating.Clear();
            list_nhucauvediadiem.Clear();
            list_Duanquantam.Clear();
            list_gender_optionset.Clear();
            list_industrycode_optionset.Clear();

            this.loadIndustrycode();
        }

        public async Task LoadOneLead(String leadid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='lead'>
                                    <all-attributes />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='leadid' operator='eq' value='{" + leadid + @"}' />
                                    </filter>
                                    <link-entity name='bsd_topic' from='bsd_topicid' to='bsd_topic' visible='false' link-type='outer'>
                                        <attribute name='bsd_name'  alias='bsd_topic_label'/>
                                    </link-entity>
                                    <link-entity name='transactioncurrency' from='transactioncurrencyid' to='transactioncurrencyid' visible='false' link-type='outer'>
                                        <attribute name='currencyname'  alias='transactioncurrencyid_label'/>
                                    </link-entity>
                                    <link-entity name='campaign' from='campaignid' to='campaignid' visible='false' link-type='outer'>
                                        <attribute name='name'  alias='campaignid_label'/>
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer'>
                                        <attribute name='bsd_countryname'  alias='bsd_country_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_country_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer'>
                                        <attribute name='bsd_provincename'  alias='bsd_province_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_province_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer'>
                                        <attribute name='new_name'  alias='bsd_district_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_district_en'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadFormModel>>("leads", fetch);
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            }

            this.singleLead = tmp;
        }

        public async Task<Boolean> updateLead(LeadFormModel lead)
        {
            string path = "/leads(" + lead.leadid + ")";
            var content = await this.getContent();

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

        public async Task<CrmApiResponse> createLead()
        {
            string path = "/leads";
            singleLead.leadid = Guid.NewGuid();
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            return result;
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid leadId)
        {
            var result = await CrmHelper.SetNullLookupField("leads", leadId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["leadid"] = singleLead.leadid;
            data["subject"] = singleLead.bsd_topic_label;
            data["fullname"] = singleLead.fullname;
            data["firstname"] = singleLead.fullname;
            data["mobilephone"] = singleLead.mobilephone;
            data["telephone1"] = singleLead.telephone1;
            data["jobtitle"] = singleLead.jobtitle;
            data["emailaddress1"] = singleLead.emailaddress1;
            data["companyname"] = singleLead.companyname;
            data["websiteurl"] = singleLead.websiteurl;
            data["address1_composite"] = singleLead.address1_composite;
            data["address1_line1"] = singleLead.address1_line1;
            data["address1_line2"] = singleLead.address1_line2;
            data["address1_line3"] = singleLead.address1_line3;
            data["address1_city"] = singleLead.address1_city;
            data["address1_stateorprovince"] = singleLead.address1_stateorprovince;
            data["address1_postalcode"] = singleLead.address1_postalcode;
            data["address1_country"] = singleLead.address1_country;
            data["description"] = singleLead.description;
            data["industrycode"] = singleLead.industrycode;
            if (!string.IsNullOrWhiteSpace(singleLead.revenue))
            {
                data["revenue"] = decimal.Parse(singleLead.revenue);
            }
            data["numberofemployees"] = singleLead.numberofemployees;
            data["sic"] = singleLead.sic;
            data["donotsendmm"] = singleLead.donotsendmm.ToString();
            data["lastusedincampaign"] = singleLead.lastusedincampaign.HasValue ? (DateTime.Parse(singleLead.lastusedincampaign.ToString()).ToLocalTime()).ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"") : null;

            if (singleLead._transactioncurrencyid_value == null)
            {
                await DeletLookup("transactioncurrencyid", singleLead.leadid);
            }
            else
            {
                data["transactioncurrencyid@odata.bind"] = "/transactioncurrencies(" + singleLead._transactioncurrencyid_value + ")"; /////Lookup Field
            }


            if (singleLead._campaignid_value == null)
            {
                await DeletLookup("CampaignId", singleLead.leadid);
            }
            else
            {
                data["campaignid@odata.bind"] = "/campaigns(" + singleLead._campaignid_value + ")"; /////Lookup Field
            }
            return data;
        }


        //////// TOPIC LOOKUP AREA
        /// //////////////

        public async Task LoadTopicsForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='bsd_topic'>
                        <attribute name='bsd_topicid' alias='Id'/>
                        <attribute name='bsd_name' alias='Name'/>
                        <attribute name='createdon' />
                        <order attribute='bsd_name' descending='false' />
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("bsd_topics", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            foreach (var x in result.value)
            {
                list_topic_lookup.Add(x);
            }
        }


        ////////// LEADRATING AREA
        /// /////////

        public async Task LoadLeadsRating()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_leadsrating'>
                                    <attribute name='bsd_leadsratingid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_point' />
                                    <attribute name='createdon' />
                                    <attribute name='statecode' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_enddate' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statecode' operator='eq' value='0' />
                                        <condition attribute='statuscode' operator='ne' value='1' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadsRating>>("bsd_leadsratings", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            foreach (var x in result.value)
            {
                list_leadrating.Add(x);
            }
        }

        public LeadsRating getLeadsRating(String id)
        {
            if (id == null)
                return null;
            return list_leadrating.ToList().FirstOrDefault(x => x.bsd_leadsratingid == id);
        }

        ///////////// CURRENCY LOOKUP AREA
        ////// ///////

        public async Task LoadCurrenciesForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='transactioncurrency'>
                                    <attribute name='transactioncurrencyid' alias='Val'/>
                                    <attribute name='currencyname' alias='Label'/>
                                    <order attribute='currencyname' descending='false' />
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("transactioncurrencies", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            foreach (var x in result.value)
            {
                list_currency_lookup.Add(x);
            }
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


        ////////// CAMPAIGN LOOKP AREA
        ////

        public async Task LoadCampainsForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='campaign'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='campaignid' alias='Val'/>
                                    <order attribute='name' descending='true' />
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("campaigns", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            foreach (var x in result.value)
            {
                list_campaign_lookup.Add(x);
            }
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
            string fetch = $@"<fetch version='1.0' count='3' page='{PageNhuCauDiaDiem}' output-format='xml-platform' mapping='logical' distinct='false'>
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

            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreNhuCauDiaDiem = false;
            }
            else
            {
                ShowMoreNhuCauDiaDiem = true;
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

        public async Task dudiemdanhgia()
        {
            //dieu kien bsd_group la khach hang va chi 1 dong diem danh gia id = 922F71AA-311E-E911-A984-000D3AA04C17 (quy dinh chi co 1 dong theo group)
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_systemsetup'>                                
                                <attribute name='bsd_totalleadsratingpoint' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                  <condition attribute='bsd_group' operator='eq' value='100000000' />
                                </filter>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Diemdanhgia>>("bsd_systemsetups", fetch);
            var tmp = result.value.FirstOrDefault(); ;
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            this.Dudiemdanhgia = tmp;
        }

        public async Task Load_DanhSachDuAn(string leadid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDuAnQuanTam}' output-format='xml-platform' mapping='logical' distinct='false'>
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

            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreDuAnQuanTam = false;
            }
            else
            {
                ShowMoreDuAnQuanTam = true;
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

        public async Task LoadCountryForLookup()
        {
            string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_country + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_country'>
                                    <attribute name='bsd_countryname' alias='Name'/>
                                    <attribute name='bsd_countryid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_countryname' descending='true' />
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("bsd_countries", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            if (result.value.Count == 0) return;

            foreach (var x in result.value)
            {
                list_country_lookup.Add(x);
            }

        }

        public async Task loadProvincesForLookup(string countryId)
        {
            string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_province + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='new_province'>
                                    <attribute name='bsd_provincename' alias='Name'/>
                                    <attribute name='new_provinceid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_provincename' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_country' operator='eq' value='" + countryId + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_provinces", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            if (result.value.Count == 0) return;

            foreach (var x in result.value)
            {
                list_province_lookup.Add(x);
            }

        }

        public async Task loadDistrictForLookup(string provinceId)
        {
            string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_district + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='new_district'>
                                <attribute name='new_name' alias='Name'/>
                                <attribute name='new_districtid' alias='Id'/>
                                <attribute name='bsd_nameen' alias='Detail'/>
                                <order attribute='new_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='new_province' operator='eq' value='" + provinceId + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_districts", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            if (result.value.Count == 0) return;

            foreach (var x in result.value)
            {
                list_district_lookup.Add(x);
            }

        }       
    }
}