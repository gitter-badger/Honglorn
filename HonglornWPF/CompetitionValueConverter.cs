﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace HonglornWPF
{
    class CompetitionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object returnValue;

            if (string.IsNullOrWhiteSpace(value as string))
            {
                returnValue = null;
            }
            else
            {
                returnValue = value;
            }

            return returnValue;
        }
    }
}