﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeFirst.Helpers
{
    public class Utlities
    {
        public static string GetURL()
        {
            HttpRequest request = HttpContext.Current.Request;
            string url = request.Url.ToString();
            return url;
        }

    }
}