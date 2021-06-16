using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowDetailPage : ContentPage
    {
        FollowDetailPageViewModel viewModel;
        public FollowDetailPage()
        {
            InitializeComponent();            
        }

        public FollowDetailPage(string id)
        {
            InitializeComponent();
            Init(id);            
        }

        private async void Init(string id)
        {
            this.BindingContext = viewModel = new FollowDetailPageViewModel();
            await viewModel.Load(id);           
        }           
    }
}