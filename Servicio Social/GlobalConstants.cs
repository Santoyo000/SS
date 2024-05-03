using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace Servicio_Social
{
    public class GlobalConstants
    {
        public static string SQL { get { return ConfigurationManager.ConnectionStrings["SQL"].ConnectionString; } }
    }
}