using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Collections.ObjectModel;
using Telerik.XamarinForms.DataGrid;
using ConasiCRM.Portable.Controls.BsdGrid;
using System.IO;
using ConasiCRM.Portable.Controls;
using permissionType = Plugin.Permissions.Abstractions.Permission;
using permissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaiLieuKinhDoanhForm : ContentPage
    {
        public Action<bool> CheckTaiLieuKinhDoanh;
        private Guid salesliteratureid;
        public TaiLieuKinhDoanhFormViewModel viewModel;

        public TaiLieuKinhDoanhForm(Guid literatureid)
        {
            InitializeComponent();
            BindingContext = viewModel = new TaiLieuKinhDoanhFormViewModel();

            salesliteratureid = literatureid;
            viewModel.IsBusy = true;
            Init();          
        }

        private async void Init()
        {
            await loadData();
            if(viewModel.TaiLieuKinhDoanh != null)
            {
                CheckTaiLieuKinhDoanh(true);
            }
            else
            {
                CheckTaiLieuKinhDoanh(false);
            }
        }
        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='salesliterature'>
                                 <all-attributes/>
                                 <order attribute='name' descending='false' />
                                 <filter type='and'>
                                    <condition attribute='salesliteratureid' operator='eq' value='" + salesliteratureid + @"' />
                                 </filter>
                                <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' >
                                  <attribute name='title' alias='subjecttitle'/>
                                </link-entity>
                              </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaiLieuKinhDoanhFormModel>>("salesliteratures", xml);
            var data = result.value.FirstOrDefault();
            if (data == null)
            {              
                await DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }            

            viewModel.TaiLieuKinhDoanh = data;
            await loadUnit(salesliteratureid);
            await loadDoiThuCanhTranh(salesliteratureid);
            await loadAllSalesLiteratureIten(salesliteratureid);

            if (viewModel.list_thongtinunit.Count < 3)
            {
                for (int i = viewModel.list_thongtinunit.Count; i < 3; i++)
                {
                    viewModel.list_thongtinunit.Add(new ListInforUnitTLKD());
                }
            }

            if (viewModel.list_thongtinduancanhtranh.Count < 3)
            {
                for (int i = viewModel.list_thongtinduancanhtranh.Count; i < 3; i++)
                {
                    viewModel.list_thongtinduancanhtranh.Add(new ListInforDoithucanhtranhTLKD());
                }
            }
            
            if (viewModel.list_salesliteratureitem.Count == 0)
            {
                for (int i = viewModel.list_salesliteratureitem.Count; i < 3; i++)
                {
                    viewModel.list_salesliteratureitem.Add(new SalesLiteratureItemListModel());
                }
            }

            viewModel.IsBusy = false;
        }

        public async Task loadUnit(Guid salesliteratureid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='product'>
                                <all-attributes/>
                                <link-entity name='productsalesliterature' intersect='true' visible='false' to='productid' from='productid'>
                                    <link-entity name='salesliterature' to='salesliteratureid' from='salesliteratureid' alias='ab'>
                                        <filter type='and'>
                                            <condition attribute='salesliteratureid' value='" + salesliteratureid + @"' uitype='salesliterature' operator='eq'/>
                                        </filter>
                                    </link-entity>
                                </link-entity>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' >
                                  <attribute name='bsd_name' alias='bsd_projectname'/>
                                </link-entity>
                                </entity>
                             </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListInforUnitTLKD>>("products", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    viewModel.list_thongtinunit.Add(item);
                }
            }
        }

        public async Task loadDoiThuCanhTranh(Guid salesliteratureid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='competitor'>
                                  <all-attributes/>
                                  <link-entity name='competitorsalesliterature' intersect='true' visible='false' to='competitorid' from='competitorid'>
                                      <link-entity name='salesliterature' to='salesliteratureid' from='salesliteratureid' alias='ab'>
                                          <filter type='and'>
                                              <condition attribute='salesliteratureid' value='" + salesliteratureid + @"' uitype='salesliterature' operator='eq'/>
                                          </filter>
                                      </link-entity>
                                  </link-entity>
                                </entity>
                             </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListInforDoithucanhtranhTLKD>>("competitors", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    viewModel.list_thongtinduancanhtranh.Add(item);
                }
            }
        }

        public async Task loadAllSalesLiteratureIten(Guid salesliteratureid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesliteratureitem'>
                                    <attribute name='title' alias='title_label' />
                                    <attribute name='modifiedon' />
                                    <attribute name='salesliteratureitemid' />
                                    <attribute name='filename' />
                                    <order attribute='modifiedon' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='salesliteratureid' operator='eq' value='{" + salesliteratureid + @"}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<SalesLiteratureItemListModel>>("salesliteratureitems", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}

            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    viewModel.list_salesliteratureitem.Add(item);
                }
            }
        }

        public async Task<bool> downloadFile_salesliteratureitem(Guid salesliteratureitemid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesliteratureitem'>
                                    <attribute name='filename' />
                                    <attribute name='documentbody' />
                                    <order attribute='modifiedon' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='salesliteratureitemid' operator='eq' value='{" + salesliteratureitemid + @"}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<SalesLiteratureItemListModel>>("salesliteratureitems", fetch);
            if(result == null)
            {
                return false;
            }
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value.FirstOrDefault();

            var fileName = data.filename;
            var body = data.documentbody;

            byte[] arr = Convert.FromBase64String(body);
            MemoryStream stream = new MemoryStream(arr);

            //FileService.SaveFile(fileName, arr);

            DependencyService.Get<IFileService>().SaveFile(fileName, arr, "Download/Conasi");

            return true;
        }

        async void DownloadFileButton_Cliked(object sender, System.EventArgs e)
        {
            if(datagrid_salesliteratureitem.SelectedItems.Count == 0)
            {
                await DisplayAlert("", "Chọn tệp để tải", "OK");
            }
            else
            {
                if (await PermissionHelper.CheckPermissions(permissionType.Storage) == permissionStatus.Granted)
                {
                    popup_dowload_file.focus();
                    popup_dowload_file.ItemSource = datagrid_salesliteratureitem.SelectedItems;
                    foreach (var t in datagrid_salesliteratureitem.SelectedItems)
                    {
                        var data = t as SalesLiteratureItemListModel;
                        if (data.salesliteratureitemid != Guid.Empty && data.status != 1)
                        {
                            var res = await this.downloadFile_salesliteratureitem(data.salesliteratureitemid);
                            if (res)
                            {
                                data.status = 1;
                            }
                            else
                            {
                                data.status = 2;
                            }
                        }
                    }
                    popup_dowload_file.isTapable = true;

                }
                    
            }
        }

        void ListViewFileDownloaded_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            SalesLiteratureItemListModel item = e.Item as SalesLiteratureItemListModel;
            this.popup_dowload_file.SelectedItem = null;
            //DisplayAlert("", item.filename, "OK");
            if(item.status == 1)
            {
                try
                {
                    DependencyService.Get<IFileService>().OpenFile(item.filename);
                }
                catch
                {
                    DisplayAlert("","Ứng dụng không được hỗ trợ. Không thể mở file","OK");
                }
                
            }
            else
            {
                foreach (var t in datagrid_salesliteratureitem.SelectedItems)
                {
                    var data = t as SalesLiteratureItemListModel;
                    if (data.salesliteratureitemid != Guid.Empty && data.status != 1)
                    {
                        data.status = 0;
                    }
                }
                DownloadFileButton_Cliked(null, null);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.popup_dowload_file.IsVisible)
            {
                this.popup_dowload_file.unFocus();
                return true;
            }

            return base.OnBackButtonPressed();
        }
    }
}
