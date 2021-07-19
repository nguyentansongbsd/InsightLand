using System;
using System.Collections.Generic;
using ConasiCRM.Portable.Helper;
using Xamarin.Forms;

namespace ConasiCRM.Portable
{
    public partial class BlankPage : Shell
    {
        public Action<bool> OnCompleted;
        public BlankPage()
        {
            InitializeComponent();
            OnCompleted?.Invoke(true);
        }
    }
}
