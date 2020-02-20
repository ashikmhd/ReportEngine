using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
namespace DataLayer
{
    public class clsDataExecution : IDataExecution, IDisposable
    {
        public SqlTransaction Trans = null;
        private SqlCommand Com = null;
        private SqlConnection Conn = null;
        private SqlDataAdapter Adptr = null;
        private bool blnStartTrans;
        public long MasterID = 0;
        public long NewIDENTITY = 0;
        private Boolean ExistStatus = false;
        public string ExistRemarks = "";


        public string ConString;
      

        public clsDataExecution()
        {
            ConString = ConfigurationManager.AppSettings["ConnString"].ToString();
        }

        //geginning transaction
        public void BeginTransaction(string ConnectionString, bool StartTransaction)
        {
            Conn = new SqlConnection(ConnectionString);
            try
            {
                Conn.Open();
                SqlCommand CMD = new SqlCommand("SET DATEFORMAT MDY", Conn);
                CMD.ExecuteNonQuery();
                CMD.Dispose();
                blnStartTrans = StartTransaction;
                if (blnStartTrans == true)
                    Trans = Conn.BeginTransaction();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //ending transcation
        public void EndTransaction(bool Rollback = false)
        {
            try
            {
                if (blnStartTrans == true)
                {
                    if (Rollback == false)
                    {
                        Trans.Commit();
                        Trans = null;
                    }
                    else
                    {
                        Trans.Rollback();
                        Trans = null;
                    }
                }
                Conn.Close();
            }
            catch (SqlException ex)
            {
                if (Trans != null) Trans.Rollback();
                Trans = null;
                Conn = null;
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    Trans = null;
                    Conn.Close();
                    Conn.Dispose();
                    Adptr = null;
                }
            }
        }
        Boolean IDataExecution.ExistStatus
        {
            get { return ExistStatus; }
            set { ExistStatus = value; }
        }
        string IDataExecution.ExistRemarks
        {
            get { return ExistRemarks; }
            set { ExistRemarks = value; }
        }
        /// <summary>
        /// this Function will not return anything,but we will get the status by ExistRemarks,ExistStatus
        /// </summary>
        /// <param name="Query">the Query should Contain RAISERROR('error',1,1),Only RAISERROR will return error</param>
        /// <param name="fields">The Name of the Field to Validate</param>
        /// <param name="Condition">Only Active Row Should Check.example Recordstastus=0 and ID = yourID </param>
        /// <param name="FieldValues">Value which passing to purticular field</param>
        /// <param name="TableName">Name Of the Table</param>
        public void CheckExists(string TableName, string[] fields, string[] FieldValues, string Condition = "")
        {
            string Query = "";
            DataTable DT = null;
            int i = 0;
            try
            {
                foreach (string Field in fields)
                {
                    Query = " Select " + Field + " From " + TableName + " where " + Field + "='" + FieldValues[i].ToString() + "'";
                    if (Condition != "")
                    {
                        Query = Query + " and " + Condition;
                    }
                    DT = FetchResultAsTable(Query, CommandType.Text);
                    if (DT != null)
                    {
                        if (DT.Rows.Count > 0)
                        {
                            this.ExistStatus = true;
                            this.ExistRemarks = " Already Exist " + Field.Split('_')[0] + "  '" + FieldValues[i].ToString() + "'";
                            return;
                        }
                    }
                    i = i + 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CheckExists(string Query, CommandType CmdType)
        {
            DataTable DT = null;
            try
            {
                DT = FetchResultAsTable(Query, CmdType);
                if (DT.Rows.Count > 0)
                {
                    this.ExistStatus = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Fetching Data As DataTable
        /// </summary>
        /// <param name="CommandText">Sql Text Query</param>
        /// <param name="CmdType">Type Of Command</param>
        /// <returns></returns>
        public DataTable FetchResultAsTable(string CommandText, CommandType CmdType)
        {
            DataTable Dt = new DataTable();
            try
            {
                Com = new SqlCommand();
                Com.Connection = Conn;
                Com.CommandText = CommandText;
                Com.CommandType = CmdType;
                if (blnStartTrans == true) Com.Transaction = Trans;
                Adptr = new SqlDataAdapter();
                Adptr.SelectCommand = Com;
                Adptr.Fill(Dt);
                Adptr.Dispose();
                Adptr = null;
            }
            catch (Exception Ex)
            {
                Dt = null;
                throw Ex;
            }
            return Dt;
        }
        /// <summary>
        /// Fetch Result As Datatable
        /// </summary>
        /// <param name="sqlcom">SqlCommand Object</param>
        /// <returns></returns>
        public DataTable FetchResultAsTable(SqlCommand sqlcom, CommandType CmdType = CommandType.StoredProcedure)
        {
            DataTable Dt = new DataTable();
            try
            {
                Com = new SqlCommand();
                Com = sqlcom;
                Com.Connection = Conn;
                Com.CommandType = CmdType;
                if (blnStartTrans == true) Com.Transaction = Trans;
                Adptr = new SqlDataAdapter();
                Adptr.SelectCommand = Com;
                Adptr.Fill(Dt);
                Adptr.Dispose();
                Adptr = null;
            }
            catch (Exception Ex)
            {
                Dt = null;
                throw Ex;
            }
            return Dt;
        }
        /// <summary>
        /// Fetch Result as Datatable
        /// </summary>
        /// <param name="spName">Stored Procedure Name</param>
        /// <param name="parameterValues">Pass the Parameter Values in Order</param>
        /// <returns></returns>
        public DataTable FetchResultAsTable(string spName, params object[] parameterValues)
        {
            DataTable Dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlParameter[] commandParameters = null;
                if ((parameterValues != null) & parameterValues.Length > 0)
                {
                    //getting parameters...
                    commandParameters = GetSpParameterSet(spName, false);
                    //assign values to parameters..
                    AssignParameterValues(commandParameters, parameterValues);
                    // executing procedure..
                    PrepareCommand(cmd, Conn, Trans, spName, commandParameters);
                    Adptr = new SqlDataAdapter();
                    Adptr.SelectCommand = cmd;
                    Adptr.Fill(Dt);
                }
            }
            catch (Exception Ex)
            {
                Dt = null;
                throw Ex;
            }
            return Dt;
        }
        /// <summary>
        /// Fetch Data For ComboBox
        /// </summary>
        /// <param name="TableName">TableName</param>
        /// <param name="ValueField">ValueField</param>
        /// <param name="TextFiled">TextFiled</param>
        /// <param name="Condition">Condition</param>
        /// <returns></returns>
        public DataTable FetchResultAsTable_Combo(string TableName, string ValueField, string TextFiled, string Condition)
        {
            DataTable Dt = new DataTable();
            string CommandText = "Select " + ValueField + "," + TextFiled + " From " + TableName + " Where " + Condition;
            try
            {
                Com = new SqlCommand();
                Com.Connection = Conn;
                Com.CommandText = CommandText;
                Com.CommandType = CommandType.Text;
                if (blnStartTrans == true) Com.Transaction = Trans;
                Adptr = new SqlDataAdapter();
                Adptr.SelectCommand = Com;
                Adptr.Fill(Dt);
                Adptr.Dispose();
                Adptr = null;
            }
            catch (Exception Ex)
            {
                Dt = null;
                throw Ex;
            }
            return Dt;
        }
        /// <summary>
        /// Fetching 
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public DataTable FetchResultAsTable_Combo(string Query)
        {
            DataTable Dt = new DataTable();
            string CommandText = Query;
            try
            {
                Com = new SqlCommand();
                Com.Connection = Conn;
                Com.CommandText = CommandText;
                Com.CommandType = CommandType.Text;
                if (blnStartTrans == true) Com.Transaction = Trans;
                Adptr = new SqlDataAdapter();
                Adptr.SelectCommand = Com;
                Adptr.Fill(Dt);
                Adptr.Dispose();
                Adptr = null;
            }
            catch (Exception Ex)
            {
                Dt = null;
                throw Ex;
            }
            return Dt;
        }
        public DataTable FetchResultAsTable_CheckBoxlist(string TableName, string ValueField, string TextFiled, string Condition)
        {
            DataTable Dt = new DataTable();
            string CommandText = " select " + ValueField + "," + TextFiled + " From " + TableName + " Where " + Condition;
            try
            {
                Com = new SqlCommand();
                Com.Connection = Conn;
                Com.CommandText = CommandText;
                Com.CommandType = CommandType.Text;
                if (blnStartTrans == true) Com.Transaction = Trans;
                Adptr = new SqlDataAdapter();
                Adptr.SelectCommand = Com;
                Adptr.Fill(Dt);
                Adptr.Dispose();
                Adptr = null;
            }
            catch (Exception Ex)
            {
                Dt = null;
                throw Ex;
            }
            return Dt;
        }
        public DataSet FetchResultAsdataset(string CommandText, CommandType CmdType)
        {
            DataSet DS = new DataSet();
            try
            {
                Com = new SqlCommand();
                Com.Connection = Conn;
                Com.CommandText = CommandText;
                Com.CommandType = CmdType;
                if (blnStartTrans == true) Com.Transaction = Trans;
                Adptr = new SqlDataAdapter();
                Adptr.SelectCommand = Com;
                Adptr.Fill(DS);
                Adptr.Dispose();
                Adptr = null;
            }
            catch (Exception Ex)
            {
                DS = null;
                throw Ex;
            }
            return DS;
        }
        public DataSet FetchResultAsdataset(SqlCommand sqlcom)
        {
            DataSet DS = new DataSet();
            try
            {
                Com = new SqlCommand();
                Com = sqlcom;
                Com.Connection = Conn;
                if (blnStartTrans == true) Com.Transaction = Trans;
                Adptr = new SqlDataAdapter();
                Adptr.SelectCommand = Com;
                Adptr.Fill(DS);
                Adptr.Dispose();
                Adptr = null;
            }
            catch (Exception Ex)
            {
                DS = null;
                throw Ex;
            }
            return DS;
        }
        public DataSet FetchResultAsdataset(string spName, params object[] parameterValues)
        {
            DataSet Ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlParameter[] commandParameters = null;
                if ((parameterValues != null) & parameterValues.Length > 0)
                {
                    //getting parameters...
                    commandParameters = GetSpParameterSet(spName, false);
                    //assign values to parameters..
                    AssignParameterValues(commandParameters, parameterValues);
                    // executing procedure..
                    PrepareCommand(cmd, Conn, Trans, spName, commandParameters);
                    Adptr = new SqlDataAdapter();
                    Adptr.SelectCommand = cmd;
                    Adptr.Fill(Ds);
                }
                Adptr = new SqlDataAdapter();
                Adptr.SelectCommand = cmd;
                Adptr.Fill(Ds);
            }
            catch (Exception Ex)
            {
                Adptr = null;
                Ds = null;
                throw Ex;
            }
            finally
            {
                Adptr = null;
            }
            return Ds;
        }
        //Object Save and making insert query;   

        //getting lastId;
        //private void GetLastID(SqlCommand cmd)
        //{
        //    cmd.Parameters.Clear();
        //    cmd.CommandText = "SP_IDENTITY";
        //    //cmd.CommandText = "SELECT @@IDENTITY AS 'Identity'";
        //    String  dd= cmd.ExecuteScalar().ToString();
        //    //NewIDENTITY = long.Parse(cmd.ExecuteScalar().ToString());
        //}
        //  Public Function GetLastID(ByRef cmd As SqlCommand) As Long
        //    cmd.CommandText = "SELECT SCOPE_IDENTITY()"
        //    GetLastID = cmd.ExecuteScalar
        //    Return GetLastID
        //End Function
        /// <summary>
        /// Return the scalar Varaible or single value
        /// </summary>
        /// <param name="Query">pass the textQuery,Stored Procedure</param>
        /// <param name="CmdType">pass the type of command!default it will take as text query</param>
        /// <returns></returns>
        public string ExecuteScalar(string Query, CommandType CmdType = CommandType.Text)
        {
            string MaxId = "";
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                cmd.CommandType = CmdType;
                cmd.CommandText = Query;
                MaxId = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                MaxId = "";
                throw ex;
            }
            return MaxId;
        }
        /// <summary>
        /// Return the scalar Varaible or single value
        /// </summary>
        /// <param name="Query">pass the textQuery,Stored Procedure</param>
        /// <param name="CmdType">pass the type of command!default it will take as text query</param>
        /// <returns></returns>
        public string ExecuteScalar(SqlCommand Command)
        {
            string MaxId = "";
            SqlCommand cmd = new SqlCommand();
            cmd = Command;
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                MaxId = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                MaxId = "";
                throw ex;
            }
            return MaxId;
        }
        //update query and making update query;
        /// <summary>
        /// Execution with query
        /// </summary>
        /// <param name="strQuery">Query For Execution</param>
        /// <param name="Cmdtype">Type of Query</param>
        /// <returns>integer value </returns>
        public int ExecuteQuery(string strQuery, CommandType Cmdtype = CommandType.Text)
        {
            int functionReturnValue = 0;
            SqlCommand cmd = new SqlCommand();
            functionReturnValue = -1;
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                cmd.CommandType = Cmdtype;
                cmd.CommandText = strQuery;
                functionReturnValue = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                functionReturnValue = -1;
                throw ex;
            }
            return functionReturnValue;
        }
        public int ExecuteQuery(SqlCommand Command, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            int functionReturnValue = 0;
            SqlCommand cmd = new SqlCommand();
            cmd = Command;

            functionReturnValue = -1;
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                cmd.CommandType = Cmdtype;
                functionReturnValue = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                functionReturnValue = -1;
                throw ex;
            }
            return functionReturnValue;
        }
        /// <summary>
        /// Filling the object valu by passing query and object
        /// </summary>
        /// <param name="SqlCommand">Pass the text Query</param>
        /// <param name="Obj">Pass the object wich to be fill</param>
        /// <returns></returns>
        public object FillSingleObject(string SqlCommand, object Obj)
        {
            SqlCommand cmd = new SqlCommand(SqlCommand, Conn);
            if (blnStartTrans == true)
                cmd.Transaction = Trans;

            SqlDataAdapter daAdp = new SqlDataAdapter(cmd);
            DataTable dsTable = new DataTable();
            try
            {
                daAdp.Fill(dsTable);
                if (dsTable.Rows.Count == 0) return Obj;
                Type objType = Obj.GetType();
                FieldInfo objPropertyInfo = default(FieldInfo);

                DataColumn Col = null;
                DataRow dRow = dsTable.Rows[0];
                foreach (DataColumn Col_loopVariable in dsTable.Columns)
                {
                    Col = Col_loopVariable;
                    objPropertyInfo = objType.GetField(Col.ColumnName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    //if (objPropertyInfo == null)
                    //{
                    //    Exception EX = new Exception(" Table Field'" + Col.ColumnName + "' Not Declared in Class OR Field declared as inavalid fromat.!");
                    //    throw EX;
                    //}
                    if (objPropertyInfo != null)
                    {
                        switch (objPropertyInfo.FieldType.FullName)
                        {
                            case "System.Int16":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt16(dRow[Col.ColumnName]));
                                break;
                            case "System.Int32":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt32(dRow[Col.ColumnName]));
                                break;
                            case "System.Int64":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt64(dRow[Col.ColumnName]));
                                break;
                            case "System.Decimal":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDecimal(dRow[Col.ColumnName]));
                                break;
                            case "System.Double":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDouble(dRow[Col.ColumnName]));
                                break;
                            case "System.Single":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToSingle(dRow[Col.ColumnName]));
                                break;
                            case "System.Byte":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToByte(dRow[Col.ColumnName]));
                                break;
                            case "System.String":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToString(dRow[Col.ColumnName]));
                                break;
                            case "System.DateTime":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDateTime(dRow[Col.ColumnName]));
                                break;
                            case "System.Boolean":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToBoolean(dRow[Col.ColumnName]));
                                break;
                        }
                    }
                }
                daAdp = null;
                cmd = null;
                return Obj;
            }
            catch (SqlException e)
            {
                throw e;
                return null;
            }
        }
        /// <summary>
        /// Filling the object valu by passing query and object
        /// </summary>
        /// <param name="SqlCommand">Pass the SqlCommand</param>
        /// <param name="Obj">Pass the object wich to be fill</param>
        /// <returns></returns>
        public object FillSingleObject(SqlCommand cmd, object Obj)
        {
            cmd.Connection = Conn;
            if (blnStartTrans == true)
                cmd.Transaction = Trans;
            SqlDataAdapter daAdp = new SqlDataAdapter(cmd);
            DataTable dsTable = new DataTable();
            try
            {
                daAdp.Fill(dsTable);
                if (dsTable.Rows.Count == 0) return Obj;
                Type objType = Obj.GetType();
                FieldInfo objPropertyInfo = default(FieldInfo);

                DataColumn Col = null;
                DataRow dRow = dsTable.Rows[0];
                foreach (DataColumn Col_loopVariable in dsTable.Columns)
                {
                    Col = Col_loopVariable;
                    objPropertyInfo = objType.GetField(Col.ColumnName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    //if (objPropertyInfo == null)
                    //{
                    //    Exception EX = new Exception(" Table Field'" + Col.ColumnName + "' Not Declared in Class OR Field declared as inavalid fromat.!");
                    //    throw EX;
                    //}
                    if (objPropertyInfo != null)
                    {
                        switch (objPropertyInfo.FieldType.FullName)
                        {
                            case "System.Int16":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt16(dRow[Col.ColumnName]));
                                break;
                            case "System.Int32":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt32(dRow[Col.ColumnName]));
                                break;
                            case "System.Int64":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt64(dRow[Col.ColumnName]));
                                break;
                            case "System.Decimal":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDecimal(dRow[Col.ColumnName]));
                                break;
                            case "System.Double":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDouble(dRow[Col.ColumnName]));
                                break;
                            case "System.Single":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToSingle(dRow[Col.ColumnName]));
                                break;
                            case "System.Byte":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToByte(dRow[Col.ColumnName]));
                                break;
                            case "System.String":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToString(dRow[Col.ColumnName]));
                                break;
                            case "System.DateTime":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDateTime(dRow[Col.ColumnName]));
                                break;
                            case "System.Boolean":
                                if (DBNull.Value != dRow[Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToBoolean(dRow[Col.ColumnName]));
                                break;
                        }
                    }
                }
                daAdp = null;
                cmd = null;
                return Obj;
            }
            catch (SqlException e)
            {
                throw e;
                return null;
            }
        }
        /// <summary>
        /// Filling the Collection Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Criteria">Sql Query</param>
        /// <param name="Obj">object (collection of what object) </param>
        /// <param name="CollectionObject">Collection object</param>
        /// <param name="CommandType">Type Of Query</param>
        /// <returns></returns>
        public object FillCollectionObject<T>(string Criteria, object Obj, List<T> CollectionObject, CommandType CommandType = CommandType.Text) where T : ICloneable
        {
            SqlCommand cmd = new SqlCommand(Criteria, Conn);
            cmd.CommandType = CommandType;
            if (blnStartTrans == true)
                cmd.Transaction = Trans;

            SqlDataAdapter daAdp = new SqlDataAdapter(cmd);
            DataTable dsTable = new DataTable();
            try
            {
                daAdp.Fill(dsTable);

                Type objType = null;
                objType = Obj.GetType();

                FieldInfo objPropertyInfo = default(FieldInfo);
                DataColumn Col = null;
                int i = 0;

                for (i = 0; i <= dsTable.Rows.Count - 1; i++)
                {
                    foreach (DataColumn Col_loopVariable in dsTable.Columns)
                    {
                        Col = Col_loopVariable;
                        objPropertyInfo = objType.GetField(Col.ColumnName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        //if (objPropertyInfo == null)
                        //{
                        //    Exception EX = new Exception(" Table Field'" + Col.ColumnName + "' Not Declared in Class OR Field declared in inavalid fromat.!");
                        //    throw EX;
                        //}
                        if (objPropertyInfo != null)
                        {
                            switch (objPropertyInfo.FieldType.FullName)
                            {
                                case "System.Int16":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt16(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Int32":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt32(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Int64":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt64(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Decimal":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDecimal(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, Convert.ToDecimal(0)); }
                                    break;
                                case "System.Double":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDouble(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Single":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToSingle(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Byte":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToByte(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.String":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToString(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, ""); }
                                    break;
                                case "System.DateTime":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDateTime(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, DateTime.Parse("01-01-1900")); }
                                    break;
                                case "System.Boolean":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToBoolean(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, false); }
                                    break;
                            }
                        }
                    }
                    T item = (T)Obj;
                    object c = item.Clone();
                    CollectionObject.Add((T)c);
                }
                dsTable.Dispose();
                daAdp.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return CollectionObject;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SqlCommand">SqlCommand object</param>
        /// <param name="Obj">Object (Collection of What)</param>
        /// <param name="CollectionObject">object Collection Class</param>
        /// <returns></returns>
        public object FillCollectionObject<T>(SqlCommand SqlCommand, object Obj, List<T> CollectionObject) where T : ICloneable
        {
            SqlCommand.Connection = Conn;
            if (blnStartTrans == true)
                SqlCommand.Transaction = Trans;
            SqlDataAdapter daAdp = new SqlDataAdapter(SqlCommand);
            DataTable dsTable = new DataTable();
            try
            {
                daAdp.Fill(dsTable);

                Type objType = null;
                objType = Obj.GetType();

                FieldInfo objPropertyInfo = default(FieldInfo);
                DataColumn Col = null;
                int i = 0;

                for (i = 0; i <= dsTable.Rows.Count - 1; i++)
                {
                    foreach (DataColumn Col_loopVariable in dsTable.Columns)
                    {
                        Col = Col_loopVariable;
                        objPropertyInfo = objType.GetField(Col.ColumnName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        //if (objPropertyInfo == null)
                        //{
                        //    Exception EX = new Exception(" Table Field'" + Col.ColumnName + "' Not Declared in Class OR Field declared in inavalid fromat.!");
                        //    throw EX;
                        //}
                        if (objPropertyInfo != null)
                        {
                            switch (objPropertyInfo.FieldType.FullName)
                            {
                                case "System.Int16":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt16(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Int32":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt32(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Int64":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToInt64(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Decimal":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDecimal(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, Convert.ToDecimal(0)); }
                                    break;
                                case "System.Double":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDouble(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Single":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToSingle(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.Byte":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToByte(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, 0); }
                                    break;
                                case "System.String":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToString(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, ""); }
                                    break;
                                case "System.DateTime":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToDateTime(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, DateTime.Parse("01-01-1900")); }
                                    break;
                                case "System.Boolean":
                                    if (DBNull.Value != dsTable.Rows[i][Col.ColumnName]) objPropertyInfo.SetValue(Obj, Convert.ToBoolean(dsTable.Rows[i][Col.ColumnName]));
                                    else { objPropertyInfo.SetValue(Obj, false); }
                                    break;
                            }
                        }
                    }
                    T item = (T)Obj;
                    object c = item.Clone();
                    CollectionObject.Add((T)c);
                }
                dsTable.Dispose();
                daAdp.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return CollectionObject;
        }
        /// <summary>
        /// getting Maxximum ID of purticular Table
        /// </summary>
        /// <param name="TableName">TableName</param>
        /// <param name="Field">FieldName</param>
        /// <param name="Condition">Condition</param>
        /// <returns></returns>
        public string GetMaxId(string TableName, string Field, string Condition)
        {
            string MaxId = "";
            string Query = " SELECT ISNULL(MAX(" + Field + "), 0) + 1 AS MaxID FROM " + TableName;
            if (Condition != "")
            {
                Query += " where " + Condition;
            }
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Query;
                MaxId = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException ex)
            {
                MaxId = "";
                throw ex;
            }
            return MaxId;
        }
        //void SaveCollectionObject<T>(string TableName, List<T> CollectionObject, long NewID, string[] SkipClmn = null) where T : ICloneable
        //{
        //    try
        //    {
        //        foreach (Object objSave in CollectionObject)
        //        {
        //            Save(TableName, objSave, ref NewID, SkipClmn);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        /// <summary>
        /// Saving Data by Passing Object
        /// </summary>
        /// <param name="TableName">Pass the Table Name to Save </param>
        /// <param name="objSave">Pass the data filled object or entity or class</param>
        /// <param name="NewID">pass the primary key variable which will return the identity key of current Affected</param>
        /// <param name="SkipClmn">pass the table column which you want to skip saving Data</param>
        /// <returns></returns>
        public long Save(string TableName, object objSave, ref long NewID, string[] SkipClmn = null)
        {
            object functionReturnValue = null;
            functionReturnValue = 0;
            // pass default 0 if not saved
            try
            {
                string idCol = string.Empty;

                SqlCommand cmd = new SqlCommand("Select * from " + TableName + " where 1=2", Conn);
                //TO GET THE TABLE STRUCTURE
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;

                SqlDataAdapter daAdp = new SqlDataAdapter(cmd);
                DataTable dsTable = new DataTable();

                daAdp.Fill(dsTable);
                if (SkipClmn != null)
                {
                    foreach (string Cols in SkipClmn)
                    {
                        dsTable.Columns.Remove(Cols);
                    }
                }
                Type objType = objSave.GetType();

                FieldInfo objPropInfo = null;
                DataColumn Col = null;
                string strFields = " (";
                string strValues = " values (";


                foreach (DataColumn Col_loopVariable in dsTable.Columns)
                {
                    Col = Col_loopVariable;
                    objPropInfo = objType.GetField(Col.ColumnName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (objPropInfo == null)
                    {
                        Exception EX = new Exception(" Table Field '" + Col.ColumnName + "' Not Declared in Class OR Field declared as inavalid fromat.!");
                        throw EX;
                    }
                    if ((objPropInfo != null))
                    {
                        if (((objPropInfo.GetValue(objSave)) != null))
                        {
                            if (!(Col.ColumnName.Substring(Col.ColumnName.ToString().Length - 3, 3) == "_ID"))
                            {
                                strFields = strFields + Col.ColumnName + ",";
                                switch (Col.DataType.ToString())
                                {
                                    case "System.Int16":
                                        strValues = strValues + objPropInfo.GetValue(objSave) + ",";
                                        break;
                                    case "System.Int32":
                                        strValues = strValues + objPropInfo.GetValue(objSave) + ",";
                                        break;
                                    case "System.Int64":
                                        strValues = strValues + objPropInfo.GetValue(objSave) + ",";
                                        break;
                                    case "System.Decimal":
                                        strValues = strValues + objPropInfo.GetValue(objSave) + ",";
                                        break;
                                    case "System.Double":
                                        strValues = strValues + objPropInfo.GetValue(objSave) + ",";
                                        break;
                                    case "System.Single":
                                        strValues = strValues + objPropInfo.GetValue(objSave) + ",";
                                        break;
                                    case "System.Byte":
                                        strValues = strValues + objPropInfo.GetValue(objSave) + ",";
                                        break;
                                    case "System.String":
                                        strValues = strValues + "'" + objPropInfo.GetValue(objSave).ToString().Replace("'", "`") + "',";
                                        break;
                                    case "System.DateTime":
                                        strValues = strValues + "convert(datetime,'" + Convert.ToDateTime(objPropInfo.GetValue(objSave)) + "',102),";
                                        break;
                                    case "System.Boolean":
                                        strValues = strValues + Convert.ToInt32(objPropInfo.GetValue(objSave)) + ",";
                                        break;
                                }
                            }
                            else
                            {
                                idCol = Col.ColumnName;
                            }
                        }
                    }
                    //NXT:
                }
                //strFields = Strings.Left(strFields, strFields.Length - 1) + ")";
                // strFields = Left(strFields, strFields.Length - 1) & ")"
                strFields = strFields.Substring(0, strFields.Length - 1) + ")";
                strValues = strValues.Substring(0, strValues.Length - 1) + ")";

                cmd.CommandText = "insert into " + TableName + strFields + strValues + "  SELECT SCOPE_IDENTITY()";

                //cmd.ExecuteNonQuery();
                //GetLastID(cmd);
                String Value = cmd.ExecuteScalar().ToString();
                //cmd.ExecuteNonQuery();
                if (Value != "") NewID = long.Parse(Value);
                //NewID = NewIDENTITY;
                cmd.Dispose();
                cmd = null;
                return 1;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saving Data Using SqlCommand 
        /// </summary>
        /// <param name="SqlCmd">Pass the SqlCommand with stored procedure</param>
        /// <param name="NewID">pass the primary key variable which will return the identity key of current Affected</param>
        /// <returns></returns>
        public void Save(SqlCommand SqlCmd, ref long NewID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd = SqlCmd;
            int execute = 0;
            String Value = "";
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                //execute = cmd.ExecuteNonQuery();
                Value = Convert.ToString(cmd.ExecuteScalar());
                if (Value != "") NewID = long.Parse(Value);
                //if((execute > 0))
                //{
                //   GetLastID(cmd);
                //   NewID = NewIDENTITY;
                //}
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saving Data Using Stored Procedure
        /// </summary>
        /// <param name="spName">Pass the Stored ProcedureName</param>
        /// <param name="NewID">Identity key Will Return,assign the variable</param>
        /// <param name="parameterValues">pass the parameters curresponding to Procedure Parameters inOrder  </param>
        public void Save(string spName, ref long NewID, params object[] parameterValues)
        {
            try
            {
                SqlParameter[] commandParameters = null;
                //getting parameters...
                commandParameters = GetSpParameterSet(spName, false);
                //assign values to parameters..
                AssignParameterValues(commandParameters, parameterValues);
                // executing procedure..
                Execute(spName, false, commandParameters);
                NewID = NewIDENTITY;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Save the Data By Passing textquery
        /// </summary>
        /// <param name="Query">Pass the Query which to Execute</param>
        /// <param name="NewID">pass the primary key variable which will return the identity key of current Affected</param>
        public void Save(string Query, ref long NewID)
        {
            int Execute = 0;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                cmd.CommandType = CommandType.Text; ;
                cmd.CommandText = Query + "  SELECT SCOPE_IDENTITY()";
                //Execute = cmd.ExecuteNonQuery();
                String Value = cmd.ExecuteScalar().ToString();
                if (Value != "") NewID = long.Parse(Value);
                //if (Execute > 0)
                //{ GetLastID(cmd); NewID = NewIDENTITY;}
            }
            catch (SqlException ex)
            {
                Execute = -1;
                throw ex;
            }
        }
        /// <summary>
        /// Update Data Using Stored Procedure
        /// </summary>
        /// <param name="spName">Pass the Stored ProcedureName</param>
        /// <param name="parameterValues">pass the parameters curresponding to Procedure Parameters inOrder  </param>
        /// 
        public int Update(string spName, params object[] parameterValues)
        {
            int Update = 0;
            try
            {
                SqlParameter[] commandParameters = null;
                //getting parameters...
                commandParameters = GetSpParameterSet(spName, false);
                //assign values to parameters..
                AssignParameterValues(commandParameters, parameterValues);
                // executing procedure..
                Update = Execute(spName, true, commandParameters);
            }
            catch (Exception ex)
            {
                Update = -1;
                throw ex;
            }
            return Update;
        }
        /// <summary>
        /// Updating Table  Data By Passing Filled Object
        /// </summary>
        /// <param name="TableName">Pass the Table Name</param>
        /// <param name="objUpdate">Pass the Filled Object or Entity or Class</param>
        /// <param name="strCondition">pass the any Condition for Updation</param>
        /// <param name="Log">set true or false which will decide wheather tha data need to log</param>
        /// <param name="SkipClmn">Pass the Column name to skip th updation</param>
        /// <returns></returns>
        public int Update(string TableName, object objUpdate, string strCondition, bool Log, string[] SkipClmn = null)
        {
            string idCol = string.Empty;
            int intNoUpdate = -1;
            try
            {
                SqlCommand cmd = new SqlCommand("Select * from " + TableName + " where 1=2", Conn);
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                SqlDataAdapter daAdp = new SqlDataAdapter(cmd);
                DataTable dsTable = new DataTable();
                daAdp.Fill(dsTable);
                if (SkipClmn != null)
                {
                    foreach (string Cols in SkipClmn)
                    {
                        dsTable.Columns.Remove(Cols);
                    }
                }
                Type objType = null;
                objType = objUpdate.GetType();
                FieldInfo objPropInfo = default(FieldInfo);
                DataColumn Col = null;
                string strValues = "Update " + TableName + " set ";
                //Dim intId As Integer = -1
                foreach (DataColumn Col_loopVariable in dsTable.Columns)
                {
                    Col = Col_loopVariable;
                    objPropInfo = objType.GetField(Col.ColumnName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (objPropInfo == null)
                    {
                        Exception EX = new Exception(" Table Field'" + Col.ColumnName + "' Not Declared in Class OR Field declared as inavalid fromat.!");
                        throw EX;
                    }
                    if ((objPropInfo != null))
                    {
                        if ((objPropInfo.GetValue(objUpdate) != null))
                        {
                            if (!(Col.ColumnName.Substring(Col.ColumnName.ToString().Length - 3, 3) == "_ID"))
                            {
                                switch (Col.DataType.ToString())
                                {
                                    case "System.Int16":
                                        strValues = strValues + Col.ColumnName + "=" + objPropInfo.GetValue(objUpdate) + ",";
                                        break;
                                    case "System.Int32":
                                        strValues = strValues + Col.ColumnName + "=" + objPropInfo.GetValue(objUpdate) + ",";
                                        break;
                                    case "System.Int64":
                                        strValues = strValues + Col.ColumnName + "=" + objPropInfo.GetValue(objUpdate) + ",";
                                        break;
                                    case "System.Decimal":
                                        strValues = strValues + Col.ColumnName + "=" + objPropInfo.GetValue(objUpdate) + ",";
                                        break;
                                    case "System.Double":
                                        strValues = strValues + Col.ColumnName + "=" + objPropInfo.GetValue(objUpdate) + ",";
                                        break;
                                    case "System.Single":
                                        strValues = strValues + Col.ColumnName + "=" + objPropInfo.GetValue(objUpdate) + ",";
                                        break;
                                    case "System.Byte":
                                        strValues = strValues + Col.ColumnName + "=" + objPropInfo.GetValue(objUpdate) + ",";
                                        break;
                                    case "System.String":
                                        strValues = strValues + Col.ColumnName + "='" + objPropInfo.GetValue(objUpdate).ToString().Replace("'", "`") + "',";
                                        break;
                                    case "System.DateTime":
                                        strValues = strValues + Col.ColumnName + "=Convert(datetime,'" + objPropInfo.GetValue(objUpdate) + "',102),";
                                        break;
                                    case "System.Boolean":
                                        strValues = strValues + Col.ColumnName + "=" + Convert.ToInt32(objPropInfo.GetValue(objUpdate)) + ",";
                                        break;
                                }
                            }
                            else
                            {
                                idCol = Col.ColumnName;
                            }
                        }
                    }
                    //Else
                    //intId = objPropInfo.GetValue(objUpdate)
                    //End If
                }
                strValues = strValues.Substring(0, strValues.Length - 1) + " " + " where " + strCondition;
                cmd.CommandText = strValues;
                intNoUpdate = cmd.ExecuteNonQuery();
                //if (Log == true)
                //{
                //    CreateLog(TableName, strCondition);
                //}

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return intNoUpdate;

        }
        /// <summary>
        /// Update Data Using SqlCommand
        /// </summary>
        /// <param name="SqlCmd">Pass the SqlCommand with stored procedure</param>
        /// <returns></returns>
        public int Update(SqlCommand SqlCmd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd = SqlCmd;
            int execute = 0;
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                execute = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                execute = -1;
                throw ex;
            }
            return execute;
        }
        /// <summary>
        /// Update the Data By Passing textquery
        /// </summary>
        /// <param name="Query">Pass the Query which to Execute</param>
        public int Update(string Query)
        {
            int Execute = 0;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                cmd.CommandType = CommandType.Text; ;
                cmd.CommandText = Query;
                Execute = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Execute = -1;
                throw ex;
            }
            return Execute;
        }
        /// <summary>
        /// Delete Row Using Stored Procedure
        /// </summary>
        /// <param name="spName">Pass the Stored ProcedureName</param>
        /// <param name="parameterValues">pass the parameters curresponding to Procedure Parameters inOrder  </param>
        /// 
        public int Delete(string spName, params object[] parameterValues)
        {
            int Delete = 0;
            try
            {
                SqlParameter[] commandParameters = null;
                //getting parameters...
                commandParameters = GetSpParameterSet(spName, false);
                //assign values to parameters..
                AssignParameterValues(commandParameters, parameterValues);
                // executing procedure..
                Delete = Execute(spName, true, commandParameters);
            }
            catch (Exception ex)
            {
                Delete = -1;
                throw ex;
            }
            return Delete;
        }
        /// <summary>
        /// Delete Row Using SqlCommand
        /// </summary>
        /// <param name="SqlCmd">Pass the SqlCommand with stored procedure</param>
        /// <returns></returns>
        public int Delete(SqlCommand SqlCmd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd = SqlCmd;
            int execute = 0;
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                execute = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                execute = 0;
                throw ex;
            }
            return execute;
        }
        /// <summary>
        /// Delete Row By Passing textquery
        /// </summary>
        /// <param name="Query">Pass the Query which to Execute</param>
        public int Delete(string Query)
        {
            int Execute = 0;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                cmd.Connection = Conn;
                cmd.CommandType = CommandType.Text; ;
                cmd.CommandText = Query;
                Execute = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Execute = -1;
                throw ex;
            }
            return Execute;
        }
        //ExecuteScalar
        private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        private SqlParameter[] GetSpParameterSet(string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] cachedParameters = null;
            try
            {

                string hashKey = null;
                hashKey = spName + (includeReturnValueParameter == true);
                cachedParameters = (SqlParameter[])paramCache[hashKey];
                if ((cachedParameters == null))
                {
                    paramCache[hashKey] = DiscoverSpParameterSet(spName, includeReturnValueParameter);
                    cachedParameters = (SqlParameter[])paramCache[hashKey];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CloneParameters(cachedParameters);
        }
        //GetSpParameterSet
        private void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            try
            {
                int i = 0;
                int j = 0;

                if ((commandParameters == null) & (parameterValues == null))
                {
                    //do nothing if we get no data
                    return;
                }

                // we must have the same number of values as we pave parameters to put them in
                if (commandParameters.Length != parameterValues.Length)
                {
                    Exception ex = new Exception("Parameter count does not match Parameter Value count.", new Exception("Inner Exeption"));
                    throw ex;
                }
                //value array
                j = commandParameters.Length - 1;
                for (i = 0; i <= j; i++)
                {
                    commandParameters[i].Value = parameterValues[i];
                }

            }
            catch (Exception ex)
            {
                //ex = new Exception("The Count of Procedure Parameters and Passed Parameters Count  are Not Tallying!");
                throw ex;
            }
        }
        //AssignParameterValues
        private SqlParameter[] DiscoverSpParameterSet(string spName, bool includeReturnValueParameter, params object[] parameterValues)
        {
            SqlCommand cmd = new SqlCommand(spName, Conn);
            SqlParameter[] discoveredParameters = null;
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (blnStartTrans == true)
                    cmd.Transaction = Trans;
                SqlCommandBuilder.DeriveParameters(cmd);
                if (!includeReturnValueParameter)
                {
                    cmd.Parameters.RemoveAt(0);
                }
                discoveredParameters = new SqlParameter[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(discoveredParameters, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
            return discoveredParameters;

        }
        //DiscoverSpParameterSet
        //CloneParameters
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {

            int i = 0;
            int j = originalParameters.Length - 1;
            SqlParameter[] clonedParameters = new SqlParameter[j + 1];

            for (i = 0; i <= j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }
        //CloneParameters
        //ExecuteScalar
        private int Execute(string commandText, Boolean Update = false, params SqlParameter[] commandParameters)
        {
            int Execute = 0;
            SqlCommand cmd = new SqlCommand();
            String Value = "";
            try
            {
                //create a command and prepare it for execution
                PrepareCommand(cmd, Conn, Trans, commandText, commandParameters);
                //Execute=cmd.ExecuteNonQuery();
                Value = cmd.ExecuteScalar().ToString();
                cmd.Parameters.Clear();
                if (Update == false)
                {
                    if (Value != "") { NewIDENTITY = long.Parse(Value); }
                    //GetLastID(cmd);cmd.Parameters.Clear();
                }
                //clearing parametrs..
            }
            catch (Exception ex)
            {
                cmd.Parameters.Clear();
                Execute = -1;
                throw ex;
            }
            return Execute;
        }
        //ExecuteScalar
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, string commandText, SqlParameter[] commandParameters)
        {
            //associate the connection with the command
            command.Connection = connection;
            //set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;
            //if we were provided a transaction, assign it.
            if ((transaction != null))
            {
                command.Transaction = transaction;
            }
            //set the command type
            command.CommandType = CommandType.StoredProcedure;
            //attach the command parameters 
            if ((commandParameters != null))
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }
        //PrepareCommand
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if (p.Direction == ParameterDirection.InputOutput & p.Value == null)
                {
                    p.Value = null;
                }
                command.Parameters.Add(p);
            }
        }
        //AttachParameters
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    //TODO: free unmanaged resources when explicitly called
                }
                // TODO: free shared unmanaged resources
            }
            this.disposedValue = true;
        }
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }


  


        /********* Execute_Sclar()  ---- Added by Mojammel on 11-11-2017*********/

        #region ConnectionOpen
        /********* For opening the connection with/without transaction *********/
        private void ConnectionOpen(bool IsTransRequeired)
        {
            if (Conn == null)
            {
                Conn = new SqlConnection(ConString);
                Conn.Open();
                if (IsTransRequeired)
                    Trans = Conn.BeginTransaction();
            }
            else if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
                if (IsTransRequeired)
                    Trans = Conn.BeginTransaction();
            }
            else if (IsTransRequeired)
            {
                if (Trans == null)
                {
                    Trans = Conn.BeginTransaction();
                }
            }
        }
        #endregion

        public string Execute_Sclar(string SP, bool IsTransRequired)
        {
            try
            {
                ConnectionOpen(IsTransRequired);
                Com = new SqlCommand(SP, Conn, Trans);
                Com.CommandType = CommandType.StoredProcedure;

                string Result = (string)Com.ExecuteScalar();
                return Result;
            }
            catch (Exception ex)
            {
                if (IsTransRequired)
                    FinishTransaction(false);
                return string.Empty; ;
            }
            finally
            {
                if (!IsTransRequired)
                    ConnectionClose();
            }
        }

        private void ConnectionClose()
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                Conn = null;
            }
        }

        protected void FinishTransaction(bool IsCommit)
        {
            try
            {
                if (IsCommit == true)
                {
                    if (Trans != null)
                    {
                        if (Trans.Connection != null && Trans.Connection.State != ConnectionState.Closed)
                            Trans.Commit();
                    }
                }
                else
                {
                    if (Trans != null)
                    {
                        try
                        {
                            Trans.Rollback();
                        }
                        catch { }
                    }
                }

                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                        Conn.Close();
                    Conn.Dispose();
                    Conn = null;
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (Trans != null)
                {
                    Trans.Dispose();
                    Trans = null;
                }

                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                        Conn.Close();
                    Conn.Dispose();
                    Conn = null;
                }
            }
        }

        /********* ExecuteNonQuery() with parameters and one integer type return parameter *********/
        public int Execute(string SP, string[] ParamNames, object[] ParamVals, bool IsTransRequired, bool IsReturn)
        {
            try
            {
                ConnectionOpen(IsTransRequired);
                Com = new SqlCommand(SP, Conn, Trans);
                Com.CommandType = CommandType.StoredProcedure;
                SetParameters(Com, ParamNames, ParamVals);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.Direction = ParameterDirection.ReturnValue;
                Com.Parameters.Add(sqlParam);

                int Result = Com.ExecuteNonQuery();
                return Convert.ToInt32(sqlParam.Value);
            }
            catch (Exception ex)
            {
                if (IsTransRequired)
                    FinishTransaction(false);
                return 0;
            }
            finally
            {
                if (!IsTransRequired)
                    ConnectionClose();
            }
        }

        private void SetParameters(SqlCommand sqlCmd, string[] ParamNames, object[] ParamVals)
        {
            if (ParamNames != null)
            {
                for (int i = 0; i < ParamNames.Length; i++)
                {
                    sqlCmd.Parameters.AddWithValue(ParamNames[i], ParamVals[i]);
                }
            }
        }
    }
}
