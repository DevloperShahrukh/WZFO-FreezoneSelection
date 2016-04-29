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
    public string getstrConnection()
    {
        return ""; //Str_StringCon;
    }
    public SqlConnection getConnection()
    {
        using (SPSite site = new SPSite(SPContext.Current.Web.Site.RootWeb.Url))
        {
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists.TryGetList("Configuration");
                SPQuery query = new SPQuery();
                query.Query = "<Where>"
                    + "<Eq>"
                    + "<FieldRef Name='Title' /><Value Type='Text'>Connection String</Value>"
                    + "</Eq>"
                    + "</Where>";
                query.ViewFields = "<FieldRef Name='Value' />";
                SPListItemCollection item = list.GetItems(query);
                Str_StringCon = "Data Source=SPS2013;Initial Catalog=WFZO;User ID=sa; Password=P@ssw0rd" ;
                //Decrypt(item[0]["Value"].ToString())
                return new SqlConnection(Str_StringCon);
            }
        }
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
