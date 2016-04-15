using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;

public class ClsDBAccess
{
    private SqlConnection conObj;
    private SqlConnection conObjTransaction;
    public ClsDBAccess()
    {
        conObj = new Connection().getConnection();
        conObjTransaction = new Connection().getConnection();
    }
    public string InsertData(string qry)
    {
        //SqlConnection con =new SqlConnection(_DBCon);
        SqlCommand cmd = new SqlCommand(qry + " select scope_identity() ", conObj);
        conObj.Open();
        string ID = cmd.ExecuteScalar().ToString();
        conObj.Close();
        return ID;
    }
    public string InsertData(string qry, SqlConnection con, SqlTransaction trans)
    {
        //SqlConnection con =new SqlConnection(_DBCon);
        SqlCommand cmd = new SqlCommand(qry + " select scope_identity() ", con);
        //con.Open();
        cmd.Transaction = trans;
        string ID = cmd.ExecuteScalar().ToString();
        //con.Close();
        return ID;
    }
    public string InsertData(string procedureName, Hashtable Parameters)
    {

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = procedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }

        SqlDataAdapter ad = new SqlDataAdapter();
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.Fill(ds);
        string ID = "";
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ID = ds.Tables[0].Rows[0][0].ToString();
            }
        }
        //conObj.Open();         
        // ID = cmd.ExecuteScalar().ToString();
        //conObj.Close();
        return ID;
    }
    public void DelUpdateData(string qry, SqlConnection con, SqlTransaction trans)
    {
        //SqlConnection con =new SqlConnection(_DBCon);
        SqlCommand cmd = new SqlCommand(qry, con);
        // con.Open();
        cmd.Transaction = trans;
        cmd.ExecuteNonQuery();

        // con.Close();
    }
    public void DelUpdateData(string qry)
    {
        //SqlConnection con =new SqlConnection(_DBCon);
        SqlCommand cmd = new SqlCommand(qry, conObj);
        conObj.Open();
        cmd.ExecuteNonQuery();
        conObj.Close();
    }
    public void BindList(DropDownList lst, string TableName, string TextField, string ValueField)
    {
        string qry = "select " + TextField + "," + ValueField + " from " + TableName + " order by " + TextField;
        //SqlConnection con =new SqlConnection(_DBCon);
        SqlDataAdapter ad = new SqlDataAdapter(qry, conObj);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        lst.Items.Clear();
        lst.DataSource = ds;
        lst.DataTextField = TextField;
        lst.DataValueField = ValueField;
        lst.DataBind();
        ds = null;
    }
    public DataSet FillData(string Qry)
    {
        //string qry = "select * from  order by " + TextField;
        //SqlConnection con =new SqlConnection(_DBCon);
        SqlDataAdapter ad = new SqlDataAdapter(Qry, conObj);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        return ds;
    }
    public DataView FillData(string procedureName, Hashtable Parameters, string orderBy)
    {
        DataView dv;
        SqlDataAdapter ad = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = procedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {
            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.Fill(ds);
        dv = new DataView(ds.Tables[0]);
        if (ds.Tables.Count > 0)
        {
            dv.Sort = orderBy;
        }

        return dv;
    }
    public void BindList(string TableName, DropDownList lst, string TextField, string ValueField, string Alias)
    {
        string qry = "select " + Alias + "," + ValueField + " from " + TableName + "where IsActive=1 order by " + TextField;
        //SqlConnection con =new SqlConnection(_DBCon);
        SqlDataAdapter ad = new SqlDataAdapter(qry, conObj);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        lst.Items.Clear();
        lst.DataSource = ds;
        lst.DataTextField = TextField;
        lst.DataValueField = ValueField;
        lst.DataBind();
        ds = null;
    }
    public void BindListPrison(DropDownList lst)
    {
        string qry = "select Id,Name from Prison where IsActive=1 order by Name";
        //SqlConnection con =new SqlConnection(_DBCon);
        SqlDataAdapter ad = new SqlDataAdapter(qry, conObj);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        lst.DataSource = ds;
        lst.DataTextField = "Name";
        lst.DataValueField = "Id";
        lst.DataBind();
        ds = null;
    }
    public void BindList(DropDownList lst, string TableName, string Filter, string TextField, string ValueField)
    {
        string qry = "select " + TextField + "," + ValueField + " from " + TableName + " where " + Filter + " and IsActive=1 order by " + TextField;
        //SqlConnection con = new SqlConnection(_DBCon);
        SqlDataAdapter ad = new SqlDataAdapter(qry, conObj);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        lst.Items.Clear();
        lst.DataSource = ds;
        lst.DataTextField = TextField;
        lst.DataValueField = ValueField;
        lst.DataBind();
        ds = null;
    }
    public DataView BindList(DropDownList lst, string ProcedureName, string orderBy, Hashtable Parameters, string TextField, string ValueField)
    {
        DataView dv;
        SqlDataAdapter ad = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = ProcedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.Fill(ds);
        dv = new DataView(ds.Tables[0]);
        //if (ds.Tables.Count > 0)
        //{
            dv.Sort = orderBy;
        //}
        lst.Items.Clear();
        lst.DataSource = dv;
        lst.DataTextField = TextField;
        lst.DataValueField = ValueField;
        lst.DataBind();
        return dv;
    }
    public void bindGrid(GridView dg, string tableName, string orderBy)
    {
        //SqlConnection con = GetConnection(); 
        SqlDataAdapter ad = new SqlDataAdapter("select * from " + tableName + " order by " + orderBy, conObj);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        dg.DataSource = ds;
        dg.DataBind();
        ds = null;
    }
    public DataView BindGridProc(GridView dg, string ProcedureName, string orderBy, Hashtable Parameters)
    {
        DataView dv;
        SqlDataAdapter ad = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = ProcedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.Fill(ds);
        dv = new DataView(ds.Tables[0]);
        if (ds.Tables.Count > 0)
        {
            dv.Sort = orderBy;
        }
        dg.DataSource = dv;
        dg.DataBind();
        return dv;
    }
    public DataView BindGridProc(DataGrid dg, string ProcedureName, string orderBy, Hashtable Parameters)
    {
        DataView dv;
        SqlDataAdapter ad = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = ProcedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.Fill(ds);
        dv = new DataView(ds.Tables[0]);
        dv.Sort = orderBy;
        dg.DataSource = dv;
        dg.DataBind();
        return dv;
    }
    public void BindCombo(DropDownList ddl, String Name, String ID, String ProcName)
    {
        conObj.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conObj;
        cmd.CommandText = ProcName;
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        ddl.Items.Clear();
        if (dt.Rows.Count > 0)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = Name;
            ddl.DataValueField = ID;
            ddl.DataBind();
        }
        conObj.Close();
    }
    public void BindRadioButtonList(RadioButtonList rbl, String Name, String ID, String ProcName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conObj;
        cmd.CommandText = ProcName;
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {

            rbl.DataSource = dt;
            rbl.DataTextField = Name;
            rbl.DataValueField = ID;
            rbl.DataBind();
        }
    }
    public string DeleteData(string procedureName, Hashtable Parameters)
    {

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = procedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }

        SqlDataAdapter ad = new SqlDataAdapter();
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.Fill(ds);
        string ID = "";
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ID = ds.Tables[0].Rows[0][0].ToString();
            }
        }
        //conObj.Open();         
        // ID = cmd.ExecuteScalar().ToString();
        //conObj.Close();
        return ID;
    }
    public DataSet SelectDataProc(string procedureName, Hashtable Parameters)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = procedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }

        SqlDataAdapter ad = new SqlDataAdapter();
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
       ad.Fill(ds);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

            }
        }

        return ds;

    }
    public DataSet  SelectTableSchemaProc(string procedureName, Hashtable Parameters)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = procedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }

        SqlDataAdapter ad = new SqlDataAdapter();
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.FillSchema(ds, SchemaType.Source);   
            

        return ds;

    }
    public SqlTransaction OpenTransaction()
    {
        conObjTransaction.Open();
        SqlTransaction transaction = conObjTransaction.BeginTransaction(IsolationLevel.ReadUncommitted);
        return transaction;
    }
    public bool CloseTransaction(SqlTransaction transaction)
    {
        transaction.Commit();
        conObjTransaction.Close();    
        return true;
    }
    public void ErrorInTransaction(SqlTransaction transaction)
    {
        transaction.Rollback();
        conObjTransaction.Close();             
    }
    public string InsertData(string procedureName, Hashtable Parameters,SqlTransaction transaction)
    {

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = procedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObjTransaction;
        cmd.Transaction = transaction;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }

        SqlDataAdapter ad = new SqlDataAdapter();
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.Fill(ds);
        string ID = "";
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ID = ds.Tables[0].Rows[0][0].ToString();
            }
        }
        //conObj.Open();         
        // ID = cmd.ExecuteScalar().ToString();
        //conObj.Close();
        return ID;
    }
    public DataView BindList(ListBox lb, string ProcedureName, string orderBy, Hashtable Parameters, string TextField, string ValueField)
    {
        DataView dv;
        SqlDataAdapter ad = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = ProcedureName;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = conObj;
        //int loopCounter=0;
        ICollection ParamKeys = Parameters.Keys;
        foreach (object key in ParamKeys)
        {

            cmd.Parameters.Add(new SqlParameter(key.ToString(), Parameters[key.ToString()]));
        }
        ad.SelectCommand = cmd;
        DataSet ds = new DataSet();
        ad.Fill(ds);
        dv = new DataView(ds.Tables[0]);
        //if (ds.Tables.Count > 0)
        //{
        dv.Sort = orderBy;
        //}
        lb.Items.Clear();
        lb.DataSource = dv;
        lb.DataTextField = TextField;
        lb.DataValueField = ValueField;
        lb.DataBind();
        return dv;
    }
    public void BindCheckBoxList(CheckBoxList chk, String Name, String ID, String ProcName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conObj;
        cmd.CommandText = ProcName;
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {

            chk.DataSource = dt;
            chk.DataTextField = Name;
            chk.DataValueField = ID;
            chk.DataBind();
        }
    }
}
