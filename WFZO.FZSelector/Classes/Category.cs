using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Category
{
    private int categoryID;
    private string categoryName;
    private List<Subcategory> subcategoryList = new List<Subcategory>();


    public int CategoryID
    {
        get { return this.categoryID; }
        set { this.categoryID = value; }
    }

    public string CategoryName
    {
        get { return this.categoryName; }
        set { this.categoryName = value; }
    }

    public List<Subcategory> SubcategoryList
    {
        get { return this.subcategoryList; }
        set { this.subcategoryList = value; }
    }

    public List<Category> GetCategories(string countries, ClsDBAccess obj)
    {

        List<Category> categoryList = new List<Category>();

        Hashtable par = new Hashtable();
        par.Add("@CountryId", countries);
        DataSet ds = obj.SelectDataProc("GetSubcategoryDataByCountryID", par);
        DataTable dt = ds.Tables[0];

        try
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Category category = new Category();
                category.CategoryID = Convert.ToInt32(dt.Rows[i]["CategoryId"]);
                category.CategoryName = dt.Rows[i]["CategoryName"] as String;

                if (!DoesCategoryIDAlreadyExists(category.CategoryID, categoryList))
                {
                    categoryList.Add(category);
                  
                    categoryList[i].subcategoryList.Add(new Subcategory(Convert.ToInt32(dt.Rows[i]["SubCategoryID"]), dt.Rows[i]["SubCategoryName"] as String));
                }
                else
                {
                    categoryList[i].subcategoryList.Add(new Subcategory(Convert.ToInt32(dt.Rows[i]["SubCategoryID"]), dt.Rows[i]["SubCategoryName"] as String));
                }
            }



        }
        catch (Exception ex)
        {
            string exception = ex.Message;
            // Logg the exception here
        }

        return categoryList;
    }

    // Helper method 
    private bool DoesCategoryIDAlreadyExists(int categoryID, List<Category> categoryIDList)
    {
        bool result = false;

        foreach (Category category in categoryIDList)
        {
            if (category.CategoryID == categoryID)
            {
                result = true;
                break;
            }
        }

        return result;
    }


    public Category()
    {



    }


}
