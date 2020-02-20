using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class UnPivotTable
    {
        public DataTable UnpivotDataTable(DataTable pivoted)
        {
            string[] columnNames = pivoted.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName)
                .ToArray();

            var unpivoted = new DataTable("unpivot");
            //unpivoted.Columns.Add(pivoted.Columns[0].ColumnName, pivoted.Columns[0].DataType);
            unpivoted.Columns.Add("ReportFieldOrder", typeof(int));
            unpivoted.Columns.Add("ColumnName", typeof(string));
            unpivoted.Columns.Add("Value", typeof(string));

            int RowNo = 1;
            for (int r = 0; r < pivoted.Rows.Count; r++)
            {
                for (int c = 0; c < columnNames.Length; c++)
                {
                    var value = pivoted.Rows[r][c]?.ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        unpivoted.Rows.Add(RowNo, columnNames[c], value);                       
                    }
                }
                RowNo++;
            }

            return unpivoted;
        }
    }
}
