using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kendoTest.Views.Shared
{
    public partial class Kendo : System.Web.Mvc.ViewMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string CurrentCultureName()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName;
        }
    }
}