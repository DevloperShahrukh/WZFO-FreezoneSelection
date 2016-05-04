using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using Microsoft.SharePoint.Administration;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WFZO.FZSelector.Classes
{
    public static class Helper
    {
        //     public string fromAddress = "mohsinabdul@gmail.com";
        //     public string fromName = "Mobilink LMS Admin";
        public static string mailServer = SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address;

        public static string GetConfigurationValue(string key)
        {
            using (SPSite site = new SPSite(SPContext.Current.Web.Site.RootWeb.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList("Configuration");
                    SPQuery query = new SPQuery();
                    query.Query = "<Where>"
                        + "<Eq>"
                        + "<FieldRef Name='Title' /><Value Type='Text'>"+ key +"</Value>"
                        + "</Eq>"
                        + "</Where>";
                    query.ViewFields = "<FieldRef Name='Value' />";
                    SPListItemCollection items = list.GetItems(query);

                    if (items.Count > 0)
                        return Convert.ToString(items[0]["Value"]);
                    else
                        return string.Empty;
                }
            }
        }

        public static string getfromAddress()
        {
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPQuery query = new SPQuery();
                    query.Query = @"<Where>
                                    <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.Title + @"' />
                                           <Value Type='" + Commons.Type.Text + @"'>FeedBackEmailTemplate</Value>
                                         </Eq>
                                </Where>";
                    DataTable emailTemplateItem = web.Lists.TryGetList(Constants.List.Configuration.Name).GetItems(query).GetDataTable();
                    return Convert.ToString(emailTemplateItem.Rows[0]["FromEmail"]);
                }
            }
            //   return GetAppSetting("LMS_FromAddress");
        }
        public static string getfromName()
        {
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPQuery query = new SPQuery();
                    query.Query = @"<Where>
                                    <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.Title + @"' />
                                           <Value Type='" + Commons.Type.Text + @"'>FeedBackEmailTemplate</Value>
                                         </Eq>
                                </Where>";
                    DataTable emailTemplateItem = web.Lists.TryGetList(Constants.List.Configuration.Name).GetItems(query).GetDataTable();
                    return Convert.ToString(emailTemplateItem.Rows[0]["FromName"]);
                }
            }
            //     return GetAppSetting("LMS_FromName");
        }

        public static SPUser GetUserFromSpListItem(SPWeb web, SPListItem item, string columnName)
        {
            return new SPFieldUserValue(web, item[columnName].ToString()).User;
        }

        public static SPContentTypeId GetContentId(SPSite Site, string Name)
        {

            SPWeb Web = Site.RootWeb;
            return Web.ContentTypes[Name].Id;

        }

        public static SPListItem GetListItemById(string ListName, int ListItemId, SPWeb Web)
        {
            SPListItem Item = null;
            try
            {
                return Web.Lists.TryGetList(ListName).GetItemById(ListItemId);
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
               
            }
            return Item;
        }

        public static SPListItemCollection GetListItemsByQuery(string ListName, SPQuery CamlQuery, SPWeb Web)
        {
            SPListItemCollection SpListItemColl = null;
            try
            {
                SpListItemColl = Web.Lists.TryGetList(ListName).GetItems(CamlQuery);
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
            }
            return SpListItemColl;
        }

        public static DataTable GetDTByQuery(string ListName, SPQuery CamlQuery, SPWeb Web)
        {
            DataTable Dt = null;
            try
            {
                Dt = Web.Lists.TryGetList(ListName).GetItems(CamlQuery).GetDataTable();
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
            }
            return Dt;
        }

        public static SPPrincipal FindUserOrSiteGroup(SPSite site, string userOrGroup)
        {

            SPPrincipal myUser = null;

            if (Microsoft.SharePoint.Utilities.SPUtility.IsLoginValid(site, userOrGroup))
            {
                myUser = site.RootWeb.EnsureUser(userOrGroup);
            }

            else
            {
                //might be a group
                foreach (SPGroup g in site.RootWeb.SiteGroups)
                {
                    if (g.Name.ToUpper(System.Globalization.CultureInfo.InvariantCulture) == userOrGroup.ToUpper(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        myUser = g;
                        break;
                    }
                }
            }
            return myUser;
        }

        public static void AddUserToAGroup(string userLoginName, string userGroupName)
        {
            //Executes this method with Full Control rights even if the user does not otherwise have Full Control
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite Site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb Web = Site.OpenWeb())
                    {
                        try
                        {

                            //Allow updating of some sharepoint lists, (here spUsers, spGroups etc...)
                            Web.AllowUnsafeUpdates = true;
                            SPUser spUser = Web.EnsureUser(userLoginName);

                            if (spUser != null)
                            {
                                SPGroup spGroup = Web.SiteGroups[userGroupName];
                                //isMember = Web.IsCurrentUserMemberOfGroup(spGroup.ID);  

                                if (spGroup != null)
                                {
                                    spGroup.AddUser(spUser);
                                    spGroup.Update();
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            throw ex;
                            //Error handling logic should go here
                        }
                        finally
                        {
                            Web.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }
        public static string GetAppSetting(string key)
        {
            string conn = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(conn))
                return conn;
            else
                return string.Empty;

        }
        public static string GetSrcFromImgTag(string imgTag)
        {
            int start = imgTag.IndexOf("src=") + 5;
            int end = imgTag.IndexOf("\"", start);

            if (end > start)
                return imgTag.Substring(start, end - start);
            else
                return "";

        }
        public static void SuccessMessage(string Message, Page Context)
        {
            SPPageStatusSetter newStatusCtl = new SPPageStatusSetter();
            newStatusCtl.AddStatus("Successful", Message, SPPageStatusColor.Green);
            Context.Controls.Add(newStatusCtl);
        }
        public static void ErrorMessage(string Message, Page Context)
        {
            SPPageStatusSetter newStatusCtl = new SPPageStatusSetter();
            newStatusCtl.AddStatus("Error", Message, SPPageStatusColor.Red);
            Context.Controls.Add(newStatusCtl);
        }

        public static void LogMessage(string component, string message)
        {
            try
            {
                SPDiagnosticsService.Local.WriteTrace(0, new Microsoft.SharePoint.Administration.SPDiagnosticsCategory(Assembly.GetExecutingAssembly().GetName().Name, Microsoft.SharePoint.Administration.TraceSeverity.Monitorable, Microsoft.SharePoint.Administration.EventSeverity.Information), Microsoft.SharePoint.Administration.TraceSeverity.Monitorable, component + " *** " + message, "");
            }
            catch (Exception ex)
            {
                string Exceptionn = ex.Message;
            }
        }
        public static string LogException(Exception exception, Page page)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite Site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb Web = Site.RootWeb)
                        {

                            //Allow updating of some sharepoint lists, (here spUsers, spGroups etc...)
                            Web.AllowUnsafeUpdates = true;

                            SPList Exceptionlist = Web.Lists.TryGetList(Constants.List.Exception.Name);
                            SPListItem ExceptionItem = Exceptionlist.AddItem();
                            ExceptionItem[Constants.List.Exception.Fields.Message] = exception.Message;
                            ExceptionItem[Constants.List.Exception.Fields.Stacktrace] = exception.StackTrace;

                            ExceptionItem.Update();
                            Web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                SPDiagnosticsService.Local.WriteTrace(0, new Microsoft.SharePoint.Administration.SPDiagnosticsCategory(Assembly.GetExecutingAssembly().GetName().Name, Microsoft.SharePoint.Administration.TraceSeverity.Unexpected, Microsoft.SharePoint.Administration.EventSeverity.Error), Microsoft.SharePoint.Administration.TraceSeverity.Unexpected, exception.Message, exception.StackTrace);
                if (page != null)
                {
                    ErrorMessage("An error has occurred. Please contact your system administrator", page);
                }
            }
            catch (Exception ex)
            { LogMessage("Mobilink LMS Exception", "***MESSAGE: " + ex.Message + "  ***STACK_TRACE: " + ex.StackTrace); }
            return "***MESSAGE: " + exception.Message + "  ***STACK_TRACE: " + exception.StackTrace;
        }
        public static string LogException(Exception exception, Page page, string ExtraParameter)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite Site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb Web = Site.RootWeb)
                        {

                            //Allow updating of some sharepoint lists, (here spUsers, spGroups etc...)
                            Web.AllowUnsafeUpdates = true;

                            SPList Exceptionlist = Web.Lists.TryGetList(Constants.List.Exception.Name);
                            SPListItem ExceptionItem = Exceptionlist.AddItem();
                            ExceptionItem[Constants.List.Exception.Fields.Message] = exception.Message + ExtraParameter;
                            ExceptionItem[Constants.List.Exception.Fields.Stacktrace] = exception.StackTrace;

                            ExceptionItem.Update();
                            Web.AllowUnsafeUpdates = false;
                        }
                    }

                });



                SPDiagnosticsService.Local.WriteTrace(0, new Microsoft.SharePoint.Administration.SPDiagnosticsCategory(Assembly.GetExecutingAssembly().GetName().Name, Microsoft.SharePoint.Administration.TraceSeverity.Unexpected, Microsoft.SharePoint.Administration.EventSeverity.Error), Microsoft.SharePoint.Administration.TraceSeverity.Unexpected, exception.Message, exception.StackTrace);
                if (page != null)
                {
                    ErrorMessage("An error has occurred. Please contact your system administrator", page);
                }
            }
            catch (Exception ex)
            { LogMessage("Mobilink LMS Exception", "***MESSAGE: " + ex.Message + "  ***STACK_TRACE: " + ex.StackTrace); }
            return "***MESSAGE: " + exception.Message + "  ***STACK_TRACE: " + exception.StackTrace;
        }

        #region email_methods
        public static void SendEmail(string to, string toAddress, string subject, string body, MailAddressCollection cc)
        {
            try
            {
                if (!string.IsNullOrEmpty(toAddress) && !string.IsNullOrEmpty(getfromAddress()))
                {

                    MailMessage mMailMessage = new MailMessage();
                    mMailMessage.From = new MailAddress(getfromAddress(), getfromName());
                    mMailMessage.To.Add(new MailAddress(toAddress, to));

                    if (cc != null)
                    {
                        foreach (MailAddress address in cc)
                        {
                            mMailMessage.CC.Add(address);
                        }
                    }

                    //Added HardCoded CC
                    mMailMessage.CC.Add(new MailAddress(getfromAddress(), getfromName()));

                    mMailMessage.Subject = subject;
                    mMailMessage.Body = body;
                    mMailMessage.IsBodyHtml = true;
                    mMailMessage.Priority = MailPriority.Normal;
                    SmtpClient mSmtpClient = new SmtpClient(mailServer);
                    //SmtpClient mSmtpClient = new SmtpClient();

                    //mSmtpClient.Host = "smtp.gmail.com";
                    //mSmtpClient.Port = 587;
                    //mSmtpClient.EnableSsl = true;
                    //mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //mSmtpClient.UseDefaultCredentials = false;
                    //mSmtpClient.Credentials = new System.Net.NetworkCredential("test1email111@gmail.com", "testemail123");
                    mSmtpClient.Timeout = 20000;
                    mSmtpClient.Send(mMailMessage);
                }
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
            }
        }
        public static void SendEmail(string to, string toAddress, string subject, string body, List<string> emailCC)
        {
            try
            {
                string strEmail = string.Empty;
                if (!string.IsNullOrEmpty(toAddress) && !string.IsNullOrEmpty(getfromAddress()))
                {
                    MailMessage mMailMessage = new MailMessage();
                    mMailMessage.From = new MailAddress(getfromAddress(), getfromName());
                    mMailMessage.To.Add(new MailAddress(toAddress, to));
                    for (int i = 0; i < emailCC.Count; i++)
                    {
                        strEmail = emailCC[i].Trim();
                        if (IsEmail(strEmail))
                        {
                            mMailMessage.CC.Add(strEmail);
                        }
                    }

                    //Added HardCoded CC
                    mMailMessage.CC.Add(new MailAddress(getfromAddress(), getfromName()));

                    mMailMessage.Subject = subject;
                    mMailMessage.Body = body;
                    mMailMessage.IsBodyHtml = true;
                    mMailMessage.Priority = MailPriority.Normal;
                    SmtpClient mSmtpClient = new SmtpClient(mailServer);
                    //SmtpClient mSmtpClient = new SmtpClient();

                    //mSmtpClient.Host = "smtp.gmail.com";
                    //mSmtpClient.Port = 587;
                    //mSmtpClient.EnableSsl = true;
                    //mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //mSmtpClient.UseDefaultCredentials = false;
                    //mSmtpClient.Credentials = new System.Net.NetworkCredential("test1email111@gmail.com", "testemail123");
                    //mSmtpClient.Timeout = 20000;
                    mSmtpClient.Send(mMailMessage);
                }
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
            }
        }
        public static void SendEmail(MailAddressCollection multiTo, string subject, string body, MailAddressCollection cc)
        {
            try
            {
                if (multiTo.Count > 0 && !string.IsNullOrEmpty(getfromAddress()))
                {
                    MailMessage mMailMessage = new MailMessage();
                    mMailMessage.From = new MailAddress(getfromAddress(), getfromName());
                    foreach (MailAddress address in multiTo)
                    {
                        mMailMessage.To.Add(address);
                    }
                    if (cc != null)
                    {
                        foreach (MailAddress address in cc)
                        {
                            mMailMessage.CC.Add(address);
                        }
                    }

                    //Added HardCoded CC
                    mMailMessage.CC.Add(new MailAddress(getfromAddress(), getfromName()));

                    mMailMessage.Subject = subject;
                    mMailMessage.Body = body;
                    mMailMessage.IsBodyHtml = true;
                    mMailMessage.Priority = MailPriority.Normal;

                    SmtpClient mSmtpClient = new SmtpClient(mailServer);
                    //SmtpClient mSmtpClient = new SmtpClient();

                    //mSmtpClient.Host = "smtp.gmail.com";
                    //mSmtpClient.Port = 587;
                    //mSmtpClient.EnableSsl = true;
                    //mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //mSmtpClient.UseDefaultCredentials = false;
                    //mSmtpClient.Credentials = new System.Net.NetworkCredential("test1email111@gmail.com", "testemail123");
                    //mSmtpClient.Timeout = 20000;
                    mSmtpClient.Send(mMailMessage);
                }
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
            }
        }
        private static bool IsEmail(string email)
        {
            string matchEmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            if (!string.IsNullOrEmpty(email))
                return System.Text.RegularExpressions.Regex.IsMatch(email, matchEmailPattern);
            else return false;
        }

        //myfunctionforFeeddbackEmail
        public static bool PrepareEmail(int emailTemplateId, SPWeb web, string FromName, string FromEmail, string subject)
        {
            try
            {
                MailAddressCollection multiTo = new MailAddressCollection();
                SPQuery query = new SPQuery();
                query.Query = @"<Where>
                                    <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.Title + @"' />
                                           <Value Type='" + Commons.Type.Text + @"'>FeedBackEmailTemplate</Value>
                                         </Eq>
                                </Where>";
                DataTable emailTemplateItem = web.Lists.TryGetList(Constants.List.Configuration.Name).GetItems(query).GetDataTable();

                string emailBody = Convert.ToString(emailTemplateItem.Rows[0][Constants.List.Configuration.Fields.Value]);

                //LogMessage("PrepareEmail", emailBody + " : " + getfromName() + " : " + employee.Email);

                if (!string.IsNullOrEmpty(emailBody) && !string.IsNullOrEmpty(getfromName()) && !string.IsNullOrEmpty(FromEmail))
                {
                    emailBody = Regex.Replace(emailBody, "&quot;", "\"");
                    emailBody = Regex.Replace(emailBody, "%23", "#");
                    emailBody = Regex.Replace(emailBody, "###UserName###", FromName);
                    //  emailBody = Regex.Replace(emailBody, "###CoursePath###", web.Url + "/" + course.Folder.Url + "/" + Constants.PagesLink.CourseLandingPageName);
                    emailBody = Regex.Replace(emailBody, "###RefferedBy###", FromEmail);
                    //string taskLink = SPContext.Current.Web.Url + "/" + "_layouts/listform.aspx?PageType=6&ListId=" + wFTaskItem.ParentList.ID + "&ID=" + Convert.ToString(wFTaskItem[Constants.List.Common.ID]);

                    //emailBody = emailBody.Replace("###TaskURL###", @"<a href='" + taskLink + "'><font color='#0072bc'>Click to View</font></a>");

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPSite oSite = new SPSite(SPContext.Current.Web.Url);
                        SPWeb oweb = oSite.OpenWeb();

                        //SPGroup ccGroup = oweb.SiteGroups["FIN-INS-Email-Group"];
                        MailAddressCollection multiCc = new MailAddressCollection();

                        //foreach (SPUser user in ccGroup.Users)
                        //{
                        //    if (!string.IsNullOrEmpty(user.Email))
                        //    {
                        //        string _toEmail = user.Email;
                        //        string _to = user.Name;
                        //        multiCc.Add(new MailAddress(_toEmail, _to));
                        //    }
                        //}
                        //if (Convert.ToBoolean(emailTemplateItem[Constants.List.EmailTemplates.Fields.ManagerInCC]))
                        //{
                        //    //SPUser user = Common.GetUserFromSpListItem(oweb, workflowProperties.Item, Constants.List.Initiation.InitiationBaseColumn.PocManager);
                        //    if (manager != null && !string.IsNullOrEmpty(manager.Email))
                        //    {
                        //        string _toEmail = manager.Email;
                        //        string _to = manager.Name;
                        //        multiCc.Add(new MailAddress(_toEmail, _to));
                        //    }
                        //}

                        SendEmail(FromName, FromEmail, subject, emailBody, multiCc);
                        //oweb.Close();
                        //oweb.Dispose();
                        //oSite.Dispose();
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
                return false;
            }
        }
        //public static bool PrepareEmail(int emailTemplateId, SPWeb web, SPUser employee, SPListItem course)
        //{
        //    try
        //    {
        //        MailAddressCollection multiTo = new MailAddressCollection();

        //        SPListItem emailTemplateItem = GetListItemById(Constants.List.EmailTemplates.Name, emailTemplateId, web);
        //        string emailBody = Convert.ToString(emailTemplateItem[Constants.List.EmailTemplates.Fields.Body]);

        //        //LogMessage("PrepareEmail", emailBody + " : " + getfromName() + " : " + employee.Email);

        //        if (!string.IsNullOrEmpty(emailBody) && !string.IsNullOrEmpty(getfromName()) && !string.IsNullOrEmpty(employee.Email))
        //        {
        //            string subject = Convert.ToString(emailTemplateItem[Constants.List.EmailTemplates.Fields.Subject]);

        //            emailBody = Regex.Replace(emailBody, "&quot;", "\"");
        //            emailBody = Regex.Replace(emailBody, "%23", "#");
        //            emailBody = Regex.Replace(emailBody, "###UserName###", employee.Name);
        //            emailBody = Regex.Replace(emailBody, "###CourseTitle###", course["CourseTitle"].ToString());
        //            emailBody = Regex.Replace(emailBody, "###CourseDuration###", Convert.ToString(course["ExpiryPeriod"]));
        //            //  emailBody = Regex.Replace(emailBody, "###CoursePath###", web.Url + "/" + course.Folder.Url + "/" + Constants.PagesLink.CourseLandingPageName);
        //            emailBody = Regex.Replace(emailBody, "###RefferedBy###", web.CurrentUser.Name);
        //            //string taskLink = SPContext.Current.Web.Url + "/" + "_layouts/listform.aspx?PageType=6&ListId=" + wFTaskItem.ParentList.ID + "&ID=" + Convert.ToString(wFTaskItem[Constants.List.Common.ID]);

        //            //emailBody = emailBody.Replace("###TaskURL###", @"<a href='" + taskLink + "'><font color='#0072bc'>Click to View</font></a>");

        //            SPSecurity.RunWithElevatedPrivileges(delegate()
        //            {
        //                SPSite oSite = new SPSite(SPContext.Current.Web.Url);
        //                SPWeb oweb = oSite.OpenWeb();

        //                //SPGroup ccGroup = oweb.SiteGroups["FIN-INS-Email-Group"];
        //                MailAddressCollection multiCc = new MailAddressCollection();

        //                //foreach (SPUser user in ccGroup.Users)
        //                //{
        //                //    if (!string.IsNullOrEmpty(user.Email))
        //                //    {
        //                //        string _toEmail = user.Email;
        //                //        string _to = user.Name;
        //                //        multiCc.Add(new MailAddress(_toEmail, _to));
        //                //    }
        //                //}
        //                //if (Convert.ToBoolean(emailTemplateItem[Constants.List.EmailTemplates.Fields.ManagerInCC]))
        //                //{
        //                //    //SPUser user = Common.GetUserFromSpListItem(oweb, workflowProperties.Item, Constants.List.Initiation.InitiationBaseColumn.PocManager);
        //                //    if (manager != null && !string.IsNullOrEmpty(manager.Email))
        //                //    {
        //                //        string _toEmail = manager.Email;
        //                //        string _to = manager.Name;
        //                //        multiCc.Add(new MailAddress(_toEmail, _to));
        //                //    }
        //                //}

        //                SendEmail(employee.Name, employee.Email, subject, emailBody, multiCc);
        //                //oweb.Close();
        //                //oweb.Dispose();
        //                //oSite.Dispose();
        //            });
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
        //        return false;
        //    }
        //}
        //public static bool PrepareEmailForAdmin(int emailTemplateId, SPWeb web, SPUser employee, SPListItem course)
        //{
        //    try
        //    {
        //        MailAddressCollection multiTo = new MailAddressCollection();

        //        SPListItem emailTemplateItem = GetListItemById(Constants.List.EmailTemplates.Name, emailTemplateId, web);

        //        string emailBody = Convert.ToString(emailTemplateItem[Constants.List.EmailTemplates.Fields.Body]);

        //        //LogMessage("PrepareEmailForAdmin", Convert.ToString(emailBody) + " : " + Convert.ToString(getfromAddress()));

        //        if (!string.IsNullOrEmpty(emailBody) && !string.IsNullOrEmpty(getfromAddress()) /*&& !string.IsNullOrEmpty(employee.Email)*/)
        //        {
        //            string subject = Convert.ToString(emailTemplateItem[Constants.List.EmailTemplates.Fields.Subject]);

        //            emailBody = Regex.Replace(emailBody, "&quot;", "\"");
        //            emailBody = Regex.Replace(emailBody, "%23", "#");
        //            emailBody = Regex.Replace(emailBody, "###UserName###", employee.Name);
        //            emailBody = Regex.Replace(emailBody, "###CourseTitle###", course["CourseTitle"].ToString());
        //            emailBody = Regex.Replace(emailBody, "###CourseDuration###", Convert.ToString(course["ExpiryPeriod"]));
        //            //emailBody = Regex.Replace(emailBody, "###CoursePath###", web.Url + "/" + course.Folder.Url + "/" + Constants.PagesLink.CourseLandingPageName);

        //            //string taskLink = SPContext.Current.Web.Url + "/" + "_layouts/listform.aspx?PageType=6&ListId=" + wFTaskItem.ParentList.ID + "&ID=" + Convert.ToString(wFTaskItem[Constants.List.Common.ID]);

        //            //emailBody = emailBody.Replace("###TaskURL###", @"<a href='" + taskLink + "'><font color='#0072bc'>Click to View</font></a>");

        //            SPSecurity.RunWithElevatedPrivileges(delegate()
        //            {
        //                SPSite oSite = new SPSite(SPContext.Current.Web.Url);
        //                SPWeb oweb = oSite.OpenWeb();

        //                //SPGroup ccGroup = oweb.SiteGroups["FIN-INS-Email-Group"];
        //                MailAddressCollection multiCc = new MailAddressCollection();
        //                //foreach (SPUser user in ccGroup.Users)
        //                //{
        //                //    if (!string.IsNullOrEmpty(user.Email))
        //                //    {
        //                //        string _toEmail = user.Email;
        //                //        string _to = user.Name;
        //                //        multiCc.Add(new MailAddress(_toEmail, _to));
        //                //    }
        //                //}
        //                //if (Convert.ToBoolean(emailTemplateItem[Constants.List.EmailTemplates.Fields.ManagerInCC]))
        //                //{
        //                //    //SPUser user = Common.GetUserFromSpListItem(oweb, workflowProperties.Item, Constants.List.Initiation.InitiationBaseColumn.PocManager);
        //                //    if (manager != null && !string.IsNullOrEmpty(manager.Email))
        //                //    {
        //                //        string _toEmail = manager.Email;
        //                //        string _to = manager.Name;
        //                //        multiCc.Add(new MailAddress(_toEmail, _to));
        //                //    }
        //                //}

        //                SPGroup ToGroup = oweb.SiteGroups["LMS Admin"];
        //                MailAddressCollection adminMultiTo = new MailAddressCollection();
        //                foreach (SPUser user in ToGroup.Users)
        //                {
        //                    if (!string.IsNullOrEmpty(user.Email))
        //                    {
        //                        string _toEmail = user.Email;
        //                        string _to = user.Name;
        //                        adminMultiTo.Add(new MailAddress(_toEmail, _to));
        //                    }
        //                }
        //                SendEmail(adminMultiTo, subject, emailBody, multiCc);
        //                //oweb.Close();
        //                //oweb.Dispose();
        //                //oSite.Dispose();
        //            });
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
        //        return false;
        //    }
        //}
        //public static bool PrepareEmail(int emailTemplateId, SPWeb web, SPUser employee, SPListItem course, int days)
        //{
        //    try
        //    {
        //        MailAddressCollection multiTo = new MailAddressCollection();

        //        SPListItem emailTemplateItem = GetListItemById(Constants.List.EmailTemplates.Name, emailTemplateId, web);
        //        string emailBody = Convert.ToString(emailTemplateItem[Constants.List.EmailTemplates.Fields.Body]);


        //        if (!string.IsNullOrEmpty(emailBody) && !string.IsNullOrEmpty(getfromAddress()) && !string.IsNullOrEmpty(employee.Email))
        //        {
        //            string subject = Convert.ToString(emailTemplateItem[Constants.List.EmailTemplates.Fields.Subject]);

        //            emailBody = Regex.Replace(emailBody, "&quot;", "\"");
        //            emailBody = Regex.Replace(emailBody, "%23", "#");
        //            emailBody = Regex.Replace(emailBody, "###UserName###", employee.Name);
        //            emailBody = Regex.Replace(emailBody, "###CourseTitle###", course["CourseTitle"].ToString());
        //            emailBody = Regex.Replace(emailBody, "###Days###", Convert.ToString(days));
        //            //emailBody = Regex.Replace(emailBody, "###CoursePath###", web.Url + "/" + course.Folder.Url + "/" + Constants.PagesLink.CourseLandingPageName);

        //            //SPGroup ccGroup = oweb.SiteGroups["FIN-INS-Email-Group"];
        //            MailAddressCollection multiCc = new MailAddressCollection();
        //            //foreach (SPUser user in ccGroup.Users)
        //            //{
        //            //    if (!string.IsNullOrEmpty(user.Email))
        //            //    {
        //            //        string _toEmail = user.Email;
        //            //        string _to = user.Name;
        //            //        multiCc.Add(new MailAddress(_toEmail, _to));
        //            //    }
        //            //}
        //            //if (Convert.ToBoolean(emailTemplateItem[Constants.List.EmailTemplates.Fields.ManagerInCC]))
        //            //{
        //            //    //SPUser user = Common.GetUserFromSpListItem(oweb, workflowProperties.Item, Constants.List.Initiation.InitiationBaseColumn.PocManager);
        //            //    if (manager != null && !string.IsNullOrEmpty(manager.Email))
        //            //    {
        //            //        string _toEmail = manager.Email;
        //            //        string _to = manager.Name;
        //            //        multiCc.Add(new MailAddress(_toEmail, _to));
        //            //    }
        //            //}

        //            SendEmail(employee.Name, employee.Email, subject, emailBody, multiCc);
        //            //oweb.Close();
        //            //oweb.Dispose();
        //            //oSite.Dispose();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
        //        return false;
        //    }
        //}

        #endregion


        public static bool IntegerInputValidation(string QuerStringValue)
        {
            return Regex.IsMatch(QuerStringValue, @"^\d+$");
        }

        public static bool OnlyStringInputValidation(string QuerStringValue)
        {
            return Regex.IsMatch(QuerStringValue, @"^[a-zA-Z\s]+");
            // /^[A-z]+$/"

        }
        //public static string LoadRequestorDept(string key)
        //{
        //    string keyValue = "";
        //    try
        //    {
        //        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
        //        {
        //            SPServiceContext serviceContext = SPServiceContext.GetContext(site);

        //            UserProfileManager upm = new UserProfileManager(serviceContext);
        //            // PeopleManager p = new PeopleManager(serviceContext);
        //            UserProfile user = upm.GetUserProfile(SPContext.Current.Web.CurrentUser.LoginName);
        //            //   UserProfile user = upm.GetUserProfile("corporate-jcctv\\DAhmad");
        //            String property = key;
        //            UserProfileValueCollection col = user[property];

        //            keyValue = col.Value.ToString();

        //        }
        //    }
        //    catch
        //    { }

        //    return keyValue;
        //}
    }
}
