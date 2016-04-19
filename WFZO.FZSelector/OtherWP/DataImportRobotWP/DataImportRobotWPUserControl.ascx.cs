using OfficeOpenXml;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WFZO.FZSelector.OtherWP.DataImportRobotWP
{
    public partial class DataImportRobotWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (FileUpload1.HasFile)
            {
                if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                {
                    ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                    package.ToDataTable();

                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                {
                    ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                    package.FreeZoneImport();

                }
            }
        }

        protected void btnSetupData_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                {
                    ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                    package.InsertUpdateSetUpData();

                }
            }
        }
    }
}
