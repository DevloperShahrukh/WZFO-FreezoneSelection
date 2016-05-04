using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Security.Cryptography;
using System.IO;
using WFZO.FZSelector.Classes;

public class Connection
{
    private string Str_StringCon; //;= ConfigurationManager.ConnectionStrings["UBLconnection"].ToString(); 
    public Connection(){}

    public SqlConnection getConnection()
    {
        string value = Helper.GetConfigurationValue("Connection String");
        if (value != string.Empty)
        {
            //value = Encryption.Decrypt(value);
            return new SqlConnection(value);
        }
        else
            return null;
    }

    
    public static void saveConfiguration()
    {
        using (SPSite site = new SPSite(SPContext.Current.Web.Site.RootWeb.Url))
        {
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList("Configuration");
                SPListItem item = list.Items.Add();
                web.AllowUnsafeUpdates = true;
                item["Title"] = "Connection String";
                item["Value"] = Encryption.Encrypt("Data Source=SPS2013;Initial Catalog=WFZO;User ID=sa; Password='P@ssw0rd'");
                item.Update();
                web.AllowUnsafeUpdates = false;

            }
        }
    }
}
