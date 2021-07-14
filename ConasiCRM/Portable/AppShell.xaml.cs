using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using ConasiCRM.Portable.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public Action<bool> OnCompleted { get; set; }
        private AppShellViewModel viewModel;
        public AppShell()
        {
            LoadingHelper.Show();
            InitializeComponent();
            Init();
            LoadingHelper.Hide();
        }

        public async void Init()
        {
            appShell.CurrentItem = BanHang;
            this.BindingContext = viewModel = new AppShellViewModel();
            OnCompleted?.Invoke(true);
        }
    }
}