﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Helper
{
    public static class DateHelper
    {
        public static DateTimeOffset ParseDate(string text)
        {
            //if (string.IsNullOrEmpty(text)) return ;

            text = text.Trim();
            string utext = text.ToUpper();

            var now = DateTimeOffset.UtcNow;

            if (utext.Equals("NOW"))
            {
                return now;
            }

            if (utext.StartsWith("NOW-"))
            {
                TimeSpan delta = XmlConvert.ToTimeSpan(utext.Substring(4));
                return now.Subtract(delta);
            }

            // Support the special case of "+" being url decoded to " " in case
            // the client forgot to encode the plus correctly using "%2b"
            if (utext.StartsWith("NOW+") || utext.StartsWith("NOW "))
            {
                TimeSpan delta = XmlConvert.ToTimeSpan(utext.Substring(4));
                return now.Add(delta);
            }

            return DateTimeOffset.Parse(text);
        }
    }
}
