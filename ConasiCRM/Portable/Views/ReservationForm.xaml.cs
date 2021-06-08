using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;
using Telerik.XamarinForms.Primitives;
using Telerik.XamarinForms.Input;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationForm : ContentPage
    {
        private readonly ReservationFormViewModel viewModel;
        private Guid ReservationId;

        public ReservationForm(Guid reservationID)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ReservationFormViewModel();
            viewModel.IsBusy = true;
            this.ReservationId = reservationID;
            viewModel.ModalLookUp = modalLookUp;
            viewModel.InitializeModal();
            viewModel.AfterLookUpClose += AfterLookUpClose;
            Load();

            #region LoadCoOwners
            MessagingCenter.Subscribe<CoOwnerForm, bool>(this, "LoadCoOwners", async (s, arg) =>
            {
                viewModel.IsBusy = true;
                viewModel.CoownerList.Clear();
                await LoadCoOwners();
                viewModel.IsBusy = false;
            });
            #endregion

            #region LoadHandoverConditions
            MessagingCenter.Subscribe<ReservationFormViewModel, bool>(this, "LoadHandoverConditions", async (s, arg) =>
            {
                viewModel.HandoverConditionList.Clear();
                await LoadhandoverConditions();
                viewModel.IsBusy = false;
            });
            #endregion

            #region LoadPromotions
            MessagingCenter.Subscribe<ReservationFormViewModel, bool>(this, "LoadPromotions", async (s, arg) =>
            {
                viewModel.PromotionList.Clear();
                await LoadPromotions();
                viewModel.IsBusy = false;
            });
            #endregion

            #region LoadSpecialDiscounts
            MessagingCenter.Subscribe<SpecialDiscountForm, bool>(this, "LoadSpecialDiscounts", async (s, arg) =>
            {
                viewModel.SpecialDiscountList.Clear();
                await LoadSpecialDiscounts();
                viewModel.IsBusy = false;
            });
            #endregion            
        }

        public async void Load()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='quote'>
                <attribute name='name' />
                <attribute name='quoteid' />
                <attribute name='customerid' />
                <attribute name='quotenumber' />
                <attribute name='bsd_quotationnumber' />
                <attribute name='bsd_quotecodesams' />
                <attribute name='bsd_xacnhanckmuasi' />
                <attribute name='statuscode' />

                <attribute name='bsd_bookingfee' />
                <attribute name='bsd_depositfee' />

                <attribute name='bsd_detailamount' />
                <attribute name='bsd_discount' />
                <attribute name='bsd_packagesellingamount' />
                <attribute name='bsd_totalamountlessfreight' />
                <attribute name='bsd_landvaluededuction' />
                <attribute name='totaltax' />
                <attribute name='bsd_freightamount' />
                <attribute name='totalamount' />

                <attribute name='bsd_numberofmonthspaidmf' />
                <attribute name='bsd_managementfee' />
                <attribute name='bsd_discounts' />
                <order attribute='createdon' descending='true' />
                <filter type='and'>
                  <condition attribute='quoteid' operator='eq' uitype='quote' value='" + ReservationId + @"' />
                </filter>
                <link-entity name='product' from='productid' to='bsd_unitno' link-type='inner' alias='aa'>
                      <attribute name='name' alias='unit_name' />
                      <attribute name='statuscode' alias='unit_statuscode' />
                      <attribute name='bsd_netsaleablearea' alias='unit_netsaleablearea' />
                      <attribute name='bsd_constructionarea' alias='unit_constructionarea' />
                </link-entity>
                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_f6c625518704e911a98b000d3aa2e890'>
                      <attribute name='contactid' alias='contact_id' />
                      <attribute name='bsd_fullname' alias='contact_name' />
                    </link-entity>
                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a_b1c625518704e911a98b000d3aa2e890'>
                      <attribute name='accountid' alias='account_id' />
                      <attribute name='bsd_name' alias='account_name' />
                </link-entity>
                <link-entity name='opportunity' from='opportunityid' to='opportunityid' visible='false' link-type='outer' alias='a_a2ff24578704e911a98b000d3aa2e890'>
                     <attribute name='bsd_queuenumber' alias='queuenumber' />
                </link-entity>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a_ae0b05eeb214e911a97f000d3aa04914'>
                     <attribute name='bsd_name' alias='project_name' />
                </link-entity>
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='a_dab1e1e7b214e911a97f000d3aa04914'>
                     <attribute name='bsd_phaseslaunchid' alias='phaseslaunch_id' />
                     <attribute name='bsd_name' alias='phaseslaunch_name' />
                </link-entity>
                <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' visible='false' link-type='outer' alias='a_060025578704e911a98b000d3aa2e890'>
                      <attribute name='name' alias='pricelevel_name' />
                </link-entity>
                <link-entity name='bsd_paymentscheme' from='bsd_paymentschemeid' to='bsd_paymentscheme' visible='false' link-type='outer' alias='a_8524eae1b214e911a97f000d3aa04914'>
                      <attribute name='bsd_paymentschemeid' alias='paymentscheme_id' />
                      <attribute name='bsd_name' alias='paymentscheme_name' />
                </link-entity>
                <link-entity name='bsd_taxcode' from='bsd_taxcodeid' to='bsd_taxcode' visible='false' link-type='outer' alias='a_120c05eeb214e911a97f000d3aa04914'>
                       <attribute name='bsd_value' alias='tax' />
                 </link-entity>
                <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discountlist' visible='false' link-type='outer' alias='a_f577eedbb214e911a97f000d3aa04914'>
                     <attribute name='bsd_discounttypeid' alias='discountlist_id'  />
                     <attribute name='bsd_name' alias='discountlist_name' />
                </link-entity>
              </entity>
            </fetch>";
            // join với contact + account de lấy name.
            // join với product để lấy unit name, unit status có sẵn trên quote.
            // join với queue để lấy mã phiếu đặt chô
            // joi project, dot mo ban, bang gia, lich thanh toan
            // join voi bsd_taxcode de lay phan tram thue.
            // join voi discount list dể lấy tên chiết khấu và mã, dugnf để lấy danh sách discounts..

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationFormModel>>("quotes", fetchXml);
            var reservation = result.value.SingleOrDefault();

            if (reservation == null)
            {
                await DisplayAlert("", "Lỗi, không tìm thấy Reservation", "Đóng");
                return;
            }

            if (reservation.paymentscheme_id != Guid.Empty)
            {
                viewModel.PaymentScheme = new LookUp()
                {
                    Id = reservation.paymentscheme_id,
                    Name = reservation.paymentscheme_name
                };
            }
            if (reservation.discountlist_id == Guid.Empty)
            {
                ExpanderThongTinChietKhau.IsVisible = false;
            }

            if (reservation.contact_id != Guid.Empty)
            {
                viewModel.Customer = new CustomerLookUp()
                {
                    Id = reservation.contact_id,
                    Name = reservation.contact_name,
                    Type = 1
                };
            }
            else if (reservation.account_id != Guid.Empty)
            {
                viewModel.Customer = new CustomerLookUp()
                {
                    Id = reservation.account_id,
                    Name = reservation.account_name,
                    Type = 2
                };
            }

            viewModel.Reservation = reservation;

            var tasks = new Task[6]
            {
                LoadCoOwners(),
                LoadhandoverConditions(),
                LoadPromotions(),
                LoadSpecialDiscounts(),
                LoadInstallments(),
                LoadDiscounts()
            };
            await Task.WhenAll(tasks);
            //await mainScrollView.ScrollToAsync(DiscountInfoExp, ScrollToPosition.Start, false);
            VisibleControls();
            InitButtonGroup();
            viewModel.IsBusy = false;
        }

        private void VisibleControls()
        {
            if (viewModel.Reservation.queuenumber != null) // từ queu qua ko dc sua khach hang
            {
                btnOpenLookUpCustomer.IsEnable = false; // khi chon lich thanh toan xong thi cung set lai thanh false.
                btnSaveForm.IsVisible = false;
            }

            var status = viewModel.Reservation.statuscode;

            if (status != 100000007) // khác quotation.
            {
                // ẩn mấy nút trên grid.
                btnViewCoOwner.IsVisible = btnNewCoOwner.IsVisible = btnDeleteCoOwner.IsVisible = false;
                btnNewHandoverCondition.IsVisible = btnDeleteHandoverCondition.IsVisible = false;
                btnOpenNewPromotion.IsVisible = btnDeletePromotion.IsVisible = false;
                btnViewSpecialDiscount.IsVisible = btnOpenNewSpecialDiscount.IsVisible = btnDeleteSpecicalDiscount.IsVisible = false;

                // disabled lookup.
                btnOpenLookUpCustomer.IsEnable = false; // khi chon lich thanh toan xong thi cung set lai thanh false.
                LookUpLichThanhToan.IsEnable = false;
                btnSaveForm.IsVisible = false;
            }

            // Nút tạo lịch thanh toán.
            btnPaymentSchemeGenerate.IsVisible = btnSignQuotation.IsVisible = status == 100000007;// Quotation

            // Nút hủy đặt cọc, Trạng thái  =  (In Progress, Reservation,Collected,Quotation),  va khác Expired Quotation va khác Reject
            btnCancelQuotation.IsVisible = (status == 1 || status == 100000000 || status == 100000006 || status == 100000007) && status != 100000008 && status != 100000003;
        }

        // set width cho button.
        private void InitButtonGroup()
        {
            gridBtnGroup.ColumnDefinitions.Clear();
            var buttons = gridBtnGroup.Children.Where(x => x is RadButton && x.IsVisible == true).ToList();
            var count = buttons.Count();
            for (int i = 0; i < count; i++)
            {
                gridBtnGroup.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
                Grid.SetColumn(buttons[i], i);
            }
        }

        public async Task LoadInstallments()
        {
            var installment_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationInstallmentModel>>("bsd_paymentschemedetails", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_paymentschemedetail'>
                <attribute name='bsd_paymentschemedetailid' />
                <attribute name='bsd_ordernumber' />
                <attribute name='bsd_name' />
                <attribute name='bsd_duedate' />
                <attribute name='statuscode' />
                <attribute name='bsd_amountofthisphase' />
                <attribute name='bsd_amountwaspaid' />
                <attribute name='bsd_depositamount' />
                <attribute name='bsd_waiveramount' />
                <order attribute='bsd_ordernumber' descending='false' />
                <filter type='and'>
                  <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='" + ReservationId + @"' />
                </filter>
              </entity>
            </fetch>");
            var installmentList = installment_result.value.OrderBy(x => x.bsd_ordernumber).ToList();
            var installmentCount = installmentList.Count;
            if (installmentCount > 0)
            {
                viewModel.DaTaoLichThanhToan = true;
                btnOpenLookUpCustomer.IsEnable = false;
                btnSaveForm.IsVisible = false;
                for (int i = 0; i < installmentCount; i++)
                {
                    viewModel.InstallmentList.Add(installmentList[i]);
                }
            }
            for (int i = installmentCount + 1; i < 4; i++)
            {
                viewModel.InstallmentList.Add(new ReservationInstallmentModel());
            }
        }

        public async Task LoadCoOwners()
        {
            var coowner_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationCoowner>>("bsd_coowners", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_coowner'>
                <attribute name='bsd_coownerid' />
                <attribute name='bsd_name' />
                <attribute name='bsd_relationship' />
                <attribute name='createdon' />
                <order attribute='bsd_name' descending='false' />
                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='a_1324f6d5b214e911a97f000d3aa04914'>
                  <attribute name='bsd_name' alias='account_name' />
                </link-entity>
                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='a_6b0d05eeb214e911a97f000d3aa04914'>
                  <attribute name='bsd_fullname' alias='contact_name' />
                </link-entity>
                 <filter type='and'>
                      <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='" + ReservationId + @"' />
                  </filter>
              </entity>
            </fetch>");
            var coownerList = coowner_result.value;
            var coownerCount = coownerList.Count;
            if (coownerCount > 0)
            {
                for (int i = 0; i < coownerCount; i++)
                {
                    var item = coownerList[i];
                    item.ReservationName = viewModel.Reservation.name;

                    viewModel.CoownerList.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    viewModel.CoownerList.Add(new ReservationCoowner());
                }
            }
        }

        public async Task LoadhandoverConditions()
        {
            var handoverConditionResult = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationHandoverCondition>>("bsd_packagesellings", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_packageselling'>
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_type' />
                <attribute name='bsd_priceperm2' />
                <attribute name='bsd_amount' />
                <attribute name='bsd_percent' />
                <attribute name='bsd_method' />
                <attribute name='bsd_packagesellingid' />
                <order attribute='createdon' descending='true' />
                <link-entity name='bsd_quote_bsd_packageselling' from='bsd_packagesellingid' to='bsd_packagesellingid' visible='false' intersect='true'>
                <link-entity name='quote' from='quoteid' to='quoteid' alias='ab'>
                     <filter type='and'>
                          <condition attribute='quoteid' operator='eq' uitype='quote' value='" + ReservationId + @"' />
                     </filter>
                </link-entity>
            </link-entity>
              </entity>
            </fetch>");
            var handoverConditionList = handoverConditionResult.value;
            var handoverConditionCount = handoverConditionList.Count;
            if (handoverConditionCount > 0)
            {
                for (int i = 0; i < handoverConditionCount; i++)
                {
                    viewModel.HandoverConditionList.Add(handoverConditionList[i]);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    viewModel.HandoverConditionList.Add(new ReservationHandoverCondition());
                }
            }
        }

        public async Task LoadPromotions()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
              <entity name='bsd_promotion'>
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_values' />
                <attribute name='statuscode' />
                <attribute name='bsd_startdate' />
                <attribute name='ownerid' />
                <attribute name='bsd_enddate' />
                <attribute name='bsd_description' />
                <attribute name='bsd_promotionid' />
                <order attribute='createdon' descending='true' />
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='inner' alias='a_c72b74f6fa82e61180f23863bb367d40'>
                      <attribute name='bsd_name' alias='phaseslaunch_name' />
                      <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='inner' alias='ad'>
                           <attribute name='bsd_name' alias='project_name' />                        
                        </link-entity>
                </link-entity>
                <link-entity name='bsd_quote_bsd_promotion' from='bsd_promotionid' to='bsd_promotionid' visible='false' intersect='true'>
                      <link-entity name='quote' from='quoteid' to='quoteid' alias='ae'>
                             <filter type='and'>
                                    <condition attribute='quoteid' operator='eq' uiname='A,20.10' uitype='quote' value='" + ReservationId + @"' />
                              </filter>
                      </link-entity>
                </link-entity>
              </entity>
            </fetch>";
            var promotionsResult = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationPromotionModel>>("bsd_promotions", xml);
            var promotionList = promotionsResult.value;
            var promotionCount = promotionList.Count;
            if (promotionCount > 0)
            {
                for (int i = 0; i < promotionCount; i++)
                {
                    viewModel.PromotionList.Add(promotionList[i]);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    viewModel.PromotionList.Add(new ReservationPromotionModel());
                }
            }

        }

        public async Task LoadSpecialDiscounts()
        {
            var xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_discountspecial'>
                <attribute name='bsd_discountspecialid' />
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_reasons' />
                <attribute name='statuscode' />
                <attribute name='bsd_percentdiscount' />
                <attribute name='bsd_amountdiscount' />
                <order attribute='bsd_name' descending='false' />
                <filter type='and'>
                  <condition attribute='bsd_quote' operator='eq' uiname='A,20.10' uitype='quote' value='" + ReservationId + @"' />
                </filter>
                <link-entity name='systemuser' from='systemuserid' to='createdby' visible='false' link-type='outer' alias='a_769c3b2db214e911a97f000d3aa04914'>
                  <attribute name='fullname' alias='createdby_name' />
                </link-entity>
              </entity>
            </fetch>";

            var discountSpecialsResult = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationSpecialDiscountListModel>>("bsd_discountspecials", xml);
            var discountSpecialList = discountSpecialsResult.value;
            var discountSpecialListCount = discountSpecialList.Count;

            if (discountSpecialListCount > 0)
            {
                for (int i = 0; i < discountSpecialListCount; i++)
                {
                    var item = discountSpecialList[i];
                    item.reservation_name = viewModel.Reservation.name;
                    viewModel.SpecialDiscountList.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    viewModel.SpecialDiscountList.Add(new ReservationSpecialDiscountListModel());
                }
            }
        }

        public async Task LoadDiscounts()
        {
            if (viewModel.Reservation.discountlist_id != Guid.Empty)
            {
                string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                  <entity name='bsd_discount'>
                    <attribute name='bsd_discountid' alias='Val' />
                    <attribute name='bsd_name' alias='Label' />
                    <attribute name='createdon' />
                    <attribute name='bsd_startdate' />
                    <attribute name='bsd_enddate' />
                    <order attribute='bsd_name' descending='false' />
                    <link-entity name='bsd_bsd_discounttype_bsd_discount' from='bsd_discountid' to='bsd_discountid' visible='false' intersect='true'>
                      <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discounttypeid' alias='ab'>
                        <filter type='and'>
                          <condition attribute='bsd_discounttypeid' operator='eq' uitype='bsd_discounttype' value='" + viewModel.Reservation.discountlist_id + @"' />
                        </filter>
                      </link-entity>
                    </link-entity>
                  </entity>
                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationDiscountOptionSet>>("bsd_discounts", fetchXml);
                if (result != null)
                {
                    var list = result.value;
                    var count = list.Count;

                    string[] checkedList = new string[] { };
                    if (!string.IsNullOrWhiteSpace(viewModel.Reservation.bsd_discounts))
                    {
                        checkedList = viewModel.Reservation.bsd_discounts.Split(',');
                    }

                    for (int i = 0; i < count; i++)
                    {
                        var item = list[i];
                        if (DateTime.Now < item.bsd_startdate || DateTime.Now > item.bsd_enddate)
                        {
                            item.IsExpired = true;
                        }
                        if (checkedList.Any(x => x == item.Val))
                        {
                            item.Selected = true;
                        }
                        viewModel.Discounts.Add(item);
                    }
                    listView.ItemsSource = viewModel.Discounts;
                }
            }
        }

        private async void PaymentSchemeGenerate_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Customer == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn và lưu Khách hàng", "Đóng");
                return;
            }
            if (viewModel.PaymentScheme == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn Lịch thanh toán", "Đóng");
                return;
            }

            bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn tạo lịch thanh toán không ?", "Đồng ý", "Hủy");
            if (confirm == false) return;
            viewModel.IsBusy = true;
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_paymentscheme@odata.bind"] = $"/bsd_paymentschemes({viewModel.PaymentScheme.Id.ToString()})";

            CrmApiResponse updateResponse = await CrmHelper.PatchData($"/quotes({ReservationId})", data);
            if (updateResponse.IsSuccess)
            {
                // sau khi cập nhật lại field lịch thanh toán. tiến hành gọi action để generate ra chi tiết lịch thanh toán.
                updateResponse = await CrmHelper.PostData($"/quotes({ReservationId})/Microsoft.Dynamics.CRM.bsd_Action_Resv_Gene_PMS");
                if (updateResponse.IsSuccess)
                {
                    await DisplayAlert("Thông báo", "Tạo lịch thanh toán thành công.", "Đóng");
                    viewModel.InstallmentList.Clear();
                    await LoadInstallments();
                    InitButtonGroup();
                }
                else
                {
                    await DisplayAlert("Lỗi", updateResponse.GetErrorMessage(), "Đóng");
                }
            }
            else
            {
                await DisplayAlert("Lỗi", updateResponse.GetErrorMessage(), "Đóng");
            }
            viewModel.IsBusy = false;
        }

        //xác nhận đạt cọc
        private async void SignQuotation_Clicked(object sender, EventArgs e)
        {
            if (viewModel.PaymentScheme == null || viewModel.InstallmentList.Any(x => x.bsd_name != null) == false)
            {
                await DisplayAlert("Thông báo", "Vui lòng tạo lịch thanh toán.", "Đóng");
                return;
            }
            bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn xác nhận đặt cọc không ?", "Đồng ý", "Hủy");
            if (confirm == false) return;
            // mở khung chọn ngày.
            QuotationSignedDatePicker.Focus();
        }

        //khi chọn ngày sign quotation.
        private async void QuotationSignedDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            var quotaionSignedDate = QuotationSignedDatePicker.Date;
            viewModel.IsBusy = true;
            var signQuoteResponse = await CrmHelper.PostData($"/quotes({ReservationId})/Microsoft.Dynamics.CRM.bsd_Action_QuotationReservation_ConvertToReservation_Mobile", new
            {
                bsd_quotationsigneddate = quotaionSignedDate
            });
            if (signQuoteResponse.IsSuccess)
            {
                await Navigation.PushAsync(new ReservationForm(ReservationId));
                Navigation.RemovePage(this);
            }
            else
            {
                await DisplayAlert("Thông báo", signQuoteResponse.GetErrorMessage(), "Đóng");
            }
            viewModel.IsBusy = false;
        }

        // Hủy đặt cọc
        private async void BtnCancelQuotation_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn hủy đặt cọc không ?", "Đồng ý", "Hủy");
            if (confirm == false) return;
            viewModel.IsBusy = true;
            var cancleQuotationResponse = await CrmHelper.PostData($"/quotes({ReservationId})/Microsoft.Dynamics.CRM.bsd_Action_Reservation_Cancel");
            if (cancleQuotationResponse.IsSuccess)
            {
                await Navigation.PushAsync(new ReservationForm(ReservationId));
                Navigation.RemovePage(this);
            }
            else
            {
                await DisplayAlert("Thông báo", cancleQuotationResponse.GetErrorMessage(), "Đóng");
            }
            viewModel.IsBusy = false;
        }

        // Mở form tạo Người đồng sở hữu
        private async void BtnNewCoOwner_Clicked(object sender, EventArgs e)
        {
            //var test = await DisplayActionSheet("Cawn ho", "Đóng", null, "Xem thông tin căn hộ", "Tạo giữ chỗ", "Tạo đặt cọc");
            //await DisplayAlert("", test, "Dong");
            await Navigation.PushAsync(new CoOwnerForm(new LookUp()
            {
                Id = ReservationId,
                Name = viewModel.Reservation.name
            }));
        }

        private async void ViewCoOwner_Clicked(object sender, EventArgs e)
        {
            if (gridCoOwner.SelectedItem == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn Người đồng sở hữu muốn xóa", "Đóng");
                return;
            }

            var item = gridCoOwner.SelectedItem as ReservationCoowner;

            await Navigation.PushAsync(new CoOwnerForm(item.bsd_coownerid));
        }

        // Xóa người đồng sở hữu
        private async void BtnDeleteCoOwner_Clicked(object sender, EventArgs e)
        {
            if (gridCoOwner.SelectedItem == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn Người đồng sở hữu muốn xóa", "Đóng");
                return;
            }

            var conform = await DisplayAlert("Xác nhận", "Bạn có muốn xóa người đồng sở hữu này không ?", "Đồng ý", "Hủy");
            if (conform == false) return;
            viewModel.IsBusy = true;
            ReservationCoowner item = gridCoOwner.SelectedItem as ReservationCoowner;
            var deleteResponse = await CrmHelper.DeleteRecord($"/bsd_coowners({item.bsd_coownerid})");
            if (deleteResponse.IsSuccess)
            {
                // bỏ chọn vì đã xóa, ko là nó vẫn lưu
                gridCoOwner.SelectedItem = null;
                this.viewModel.CoownerList.Clear();
                await this.LoadCoOwners();
            }
            else
            {
                await DisplayAlert("Thông báo", deleteResponse.GetErrorMessage(), "Đóng");
            }
            viewModel.IsBusy = false;
        }

        // Mở form tạo điều kiện bàn giao
        private void BtnNewHandoverCondition_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            if (viewModel.HandoverConditionConfig.FetchXml == null)
            {
                viewModel.HandoverConditionConfig.FetchXml = @"<fetch count='20' page='{0}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='bsd_packageselling'>
                    <attribute name='bsd_name' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <attribute name='bsd_packagesellingid' alias='Id' />
                    <order attribute='createdon' descending='true' />
                    <filter type='and'>
                          <condition attribute='statuscode' operator='eq' value='1' />
                          <condition attribute='statecode' operator='eq' value='0' />
                          <condition attribute='bsd_name' operator='like' value='%{1}%' />
                     </filter>
                    <link-entity name='bsd_bsd_phaseslaunch_bsd_packageselling' from='bsd_packagesellingid' to='bsd_packagesellingid' visible='false' intersect='true'>
                          <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' alias='aa'>
                            <filter type='and'>
                              <condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='" + viewModel.Reservation.phaseslaunch_id + @"' />
                            </filter>
                          </link-entity>
                     </link-entity>
                  </entity>
                </fetch>";
            }

            viewModel.CurrentLookUpConfig = viewModel.HandoverConditionConfig;
            viewModel.ProcessLookup(nameof(viewModel.HandoverConditionConfig));
        }

        // Mở popup lịch thanh toán
        private void LichThanhToan_OpenClicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.CurrentLookUpConfig = viewModel.PaymentschemeConfig;
            viewModel.ProcessLookup(nameof(viewModel.PaymentschemeConfig));
        }

        // Xóa điều kiện bàn giao
        private async void BtnDeleteHandoverCondition_Clicked(object sender, EventArgs e)
        {
            if (gridHandoverCondition.SelectedItem == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn điều kiện bàn giao muốn xóa", "Đóng");
                return;
            }

            var conform = await DisplayAlert("Xác nhận", "Bạn có muốn xóa điều kiện bàn giao này không ?", "Đồng ý", "Hủy");
            if (conform == false) return;
            viewModel.IsBusy = true;
            ReservationHandoverCondition item = gridHandoverCondition.SelectedItem as ReservationHandoverCondition;
            var deleteResponse = await CrmHelper.DeleteRecord($"/quotes({viewModel.Reservation.quoteid})/bsd_quote_bsd_packageselling({item.bsd_packagesellingid})/$ref");
            if (deleteResponse.IsSuccess)
            {
                // bỏ chọn vì đã xóa, ko là nó vẫn lưu
                gridHandoverCondition.SelectedItem = null;
                this.viewModel.HandoverConditionList.Clear();
                await this.LoadhandoverConditions();
            }
            else
            {
                await DisplayAlert("Thông báo", deleteResponse.GetErrorMessage(), "Đóng");
            }
            viewModel.IsBusy = false;
        }

        // Mở pop up add Khuyến mãi.
        private void BtnOpenNewPromotion_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            if (viewModel.PromotionConfig.FetchXml == null)
            {
                viewModel.PromotionConfig.FetchXml = @"<fetch count='20' page='{0}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='bsd_promotion'>
                    <attribute name='bsd_name' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <attribute name='bsd_promotionid' alias='Id' />
                    <order attribute='createdon' descending='true' />
                    <filter type='and'>
                          <condition attribute='statuscode' operator='eq' value='100000001' />
                          <condition attribute='statecode' operator='eq' value='0' />
                          <condition attribute='bsd_name' operator='like' value='%{1}%' />
                    </filter>
                      <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='inner' alias='a_c72b74f6fa82e61180f23863bb367d40'>
                          <attribute name='bsd_projectid' />
                          <filter type='and'>
                            <condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='" + viewModel.Reservation.phaseslaunch_id + @"' />
                          </filter>
                        </link-entity>
                  </entity>
                </fetch>";
            }

            viewModel.CurrentLookUpConfig = viewModel.PromotionConfig;
            viewModel.ProcessLookup(nameof(viewModel.PromotionConfig));
        }

        // Xóa khuyến mãi
        private async void BtnDeletePromotion_Clicked(object sender, EventArgs e)
        {
            if (gridPromotions.SelectedItem == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn khuyến mại muốn xóa", "Đóng");
                return;
            }

            var conform = await DisplayAlert("Xác nhận", "Bạn có muốn xóa khuyến mại này không ?", "Đồng ý", "Hủy");
            if (conform == false) return;
            viewModel.IsBusy = true;
            ReservationPromotionModel item = gridPromotions.SelectedItem as ReservationPromotionModel;
            var deleteResponse = await CrmHelper.DeleteRecord($"/quotes({viewModel.Reservation.quoteid})/bsd_quote_bsd_promotion({item.bsd_promotionid})/$ref");
            if (deleteResponse.IsSuccess)
            {
                // bỏ chọn vì đã xóa, ko là nó vẫn lưu
                gridPromotions.SelectedItem = null;
                this.viewModel.PromotionList.Clear();
                await this.LoadPromotions();
            }
            else
            {
                await DisplayAlert("Thông báo", deleteResponse.GetErrorMessage(), "Đóng");
            }
            viewModel.IsBusy = false;
        }

        // xóa chiết khấu đặc biệt
        private async void BtnDeleteSpecicalDiscount_Clicked(object sender, EventArgs e)
        {
            if (gridSpecialDiscount.SelectedItem == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn chiết khấu muốn xóa", "Đóng");
                return;
            }

            var conform = await DisplayAlert("Xác nhận", "Bạn có muốn xóa chiết khấu này không ?", "Đồng ý", "Hủy");
            if (conform == false) return;
            viewModel.IsBusy = true;
            ReservationSpecialDiscountListModel item = gridSpecialDiscount.SelectedItem as ReservationSpecialDiscountListModel;
            var deleteResponse = await CrmHelper.DeleteRecord($"/bsd_discountspecials({item.bsd_discountspecialid})");
            if (deleteResponse.IsSuccess)
            {
                // bỏ chọn vì đã xóa, ko là nó vẫn lưu
                gridSpecialDiscount.SelectedItem = null;
                this.viewModel.SpecialDiscountList.Clear();
                await this.LoadSpecialDiscounts();
            }
            else
            {
                await DisplayAlert("Thông báo", deleteResponse.GetErrorMessage(), "Đóng");
            }
            viewModel.IsBusy = false;
        }

        // mở form thêm chiết khấu đặc biệt
        private async void BtnOpenNewSpecialDiscount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SpecialDiscountForm(new LookUp()
            {
                Id = ReservationId,
                Name = viewModel.Reservation.name
            }));
        }

        // xem chiết khấu đặc biệt
        private async void BtnViewSpecicalDiscount_Clicked(object sender, EventArgs e)
        {
            if (gridSpecialDiscount.SelectedItem == null)
            {
                await DisplayAlert("Thông báo", "Chọn chiết khấu muốn xem", "Đóng");
                return;
            }

            var selectedItem = gridSpecialDiscount.SelectedItem as ReservationSpecialDiscountListModel;
            await Navigation.PushAsync(new SpecialDiscountForm(selectedItem.bsd_discountspecialid));
        }

        // khi check chon discount trong thong tin chiet khau
        private void DiscountsListView_ItemTapped(object sender, EventArgs e)
        {
            var stacklayout = (StackLayout)sender;
            var tapGes = stacklayout.GestureRecognizers[0] as TapGestureRecognizer;
            var item = (ReservationDiscountOptionSet)tapGes.CommandParameter;
            if (item.IsExpired || viewModel.Reservation.statuscode != 100000007) return; // hết hạn, hoặc là quote không phải trạng thái quotation.

            // Toggle lại giá trị true/false
            item.Selected = !item.Selected;

            // generate lai string cach nhau dau phay, de luu vao field discounts.
            viewModel.Reservation.bsd_discounts = string.Join(",", viewModel.Discounts.Where(x => x.Selected).Select(x => x.Val).ToArray());

            // hiển nút lưu.
            if (btnUpdateDiscounts.IsVisible == false)
            {
                btnUpdateDiscounts.IsVisible = true;
            }
        }

        // Cập nhật lại check khấu khi check chọn.
        private async void BtnUpdateDiscounts_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            var data = new Dictionary<string, object>();
            data["bsd_discounts"] = viewModel.Reservation.bsd_discounts ?? "";

            var response = await CrmHelper.PatchData($"/quotes({ReservationId})", data);
            if (response.IsSuccess)
            {
                btnUpdateDiscounts.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Thông báo", response.GetErrorMessage(), "Đóng");
            }
            viewModel.IsBusy = false;
        }

        // save form (khách hàng và lịch thanh toán)
        private async void BtnSaveForm_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Customer == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn khách hàng", "Đóng");
                return;
            }
            if (viewModel.DaTaoLichThanhToan)
            {
                await DisplayAlert("Thông báo", "Đã tạo lịch thanh toán, không thể cập nhật báo giá", "Đóng");
                return;
            }
            if (viewModel.Customer != null)
            {
                IDictionary<string, object> data = new Dictionary<string, object>();
                CrmApiResponse clearLookupResponse = new CrmApiResponse()
                {
                    IsSuccess = true
                };
                if (viewModel.Customer.Type == 1)
                {
                    data["customerid_contact@odata.bind"] = $"/contacts({viewModel.Customer.Id})";
                    clearLookupResponse = await CrmHelper.SetNullLookupField("quotes", ReservationId, "customerid_account");
                }
                else
                {
                    data["customerid_account@odata.bind"] = $"/accounts({viewModel.Customer.Id})";
                    clearLookupResponse = await CrmHelper.SetNullLookupField("quotes", ReservationId, "customerid_contact");
                }

                if (clearLookupResponse.IsSuccess)
                {
                    CrmApiResponse res = await CrmHelper.PatchData($"/quotes({ReservationId})", data);
                    if (res.IsSuccess)
                    {
                        await DisplayAlert("Thông báo", "Cập nhật thành công", "Đóng");
                    }
                    else
                    {
                        await DisplayAlert("Thông báo", res.GetErrorMessage(), "close");
                    }
                }
                else
                {
                    await DisplayAlert("Thông báo", clearLookupResponse.GetErrorMessage(), "close");
                }
            }
        }

        // mở lookup khách hàng
        private void BtnOpenLookUpCustomer_OpenClicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.InitCustomerLookUpHeader();
            viewModel.BtnContact.Clicked += ContactOpen;
            viewModel.BtnAccount.Clicked += AccountOpen;
            ContactOpen(viewModel.BtnContact, EventArgs.Empty);
        }

        public void OnSwitch()
        {
            if (viewModel.searchBar.Text != null && viewModel.searchBar.Text.Length > 0)
            {
                viewModel.CurrentLookUpConfig.LookUpData.Clear();
                viewModel.searchBar.Text = null;
            }
        }

        public void ContactOpen(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.gridCustomer.IsVisible = true;
            if (viewModel.AccountLookUpConfig.ListView != null) viewModel.AccountLookUpConfig.ListView.IsVisible = false;
            OnSwitch();
            viewModel.BtnContact.BackgroundColor = Color.FromHex("#999999");
            viewModel.BtnAccount.BackgroundColor = Color.Transparent;
            viewModel.CurrentLookUpConfig = viewModel.ContactLookUpConfig;
            viewModel.ProcessLookup(nameof(viewModel.ContactLookUpConfig));
        }

        public void AccountOpen(object sender, EventArgs e)
        {

            viewModel.IsBusy = true;
            viewModel.gridCustomer.IsVisible = true;
            viewModel.ContactLookUpConfig.ListView.IsVisible = false;
            OnSwitch();
            viewModel.BtnContact.BackgroundColor = Color.Transparent;
            viewModel.BtnAccount.BackgroundColor = Color.FromHex("#999999");
            viewModel.CurrentLookUpConfig = viewModel.AccountLookUpConfig;
            viewModel.ProcessLookup(nameof(viewModel.AccountLookUpConfig));
        }

        public void AfterLookUpClose(object sender, EventArgs e)
        {
            if (viewModel.gridCustomer == null || viewModel.gridCustomer.IsVisible == false) return;
            viewModel.BtnContact.Clicked -= ContactOpen;
            viewModel.BtnAccount.Clicked -= AccountOpen;
            viewModel.gridCustomer.IsVisible = false;
        }
    }
}