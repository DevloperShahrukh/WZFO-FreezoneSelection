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
                Str_StringCon = "Data Source=SPS2013;Initial Catalog=WFZOFinal;User ID=sa; Password=P@ssw0rd" ;
                //Decrypt(item[0]["Value"].ToString())
                return new SqlConnection(Str_StringCon);
            }
        }
    }

    public static string Encrypt(string clearText)
    {
        var encryptionKey = "WFZO123";
        var clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (var encryptor = Aes.Create())
        {
            var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    public static string Decrypt(string cipherText)
    {
        var encryptionKey = "WFZO123";
        var cipherBytes = Convert.FromBase64String(cipherText);
        using (var encryptor = Aes.Create())
        {
            var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
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
                item["Value"] = Connection.Encrypt("Data Source=SPS2013;Initial Catalog=WFZO;User ID=sa; Password='P@ssw0rd'");
                item.Update();
                web.AllowUnsafeUpdates = false;

            }
        }
    }
}
