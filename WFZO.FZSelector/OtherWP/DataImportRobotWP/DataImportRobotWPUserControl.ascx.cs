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


        protected void BtnCountryLevelImportID_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                    {
                        ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                        errorMessage.Value = package.ToDataTable();

                        if (string.IsNullOrWhiteSpace(errorMessage.Value))
                        {
                            lblerrorMessage.Text = "Data Inserted Successfully";
                            lblerrorMessage.ForeColor = System.Drawing.Color.Green;
                            lblerrorMessage.Visible = true;
                        }
                        else
                        {
                            lblerrorMessage.Text = "An error occured while inserting or updating Country Level data";
                            lblerrorMessage.ForeColor = System.Drawing.Color.Red;
                            lblerrorMessage.Visible = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Button1_Click", SPContext.Current.Site);

                if (!string.IsNullOrWhiteSpace(errorMessage.Value))
                {
                    lblerrorMessage.Text = "An error occured while inserting or updating data";
                    lblerrorMessage.ForeColor = System.Drawing.Color.Red;
                    lblerrorMessage.Visible = true;
                }
             
            }
        }

        protected void BtnFreeZoneLevelImportID_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                    {
                        ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                        errorMessage.Value = package.FreeZoneImport();
                        if (string.IsNullOrWhiteSpace(errorMessage.Value))
                        {
                            lblerrorMessage.Text = "Data Inserted Successfully";
                            lblerrorMessage.ForeColor = System.Drawing.Color.Green;
                            lblerrorMessage.Visible = true;
                        }
                        else
                        {
                            lblerrorMessage.Text = "An error occured while inserting or updating Freezone data";
                            lblerrorMessage.ForeColor = System.Drawing.Color.Red;
                            lblerrorMessage.Visible = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "BtnFreeZoneLevelImportID_Click", SPContext.Current.Site);

                if (!string.IsNullOrWhiteSpace(errorMessage.Value))
                {
                    lblerrorMessage.Text = "An error occured while inserting or updating data";
                    lblerrorMessage.ForeColor = System.Drawing.Color.Red;
                    lblerrorMessage.Visible = true;
                }
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
                        errorMessage.Value = package.InsertUpdateSetUpData();
                        if (string.IsNullOrWhiteSpace(errorMessage.Value))
                        {
                            lblerrorMessage.Text = "Data Inserted Successfully";
                            lblerrorMessage.ForeColor = System.Drawing.Color.Green;
                            lblerrorMessage.Visible = true;
                        }
                        else
                        {
                            lblerrorMessage.Text = "An error occured while inserting or updating Setup data";
                            lblerrorMessage.ForeColor = System.Drawing.Color.Red;
                            lblerrorMessage.Visible = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "btnSetupData_Click", SPContext.Current.Site);

                if(!string.IsNullOrWhiteSpace(errorMessage.Value))
                {
                    lblerrorMessage.Text = "An error occured while inserting or updating data";
                    lblerrorMessage.ForeColor = System.Drawing.Color.Red;
                    lblerrorMessage.Visible = true;
                }
            }
        }

        protected void BtnProfileData_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                    {
                        ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                        errorMessage.Value = package.ProfileDataImport();
                        if (string.IsNullOrWhiteSpace(errorMessage.Value))
                        {
                            lblerrorMessage.Text = "Data Inserted Successfully";
                            lblerrorMessage.ForeColor = System.Drawing.Color.Green;
                            lblerrorMessage.Visible = true;
                        }
                        else
                        {
                            lblerrorMessage.Text = "An error occured while inserting or Profile data";
                            lblerrorMessage.ForeColor = System.Drawing.Color.Red;
                            lblerrorMessage.Visible = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "BtnProfileData_Click", SPContext.Current.Site);

                if (!string.IsNullOrWhiteSpace(errorMessage.Value))
                {
                    lblerrorMessage.Text = "An error occured while inserting or updating Profile data";
                    lblerrorMessage.ForeColor = System.Drawing.Color.Red;
                    lblerrorMessage.Visible = true;
                }
            }
        }

   

  
    }
}
