﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class SearchSettingsPage : ContentPage
    {
        GlobalData gd = GlobalData.GetInstance;
        Entry userNameEntry;
        DatePicker datePicker;
        DatePicker datePickerLast;
        Button reset;
        Button cancel;
        public SearchSettingsPage()
        {
            DateTime today = DateTime.Today;
            userNameEntry = new Entry()
            {
                Text = gd.SearchUserName
            };
            datePicker = new DatePicker()
            {
                Format = "D",
                Date = gd.SearchDateTime
            };

            datePickerLast = new DatePicker()
            {
                Format = "D",
                Date = gd.SearchDateTimeLast
            };

            reset = new Button { Text = "Nulstil", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            cancel = new Button { Text = "Fortryd", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };

            reset.Clicked += Reset_Clicked;
            cancel.Clicked += Cancel_Clicked;
            datePicker.DateSelected += DatePicker_DateSelected;
            datePickerLast.DateSelected += DatePickerLast_DateSelected;
            userNameEntry.TextChanged += UserNameEntry_TextChanged;
            Content = new StackLayout
            {
                Children =
                {
                    userNameEntry,
                    datePicker,
                    datePickerLast,
                    reset,
                    cancel
                }
            };
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void DatePickerLast_DateSelected(object sender, DateChangedEventArgs e)
        {
            gd.SearchDateTimeLast = datePickerLast.Date;
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            gd.SearchDateTime = new DateTime(1800, 1, 1);
            gd.SearchDateTimeLast = new DateTime(2300, 1, 1);
            gd.SearchUserName = null;
            Navigation.PopModalAsync();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            gd.SearchDateTime = datePicker.Date;
            datePickerLast.MinimumDate = datePicker.Date;
        }

        private void UserNameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (userNameEntry.Text == "")
            {
                gd.SearchUserName = null;
            }
            else
            {
                gd.SearchUserName = userNameEntry.Text;
            }
        }
    }
}
