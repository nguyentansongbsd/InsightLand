using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class ProjectInfoViewModel : BaseViewModel
    {
        private ProjectInfoModel _project;
        public ProjectInfoModel Project
        {
            get => _project;
            set
            {
                if (_project != value)
                {
                    _project = value;
                    OnPropertyChanged(nameof(Project));
                }
            }
        }


        public ObservableCollection<Project_DuAnCanhTranhModel> DuAnCanhTranh_List { get; set; }
        public ObservableCollection<Project_DoiThuCanhTranhModel> DoiThuCanhTranh_List { get; set; }

        public ProjectInfoViewModel()
        {
            IsBusy = true;
            DuAnCanhTranh_List = new ObservableCollection<Project_DuAnCanhTranhModel>();
            DoiThuCanhTranh_List = new ObservableCollection<Project_DoiThuCanhTranhModel>();
        }
    }
}
