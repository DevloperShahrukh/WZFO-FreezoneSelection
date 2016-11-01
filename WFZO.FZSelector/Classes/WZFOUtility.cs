using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace WFZO.FZSelector.Classes
{
    public class WZFOUtility
    {

        public const int FullMember = 1;
        public const int AsscociateMemebr = 2;
        public const int Partner = 3;
        public static string EventCategoryDefaultImage = "/Style Library/Images/EventCategoryDefault.png";

        public static string GetZeroIndexAttachmentURL(SPListItem spotlightItm)
        {
            string imgURL = string.Empty;
            SPAttachmentCollection attachments = spotlightItm.Attachments;
            if (attachments != null && attachments.Count > 0)
            {
                imgURL = string.Format("{0}{1}", attachments.UrlPrefix, attachments[0]);
            }

            return imgURL;
        }


        public static bool IsUserMemberOfGroup(SPUser user, string groupName)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(groupName) && user != null)
            {
                foreach (SPGroup group in user.Groups)
                {
                    if (group.Name == groupName)
                    {                         // found it
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }
        public static void LogException(Exception ex, string MethodName, SPSite S)
        {
            try
            {


                SPSecurity.RunWithElevatedPrivileges(delegate()
                {

                    using (SPSite site1 = new SPSite(S.ID))
                    {
                        using (SPWeb web1 = site1.OpenWeb())
                        {
                            if (ex != null)
                            {
                                web1.AllowUnsafeUpdates = true;
                                SPList list = web1.Lists[Constants.List.Exception.Name];

                                //Add a new item in the List
                                SPListItem itemToAdd = list.Items.Add();
                                itemToAdd[Constants.List.BaseColumns.Title] = MethodName;
                                itemToAdd[Constants.List.Exception.Fields.Error] = ex.Message;
                                if (ex.InnerException != null)
                                {
                                    itemToAdd[Constants.List.Exception.Fields.Detail] = ex.InnerException.ToString();
                                }

                                if (ex.StackTrace != null)
                                {
                                    itemToAdd[Constants.List.Exception.Fields.Stacktrace] = ex.StackTrace;
                                }
                                itemToAdd.Update();
                                web1.AllowUnsafeUpdates = false;
                            }


                        }
                    }
                });



            }
            catch (Exception ex1)
            {
            }
        }
        public static void LogMessage(string detail, string MethodName, SPSite S)
        {
            try
            {

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {

                    using (SPSite site1 = new SPSite(S.ID))
                    {
                        using (SPWeb web1 = site1.OpenWeb())
                        {

                                web1.AllowUnsafeUpdates = true;
                                SPList list = web1.Lists[Constants.List.Exception.Name];

                                //Add a new item in the List
                                SPListItem itemToAdd = list.Items.Add();
                                itemToAdd[Constants.List.BaseColumns.Title] = MethodName;

                                itemToAdd[Constants.List.Exception.Fields.Detail] = detail;

                                itemToAdd.Update();
                                web1.AllowUnsafeUpdates = false;


                        }
                    }
                });



            }
            catch (Exception ex1)
            {
            }
        }

        public static void LogEmail(string EmailTo, string Subject, string EmailContent, string Source, SPSite S)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {

                    using (SPSite site1 = new SPSite(S.ID))
                    {
                        using (SPWeb web1 = site1.OpenWeb())
                        {
                            if (EmailTo != null && EmailTo.Length > 0)
                            {
                                web1.AllowUnsafeUpdates = true;
                                SPList list = web1.Lists["EmailLogging"];
                                //Add a new item in the List
                                if (EmailTo.Contains(";"))
                                {
                                    foreach (string email in EmailTo.Split(';'))
                                    {
                                        SPListItem itemToAdd = list.Items.Add();
                                        itemToAdd["Email_x0020_To"] = email;
                                        itemToAdd["Subject"] = Subject;
                                        itemToAdd["Email_x0020_Content"] = EmailContent;
                                        itemToAdd["Source"] = Source;
                                        itemToAdd["Email_x0020_Date"] = DateTime.Now;
                                        itemToAdd.Update();
                                    }
                                }
                                else
                                {
                                    SPListItem itemToAdd = list.Items.Add();
                                    itemToAdd["Email_x0020_To"] = EmailTo;
                                    itemToAdd["Subject"] = Subject;
                                    itemToAdd["Email_x0020_Content"] = EmailContent;
                                    itemToAdd["Source"] = Source;
                                    itemToAdd["Email_x0020_Date"] = DateTime.Now;
                                    itemToAdd.Update();
                                }

                                web1.AllowUnsafeUpdates = false;
                            }


                        }
                    }
                });



            }
            catch (Exception ex1)
            {
                WZFOUtility.LogException(ex1, "While Loging EMail", SPContext.Current.Site);
            }
        }
        public static void bindCombo(System.Web.UI.WebControls.DropDownList ddl, string ListName)
        {

            SPSite site = SPContext.Current.Site;
            SPList Varlist = site.RootWeb.Lists.TryGetList(ListName);
            ddl.DataSource = Varlist.GetItems("ID", "Title");
            ddl.DataTextField = "Title";
            ddl.DataValueField = "ID";
            ddl.DataBind();
        }

        public static void SendEmail(String TOEmail, SPWeb web, string Body, string Subject, bool isGroup)
        {

            try
            {
                MailMessage message = new MailMessage();
                //Get the Sharepoint SMTP information from the //SPAdministrationWebApplication 
                message.From = new MailAddress(SPAdministrationWebApplication.Local.OutboundMailSenderAddress.ToString());

                //message.To.Add(new MailAddress(TOEmail));
                if (isGroup == false)
                {
                    message.To.Add(new MailAddress(TOEmail));
                }
                else
                {

                    SPGroup group = web.Groups[TOEmail];
                    foreach (SPUser user in group.Users)
                    {
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.Email))
                            {
                                message.To.Add(new MailAddress(user.Email));

                            }
                        }


                    }

                }
                message.IsBodyHtml = true;

                //Set the subject and body of the message

                message.Body = Body;
                message.Subject = Subject;

                //Create the SMTP client object and send the message
                SmtpClient smtpClient = new SmtpClient(SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address);
                smtpClient.Send(message);
            }

            catch (Exception ex)
            {
               
                WZFOUtility.LogException(ex, "WFZOUtility - SendEmail", SPContext.Current.Site);
            }

        }
        public static void SendEmail(String TOEmail)
        {

            try
            {
                MailMessage message = new MailMessage();
                //Get the Sharepoint SMTP information from the //SPAdministrationWebApplication 
                message.From = new MailAddress(SPAdministrationWebApplication.Local.OutboundMailSenderAddress.ToString());

                message.To.Add(new MailAddress(TOEmail));
                message.IsBodyHtml = true;

                //Set the subject and body of the message

                message.Body = "Your request has been submitted successfully and one of World FZO representative will coordinate you shortly";
                message.Subject = "WZFO Registration Info";



                //Create the SMTP client object and send the message
                SmtpClient smtpClient = new SmtpClient(SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address);
                smtpClient.Send(message);
            }

            catch (Exception ex)
            {

            }
        }
        public static string GenerateID(int id, string StandardString)
        {
            string strID = id.ToString();
            string strCode = "00000".Remove(0, strID.Length);
            string StrResult = StandardString + strCode + strID;

            return StrResult;
        }
        public static void SendEmailWithAttachemnt(String TOEmail, SPWeb web, String Password, string Action, string Name, bool IsAttachment, bool IsGroup = false, string RefCode = "")
        {
            try
            {
                MailMessage message = new MailMessage();
                //Get the Sharepoint SMTP information from the //SPAdministrationWebApplication 
                message.From = new MailAddress(SPAdministrationWebApplication.Local.OutboundMailSenderAddress.ToString());
                string emailto = "";
                if (IsGroup == false)
                {
                    message.To.Add(new MailAddress(TOEmail));
                    emailto += TOEmail + ";";
                }
                else
                {

                    SPGroup group = web.Groups[TOEmail];
                    foreach (SPUser user in group.Users)
                    {
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.Email))
                            {
                                message.To.Add(new MailAddress(user.Email));
                                emailto += user.Email + ";";

                            }
                        }


                    }

                }

                message.IsBodyHtml = true;


                SPList EmailLst = web.Lists["Email Templates"];
                SPQuery query = new SPQuery();
                query.RowLimit = 1;
                query.Query = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + Action + "</Value></Eq></Where>";

                SPListItemCollection EmailItem = EmailLst.GetItems(query);
                if (EmailItem.Count > 0)
                {
                    message.Body = EmailItem[0]["Email_x0020_Body"].ToString().Replace("&lt;NAME&gt;", Name);
                    message.Subject = EmailItem[0]["Email_x0020_Subject"].ToString();
                    if (message.Body.Contains("RefCode"))
                    {
                        message.Body = message.Body.Replace("&lt;RefCode&gt;", RefCode);
                    }
                    if (message.Body.Contains("LoginID"))
                    {
                        message.Body = message.Body.Replace("&lt;LoginID&gt;", TOEmail);
                    }
                    if (message.Body.Contains("Password"))
                    {
                        message.Body = message.Body.Replace("&lt;Password&gt;", Password);
                    }
                }

                //Set the subject and body of the message
                //message.Body = "Thank you for registration with WFZO you can connect by using follwoing credentials<br> User ID =" + TOEmail + " <br>Password=" + Password;
                //message.Subject = "Registration Confirmation.";

                if (IsAttachment)
                {
                    //Get the Document Library from where you want to sed the attachment
                    SPList splDataSpringsLibrary = web.Lists["Documents"];

                    //Get the Url of the file to be sent as an attachment
                    string strUrl = "http://vm-2:444/Documents/Commands.txt";
                    //Get the file to be sent as an attachment
                    SPFile file = splDataSpringsLibrary.ParentWeb.GetFile(strUrl);

                    //Add the attachment
                    //message.Attachments.Add(new Attachment())
                    message.Attachments.Add(new Attachment(file.OpenBinaryStream(), file.Name));
                }
                //Create the SMTP client object and send the message


                try
                {
                    SmtpClient smtpClient = new SmtpClient(SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address);
                    smtpClient.Send(message);
                }
                catch (Exception ex)
                {
                    WZFOUtility.LogException(ex, "While Sending Email for " + Action + " in SendEmailWithAttachment", SPContext.Current.Site);
                }
                LogEmail(emailto.Substring(0, emailto.Length - 1), message.Subject, message.Body, Action, web.Site);

            }

            catch (Exception ex)
            {

            }


        }

        public static string LimitString(string Str, int Limit)
        {


            if (Str.Length > Limit)
            {
                Str = Str.Substring(0, Limit) + "...";
            }

            return Str;
        }

        public static string GetFormatedDate(DateTime date)
        {
            string YY = date.Year.ToString();
            string MM = string.Empty;
            string DD = string.Empty;
            if (date.Month < 10) MM = "0" + date.Month.ToString();
            else MM = date.Month.ToString();
            if (date.Day < 10) DD = "0" + date.Day.ToString();
            else DD = date.Day.ToString();
            return YY + MM + DD;
        }
        public static string GetFormattedTime(string time)
        {
            string[] times = time.Split(':');
            string HH = string.Empty;
            string MM = string.Empty;
            if (Convert.ToInt32(times[0]) < 10) HH = "0" + times[0];
            else HH = times[0];
            if (Convert.ToInt32(times[1]) < 10) MM = "0" + times[0];
            else MM = times[1];
            return HH + MM + "00Z";

        }

        public static string getWFZOPrimarySiteUrl()
        {
            return "http://sps2013";

        }

        //public static void resizeImage( string imagePath, int width, int height)
        //{
        //    System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
        //    float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;

        //    Bitmap bitMAP1 = new Bitmap(image,width, height);
        //    Graphics imgGraph = Graphics.FromImage(bitMAP1);
        //    var imgDimesions = new Rectangle(0, 0, width, height);
        //    bitMAP1.DrawImage(image, imageRectangle);
        //    bitMAP1.Save(Server.MapPath("images/Shops/" + fileupload1.filename), ImageFormat.Jpeg);
        //    bitMAP1.Dispose();
        //    bitMAP1.Dispose();
        //    image.Dispose();
        //}

        /*
        public static string GetAuthenticationProviderProvider(SPSite site, AuthenticationProviderType authenticationProviderType)
        {
            // get membership provider of whichever zone in the web app is fba enabled 
            SPIisSettings settings = GetFBAIisSettings(site);
            if (authenticationProviderType == AuthenticationProviderType.MembershipProvider)
                return settings.FormsClaimsAuthenticationProvider.MembershipProvider;
            else if (authenticationProviderType == AuthenticationProviderType.RoleProvider)
                return settings.FormsClaimsAuthenticationProvider.RoleProvider;
            else
                return string.Empty;
        }

        public static SPIisSettings GetFBAIisSettings(SPSite site)
        {
            SPIisSettings settings = null;

            // try and get FBA IIS settings from current site zone
            try
            {
                foreach (SPAlternateUrl alternateUrl in site.WebApplication.AlternateUrls)
                {
                    settings = site.WebApplication.IisSettings[alternateUrl.UrlZone];
                    if (settings.AuthenticationMode == AuthenticationMode.Forms && settings.AllowAnonymous == true)
                    {
                        return settings;
                    }
                }
            }
            catch
            {
                // expecting errors here so do nothing                 
            }

            // check each zone type for an FBA enabled IIS site
            foreach (SPUrlZone zone in Enum.GetValues(typeof(SPUrlZone)))
            {
                try
                {
                    settings = site.WebApplication.IisSettings[(SPUrlZone)zone];
                    if (settings.AuthenticationMode == AuthenticationMode.Forms)
                        return settings;
                }
                catch
                {
                    // expecting errors here so do nothing                 
                }
            }

            // return null if FBA not enabled
            return null;
        }

        public enum AuthenticationProviderType
        {
            MembershipProvider,
            RoleProvider
        }*/
    }
}
