﻿using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Synchronizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class ConnectionSettingsPage : ContentPage
    {
        Entry connectionSettingString;
        Button enter;
        Button cancel;

        StackLayout layout;
        ConnectionSettings settings;
        MaintenanceDatabase db = App.Database;
        SynchronizerFacade facade = SynchronizerFacade.GetInstance;
        string old;

        public ConnectionSettingsPage()
        {

            connectionSettingString = new Entry();

            if (db.GetConnectionSetting(0) != null)
            {
                settings = db.GetConnectionSetting(0).Result;
                old = settings.BaseAddress;
                connectionSettingString.Text = settings.BaseAddress;
            }
            else
            {
                connectionSettingString.Placeholder = "Indtast url til API";
            }

            enter = new Button { Text = "OK", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            cancel = new Button { Text = "Cancel", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };

            enter.Clicked += Enter_Clicked;
            cancel.Clicked += Cancel_Clicked;
            layout = new StackLayout
            {
                Children =
                {
                    connectionSettingString,
                    enter,
                    cancel
                }
            };
            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                layout.Padding = new Thickness(0, 20, 0, 0);
            }
            Content = layout;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PopModalAsync();
        }

        private async void Enter_Clicked(object sender, EventArgs e)
        {

            if (connectionSettingString.Text != null)
            {
                settings.BaseAddress = connectionSettingString.Text;
                await db.UpdateConnectionSetting(settings);

                try
                {
                    bool connected = await facade.MaintenanceTaskSynchronizer.HasConnectionToNAV();
                    if (connected)
                    {
                        await DisplayAlert("Forbindelse", "Adressen er opdateret og enheden har forbindelse til NAV", "OK");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Forbindelse", "Enheden har ikke forbindelse til NAV. Tjek om addressen er korrekt", "OK");
                        settings.BaseAddress = old;
                        await db.UpdateConnectionSetting(settings);
                    }
                }
                catch
                {
                    await DisplayAlert("Forbindelse", "Enheden har ikke forbindelse til NAV. Tjek om addressen er korrekt", "OK");
                    settings.BaseAddress = old;
                    await db.UpdateConnectionSetting(settings);
                }
            }
        }
    }
}
