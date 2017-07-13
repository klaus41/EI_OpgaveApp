﻿using EI_OpgaveApp.Database;
using EI_OpgaveApp.Synchronizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public partial class SettingsPage : ContentPage
    {
        ActivityIndicator ai;
        StackLayout layout;
        Color buttonColor;

        Button syncButton;
        Button dropCreateButton;
        Button deleteDbButton;
        Button checkConnectionButton;
        Button searchSettingsButton;

        Label version;

        SynchronizerFacade facade = SynchronizerFacade.GetInstance;
        public SettingsPage()
        {
            string versionno = DependencyService.Get<IApp>().GetVersionCode(); //"Version 4.03";
            buttonColor = Color.FromRgb(135, 206, 250);
            BackgroundColor = Color.White;
            //MakeToolBar();
            Title = "Indstillinger";

            NavigationPage.SetHasNavigationBar(this, false);
            layout = new StackLayout { Padding = 10, };

            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                layout.Padding = new Thickness(0, 20, 0, 0);
            }
            dropCreateButton = new Button { Text = "Nulstil lokal database", BackgroundColor = buttonColor, TextColor = Color.White };
            syncButton = new Button { Text = "Synkroniser", BackgroundColor = buttonColor, TextColor = Color.White };
            deleteDbButton = new Button { Text = "Slet data fra lokal database", BackgroundColor = buttonColor, TextColor = Color.White };
            checkConnectionButton = new Button { Text = "Tjek forbindelse til NAV", BackgroundColor = buttonColor, TextColor = Color.White };
            searchSettingsButton = new Button { Text = "Administrer søgefilter", BackgroundColor = buttonColor, TextColor = Color.White };

            version = new Label() { Text = versionno, VerticalOptions = LayoutOptions.EndAndExpand };

            searchSettingsButton.Clicked += (s, e) =>
            {
                ShowActivityIndicator();
                Navigation.PushModalAsync(new SearchSettingsPage());
                RemoveActivityIndicator();
            };

            checkConnectionButton.Clicked += async (s, e) =>
            {
                ShowActivityIndicator();
                bool connected = await facade.MaintenanceTaskSynchronizer.HasConnectionToNAV();
                RemoveActivityIndicator();
                if (connected)
                {
                    await DisplayAlert("Forbindelse", "Enheden har forbindelse til NAV", "OK");
                }
                else
                {
                    await DisplayAlert("Forbindelse", "Enheden har ikke forbindelse til NAV. Tjek om telefonen er tilsluttet WiFi eller om data er slået fra", "OK");
                }
            };

            deleteDbButton.Clicked += async (s, e) =>
            {
                var action = await DisplayAlert("Advarsel", "Er du sikker på, du vil slette den lokale database?", "Ja", "Cancel");
                if (action)
                {
                    ShowActivityIndicator();
                    facade.MaintenanceTaskSynchronizer.DeleteDB();
                    RemoveActivityIndicator();
                    await DisplayAlert("Nulstilling", "Den lokale database er nu nulstillet", "OK");

                }
            };

            dropCreateButton.Clicked += async (s, e) => 
            {
                var action = await DisplayAlert("Advarsel", "Er du sikker på, du vil slette den lokale database og erstatte den med data fra NAV?", "Ja", "Cancel");
                if (action)
                {
                    ShowActivityIndicator();

                    try
                    {
                        var data = await facade.MaintenanceTaskSynchronizer.DeleteAndPopulateDb();
                        facade.TimeRegistrationSynchronizer.DeleteAndPopulateDb();
                        facade.MaintenanceActivitySynchronizer.DeleteAndPopulateDb();
                        RemoveActivityIndicator();
                        await DisplayAlert("Synkronisering", "Synkronisering færdig", "OK");
                    }
                    catch
                    {
                        RemoveActivityIndicator();
                        await DisplayAlert("Advarsel!", "Database nulstillet.Der kunne ikke hentes data fra NAV!", "OK");
                    }
                }


            };
            syncButton.Clicked += async (s, e) =>
            {
                ShowActivityIndicator();
                try
                {
                    await facade.MaintenanceTaskSynchronizer.SyncDatabaseWithNAV();
                    await facade.TimeRegistrationSynchronizer.SyncDatabaseWithNAV();
                    await facade.MaintenanceActivitySynchronizer.SyncDatabaseWithNAV();
                    facade.JobRecLineSynchronizer.SyncDatabaseWithNAV();
                    facade.ResourcesSynchronizer.SyncDatabaseWithNAV();
                    facade.CustomerSynchronizer.SyncDatabaseWithNAV();
                    facade.JobSynchronizer.SyncDatabaseWithNAV();
                    facade.JobTaskSynchronizer.SyncDatabaseWithNAV();

                    RemoveActivityIndicator();
                    await DisplayAlert("Synkronisering", "Enheden er nu synkroniseret med NAV", "OK");
                }
                catch
                {

                    RemoveActivityIndicator();
                    await DisplayAlert("Advarsel!", "Enheden kunne ikke synkronisere med NAV", "OK");
                }
            };

            //layout.Children.Add(dropCreateButton);
            layout.Children.Add(syncButton);
            layout.Children.Add(deleteDbButton);
            layout.Children.Add(checkConnectionButton);
            layout.Children.Add(searchSettingsButton);
            layout.Children.Add(version);

            Content = new ScrollView { Content = layout };

        }
        private void RemoveActivityIndicator()
        {
            layout.Children.Remove(ai);
            syncButton.IsEnabled = true;
            dropCreateButton.IsEnabled = true;
            deleteDbButton.IsEnabled = true;
            checkConnectionButton.IsEnabled = true;
            searchSettingsButton.IsEnabled = true;
        }

        private void ShowActivityIndicator()
        {
            ai = new ActivityIndicator()
            {
                IsRunning = true
            };
            layout.Children.Add(ai);

            syncButton.IsEnabled = false;
            dropCreateButton.IsEnabled = false;
            deleteDbButton.IsEnabled = false;
            checkConnectionButton.IsEnabled = false;
            searchSettingsButton.IsEnabled = false;
        }
    }
}
