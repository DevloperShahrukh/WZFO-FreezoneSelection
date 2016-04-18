using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using WFZO.FZSelector.Classes;


namespace WFZO.FZSelector.Layouts.WFZO.FZSelector
{
    public partial class WebMethods : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [System.Web.Services.WebMethod()]

        public static bool UpdateCategoryAnalytics(string SubCategoryIds, string ModuleName)
        {
            string[] SplittedIds = SubCategoryIds.Split(',');

            foreach (string SubCategoryId in SplittedIds)
            {
                if (Convert.ToInt32(SubCategoryId) != 0)
                {
                    SqlParameter sqpSubCategoryId = new SqlParameter("@SubCategoryId", SqlDbType.Int);
                    sqpSubCategoryId.Value = Convert.ToInt32(SubCategoryId);

                    SqlParameter sqpModule = new SqlParameter("@Module", SqlDbType.VarChar, 50);
                    sqpModule.Value = ModuleName;

                    ClsDBAccess.GetIntScalarVal("UpdateCategoryAnalytics", sqpSubCategoryId, sqpModule);
                }
            }

            return true;
        }



        [System.Web.Services.WebMethod()]
        public static bool UpdateFreeZoneAnalytics(List<FreezoneAnalyticData> DataArray)
        {
            foreach (FreezoneAnalyticData Freezone in DataArray)
            {

                SqlParameter sqpRegionId = new SqlParameter("@RegionId", SqlDbType.Int);
                sqpRegionId.Value = Convert.ToInt32(Freezone.RegionId);

                SqlParameter sqpCountryId = new SqlParameter("@CountryId", SqlDbType.VarChar, 50);
                sqpCountryId.Value = Convert.ToInt32(Freezone.CountryId);

                SqlParameter sqpCityId = new SqlParameter("@CityId", SqlDbType.Int);
                sqpCityId.Value = Convert.ToInt32(Freezone.CityId);

                SqlParameter sqpFreezoneId = new SqlParameter("@FreezoneId", SqlDbType.VarChar, 50);
                sqpFreezoneId.Value = Convert.ToInt32(Freezone.FreezoneId);

                ClsDBAccess.GetIntScalarVal("UpdateFreeZoneAnalytics", sqpRegionId, sqpCountryId, sqpCityId, sqpFreezoneId);
            }

            return true;
        }






    }
}
