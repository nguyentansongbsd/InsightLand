using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.DataControls.ListView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage1 : MasterDetailPage
    {
        public MasterDetailPage1()
        {
            InitializeComponent();
        }

        public void LeadList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new LeadList());
            IsPresented = false;
        }

        public void AccountList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new AccountList());
            IsPresented = false;
        }

        private void ContactList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ContactList());
            IsPresented = false;
        }

        private void DirectSale_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new DirectSale());
            IsPresented = false;
        }

        private void QueueList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new QueueList());
            IsPresented = false;
        }

        private void ReservationList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ReservationList());
            IsPresented = false;
        }

        private void EventList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new EventList());
            IsPresented = false;

        }

        private void PhiMoiGioiList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new PhiMoGioiList());
            IsPresented = false;
        }

        private void TaiLieuKinhDoanHList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ListTaiLieuKinhDoanh());
            IsPresented = false;
        }

        private void DanhSachTheoDoiList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new FollowUpListPage());
            IsPresented = false;
        }

        private void PhanHoiList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ListPhanHoi());
            IsPresented = false;
        }

        private void HoaHongGiaoDichList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HoaHongGiaoDichList());
            IsPresented = false;
        }

        private void PhiMoiGioiGiaoDichList_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new PhiMoGioiGiaoDichList());
            IsPresented = false;
        }

        private void DanhBa_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new DanhBa());
            IsPresented = false;
        }

        private void HoatDong_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HoatDongList());
            IsPresented = false;
        }

        private void LichSuCuocGoi_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new LichSuCuocGoi());
            IsPresented = false;
        }

        private void LichSuTinNhan_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new LichSuTinNhan());
            IsPresented = false;
        }

        private void LichLamViec_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new LichLamViec());
            IsPresented = false;
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}