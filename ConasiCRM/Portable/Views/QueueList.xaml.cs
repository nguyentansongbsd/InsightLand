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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueueList : ContentPage
    {
        private readonly QueuListViewModel viewModel;
        public ICRMService<QueueListModel> service;
        public QueueList()
        {
            InitializeComponent();
            BindingContext = viewModel = new QueuListViewModel();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;           
            await Navigation.PushAsync(new AccountForm());
            viewModel.IsBusy = false;
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            QueueListModel val = e.Item as QueueListModel;
            viewModel.IsBusy = true;           
            QueueForm newPage = new QueueForm(val.opportunityid);
            newPage.CheckQueueInfo = async (CheckQueueInfo) =>
            {
                if (CheckQueueInfo == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                viewModel.IsBusy = false;
            };
        }
    }
}