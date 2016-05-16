using Microsoft.SharePoint;
using OfficeOpenXml;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.OtherWP.DataImportRobotWP
{
    public partial class DataImportRobotWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                    {
                        ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                        errorMessage.Value =  package.ToDataTable();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Button1_Click", SPContext.Current.Site);
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                    {
                        ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                        errorMessage.Value =  package.FreeZoneImport();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Button2_Click", SPContext.Current.Site);
            }
        }

        protected void btnSetupData_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                    {
                        ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                        errorMessage.Value =  package.InsertUpdateSetUpData();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "btnSetupData_Click", SPContext.Current.Site);
            }
        }
    }
}
