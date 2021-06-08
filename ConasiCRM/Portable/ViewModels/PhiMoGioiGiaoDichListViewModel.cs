using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhiMoGioiGiaoDichListViewModel : ListViewBaseViewModel<PhiMoGioiGiaoDichListModel>

    {
        private decimal _totalPMG;
        public decimal totalPMG
        {
            get => _totalPMG;
            set
            {
                if (_totalPMG != value)
                {
                    _totalPMG = value;
                    OnPropertyChanged(nameof(totalPMG));
                }
            }
        }
        private decimal _totalPMGNhan;
        public decimal totalPMGNhan
        {
            get => _totalPMGNhan;
            set
            {
                if (_totalPMGNhan != value)
                {
                    _totalPMGNhan = value;
                    OnPropertyChanged(nameof(totalPMGNhan));
                }
            }
        }
        private decimal _totalPMGConLai;
        public decimal totalPMGConLai
        {
            get => _totalPMGConLai;
            set
            {
                if (_totalPMGConLai != value)
                {
                    _totalPMGConLai = value;
                    OnPropertyChanged(nameof(totalPMGConLai));
                }
            }
        }
    }
}
