using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataLayer
{
  public interface  IDataExecution:IDisposable 
    {

       string ExistRemarks
       {
           get;
           set;

       }
        Boolean  ExistStatus
       {
           get;
           set;
       }
        void   BeginTransaction(string ConnectionString, bool StartTransaction=false);
        void EndTransaction(bool Rollback = false);
         /// <param name="TableName">Name of the table To Fetch</param>
        /// <param name="ValueField">Name of the field to fetch from table</param>
        /// <param name="TextFiled">Name of the field to Fetch from table</param>
        /// <param name="Condition">Condition of query which you passing ,which should not pass 'Where' keyword</param>
        DataTable FetchResultAsTable_Combo(string TableName, string ValueField, string TextFiled, string Condition);
        DataTable FetchResultAsTable_Combo(string Query);
        DataTable FetchResultAsTable_CheckBoxlist(string TableName, string ValueField, string TextFiled, string Condition);
        /// <summary>
      /// fetch the data as dataset
      /// </summary>
      /// <param name="spName">pass the stored procedurename</param>
        /// <param name="parameterValues">pass the values  in order same as tha declared order in storedprocedure</param>
      /// <returns></returns>
        DataSet FetchResultAsdataset(string spName, params object[] parameterValues);
        /// <summary>
        /// fetch the data as dataset by passing sqlcommand
        /// </summary>
        /// <param name="sqlcom">pass the sqlcommand</param>
        /// <returns></returns>
         DataSet FetchResultAsdataset(SqlCommand sqlcom);
        /// <summary>
        /// fetch the data as datatset by pasing text query or storedprocedure 
        /// </summary>
        /// <param name="CommandText">pass the query</param>
        /// <param name="CmdType">type of Command</param>
        /// <returns></returns>
         DataSet FetchResultAsdataset(string CommandText, CommandType CmdType=CommandType.Text);
        /// <summary>
      /// fetch the data as datatable by passing storedprocedure with parameter values 
      /// </summary>
      /// <param name="spName">pass the storedprocedure name</param>
      /// <param name="parameterValues">pass the values  in order same as tha declared order in storedprocedure</param>
      /// <returns></returns>
         DataTable FetchResultAsTable(string spName, params object[] parameterValues);
        /// <summary>
      /// fetch the data as datatable by passing sqlcommand
      /// </summary>
      /// <param name="sqlcom">pass the sqlcommand</param>
      /// <returns></returns>
         DataTable FetchResultAsTable(SqlCommand sqlcom, CommandType CmdType = CommandType.StoredProcedure);
        /// <summary>
      /// fetch the data as datattable by pasing text query or storedprocedure 
      /// </summary>
      /// <param name="CommandText">pass the query</param>
      /// <param name="CmdType">type of Command</param>
      /// <returns></returns>
         DataTable FetchResultAsTable(string CommandText, CommandType CmdType = CommandType.Text);
        /// <summary>
        /// Saving Data by Passing Object
        /// </summary>
        /// <param name="TableName">Pass the Table Name to Save </param>
        /// <param name="objSave">Pass the data filled object or entity or class</param>
        /// <param name="NewID">pass the primary key variable which will return the identity key of current Affected</param>
        /// <param name="SkipClmn">pass the table column which you want to skip saving Data</param>
        /// <returns></returns>
        long Save(string TableName, object objSave, ref long NewID, string[] SkipClmn = null);
        /// <summary>
        /// Saving Data Using SqlCommand 
        /// </summary>
        /// <param name="SqlCmd">Pass the SqlCommand with stored procedure</param>
        /// <param name="NewID">pass the primary key variable which will return the identity key of current Affected</param>
        /// <returns></returns>
        void Save(SqlCommand SqlCmd, ref long NewID);
        /// <summary>
        /// Saving Data Using Stored Procedure
        /// </summary>
        /// <param name="spName">Pass the Stored ProcedureName</param>
        /// <param name="NewID">Identity key Will Return,assign the variable</param>
        /// <param name="parameterValues">pass the parameters curresponding to Procedure Parameters inOrder  </param>
        void Save(string spName, ref long NewID, params object[] parameterValues);
        /// <summary>
        /// Save the Data By Passing textquery
        /// </summary>
        /// <param name="Query">Pass the Query which to Execute</param>
        /// <param name="NewID">pass the primary key variable which will return the identity key of current Affected</param>
        void Save(string Query, ref long NewID);
        /// <summary>
        /// Update Data Using Stored Procedure
        /// </summary>
        /// <param name="spName">Pass the Stored ProcedureName</param>
        /// <param name="parameterValues">pass the parameters curresponding to Procedure Parameters inOrder  </param>
        /// 
        int Update(string spName, params object[] parameterValues);
        /// <summary>
        /// Update Data Using SqlCommand
        /// </summary>
        /// <param name="SqlCmd">Pass the SqlCommand with stored procedure</param>
        /// <returns></returns>
        int Update(SqlCommand SqlCmd);
        /// <summary>
        /// Update the Data By Passing textquery
        /// </summary>
        /// <param name="Query">Pass the Query which to Execute</param>
        int Update(string Query);
        /// <summary>
        /// Updating Table  Data By Passing Filled Object
        /// </summary>
        /// <param name="TableName">Pass the Table Name</param>
        /// <param name="objUpdate">Pass the Filled Object or Entity or Class</param>
        /// <param name="strCondition">pass the any Condition for Updation</param>
        /// <param name="Log">set true or false which will decide wheather tha data need to log</param>
        /// <param name="SkipClmn">Pass the Column name to skip th updation</param>
        /// <returns></returns>
        int Update(string TableName, object objUpdate, string strCondition, bool Log, string[] SkipClmn = null);
        /// <summary>
        /// Delete Row Using Stored Procedure
        /// </summary>
        /// <param name="spName">Pass the Stored ProcedureName</param>
        /// <param name="parameterValues">pass the parameters curresponding to Procedure Parameters inOrder  </param>
        /// 
        int Delete(string spName, params object[] parameterValues);
        /// <summary>
        /// Delete Row Using SqlCommand
        /// </summary>
        /// <param name="SqlCmd">Pass the SqlCommand with stored procedure</param>
        /// <returns></returns>
        int Delete(SqlCommand SqlCmd);
        /// <summary>
        /// Delete Row By Passing textquery
        /// </summary>
        /// <param name="Query">Pass the Query which to Execute</param>
        int Delete(string Query);
        int ExecuteQuery(string strQuery, CommandType Cmdtype = CommandType.Text);
        int ExecuteQuery(SqlCommand Command, CommandType Cmdtype = CommandType.StoredProcedure);
        string ExecuteScalar(SqlCommand Command);
        object FillSingleObject(string SqlCommand, object Obj);
        object FillSingleObject(SqlCommand SqlCommand, object Obj);
        object FillCollectionObject<T>(SqlCommand SqlCommand, object Obj, List<T> CollectionObject) where T : ICloneable;
        object FillCollectionObject<T>(string Criteria, object Obj, List<T> CollectionObject,CommandType CommandType = CommandType.Text) where T : ICloneable;
        /// <summary>
        /// this Function will not return anything,but we will get the status by ExistRemarks,ExistStatus
        /// </summary>
        /// <param name="Query">the Query should Contain RAISERROR('error',1,1),Only RAISERROR will return error</param>
        /// <param name="fields">The Name of the Field to Validate</param>
        /// <param name="Condition">Only Active Row Should Check.example (Recordstastus=0) </param>
        /// <param name="FieldValues">Value which passing to purticular field</param>
        /// <param name="TableName">Name Of the Table</param>
        void CheckExists(string TableName, string[] fields, string[] FieldValues, string Condition = "");
       /// <summary>
        /// if exist the Basedatalyer.ExistStatus=true,else Basedatalyer.ExistStatus=false
       /// </summary>
       /// <param name="Query">Pass the Query to check exist</param>
       /// <param name="CommandType">wheather its stored procedure or text procedure</param>
        void CheckExists(string Query, CommandType CommandType = CommandType.Text);
       /// <summary>
       /// function return only single value or get the maximum Id
       /// </summary>
       /// <param name="Condition">pass the condition to get maximunID</param>
       /// <param name="Field">specify the field to get maximum </param>
       /// <param name="TableName">Name of the  Table to find</param>
        string GetMaxId(string TableName, string Field, string Condition);
        string ExecuteScalar(string Query, CommandType CmdType = CommandType.Text);
        }
}
