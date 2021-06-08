using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichLamViec : ContentPage
    {
        public LichLamViec()
        {
            InitializeComponent();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            string item = e.Item as string;
            if (item.Contains("tháng"))
            {
                Navigation.PushAsync(new LichLamViecTheoThang());
            } else if (item.Contains("tuần"))
            {
                Navigation.PushAsync(new LichLamViecTheoTuan());
            }else if (item.Contains("ngày"))
            {
                Navigation.PushAsync(new LichLamViecTheoNgay());
            }
        }
    }
}
