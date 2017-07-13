﻿using EI_OpgaveApp.Models;
using EI_OpgaveApp.Views.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views.Custom_Cells
{
    class CustomActivityCell : ViewCell
    {
        Color color = Color.Default;

        public CustomActivityCell()
        {
            Label description = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            Label reading = new Label()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            Grid mainGrid = new Grid
            {
                Padding = new Thickness(10),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(2,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) }
                }
            };

            mainGrid.Children.Add(description, 0, 0);
            mainGrid.Children.Add(reading, 2, 0);

            mainGrid.BackgroundColor = color;
            View = mainGrid;

            description.SetBinding<MaintenanceActivity>(Label.TextProperty, i => i.Activity_Description);
            reading.SetBinding<MaintenanceActivity>(Label.TextProperty, i => i.Reading);

            mainGrid.SetBinding(Label.BackgroundColorProperty, new Binding("Done", converter: new BoolToColorConverter()));
            if (Device.RuntimePlatform.Equals("iOS"))
            {
                mainGrid.Margin = 0;
            }
            else
            {
                mainGrid.Margin = 10;
            }
        }
    }
}
