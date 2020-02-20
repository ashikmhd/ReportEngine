using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using BusinessLayer.General;
using System.Web.UI.WebControls;
namespace BusinessLayer
{

    public class Base : ICloneable
    {
        public Base(BaseRequest obj)
        {
            this.BaseOutletId = obj.BaseOutletId;
            this.BaseOutletName = obj.BaseOutletName;
            this.BaseCompanyID = obj.BaseCompanyID;
            this.BaseUserId = obj.BaseUserId;
        }
        public Base() { }
        protected SqlCommand sqlCmd = null;
        protected String BaseConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnString"];
        protected String SQLQuery = "";
        public long BaseUserID = 0;
        public int BaseOutletId = 0;
        public string BaseOutletName;
        public int BaseCompanyID;
        public long BaseUserId;
        /// <summary>
        /// we can  pass the index of collection
        /// </summary>
        //public Int16 BaseDepartmentId;
        private IDataExecution p_Datalayer_Exe;
        private IDataExecution p_Datalayer_Exe2;
        private IDataExecution p_Datalayer_Exe1;
        private TransactionVariables objTransVariables = new TransactionVariables();
        public string Action = "";
        public Boolean Errorstatus = false;
        public string ErrorRemarks = "Transaction completed Successfully";
        public String Search_Text;
        public String Search_By;
        public DateTime Search_DateFrom;
        public DateTime Search_DateTo;
        protected IDataExecution BaseDataLayer
        {
            get { return p_Datalayer_Exe; }
            set { p_Datalayer_Exe = value; }
        }
        protected IDataExecution BaseDataLayer2
        {
            get { return p_Datalayer_Exe2; }
            set { p_Datalayer_Exe2 = value; }
        }
        protected IDataExecution BaseDataLayer1
        {
            get { return p_Datalayer_Exe1; }
            set { p_Datalayer_Exe1 = value; }
        }
        public TransactionVariables TransVariables
        {
            get { return objTransVariables; }
            set { objTransVariables = value; }
        }
        protected void BeginTransaction(Boolean IsBeginTrans = false)
        {
            BaseDataLayer = new DataLayer.clsDataExecution();
            BaseDataLayer.BeginTransaction(BaseConnectionString, IsBeginTrans);
            this.TransVariables.Datalayer_Exe = BaseDataLayer;
        }
        protected void BeginTransaction(string ProcedureName, string ClassName)
        {
            if (ProcedureName == this.TransVariables.TransProcedureName && ClassName == this.TransVariables.TransClassName)
            {
                BaseDataLayer = new DataLayer.clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString, this.TransVariables.IsBeginTrans);
                this.TransVariables.Datalayer_Exe = BaseDataLayer;
            }
            else
            {
                BaseDataLayer = this.TransVariables.Datalayer_Exe;
            }
        }
        protected void EndTransaction(string ProcedureName, string ClassName)
        {
            if (ProcedureName == this.TransVariables.TransProcedureName && ClassName == this.TransVariables.TransClassName)
            {
                if (BaseDataLayer != null)
                {
                    BaseDataLayer.EndTransaction(false);
                    BaseDataLayer.Dispose();
                    BaseDataLayer = null;
                }

            }
        }
        protected void EndTransaction()
        {
            if (BaseDataLayer != null)
            {
                BaseDataLayer.EndTransaction(false);
                BaseDataLayer.Dispose();
                BaseDataLayer = null;
            }
        }
        protected void RollBackTransaction(string ProcedureName, string ClassName)
        {
            if (ProcedureName == this.TransVariables.TransProcedureName && ClassName == this.TransVariables.TransClassName)
            {
                if (BaseDataLayer != null)
                {
                    BaseDataLayer.EndTransaction(true);
                    BaseDataLayer.Dispose();
                    BaseDataLayer = null;
                }

            }
        }
        protected void RollBackTransaction()
        {
            if (BaseDataLayer != null)
            {
                BaseDataLayer.EndTransaction(true);
                BaseDataLayer.Dispose();
                BaseDataLayer = null;
            }
        }
        protected void fillTransVariables(string ProcedureName, string ClassName, Boolean IsBeginTrans = true)
        {
            if (this.TransVariables.Datalayer_Exe == null)
            {
                this.TransVariables.Datalayer_Exe = null;
                this.TransVariables.ErrorRemarks = "Transaction Completed successFully";
                this.TransVariables.Errorstatus = false;
                this.TransVariables.TransClassName = ClassName;
                this.TransVariables.TransProcedureName = ProcedureName;
                this.TransVariables.IsBeginTrans = IsBeginTrans;
            }
        }
        /// <summary>
        /// displaying stylish messages
        /// </summary>
        /// <param name="Remarkstype">Pass the Value which determine type of the message!success=1,warning=2,error=3,inform=4</param>
        /// <param name="Message">Message which to display</param>
        /// <returns></returns>
        public string DisplayRemarks(int Remarkstype, string Message)
        {
            string Remarks = Message;
            if (Remarkstype == 1)//'sucss
            {
                //Remarks = "<div class='success'  id='successDiv' >" + Message + "</div>";
                //Remarks = "<div class='success' id=successDiv><div class='close' onclick=Closediv('successDiv')><span class='popup_tooltip'>Press Esc to close <span class='arrow'></span></span></div>" + Message + "</div>";
                Remarks = "<div class='success'  id='successDiv' >" + Message + "<div class='closediv' onclick=Closediv('successDiv')><img src='../Images/Close_div.gif' title='Close' /></div></div>";
            }
            else if (Remarkstype == 2)//'warning
            {
                //Remarks = "<div class='warning'  id='warningDiv' >" + Message + "</div>";
                //Remarks = "<div class='warning' id=warningDiv><div class='close' onclick=Closediv('warningDiv')><span class='popup_tooltip'>Press Esc to close <span class='arrow'></span></span></div>" + Message + "</div>";
                Remarks = "<div class='warning'  id='warningDiv' >" + Message + "<div class='closediv' onclick=Closediv('warningDiv')><img src='../Images/Close_div.gif' title='Close' /></div></div>";
            }
            else if (Remarkstype == 3)//error
            {
                //Remarks = "<div class='error'  id='errorDiv' >" + Message + "</div>";
                //Remarks = "<div class='error' id=errorDiv><div class='close' onclick=Closediv('errorDiv')></div>" + Message + "</div>";
                Remarks = "<div class='error'  id='errorDiv' >" + Message + "<div class='closediv' onclick=Closediv('errorDiv')><img src='../Images/Close_div.gif'title='Close' /></div></div>";
            }
            else if (Remarkstype == 4)//inform
            {
                //Remarks = "<div class='info'  id='infoDiv' >" + Message + "</div>";
                //Remarks = "<div class='info' id=infoDiv><div class='close' onclick=Closediv('infoDiv')><span class='popup_tooltip'>Press Esc to close <span class='arrow'></span></span></div>" + Message + "</div>";
                Remarks = "<div class='info'  id='infoDiv' >" + Message + "<div class='closediv' onclick=Closediv('infoDiv')><img src='../Images/Close_div.gif' title='Close' /></div></div>";
            }
            return Remarks;
        }

