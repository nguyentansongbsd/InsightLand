using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class TaiLieuKinhDoanhFormViewModel : FormViewModal
    {
        private ListInforUnitTLKD _singleListInforUnitTLKD;
        public ListInforUnitTLKD singleListInforUnitTLKD { get => _singleListInforUnitTLKD; set { _singleListInforUnitTLKD = value; OnPropertyChanged(nameof(singleListInforUnitTLKD)); } }
        private ListInforDoithucanhtranhTLKD _singleListInforDoithucanhtranhTLKD;
        public ListInforDoithucanhtranhTLKD singleListInforDoithucanhtranhTLKD { get => _singleListInforDoithucanhtranhTLKD; set { _singleListInforDoithucanhtranhTLKD = value; OnPropertyChanged(nameof(singleListInforDoithucanhtranhTLKD)); } }

        public ObservableCollection<ListInforUnitTLKD> list_thongtinunit { get; set; }
        public ObservableCollection<ListInforDoithucanhtranhTLKD> list_thongtinduancanhtranh { get; set; }


        private ObservableCollection<SalesLiteratureItemListModel> _list_salesliteratureitem;
        public ObservableCollection<SalesLiteratureItemListModel> list_salesliteratureitem { get => _list_salesliteratureitem;set { _list_salesliteratureitem = value; OnPropertyChanged(nameof(list_salesliteratureitem)); } }

        public TaiLieuKinhDoanhFormViewModel()
        {
            list_thongtinunit = new ObservableCollection<ListInforUnitTLKD>();
            list_thongtinduancanhtranh = new ObservableCollection<ListInforDoithucanhtranhTLKD>();
            list_salesliteratureitem = new ObservableCollection<SalesLiteratureItemListModel>();
        }


        private TaiLieuKinhDoanhFormModel _TaiLieuKinhDoanhFormModel;
        public TaiLieuKinhDoanhFormModel TaiLieuKinhDoanh
        {
            get => _TaiLieuKinhDoanhFormModel;
            set
            {
                _TaiLieuKinhDoanhFormModel = value;
                OnPropertyChanged(nameof(TaiLieuKinhDoanh));
            }
        }
    }
}

