﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFZO.FZSelector
{
    public static class ExcelPackageExtensions
    {
        public static DataTable ToDataTable(this ExcelPackage package)
        {
            SqlConnection con = new Connection().getConnection();

            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }
            // rowNumber = 4 start Main Category

            int catID = 0;
            int SubcatID = 20;
            int CountryRankingID = 0;
            bool endfile = false;
            bool endrowCol = false;
            string country = "";
            int CountryCount = 0;
            int FirstImport = 0;
            int countryInsert = 0;

            con.Open();


            /////////////////////////////////////Country Insert Update Code ////////////////////////////////////////////////////////

            //var regionFullNames = System.Globalization.CultureInfo
            //          .GetCultures(System.Globalization.CultureTypes.SpecificCultures)
            //          .Select(x => new System.Globalization.RegionInfo(x.LCID))
            //          ;
            //var twoLetterName = regionFullNames.FirstOrDefault(
            //                       region => region.EnglishName.Contains("Pakistan")
            //                    );

            //System.Globalization.RegionInfo rInfo = new System.Globalization.RegionInfo(Convert.ToString( twoLetterName ));
            //string s = rInfo.EnglishName;



            var row1 = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
            var rowRegion = workSheet.Cells[2, 1, 2, workSheet.Dimension.End.Column];
            int count2 = 1;
            foreach (var cell in row1)
            {
                if (!string.IsNullOrWhiteSpace(cell.Text) && count2 >= 10)
                {
                    string Country = rowRegion[1, cell.Start.Column].Text;

                    string RegionName = rowRegion[2, cell.Start.Column].Text;

                    if (!string.IsNullOrWhiteSpace(Country) && !string.IsNullOrWhiteSpace(RegionName) && !Country.Equals("Average") && !Country.Equals("Median") && !Country.Equals("Minimum") && !Country.Equals("Maximum"))
                    {
                        SqlParameter new_identity = new SqlParameter("@new_identity", SqlDbType.Int);

                        new_identity.Direction = ParameterDirection.Output;

                        SqlParameter RegionIDParam = new SqlParameter("@RegionName", SqlDbType.VarChar, 50);
                        RegionIDParam.Value = RegionName;
                        RegionIDParam.Direction = ParameterDirection.Input;

                        SqlParameter CountryNameParam = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                        CountryNameParam.Value = Convert.ToString(Country);
                        CountryNameParam.Direction = ParameterDirection.Input;

                        catID = InSertDataGETID(con, "InsertCountry", CountryNameParam, RegionIDParam, new_identity);
                    }

                }
                else
                {
                    // newRow[cell.Start.Column - 1] = "";
                }
                count2 = count2 + 1;
            }

            SqlParameter new_identity1 = new SqlParameter("@new_identity", SqlDbType.Int);
            new_identity1.Direction = ParameterDirection.Output;

            SqlParameter CategoryLevel1 = new SqlParameter("@CategoryLevel", SqlDbType.NVarChar, 50);
            CategoryLevel1.Value = "Country level";
            CategoryLevel1.Direction = ParameterDirection.Input;

            FirstImport = InSertDataGETID(con, "CategoryCount", new_identity1, CategoryLevel1);

            for (var rowNumber = 4; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {

                if (endfile.Equals(false))
                {


                    // var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                    // int count = 1;
                    // foreach (var cell in row)
                    // {
                    //     if (!string.IsNullOrWhiteSpace(cell.Text) && count >= 10)
                    //     {
                    //         country = cell.Text;

                    //         SqlParameter new_identity = new SqlParameter("@new_identity", SqlDbType.Int);

                    //         new_identity.Direction = ParameterDirection.Output;

                    //         SqlParameter RegionIDParam = new SqlParameter("@RegionID", SqlDbType.Int);
                    //         RegionIDParam.Value = 1;
                    //         RegionIDParam.Direction = ParameterDirection.Input;                           

                    //         SqlParameter CountryNameParam = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                    //         CountryNameParam.Value = Convert.ToString(country);
                    //         CountryNameParam.Direction = ParameterDirection.Input;

                    //         catID = InSertDataGETID(con, "InsertCountry", CountryNameParam, RegionIDParam , new_identity);
                    //     }
                    //     else
                    //     {
                    //         // newRow[cell.Start.Column - 1] = "";
                    //     }
                    //     count = count + 1;
                    // }


                    //break;




                    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                    var firstRow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];

                    var newRow = table.NewRow();

                    string a = row[rowNumber, 1].Text;
                    string b = row[rowNumber, 2].Text;

                    if (!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text))
                    {
                        //Check if Data Import First time or not


                        if (FirstImport > 0)
                        {

                            if (row[rowNumber, 1].Text.Equals("*******************************"))
                            {
                                // File End
                                endfile = true;
                                break;
                            }
                            else if ((!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text)) && (string.IsNullOrWhiteSpace(row[rowNumber, 2].Text)))
                            {
                                // Insert Category do nothing
                            }
                            else if ((!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text)) && (!string.IsNullOrWhiteSpace(row[rowNumber, 2].Text)))
                            {
                                // Check If SubCategory year Data Exist

                                SqlParameter new_identity = new SqlParameter("@new_identity", SqlDbType.Int);
                                new_identity.Direction = ParameterDirection.Output;

                                SqlParameter SubCategoryName = new SqlParameter("@SubCategoryName", SqlDbType.VarChar, 500);
                                SubCategoryName.Value = Convert.ToString(row[rowNumber, 1].Text);
                                SubCategoryName.Direction = ParameterDirection.Input;

                                SqlParameter CategoryLevel = new SqlParameter("@CategoryLevel", SqlDbType.NVarChar, 50);
                                CategoryLevel.Value = Convert.ToString("Country level");
                                CategoryLevel.Direction = ParameterDirection.Input;

                                SqlParameter new_Year1 = new SqlParameter("@Year", SqlDbType.Int);
                                new_Year1.Value = Convert.ToString(row[rowNumber, 8].Text);
                                new_Year1.Direction = ParameterDirection.Input;

                                SubcatID = InSertDataGETID(con, "DataCategoryYearExist", SubCategoryName, new_Year1, new_identity, CategoryLevel);

                                if (SubcatID > 0)/////////////////////////////Delete and Update////////////////////////////////////////////
                                {
                                    var temprow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
                                    var row2 = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                    int count = 1;
                                    foreach (var cell in row2)
                                    {
                                        if (count >= 10)
                                        {
                                            if (string.IsNullOrWhiteSpace(cell.Text))
                                            {
                                                endrowCol = true;
                                                break;
                                            }
                                            else
                                                if (!string.IsNullOrWhiteSpace(cell.Text))
                                                {
                                                    string Country = temprow[1, cell.Start.Column].Text;

                                                    SqlParameter new_CountryEXist = new SqlParameter("@new_identity", SqlDbType.Int);
                                                    new_CountryEXist.Direction = ParameterDirection.Output;

                                                    SqlParameter new_CountryName2 = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                                                    new_CountryName2.Value = Country;
                                                    new_CountryName2.Direction = ParameterDirection.Input;

                                                    CountryCount = InSertDataGETID(con, "CheckCountryExist", new_CountryEXist, new_CountryName2);

                                                    if (CountryCount == 1)
                                                    {
                                                        SqlParameter new_CountryRankingID = new SqlParameter("@new_identity", SqlDbType.Int);
                                                        new_CountryRankingID.Direction = ParameterDirection.Output;

                                                        SqlParameter new_SubCategoryId = new SqlParameter("@SubCategoryId", SqlDbType.Int);
                                                        new_SubCategoryId.Value = SubcatID;
                                                        new_SubCategoryId.Direction = ParameterDirection.Input;

                                                        SqlParameter new_CountryName = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                                                        new_CountryName.Value = Country;
                                                        new_CountryName.Direction = ParameterDirection.Input;

                                                        SqlParameter new_CountryRanking = new SqlParameter("@CountryRanking", SqlDbType.Float);

                                                        if (row2[rowNumber, cell.Start.Column].Text.Equals("N/A") || row2[rowNumber, cell.Start.Column].Text.Equals("-"))
                                                        {
                                                            new_CountryRanking.Value = -1;
                                                        }
                                                        else
                                                        {
                                                            new_CountryRanking.Value = Convert.ToDecimal(row2[rowNumber, cell.Start.Column].Text);
                                                        }

                                                        new_CountryRanking.Direction = ParameterDirection.Input;

                                                        SqlParameter new_Year = new SqlParameter("@Year", SqlDbType.Int);
                                                        new_Year.Value = Convert.ToString(row2[rowNumber, 8].Text);
                                                        new_Year.Direction = ParameterDirection.Input;

                                                        CountryRankingID = InSertDataGETID(con, "InsertCountryRanking", new_CountryRankingID, new_SubCategoryId, new_CountryName, new_CountryRanking, new_Year);
                                                    }

                                                }
                                        }
                                        else
                                        {
                                            // newRow[cell.Start.Column - 1] = "";
                                        }
                                        count = count + 1;
                                        CountryCount = 0;

                                    }
                                }///////////////////////////////////////////Get SubcategoryID and Insert New Data/////////////////////////////
                                else
                                {

                                    SqlParameter new_identity2 = new SqlParameter("@new_identity", SqlDbType.Int);
                                    new_identity2.Direction = ParameterDirection.Output;

                                    SqlParameter SubCategoryName2 = new SqlParameter("@SubCategoryName", SqlDbType.VarChar, 500);
                                    SubCategoryName2.Value = Convert.ToString(row[rowNumber, 1].Text);
                                    SubCategoryName2.Direction = ParameterDirection.Input;

                                    SqlParameter Methodology1 = new SqlParameter("@Methodology", SqlDbType.Int);
                                    Methodology1.Value = 1;
                                    Methodology1.Direction = ParameterDirection.Input;



                                    SubcatID = InSertDataGETID(con, "GetSubCategoryID", SubCategoryName2, new_identity2, Methodology1);

                                    if (SubcatID > 0)/////////////////////////////Add new Record////////////////////////////////////////////
                                    {
                                        var temprow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
                                        var row2 = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                        int count = 1;
                                        foreach (var cell in row2)
                                        {
                                            if (count >= 10)
                                            {
                                                if (string.IsNullOrWhiteSpace(cell.Text))
                                                {
                                                    endrowCol = true;
                                                    break;
                                                }
                                                else
                                                    if (!string.IsNullOrWhiteSpace(cell.Text))
                                                    {
                                                        string Country = temprow[1, cell.Start.Column].Text;

                                                        SqlParameter new_CountryEXist = new SqlParameter("@new_identity", SqlDbType.Int);
                                                        new_CountryEXist.Direction = ParameterDirection.Output;

                                                        SqlParameter new_CountryName2 = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                                                        new_CountryName2.Value = Country;
                                                        new_CountryName2.Direction = ParameterDirection.Input;

                                                        CountryCount = InSertDataGETID(con, "CheckCountryExist", new_CountryEXist, new_CountryName2);

                                                        if (CountryCount > 0)
                                                        {
                                                            SqlParameter new_CountryRankingID = new SqlParameter("@new_identity", SqlDbType.Int);
                                                            new_CountryRankingID.Direction = ParameterDirection.Output;

                                                            SqlParameter new_SubCategoryId = new SqlParameter("@SubCategoryId", SqlDbType.Int);
                                                            new_SubCategoryId.Value = SubcatID;
                                                            new_SubCategoryId.Direction = ParameterDirection.Input;

                                                            SqlParameter new_CountryName = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                                                            new_CountryName.Value = Country;
                                                            new_CountryName.Direction = ParameterDirection.Input;

                                                            SqlParameter new_CountryRanking = new SqlParameter("@CountryRanking", SqlDbType.Float);

                                                            if (row2[rowNumber, cell.Start.Column].Text.Equals("N/A") || row2[rowNumber, cell.Start.Column].Text.Equals("-"))
                                                            {
                                                                new_CountryRanking.Value = -1;
                                                            }
                                                            else
                                                            {
                                                                new_CountryRanking.Value = Convert.ToDecimal(row2[rowNumber, cell.Start.Column].Text);
                                                            }

                                                            new_CountryRanking.Direction = ParameterDirection.Input;

                                                            SqlParameter new_Year = new SqlParameter("@Year", SqlDbType.Int);
                                                            new_Year.Value = Convert.ToString(row2[rowNumber, 8].Text);
                                                            new_Year.Direction = ParameterDirection.Input;

                                                            CountryRankingID = InSertDataGETID(con, "InsertCountryRanking", new_CountryRankingID, new_SubCategoryId, new_CountryName, new_CountryRanking, new_Year);
                                                        }

                                                    }
                                            }
                                            else
                                            {
                                                // newRow[cell.Start.Column - 1] = "";
                                            }
                                            count = count + 1;
                                            CountryCount = 0;

                                        }
                                    }


                                }

                                SubcatID = 0;

                            }

                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        }
                        else   ////////////////////// True when data Entered For the first Time /////////////////////////////////////
                        {

                            if (row[rowNumber, 1].Text.Equals("*******************************"))
                            {
                                // File End
                                endfile = true;
                                break;
                            }
                            else if ((!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text)) && (string.IsNullOrWhiteSpace(row[rowNumber, 2].Text)))
                            {
                                // Insert Category

                                SqlParameter new_identity = new SqlParameter("@new_identity", SqlDbType.Int);
                                new_identity.Direction = ParameterDirection.Output;

                                SqlParameter CategoryParam = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50);
                                CategoryParam.Value = Convert.ToString(row[rowNumber, 1].Text);
                                CategoryParam.Direction = ParameterDirection.Input;

                                SqlParameter CategoryLevel = new SqlParameter("@CategoryLevel", SqlDbType.NVarChar, 50);
                                CategoryLevel.Value = Convert.ToString("Country level");
                                CategoryLevel.Direction = ParameterDirection.Input;


                                catID = InSertDataGETID(con, "InsertCategory", CategoryParam, CategoryLevel, new_identity);
                                if (catID == 0)
                                {

                                }

                            }

                            else if ((!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text)) && (!string.IsNullOrWhiteSpace(row[rowNumber, 2].Text)))
                            {
                                // Insert Subcategory and all other coloums

                                if (catID != 0)
                                {

                                    SqlParameter new_SubCategoryID = new SqlParameter("@new_identity", SqlDbType.Int);
                                    new_SubCategoryID.Direction = ParameterDirection.Output;

                                    SqlParameter CategoryId = new SqlParameter("@CategoryId", SqlDbType.Int);
                                    CategoryId.Value = catID;
                                    CategoryId.Direction = ParameterDirection.Input;

                                    SqlParameter SubCategoryName = new SqlParameter("@SubCategoryName", SqlDbType.VarChar, 500);
                                    SubCategoryName.Value = Convert.ToString(row[rowNumber, 1].Text);
                                    SubCategoryName.Direction = ParameterDirection.Input;


                                    SqlParameter Definition = new SqlParameter("@Definition", SqlDbType.VarChar, 100);
                                    Definition.Value = Convert.ToString(row[rowNumber, 2].Text);
                                    Definition.Direction = ParameterDirection.Input;

                                    SqlParameter Unit = new SqlParameter("@Unit", SqlDbType.VarChar, 50);
                                    Unit.Value = Convert.ToString(row[rowNumber, 3].Text);
                                    Unit.Direction = ParameterDirection.Input;

                                    SqlParameter Direction = new SqlParameter("@Direction", SqlDbType.Int);

                                    if (row[rowNumber, 4].Text.Equals("+"))
                                    {
                                        Direction.Value = 1;
                                    }
                                    else
                                    {
                                        Direction.Value = -1;
                                    }
                                    Direction.Direction = ParameterDirection.Input;

                                    SqlParameter Methodology = new SqlParameter("@Methodology", SqlDbType.VarChar, 100);
                                    Methodology.Value = Convert.ToString(row[rowNumber, 5].Text);
                                    Methodology.Direction = ParameterDirection.Input;

                                    SqlParameter Source = new SqlParameter("@Source", SqlDbType.VarChar, 100);
                                    Source.Value = "";
                                    Source.Direction = ParameterDirection.Input;


                                    SubcatID = InSertDataGETID(con, "InsertSubCategory", new_SubCategoryID, CategoryId, Definition, Unit, Direction, Methodology, SubCategoryName, Source);

                                    // Insert InsertCountryRanking

                                    if (SubcatID > 0)
                                    {
                                        var temprow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
                                        //foreach (var cell in temprow)
                                        //{
                                        //    int ab = cell.Start.Column;
                                        //    string s1 = cell.FullAddress;
                                        //    string first = firstRow[rowNumber, ab].Text;
                                        //}

                                        // var temprow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
                                        var row2 = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                        int count = 1;
                                        foreach (var cell in row2)
                                        {
                                            if (count >= 10)
                                            {
                                                if (string.IsNullOrWhiteSpace(cell.Text))
                                                {
                                                    endrowCol = true;
                                                    break;
                                                }
                                                else
                                                    if (!string.IsNullOrWhiteSpace(cell.Text))
                                                    {
                                                        string Country = temprow[1, cell.Start.Column].Text;

                                                        SqlParameter new_CountryEXist = new SqlParameter("@new_identity", SqlDbType.Int);
                                                        new_CountryEXist.Direction = ParameterDirection.Output;

                                                        SqlParameter new_CountryName2 = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                                                        new_CountryName2.Value = Country;
                                                        new_CountryName2.Direction = ParameterDirection.Input;

                                                        CountryCount = InSertDataGETID(con, "CheckCountryExist", new_CountryEXist, new_CountryName2);

                                                        if (CountryCount == 1)
                                                        {
                                                            SqlParameter new_CountryRankingID = new SqlParameter("@new_identity", SqlDbType.Int);
                                                            new_CountryRankingID.Direction = ParameterDirection.Output;

                                                            SqlParameter new_SubCategoryId = new SqlParameter("@SubCategoryId", SqlDbType.Int);
                                                            new_SubCategoryId.Value = SubcatID;
                                                            new_SubCategoryId.Direction = ParameterDirection.Input;

                                                            SqlParameter new_CountryName = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                                                            new_CountryName.Value = Country;
                                                            new_CountryName.Direction = ParameterDirection.Input;

                                                            SqlParameter new_CountryRanking = new SqlParameter("@CountryRanking", SqlDbType.Float);

                                                            if (row2[rowNumber, cell.Start.Column].Text.Equals("N/A") || row2[rowNumber, cell.Start.Column].Text.Equals("-"))
                                                            {
                                                                new_CountryRanking.Value = -1;
                                                            }
                                                            else
                                                            {
                                                                new_CountryRanking.Value = Convert.ToDecimal(row2[rowNumber, cell.Start.Column].Text);
                                                            }

                                                            new_CountryRanking.Direction = ParameterDirection.Input;

                                                            SqlParameter new_Year = new SqlParameter("@Year", SqlDbType.Int);
                                                            new_Year.Value = Convert.ToString(row2[rowNumber, 8].Text);
                                                            new_Year.Direction = ParameterDirection.Input;

                                                            CountryRankingID = InSertDataGETID(con, "InsertCountryRanking", new_CountryRankingID, new_SubCategoryId, new_CountryName, new_CountryRanking, new_Year);
                                                        }

                                                    }
                                            }
                                            else
                                            {
                                                // newRow[cell.Start.Column - 1] = "";
                                            }
                                            count = count + 1;
                                            CountryCount = 0;

                                        }
                                    }
                                }
                            }////////////////////////////End First Import If
                        }
                    }

                }
                else
                    break;


                //foreach (var cell in row)
                //{
                //    if (!string.IsNullOrWhiteSpace(cell.Text))
                //    {
                //        newRow[cell.Start.Column - 1] = cell.Text;
                //    }
                //    else
                //    {
                //        newRow[cell.Start.Column - 1] = "";
                //    }


                //}
                // table.Rows.Add(newRow);
            }
            con.Close();
            con = null;
            return table;
        }

        public static DataTable FreeZoneImport(this ExcelPackage package)
        {
            SqlConnection con = new Connection().getConnection();

            ExcelWorksheet workSheet = package.Workbook.Worksheets["Micro-Level Database"];
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }
            // rowNumber = 4 start Main Category

            int catID = 0;
            int SubcatID = 20;
            int FreeZoneyRankingID = 0;
            bool endfile = false;
            bool endrowCol = false;
            string country = "";
            int FreeZoneCount = 0;
            int FirstImport = 0;

            con.Open();


            SqlParameter new_identity1 = new SqlParameter("@new_identity", SqlDbType.Int);
            new_identity1.Direction = ParameterDirection.Output;

            SqlParameter CategoryLevel1 = new SqlParameter("@CategoryLevel", SqlDbType.NVarChar, 50);
            CategoryLevel1.Value = "Freezone level";
            CategoryLevel1.Direction = ParameterDirection.Input;

            FirstImport = InSertDataGETID(con, "CategoryCount", new_identity1, CategoryLevel1);

            for (var rowNumber = 4; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {

                if (endfile.Equals(false))
                {

                    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                    var firstRow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];

                    var newRow = table.NewRow();

                    string a = row[rowNumber, 1].Text;
                    string b = row[rowNumber, 2].Text;

                    if (!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text))
                    {
                        if (FirstImport > 0)
                        {
                            if (row[rowNumber, 1].Text.Equals("*******************************"))
                            {
                                // File End
                                endfile = true;
                                break;
                            }
                            else if ((!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text)) && (string.IsNullOrWhiteSpace(row[rowNumber, 2].Text)))
                            {
                                // Insert Category do nothing
                            }
                            else if ((!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text)) && (!string.IsNullOrWhiteSpace(row[rowNumber, 2].Text)))
                            {
                                // Check If SubCategory year Data Exist


                                /////////////////////////////Insert New Or Update////////////////////////////////////////////
                                var temprow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
                                var row2 = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                int count = 1;
                                foreach (var cell in row2)
                                {
                                    if (count >= 7)
                                    {
                                        if (string.IsNullOrWhiteSpace(cell.Text))
                                        {
                                            endrowCol = true;
                                            break;
                                        }
                                        else
                                            if (!string.IsNullOrWhiteSpace(cell.Text))
                                            {
                                                string FreeZoneName = temprow[1, cell.Start.Column].Text;

                                                SqlParameter new_identity = new SqlParameter("@new_identity", SqlDbType.Int);
                                                new_identity.Direction = ParameterDirection.Output;

                                                SqlParameter SubCategoryName = new SqlParameter("@SubCategoryName", SqlDbType.VarChar, 500);
                                                SubCategoryName.Value = Convert.ToString(row[rowNumber, 1].Text);
                                                SubCategoryName.Direction = ParameterDirection.Input;

                                                SqlParameter CategoryLevel = new SqlParameter("@CategoryLevel", SqlDbType.NVarChar, 50);
                                                CategoryLevel.Value = "Freezone level";
                                                CategoryLevel.Direction = ParameterDirection.Input;

                                                SqlParameter new_Year1 = new SqlParameter("@Year", SqlDbType.Int);
                                                new_Year1.Value = Convert.ToString(row[rowNumber, 6].Text);
                                                new_Year1.Direction = ParameterDirection.Input;

                                                SqlParameter new_FreeZoneName = new SqlParameter("@FreeZoneName", SqlDbType.NVarChar, 100);
                                                new_FreeZoneName.Value = FreeZoneName.Trim();
                                                new_FreeZoneName.Direction = ParameterDirection.Input;

                                                SqlParameter new_FreezoneRanking = new SqlParameter("@FreezoneRanking", SqlDbType.Float);

                                                if (row2[rowNumber, cell.Start.Column].Text.Equals("N/A") || row2[rowNumber, cell.Start.Column].Text.Equals("-"))
                                                {
                                                    new_FreezoneRanking.Value = -1;
                                                }
                                                else
                                                {
                                                    new_FreezoneRanking.Value = Convert.ToDecimal(row2[rowNumber, cell.Start.Column].Text);
                                                }

                                                new_FreezoneRanking.Direction = ParameterDirection.Input;

                                                SubcatID = InSertDataGETID(con, "InsertUpdateFreezoneRanking", SubCategoryName, new_Year1, new_identity, CategoryLevel, new_FreeZoneName, new_FreezoneRanking);

                                            }
                                    }
                                    else
                                    {
                                        // newRow[cell.Start.Column - 1] = "";
                                    }
                                    count = count + 1;

                                }
                                ///////////////////////////////////////////Get SubcategoryID and Insert New Data/////////////////////////////

                            }

                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        }
                        else   ////////////////////// True when data Entered For the first Time /////////////////////////////////////
                        {

                            if (row[rowNumber, 1].Text.Equals("*******************************"))
                            {
                                // File End
                                endfile = true;
                                break;
                            }
                            else if ((!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text)) && (string.IsNullOrWhiteSpace(row[rowNumber, 2].Text)))
                            {
                                // Insert Category

                                SqlParameter new_identity = new SqlParameter("@new_identity", SqlDbType.Int);
                                new_identity.Direction = ParameterDirection.Output;

                                SqlParameter CategoryParam = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50);
                                CategoryParam.Value = Convert.ToString(row[rowNumber, 1].Text);
                                CategoryParam.Direction = ParameterDirection.Input;

                                SqlParameter CategoryLevel = new SqlParameter("@CategoryLevel", SqlDbType.NVarChar, 50);
                                CategoryLevel.Value = Convert.ToString("FreeZone level");
                                CategoryLevel.Direction = ParameterDirection.Input;


                                catID = InSertDataGETID(con, "InsertCategory", CategoryParam, CategoryLevel, new_identity);
                                if (catID == 0)
                                {

                                }

                            }

                            else if ((!string.IsNullOrWhiteSpace(row[rowNumber, 1].Text)) && (!string.IsNullOrWhiteSpace(row[rowNumber, 2].Text)))
                            {
                                // Insert Subcategory and all other coloums

                                if (catID != 0)
                                {

                                    SqlParameter new_SubCategoryID = new SqlParameter("@new_identity", SqlDbType.Int);
                                    new_SubCategoryID.Direction = ParameterDirection.Output;

                                    SqlParameter CategoryId = new SqlParameter("@CategoryId", SqlDbType.Int);
                                    CategoryId.Value = catID;
                                    CategoryId.Direction = ParameterDirection.Input;

                                    SqlParameter SubCategoryName = new SqlParameter("@SubCategoryName", SqlDbType.VarChar, 500);
                                    SubCategoryName.Value = Convert.ToString(row[rowNumber, 1].Text);
                                    SubCategoryName.Direction = ParameterDirection.Input;


                                    SqlParameter Definition = new SqlParameter("@Definition", SqlDbType.VarChar, 100);
                                    Definition.Value = Convert.ToString(row[rowNumber, 2].Text);
                                    Definition.Direction = ParameterDirection.Input;

                                    SqlParameter Unit = new SqlParameter("@Unit", SqlDbType.VarChar, 50);
                                    Unit.Value = Convert.ToString(row[rowNumber, 3].Text);
                                    Unit.Direction = ParameterDirection.Input;

                                    SqlParameter Direction = new SqlParameter("@Direction", SqlDbType.Int);

                                    if (row[rowNumber, 4].Text.Equals("+"))
                                    {
                                        Direction.Value = 1;
                                    }
                                    else
                                    {
                                        Direction.Value = -1;
                                    }

                                    Direction.Direction = ParameterDirection.Input;

                                    SqlParameter Source = new SqlParameter("@Source", SqlDbType.VarChar, 100);
                                    Source.Value = Convert.ToString(row[rowNumber, 5].Text);
                                    Source.Direction = ParameterDirection.Input;

                                    SqlParameter Methodology = new SqlParameter("@Methodology", SqlDbType.VarChar, 100);
                                    Methodology.Value = "";
                                    Methodology.Direction = ParameterDirection.Input;

                                    SubcatID = InSertDataGETID(con, "InsertSubCategory", new_SubCategoryID, CategoryId, Definition, Unit, Direction, Methodology, SubCategoryName, Source);

                                    // Insert InsertCountryRanking

                                    if (SubcatID > 0)
                                    {
                                        var temprow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
                                        //foreach (var cell in temprow)
                                        //{
                                        //    int ab = cell.Start.Column;
                                        //    string s1 = cell.FullAddress;
                                        //    string first = firstRow[rowNumber, ab].Text;
                                        //}

                                        // var temprow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
                                        var row2 = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                        int count = 1;
                                        foreach (var cell in row2)
                                        {
                                            if (count >= 7)
                                            {
                                                if (string.IsNullOrWhiteSpace(cell.Text))
                                                {
                                                    endrowCol = true;
                                                    break;
                                                }
                                                else
                                                    if (!string.IsNullOrWhiteSpace(cell.Text))
                                                    {
                                                        string FreeZone = temprow[1, cell.Start.Column].Text;

                                                        SqlParameter new_FreeZoneEXist = new SqlParameter("@new_identity", SqlDbType.Int);
                                                        new_FreeZoneEXist.Direction = ParameterDirection.Output;

                                                        SqlParameter new_FreeZoneName = new SqlParameter("@FreeZoneName", SqlDbType.NVarChar, 50);
                                                        new_FreeZoneName.Value = FreeZone.Trim();
                                                        new_FreeZoneName.Direction = ParameterDirection.Input;

                                                        FreeZoneCount = InSertDataGETID(con, "CheckFreeZoneExist", new_FreeZoneEXist, new_FreeZoneName);

                                                        if (FreeZoneCount > 0)
                                                        {
                                                            SqlParameter new_FreeZoneRankingID = new SqlParameter("@new_identity", SqlDbType.Int);
                                                            new_FreeZoneRankingID.Direction = ParameterDirection.Output;

                                                            SqlParameter new_SubCategoryId = new SqlParameter("@SubCategoryId", SqlDbType.Int);
                                                            new_SubCategoryId.Value = SubcatID;
                                                            new_SubCategoryId.Direction = ParameterDirection.Input;

                                                            SqlParameter new_FreezoneId = new SqlParameter("@FreezoneId", SqlDbType.Int);
                                                            new_FreezoneId.Value = FreeZoneCount;
                                                            new_FreezoneId.Direction = ParameterDirection.Input;

                                                            SqlParameter new_FreezoneRanking = new SqlParameter("@FreezoneRanking", SqlDbType.Float);

                                                            if (row2[rowNumber, cell.Start.Column].Text.Equals("N/A") || row2[rowNumber, cell.Start.Column].Text.Equals("-"))
                                                            {
                                                                new_FreezoneRanking.Value = -1;
                                                            }
                                                            else
                                                            {
                                                                new_FreezoneRanking.Value = Convert.ToDecimal(row2[rowNumber, cell.Start.Column].Text);
                                                            }

                                                            new_FreezoneRanking.Direction = ParameterDirection.Input;

                                                            SqlParameter new_Year = new SqlParameter("@Year", SqlDbType.Int);
                                                            new_Year.Value = Convert.ToString(row2[rowNumber, 6].Text);
                                                            new_Year.Direction = ParameterDirection.Input;

                                                            FreeZoneyRankingID = InSertDataGETID(con, "InsertFreeZoneRanking", new_FreeZoneRankingID, new_SubCategoryId, new_FreezoneId, new_FreezoneRanking, new_Year);
                                                        }

                                                    }
                                            }
                                            else
                                            {
                                                // newRow[cell.Start.Column - 1] = "";
                                            }
                                            count = count + 1;
                                            FreeZoneCount = 0;
                                            //if (endrow.Equals(true))
                                            //    break;
                                        }


                                    }
                                }
                            }
                        }
                    }///////////////////////iF(fIRSTrOW)

                }
                else
                    break;


                //foreach (var cell in row)
                //{
                //    if (!string.IsNullOrWhiteSpace(cell.Text))
                //    {
                //        newRow[cell.Start.Column - 1] = cell.Text;
                //    }
                //    else
                //    {
                //        newRow[cell.Start.Column - 1] = "";
                //    }


                //}
                // table.Rows.Add(newRow);
            }
            con.Close();
            con = null;
            return table;
        }

        public static DataTable InsertUpdateSetUpData(this ExcelPackage package)
        {
            SqlConnection con = new Connection().getConnection(); 

            ExcelWorksheet workSheet = package.Workbook.Worksheets["SetupData"];
            DataTable table = new DataTable();

            int id = 0;
            con.Open();

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];

                string a = row[rowNumber, 1].Text;
                string b = row[rowNumber, 2].Text;

                SqlParameter new_identity = new SqlParameter("@new_identity", SqlDbType.Int);
                new_identity.Direction = ParameterDirection.Output;

                SqlParameter RegionName = new SqlParameter("@RegionName", SqlDbType.VarChar, 50);
                RegionName.Value = Convert.ToString(row[rowNumber, 1].Text);
                RegionName.Direction = ParameterDirection.Input;

                SqlParameter RegionStatus = new SqlParameter("@RegionStatus", SqlDbType.Bit);
                if (row[rowNumber, 2].Text.ToLower().Equals("1"))
                {
                    RegionStatus.Value = true;
                }
                if (row[rowNumber, 2].Text.ToLower().Equals("0"))
                {
                    RegionStatus.Value = false;
                }

                RegionStatus.Direction = ParameterDirection.Input;


                SqlParameter CountryName = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                CountryName.Value = Convert.ToString(row[rowNumber, 3].Text);
                CountryName.Direction = ParameterDirection.Input;

                SqlParameter CountryStatus = new SqlParameter("@CountryStatus", SqlDbType.Bit);
                if (row[rowNumber, 4].Text.ToLower().Equals("1"))
                {
                    CountryStatus.Value = true;
                }
                if (row[rowNumber, 4].Text.ToLower().Equals("0"))
                {
                    CountryStatus.Value = false;
                }

                CountryStatus.Direction = ParameterDirection.Input;

                SqlParameter CityName = new SqlParameter("@CityName", SqlDbType.NVarChar, 50);
                CityName.Value = Convert.ToString(row[rowNumber, 5].Text);
                CityName.Direction = ParameterDirection.Input;

                SqlParameter CityStatus = new SqlParameter("@CityStatus", SqlDbType.NVarChar, 50);
                if (row[rowNumber, 6].Text.ToLower().Equals("1"))
                {
                    CityStatus.Value = true;
                }
                if (row[rowNumber, 6].Text.ToLower().Equals("0"))
                {
                    CityStatus.Value = false;
                }


                SqlParameter FreeZoneName = new SqlParameter("@FreeZoneName", SqlDbType.NVarChar, 100);
                FreeZoneName.Value = Convert.ToString(row[rowNumber, 7].Text);
                FreeZoneName.Direction = ParameterDirection.Input;

                SqlParameter FreeZoneStatus = new SqlParameter("@FreeZoneStatus", SqlDbType.Bit);
                if (row[rowNumber, 8].Text.ToLower().Equals("1"))
                {
                    FreeZoneStatus.Value = true;
                }
                if (row[rowNumber, 8].Text.ToLower().Equals("0"))
                {
                    FreeZoneStatus.Value = false;
                }

                FreeZoneStatus.Direction = ParameterDirection.Input;

                id = InSertDataGETID(con, "InsertUpdateSetupData", RegionName, RegionStatus, CountryName, CountryStatus, CityName, CityStatus, FreeZoneName, FreeZoneStatus ,new_identity);

            }
            con.Close();
            con = null;
            return table;
        }

        public static int InSertDataGETID(SqlConnection con, string SPName, params SqlParameter[] Parameters)
        {
            int output = 0;

            SqlCommand cmd = new SqlCommand(SPName, con);

            cmd.CommandType = CommandType.StoredProcedure;
            if (Parameters != null)
                foreach (SqlParameter item in Parameters)
                    cmd.Parameters.Add(item);

            try
            {
                cmd.ExecuteScalar();

                if (SPName.Equals("InsertCountryRanking"))
                {
                    output = 0;
                }
                else
                {
                    output = Convert.ToInt32(cmd.Parameters["@new_identity"].Value);
                }

            }
            catch
            {
                if (con != null) con.Close();
                throw;
            }

            cmd = null;

            return output;
        }

    }

    

}




//var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
//               int count = 1;
//               foreach (var cell in row)
//               {
//                   if (!string.IsNullOrWhiteSpace(cell.Text) && count >= 10 )
//                   {                        
//                       country = cell.Text;

//                       SqlParameter new_identity = new SqlParameter("@new_identity", SqlDbType.Int);

//                       new_identity.Direction = ParameterDirection.Output;

//                       SqlParameter CountryNameParam = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
//                       CountryNameParam.Value = Convert.ToString(country);
//                       CountryNameParam.Direction = ParameterDirection.Input;

//                       catID = InSertDataGETID("InsertCity", CountryNameParam, new_identity);
//                   }
//                   else
//                   {
//                      // newRow[cell.Start.Column - 1] = "";
//                   }
//                   count = count + 1;
//               }


//               break;