        public object Clone()
        {
            return (object)this.MemberwiseClone();
        }


        protected void SaveErrorLog()
        {

        }
        protected int ExecuteNonQuery(SqlCommand Commmand, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            int Execute = 0;
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Commmand.CommandType = Cmdtype;
                Execute = BaseDataLayer.ExecuteQuery(Commmand);
                if (!(Execute > 0))
                {
                    this.ErrorRemarks = " Unable To Execute!";
                    this.Errorstatus = true;
                    goto ErrorHandler;
                }
                BaseDataLayer.EndTransaction();
            ErrorHandler:
                BaseDataLayer.EndTransaction(true);
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Execute;
        }
        protected String ExecuteScalar(SqlCommand Commmand, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            String Value = "";
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Commmand.CommandType = Cmdtype;
                Value = BaseDataLayer.ExecuteScalar(Commmand);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Value;
        }
        protected DataTable ExecuteDataTable(SqlCommand Commmand, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            DataTable Dt = null;
            Dt = new DataTable();
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Commmand.CommandType = Cmdtype;
                Dt = BaseDataLayer.FetchResultAsTable(Commmand, Cmdtype);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Dt;
        }
        protected DataTable ExecuteDataTable(String Commmand)
        {
            DataTable Dt = null;
            Dt = new DataTable();
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Dt = BaseDataLayer.FetchResultAsTable(Commmand, CommandType.Text);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Dt;
        }
        protected DataSet ExecuteDataset(String Commmand)
        {
            DataSet Ds = null;
            Ds = new DataSet();
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Ds = BaseDataLayer.FetchResultAsdataset(Commmand, CommandType.Text);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Ds;
        }
        protected DataSet ExecuteDataset(SqlCommand Commmand, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            DataSet Ds = null;
            Ds = new DataSet();
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Commmand.CommandType = Cmdtype;
                Ds = BaseDataLayer.FetchResultAsdataset(Commmand);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Ds;
        }
        protected int ExecuteNonQuery(ref SqlCommand Commmand, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            int Execute = 0;
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Commmand.CommandType = Cmdtype;
                Execute = BaseDataLayer.ExecuteQuery(Commmand);
                if (!(Execute > 0))
                {
                    this.ErrorRemarks = " Unable To Execute!";
                    this.Errorstatus = true;
                    goto ErrorHandler;
                }
                BaseDataLayer.EndTransaction();
            ErrorHandler:
                BaseDataLayer.EndTransaction(true);
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Execute;
        }
        protected String ExecuteScalar(ref SqlCommand Commmand, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            String Value = "";
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Commmand.CommandType = Cmdtype;
                Value = BaseDataLayer.ExecuteScalar(Commmand);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Value;
        }
        protected DataTable ExecuteDataTable(ref SqlCommand Commmand, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            DataTable Dt = null;
            Dt = new DataTable();
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Commmand.CommandType = Cmdtype;
                Dt = BaseDataLayer.FetchResultAsTable(Commmand, Cmdtype);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Dt;
        }
        protected DataSet ExecuteDataset(ref SqlCommand Commmand, CommandType Cmdtype = CommandType.StoredProcedure)
        {
            DataSet Ds = null;
            Ds = new DataSet();
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                Commmand.CommandType = Cmdtype;
                Ds = BaseDataLayer.FetchResultAsdataset(Commmand);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction(true);
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Ds;
        }




