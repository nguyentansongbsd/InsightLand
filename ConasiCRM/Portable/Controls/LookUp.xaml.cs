﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    public partial class LookUp : Grid
    {
        public Func<Task> PreOpenAsync;
        public Action PreOpen;

        public event EventHandler<LookUpChangeEvent> SelectedItemChange;
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUp), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        private LookUpView _lookUpView;
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(LookUp), null, BindingMode.TwoWay);
        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty NameDipslayProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(LookUp), null, BindingMode.TwoWay, propertyChanged: DisplayNameChang);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(LookUp), null, BindingMode.TwoWay, null);
        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }

        public ContentView ModalPopup { get; set; }

        public BottomModal BottomModal { get; set; }

        public string NameDisplay { get => (string)GetValue(NameDipslayProperty); set { SetValue(NameDipslayProperty, value); } }

        public bool FocusSearchBarOnTap = false;

        public bool PreOpenOneTime { get; set; } = true;


        public LookUp()
        {
            InitializeComponent();
            this.Entry.BindingContext = this;
            this.Entry.SetBinding(EntryNoneBorder.PlaceholderProperty, "Placeholder");
            this.BtnClear.SetBinding(Button.IsVisibleProperty, new Binding("SelectedItem") { Source = this, Converter = new Converters.NullToHideConverter() });
        }
        public void Clear_Clicked(object sender, EventArgs e)
        {
            this.SelectedItem = null;
            SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
        }
        public void HideClearButton()
        {
            BtnClear.IsVisible = false;
        }
        public async void OpenLookUp_Tapped(object sender, EventArgs e)
        {
            await OpenModal();
        }

        private static void DisplayNameChang(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            LookUp control = (LookUp)bindable;
            control.Entry.SetBinding(EntryNoneBorder.TextProperty, "SelectedItem." + newValue);
        }

        public async Task OpenModal()
        {
            if (PreOpenAsync != null)
            {
                await PreOpenAsync();
                if (PreOpenOneTime)
                {
                    PreOpenAsync = null;
                }
            }
            if (PreOpen != null)
            {
                PreOpen.Invoke();
                if (PreOpenOneTime)
                {
                    PreOpen = null;
                }
            }

            if (this.ItemsSource == null || this.BottomModal == null) return;

            if (_lookUpView == null)
            {
                _lookUpView = new LookUpView();
                _lookUpView.SetList(ItemsSource.Cast<object>().ToList(), NameDisplay);
                _lookUpView.lookUpListView.ItemTapped += async (lookUpSender, lookUpTapEvent) =>
                {
                    if (this.SelectedItem != lookUpTapEvent.Item)
                    {
                        this.SelectedItem = lookUpTapEvent.Item;
                        SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                    }
                    await BottomModal.Hide();
                };
            }
            else
            {
                _lookUpView.SetList(ItemsSource.Cast<object>().ToList(), NameDisplay);
            }

            BottomModal.Title = Placeholder;
            BottomModal.ModalContent = _lookUpView;
            await BottomModal.Show();


            if (FocusSearchBarOnTap)
            {
                _lookUpView.FocusSearchBarOnTap();
            }
        }
    }
}
