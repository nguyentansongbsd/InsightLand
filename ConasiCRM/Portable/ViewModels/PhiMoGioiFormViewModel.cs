using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhiMoGioiFormViewModel : BaseViewModel
    {
        private PhiMoGioiFormModel _phiMoiGioiFormModel;
        public PhiMoGioiFormModel PhiMoGioi
        {
            get => _phiMoiGioiFormModel;
            set
            {
                _phiMoiGioiFormModel = value;
                OnPropertyChanged(nameof(PhiMoGioi));
            }
        }
    }
}
