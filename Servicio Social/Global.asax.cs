using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using OfficeOpenXml;
using System.IO;

namespace Servicio_Social
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // C�digo que se ejecuta al iniciar la aplicaci�n
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
		    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
    }
}