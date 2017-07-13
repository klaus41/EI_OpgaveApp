﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views.Converters
{
    class DateTimeToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt = new DateTime();
            string time = null;
            if (value is DateTime)
            {
                dt = (DateTime)value;
                time = dt.AddHours(2).ToString("HH:mm");
            }
            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
