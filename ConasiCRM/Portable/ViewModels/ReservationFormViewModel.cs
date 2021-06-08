using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using System.Linq;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class ReservationFormViewModel : FormLookupViewModel
    {
        private ReservationFormModel _reservation;
        public ReservationFormModel Reservation
        {
            get => _reservation;
            set
            {
                _reservation = value;
                OnPropertyChanged(nameof(Reservation));
            }
        }

        public LookUpConfig PaymentschemeConfig { get; set; }
        public LookUpConfig HandoverConditionConfig { get; set; }
        public LookUpConfig PromotionConfig { get; set; }
        public LookUpConfig ContactLookUpConfig { get; set; }
        public LookUpConfig AccountLookUpConfig { get; set; }
        private LookUp _paymentScheme;
        public LookUp PaymentScheme
        {
            get => _paymentScheme;
            set
            {
                if (value != _paymentScheme)
                {
                    _paymentScheme = value;
                    OnPropertyChanged(nameof(PaymentScheme));
                }
            }
        }

        // khai báo để hứng dữ liệu khi chọn handover condition.
        private LookUp _bsd_packagesellings;
        public LookUp bsd_packagesellings
        {
            get => _bsd_packagesellings;
            set
            {
                if (value != null)
                {
                    IsBusy = true;
                    _bsd_packagesellings = value;
                    AddHandoverCondition();
                }
                else
                {
                    _bsd_packagesellings = value;
                }

            }
        }

        // khai báo để hứng dữ liệu khi chọn Promotion.
        private LookUp _bsd_promotion;
        public LookUp bsd_promotion
        {
            get => _bsd_promotion;
            set
            {
                if (value != null)
                {
                    IsBusy = true;
                    _bsd_promotion = value;
                    AddPromotion();
                }
                else
                {
                    _bsd_promotion = value;
                }

            }
        }

        // contact && account
        public LookUp Contact
        {
            set
            {

                if (value != null)
                {
                    Customer = new CustomerLookUp()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Type = 1
                    };
                }
            }
        }
        public LookUp Account
        {
            set
            {
                if (value != null)
                {
                    Customer = new CustomerLookUp()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Type = 2
                    };

                }

            }
        }
        private CustomerLookUp _customer;
        public CustomerLookUp Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }

        public bool DaTaoLichThanhToan { get; set; }

        // end
        // danh sacsh discount cua thong ctin chiet khau
        public ObservableCollection<ReservationDiscountOptionSet> Discounts { get; set; }

        public ObservableCollection<ReservationInstallmentModel> InstallmentList { get; set; }
        public ObservableCollection<ReservationCoowner> CoownerList { get; set; }
        public ObservableCollection<ReservationHandoverCondition> HandoverConditionList { get; set; }
        public ObservableCollection<ReservationPromotionModel> PromotionList { get; set; }
        public ObservableCollection<ReservationSpecialDiscountListModel> SpecialDiscountList { get; set; }
        public ReservationFormViewModel()
        {
            InstallmentList = new ObservableCollection<ReservationInstallmentModel>();
            CoownerList = new ObservableCollection<ReservationCoowner>();
            HandoverConditionList = new ObservableCollection<ReservationHandoverCondition>();
            PromotionList = new ObservableCollection<ReservationPromotionModel>();
            SpecialDiscountList = new ObservableCollection<ReservationSpecialDiscountListModel>();

            Discounts = new ObservableCollection<ReservationDiscountOptionSet>();

            #region contact va account lookup cònig
            ContactLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='30' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='bsd_fullname' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='bsd_fullname' descending='false' />
                    <filter type='or'>
                          <condition attribute='bsd_fullname' operator='like' value='%{1}%' />
                     </filter>
                  </entity>
                </fetch>",
                EntityName = "contacts",
                PropertyName = "Contact",
                LookUpTitle = "Chọn khách hàng"
            };

            AccountLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='30' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='account'>
                    <attribute name='accountid' alias='Id' />
                    <attribute name='bsd_name' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='bsd_name' descending='false' />
                     <filter type='or'>
                          <condition attribute='bsd_name' operator='like' value='%{1}%' />
                     </filter>
                  </entity>
                </fetch>",
                EntityName = "accounts",
                PropertyName = "Account",
                LookUpTitle = "Chọn khách hàng"
            };
            #endregion

            PaymentschemeConfig = new LookUpConfig();
            PaymentschemeConfig.LookUpTitle = "Chọn lịch thanh toán";
            PaymentschemeConfig.EntityName = "bsd_paymentschemes";
            PaymentschemeConfig.PropertyName = "PaymentScheme";
            PaymentschemeConfig.FetchXml = @"<fetch count='20' page='{0}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_paymentscheme'>
                <attribute name='bsd_name' alias='Name' />
                <attribute name='createdon' alias='Detail' /> 
                <attribute name='bsd_paymentschemeid' alias='Id' />
                <order attribute='bsd_name' descending='false' />
                <filter type='and'>
                      <condition attribute='bsd_name' operator='like' value='%{1}%' />
                 </filter>
              </entity>
            </fetch>";

            // handover condition
            HandoverConditionConfig = new LookUpConfig();
            HandoverConditionConfig.LookUpTitle = "Chọn điều kiện bàn giao";
            HandoverConditionConfig.EntityName = "bsd_packagesellings";
            HandoverConditionConfig.PropertyName = "bsd_packagesellings";

            // promotion
            PromotionConfig = new LookUpConfig();
            PromotionConfig.LookUpTitle = "Chọn khuyến mại";
            PromotionConfig.EntityName = "bsd_promotions";
            PromotionConfig.PropertyName = "bsd_promotion";
        }



        public async void AddHandoverCondition()
        {
            var isExist = this.HandoverConditionList.Any(x => x.bsd_packagesellingid == bsd_packagesellings.Id);
            if (isExist)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", bsd_packagesellings.Name + " đã tồn tại.", "Đóng");
                IsBusy = false; // tren ham set da set thanh true, o day phai set lai thanh false.
                return;
            }

            string path = $"/quotes({Reservation.quoteid})/bsd_quote_bsd_packageselling/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_packagesellings({bsd_packagesellings.Id})";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                MessagingCenter.Send<ReservationFormViewModel, bool>(this, "LoadHandoverConditions", true);
                // ben day chay qua ben kia roi ko can is busy = false nua.
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", result.GetErrorMessage(), "Đóng");
                IsBusy = false;
            }
        }

        private async void AddPromotion()
        {
            var isExist = this.PromotionList.Any(x => x.bsd_promotionid == bsd_promotion.Id);
            if (isExist)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", bsd_promotion.Name + " đã tồn tại.", "Đóng");
                IsBusy = false; // tren ham set da set thanh true, o day phai set lai thanh false.
                return;
            }

            string path = $"/quotes({Reservation.quoteid})/bsd_quote_bsd_promotion/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_promotions({bsd_promotion.Id})";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                MessagingCenter.Send<ReservationFormViewModel, bool>(this, "LoadPromotions", true);
                // ben day chay qua ben kia roi ko can is busy = false nua.
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", result.GetErrorMessage(), "Đóng");
                IsBusy = false;
            }
        }
    }
}
