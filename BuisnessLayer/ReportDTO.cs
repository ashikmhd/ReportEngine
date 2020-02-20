using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class ReportDTO
    {
        public string ConnectionString = "";
        public string ReportQuery = "";
        public string ReportHeader = "";
        public string ReportName = "";
        public List<ReportParameters> ReportParamList = new List<ReportParameters>();
    }
    public class DefaultValueDTO
    {
        public string ReportSettingValue;
        public string DefaultValue;
    }
    public class ReportParameters
    {
        public string ParameterText;
        public string ParameterValue;
    }
}
