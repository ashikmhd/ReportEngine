using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
namespace BusinessLayer
{
    public class TransactionVariables
    {
        public Boolean Errorstatus = false;
        public string ErrorRemarks = "Transaction completed Successfully";
        public int RemarksTypeNB = 0;
        public string TransProcedureName;
        /// <summary>
        /// 1=Success,2=Warning,3=Error,4=Information
        /// </summary>
        public int TypeofError;
        public string TransClassName;
        public Boolean IsBeginTrans = false;
        private IDataExecution p_Datalayer_Exe;
        public IDataExecution Datalayer_Exe
        {
            get { return p_Datalayer_Exe; }
            set { p_Datalayer_Exe = value; }
        }
        public void initialize()
        {
            Errorstatus = false;
            ErrorRemarks = "Transaction Completed Successfully";
            TypeofError = 1;
            TransProcedureName = "";
            TransClassName = "";
            IsBeginTrans = false;
            Datalayer_Exe = null;
        }
        //this clone() i used for assigning the transvariables to other class transvaribales... 
        public TransactionVariables Clone()
        {
            return (TransactionVariables)this.MemberwiseClone();
        }

    }
}
