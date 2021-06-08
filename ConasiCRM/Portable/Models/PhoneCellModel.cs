﻿using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class PhoneCellModel : BaseViewModel
    {
        public Guid activityid { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public DateTime? _scheduledstart;
        public DateTime? scheduledstart
        {
            get => this._scheduledstart;
            set
            {
                if (_scheduledstart != value)
                {
                    _scheduledstart = value;
                    OnPropertyChanged(nameof(scheduledstart));
                }
            }
        }
        public TimeSpan _timeStart { get; set; }
        public TimeSpan timeStart
        {
            get => this._timeStart;
            set
            {
                if (_timeStart != value)
                {
                    _timeStart = value;
                    OnPropertyChanged(nameof(timeStart));
                }
            }
        }
        public DateTime? _scheduledend;
        public DateTime? scheduledend
        {
            get => this._scheduledend;
            set
            {
                if (_scheduledend != value)
                {
                    _scheduledend = value;
                    OnPropertyChanged(nameof(scheduledend));
                }
            }
        }

        public TimeSpan _timeEnd;
        public TimeSpan timeEnd
        {
            get => this._timeEnd;
            set
            {
                if (_timeEnd != value)
                {
                    _timeEnd = value;
                    OnPropertyChanged(nameof(timeEnd));
                }
            }
        }
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }
        public int actualdurationminutes { get; set; }
        public OptionSet _durationValue;
        public OptionSet durationValue
        {
            get => _durationValue;
            set
            {
                if (_durationValue != value)
                {
                    _durationValue = value;
                    OnPropertyChanged(nameof(durationValue));
                }
            }
        }
        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public Guid lead_id { get; set; }
        public string lead_name { get; set; }

        public Guid user_id { get; set; }
        public string user_name { get; set; }
        public LookUp Lead
        {
            set
            {
                if (value != null)
                {
                    Customer = new CustomerLookUp()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Type = 3
                    };
                }
            }
        }
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

        // Regarding (liên quan đến)
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
        public int statecode { get; set; }
        public OptionSet _statecodeValue;
        public OptionSet statecodeValue
        {
            get => _statecodeValue;
            set
            {
                if (_statecodeValue != value)
                {
                    _statecodeValue = value;
                    OnPropertyChanged(nameof(statecodeValue));
                }
            }
        }
        public int statuscode { get; set; }
        public OptionSet _statuscodeValue;
        public OptionSet statuscodeValue
        {
            get => _statuscodeValue;
            set
            {
                if (_statuscodeValue != value)
                {
                    _statuscodeValue = value;
                    OnPropertyChanged(nameof(statuscodeValue));
                }
            }
        }
        public DateTime createdon { get; set; }

        public bool _editable;
        public bool editable
        {
            get => _editable;
            set
            {
                if (_editable != value)
                {
                    _editable = value;
                    OnPropertyChanged(nameof(editable));
                }
            }
        }
        public string phonenumber { get; set; }
        public bool directioncode { get; set; }

        // call from

    }
}
