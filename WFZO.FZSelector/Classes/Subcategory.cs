using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Subcategory
{
    private int subCategoryID;
    private string subCategoryName;

    public int SubCategoryID
    {
        get { return this.subCategoryID; }
        set { this.subCategoryID = value; } 
    }

    public string SubCategoryName
    {
        get { return this.subCategoryName; }
        set { this.subCategoryName = value; } 
    }

    public Subcategory(int subCategoryID, string subCategoryName)
    {
        this.subCategoryID = subCategoryID;
        this.subCategoryName = subCategoryName; 
    }


    public Subcategory()
    {

    }
}
