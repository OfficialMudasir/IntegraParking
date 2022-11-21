using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using backOffice.Infrastructure;

namespace PBPPlugin.Views.Shared
{
    public partial class Site : System.Web.Mvc.ViewMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public ResourceBundle GetResourceBundle()
        {
            return ResourceBundle.GetInstance();
        }

    }
}