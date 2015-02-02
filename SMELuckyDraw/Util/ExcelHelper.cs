using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMELuckyDraw.Util
{
    public class ExcelHelper
    {
        /// <summary>
        /// Read from excel to Datatable
        /// </summary>
        /// <param name="excelPath">excel file path</param>
        /// <param name="hasHeader">whether read column header, false - read, true - won't read</param>
        /// <param name="isMixedData">false - all string, true - string & numeric</param>
        /// <param name="sheetName">sheet name, default sheet1</param>
        /// <param name="startRow">read from row, start from 0</param>
        /// <param name="endRow">read to row, start from 0</param>
        /// <param name="startColumn">read from column, start from 0</param>
        /// <param name="endColumn">read to column, start from 0</param>
        /// <returns></returns>
        public static DataTable ExcelToDatatable(string excelPath, bool hasHeader, bool isMixedData, string sheetName = "sheet1", int startRow = -1, int endRow = -1, int startColumn = -1, int endColumn = -1)
        {
            LogHelper.DEBUG("Begin ExcelToDatatable");
            // check fesibillity
            if (!File.Exists(excelPath) ||
                string.IsNullOrWhiteSpace(sheetName) ||
                startRow < -1 ||
                endRow < -1 ||
                startColumn < -1 ||
                endColumn < -1 ||
                startRow > endRow ||
                startColumn > endColumn)
            {
                return null;
            }

            string HDR = hasHeader ? "Yes" : "No";
            string IMEX = isMixedData ? "1" : "0";
            string strConn;
            if (excelPath.Substring(excelPath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelPath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=" + IMEX + "\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelPath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=" + IMEX + "\"";

            DataTable outputTable = null;

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                try
                {
                    conn.Open();

                    DataTable schemaTable = conn.GetOleDbSchemaTable(
                        OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheetName + "$]", conn);
                    cmd.CommandType = CommandType.Text;

                    outputTable = new DataTable(sheetName);
                    new OleDbDataAdapter(cmd).Fill(outputTable);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", sheetName, excelPath), ex);
                }
            }

            // remove tail rows not expected
            if (endRow != -1 && endRow < outputTable.Rows.Count)
            {
                for (int i = endRow + 1; i < outputTable.Rows.Count; i++)
                {
                    outputTable.Rows.RemoveAt(endRow + 1);
                }
            }

            // remove start rows not expected
            if (startRow != -1)
            {
                for (int i = 0; i < startRow; i++)
                {
                    outputTable.Rows.RemoveAt(0);
                }
            }

            // remove tail columns not expected
            if (endColumn != -1 && endColumn < outputTable.Columns.Count)
            {
                for (int i = endColumn + 1; i < outputTable.Columns.Count; i++)
                {
                    outputTable.Columns.RemoveAt(endColumn + 1);
                }
            }

            // remove start columns not expected
            if (startColumn != -1)
            {
                for (int i = 0; i < startColumn; i++)
                {
                    outputTable.Columns.RemoveAt(0);
                }
            }

            return outputTable;
        }
    }
}
