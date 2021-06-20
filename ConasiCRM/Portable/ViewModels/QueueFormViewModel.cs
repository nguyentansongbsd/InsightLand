using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class QueueFormViewModel : FormLookupViewModel
    {
        private QueueFormModel _queueFormModel;
        public QueueFormModel QueueFormModel
        {
            get => _queueFormModel;
            set
            {
                _queueFormModel = value;
                OnPropertyChanged(nameof(QueueFormModel));
            }
        }

        public LookUpConfig ContactLookUpConfig { get; set; }
        public LookUpConfig AccountLookUpConfig { get; set; }

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

        private CustomerLookUp _khachHangGioiThieu;
        public CustomerLookUp KhachHangGioiThieu
        {
            get => _khachHangGioiThieu;
            set
            {
                if (_khachHangGioiThieu != value)
                {
                    _khachHangGioiThieu = value;
                    OnPropertyChanged(nameof(KhachHangGioiThieu));
                }
            }
        }

        public QueueFormViewModel()
        {
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

            IsBusy = true;
        }
    }
}