        protected string FetchID(string TableName, string Field, string Condition)
        {
            string ID = "0";
            string Query = " select " + Field + " from " + TableName + " where " + Condition;
            try
            {
                BaseDataLayer = new clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                ID = BaseDataLayer.ExecuteScalar(Query);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction();
                BaseDataLayer = null;
                this.Errorstatus = true;
                this.ErrorRemarks = ex.Message;
            }
            return ID;

        }
        public DateTime MakeDate(string input, string inputformat, string returnformat)
        {
            DateTime Date = DateTime.Parse("01-01-1900");
            string[] datesplit;
            string datestr;
            try
            {
                if (input == "") return Date;
                switch (inputformat)
                {
                    case "dd-MM-yyyy":
                        if (returnformat == "MM/dd/yyyy")
                        {
                            datesplit = input.Split('-');
                            datestr = datesplit[1] + "/" + datesplit[0] + "/" + datesplit[2];
                            Date = DateTime.Parse(datestr);
                        }
                        break;
                    case "dd1":

                        break;
                    case "dd/MM/yyyy":
                        if (returnformat == "MM/dd/yyyy")
                        {
                            datesplit = input.Split('/');
                            datestr = datesplit[1] + "/" + datesplit[0] + "/" + datesplit[2];
                            Date = DateTime.Parse(datestr);
                        }
                        break;
                    case "dd-MM-yyyy hh:mm":
                        if (returnformat == "MM/dd/yyyy hh:mm")
                        {
                            datesplit = input.Split('-');
                            datestr = datesplit[1] + "/" + datesplit[0] + "/" + datesplit[2];
                            Date = DateTime.Parse(datestr);
                        }
                        break;
                    default:
                        break;
                }

                //if (inputformat == "dd-MM-yyyy")
                //{

                //}
                //else if (inputformat == "MM-dd-yyyy")
                //{

                //}
                //else if (inputformat == "MM/dd/yyyy")
                //{

                //}
                //else if (inputformat == "dd/MM/yyyy")
                //{

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Date;
        }
        protected DataTable FetchData(string Query)
        {
            DataTable DT = null;
            try
            {
                if (this.TransVariables.Datalayer_Exe == null)
                {
                    BaseDataLayer = new DataLayer.clsDataExecution();
                    BaseDataLayer.BeginTransaction(BaseConnectionString);
                }
                DT = BaseDataLayer.FetchResultAsTable(Query);
                if (this.TransVariables.Datalayer_Exe == null) BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (this.TransVariables.Datalayer_Exe == null) BaseDataLayer.EndTransaction();
                this.TransVariables.Errorstatus = true;
                this.TransVariables.ErrorRemarks = ex.Message;
            }
            return DT;
        }
        protected string ExecuteScalar(string Query)
        {
            string Value = "";
            try
            {
                BaseDataLayer = new DataLayer.clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                SQLQuery = Query;
                Value = BaseDataLayer.ExecuteScalar(SQLQuery);
                BaseDataLayer.EndTransaction();
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction();
                throw ex;
            }
            finally { BaseDataLayer = null; }
            return Value;
        }
        public static string Encode(string strText)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = null;

            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(strText));
            String ba = Convert.ToBase64String(hashedDataBytes);
            return (ba.ToString());
        }


        /// <summary>
        ///  Verifies a hash against a string.
        /// </summary>
        /// <param name="input">Actual string to compare</param>
        /// <param name="hash">Encoded string</param>
        /// <returns></returns>
        public static bool verifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = Encode(input);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FillComboBox(ref DropDownList ddl, string TableName, string ValueField, string TextFiled, string Condition, DataTable _DT = null)
        {
            DataTable DT = null;
            try
            {

                if (_DT == null)
                {
                    BaseDataLayer = new clsDataExecution();
                    BaseDataLayer.BeginTransaction(BaseConnectionString);
                    DT = BaseDataLayer.FetchResultAsTable_Combo(TableName, ValueField, TextFiled, Condition);
                    _DT = DT;
                    BaseDataLayer.EndTransaction();
                }
                else
                {
                    DT = _DT;
                }
                ddl.DataSource = DT;
                ddl.DataTextField = TextFiled;
                ddl.DataValueField = ValueField;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("Not Specified", ""));
            }
            catch (Exception ex)
            {
                if (BaseDataLayer != null) BaseDataLayer.EndTransaction();
                BaseDataLayer = null;
                this.TransVariables.Errorstatus = true;
                this.TransVariables.ErrorRemarks = ex.Message;
                DT = null;
                throw ex;
            }

        }
        public string Success(string Message)
        {
            string CustomMessage = " <div class='alert alert-success alert-dismissable'>" +
                          "<button type='button' class='close' data-dismiss='alert' aria-hidden=true'>&times;</button>" +
                          "<h4>	<i class='icon fa fa-check'></i> Alert!</h4>" +
                          Message + "</div>";
            return CustomMessage;
        }
        public string Warning(string Message)
        {
            string CustomMessage = "<div class=alert alert-warning alert-dismissable'> " +
                          "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                          "<h4><i class='icon fa fa-warning'></i> Alert!</h4>" +
                           Message + "</div>";
            return CustomMessage;
        }
        public string Error(string Message)
        {
            string CustomMessage = "<div class='alert alert-danger alert-dismissable>" +
                            " <button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                            "<h4><i class='icon fa fa-ban'></i> Alert!</h4>" +
                             Message + "</div>";
            return CustomMessage;
        }
        public string Information(string Message)
        {
            string CustomMessage = "<div class='alert alert-info alert-dismissable>" +
                       "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                       "<h4><i class='icon fa fa-info'></i> Alert!</h4>" +
                        Message + "</div>";
            return CustomMessage;
        }



        public string SuccessBanner(string Message)
        {
            string CustomMessage = "<div class='callout callout-success'>" +
                        "<h4></h4> <p>" + Message + "</p></div>";
            return CustomMessage;
        }
        public string WarningBanner(string Message)
        {
            string CustomMessage = "<div class='callout callout-warning'>" +
                "<h4></h4> <p>" + Message + "</p></div>";
            return CustomMessage;
        }
        public string ErrorBanner(string Message)
        {
            string CustomMessage = "<div class='callout callout-danger'>" +
                "<h4></h4> <p>" + Message + "</p></div>";
            return CustomMessage;
        }
        public string InformationBanner(string Message)
        {
            string CustomMessage = "<div class='callout callout-info'>" +
                 "<h4></h4> <p>" + Message + "</p></div>";
            return CustomMessage;
        }
        protected void FillSingleObject(string Query, object Obj)
        {
            try
            {
                BaseDataLayer = new DataLayer.clsDataExecution();
                BaseDataLayer.BeginTransaction(BaseConnectionString);
                BaseDataLayer.FillSingleObject(Query, this);
                BaseDataLayer.EndTransaction(); BaseDataLayer = null;
            }
            catch (Exception ex)
            {
                BaseDataLayer.EndTransaction(); BaseDataLayer = null;
            }
        }
    }
}
