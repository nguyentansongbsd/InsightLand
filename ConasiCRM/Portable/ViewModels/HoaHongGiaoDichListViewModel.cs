using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class HoaHongGiaoDichListViewModel : ListViewBaseViewModel<HoaHongGiaoDichListModel>
    {
        private decimal _totalHoaHong;
        public decimal totalHoaHong
        {
            get => _totalHoaHong;
            set
            {
                if (_totalHoaHong != value)
                {
                    _totalHoaHong = value;
                    OnPropertyChanged(nameof(totalHoaHong));
                }
            }
        }
        private decimal _totalHoaHongNhan;
        public decimal totalHoaHongNhan
        {
            get => _totalHoaHongNhan;
            set
            {
                if (_totalHoaHongNhan != value)
                {
                    _totalHoaHongNhan = value;
                    OnPropertyChanged(nameof(totalHoaHongNhan));
                }
            }
        }
        private decimal _totalHoaHongConLai;
        public decimal totalHoaHongConLai
        {
            get => _totalHoaHongConLai;
            set
            {
                if (_totalHoaHongConLai != value)
                {
                    _totalHoaHongConLai = value;
                    OnPropertyChanged(nameof(totalHoaHongConLai));
                }
            }
        }
    }
}
