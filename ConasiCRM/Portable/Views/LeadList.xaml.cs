﻿using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Telerik.XamarinForms.DataGrid;
using Telerik.XamarinForms.DataGrid.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadList : ContentPage
    {
        public Action<bool> Action { get; set; }
        private readonly LeadListViewModel viewModel;
        public LeadList()
        {
            InitializeComponent();           
            this.BindingContext = viewModel = new LeadListViewModel();
            viewModel.IsBusy = true;
            Init();
            //viewModel.IsBusy = false;
        }

        public async void Init()
        {
            await viewModel.LoadData();
            if (viewModel.Data != null && viewModel.Data.Count > 0)
                Action?.Invoke(true);
            else
                Action?.Invoke(false);
            viewModel.IsBusy = false;
        }       

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;           
            await Navigation.PushAsync(new LeadForm());
            viewModel.IsBusy = false;
        }

        protected override void OnAppearing()
        {
            if (App.Current.Properties.ContainsKey("update") && App.Current.Properties["update"] as string == "1")
            {
                App.Current.Properties["update"] = "0";
                viewModel.RefreshCommand.Execute(null);
            }
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.IsBusy = true;           
            var item = e.Item as LeadListModel;
            LeadForm newPage = new LeadForm(item.leadid);
            newPage.CheckSingleLead = async (checkSingleLead) =>
            {
                if (checkSingleLead == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                viewModel.IsBusy = false;
            };                        
        }

        private async void Search_Pressed(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.IsBusy = false;
        }

        private async void Search_TextChanged(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                await viewModel.LoadOnRefreshCommandAsync();
            }
            viewModel.IsBusy = false;
        }
    }
}