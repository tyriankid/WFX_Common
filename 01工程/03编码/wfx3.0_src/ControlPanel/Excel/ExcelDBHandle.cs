using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace Hidistro.ControlPanel.Excel
{
    /// <summary>
    /// Oledb操作Excel【通用处理类】
    /// </summary>
    public class ExcelDBClass
    {
        private string _path;
        private string _oleConnStr;
        private OleDbConnection _oleConn;

        public ExcelDBClass()
        {
        }

        /// <summary>
        /// Oledb打开Excel
        /// </summary>
        /// <param name="path">文件绝对路径</param>
        /// <param name="isHDR">是否读取首行</param>
        public ExcelDBClass(string path, bool isHDR)
        {
            _path = path;
            _oleConnStr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", _path);
            if (isHDR)
                _oleConnStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            //_oleConnStr = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties=Excel 8.0;", _path);
            //if (isHDR)
            //    _oleConnStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _path + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";

            _oleConn = new OleDbConnection(_oleConnStr);
           _oleConn.Open();
        }

        public void Dispose()
        {
            try
            {
                _oleConn.Close();
                _oleConn.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取Excel文件Sheet名
        /// </summary>
        /// <returns>Sheet名称数组</returns>
        public string[] GetSheetNames()
        {
            DataTable dtSheetName = _oleConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            string[] tablenames = new string[dtSheetName.Rows.Count];
            for (int k = 0; k < dtSheetName.Rows.Count; k++)
            {
                tablenames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
                if ((tablenames[k].Substring(0, 1) == "'") && (tablenames[k].Substring(tablenames[k].Length - 1, 1) == "'"))
                    tablenames[k] = tablenames[k].Substring(1, tablenames[k].Length - 2);
            }
            return tablenames;
        }
        /// <summary>
        /// 获取Excel文件Sheet名
        /// </summary>
        /// <returns>Sheet名称数组</returns>
        public string[] GetSheetNamesEx()
        {
            DataTable dtSheetName = _oleConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            string[] strTableNames = new string[dtSheetName.Rows.Count];
            for (int k = 0; k < dtSheetName.Rows.Count; k++)
            {
                //获取sheet名称，添加到数组中取
                strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
                if ((strTableNames[k].Substring(0, 1) == "'") && (strTableNames[k].Substring(strTableNames[k].Length - 1, 1) == "'"))
                {
                    strTableNames[k] = strTableNames[k].Substring(1, strTableNames[k].Length - 2);
                }
                if (strTableNames[k].Substring(strTableNames[k].Length - 1, 1) == "$")
                {
                    strTableNames[k] = strTableNames[k].Substring(0, strTableNames[k].Length - 1);
                }
            }
            return strTableNames;
        }
        /// <summary>
        /// 根据格式化定义的Hashtable创建Excel文件Sheet
        /// </summary>
        /// <param name="htsheet">格式化Hashtable（“SheetName”key-Sheet名称字符串，“SheetField”key-Sheet列名称与类型二维数组）</param>
        /// <returns></returns>
        public bool CreateSheet(Hashtable htsheet)
        {
            bool sucess = false;
            try
            {
                string sqlCreateSheet;
                sqlCreateSheet = @"CREATE TABLE [" + htsheet["SheetName"].ToString() + "] (";
                string[,] field = (string[,])htsheet["SheetField"];
                int fieldnum = field.GetLength(0);
                for (int i = 0; i < fieldnum; i++)
                {
                    sqlCreateSheet += field[i, 0] + " " + field[i, 1] + ((i == (fieldnum - 1)) ? "" : ",");
                }
                sqlCreateSheet += ")";
                OleDbCommand cmdCreate = new OleDbCommand(sqlCreateSheet, _oleConn);
                cmdCreate.ExecuteNonQuery();
                sucess = true;
            }
            catch
            {
                sucess = false;
            }
            return sucess;
        }

        /// <summary>
        /// 根据DataTable创建与Sheet对应的格式化Hashtable，“SheetName”key-表名称，“SheetField”key-表的字段名称与类型
        /// </summary>
        /// <param name="table">用于创建格式化Hashtable的数据表</param>
        /// <returns>用于创建Sheet的格式化Hashtable</returns>
        //public Hashtable GetHashtableByDataTable(DataTable table, DataColumnCollection columns)
        public Hashtable GetHashtableByDataTable(DataColumnCollection columns)
        {
            int fieldnum = columns.Count;   // table.Columns.Count;
            string[,] field = new string[fieldnum, 2];
            for (int i = 0; i < fieldnum; i++)
            {
                field[i, 0] = "[" + columns[i].ColumnName + "]";
                Type fieldtype = columns[i].DataType;
                switch (fieldtype.Name)
                {
                    case "DateTime":
                        field[i, 1] = "DateTime";
                        break;
                    case "Boolean":
                    case "String":
                        field[i, 1] = "Text";
                        break;
                    case "Decimal":
                    case "Single":
                    case "Double":
                        field[i, 1] = "Double";
                        break;
                    case "SByte":
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "Byte":
                    case "Char":
                    case "UInt16":
                    case "UInt32":
                    case "UInt64":
                        field[i, 1] = "Integer";
                        break;
                }
            }
            Hashtable htSheet = new Hashtable();
            htSheet["SheetName"] = columns[0].Table.TableName;      // table.TableName;
            htSheet.Add("SheetField", field);
            return htSheet;
        }

        /// <summary>
        /// 根据DataTable填充Excel文件中同名Sheet的数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool FillSheetCell(DataTable table, DataColumnCollection columns)
        {
            bool sucess = false;

            try
            {
                OleDbCommand cmdInsert = new OleDbCommand();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    cmdInsert.CommandText = "Insert Into [" + table.TableName + "] (";
                    string fields = "";
                    string values = "";
                    int fieldnum = table.Columns.Count;
                    for (int j = 0; j < fieldnum; j++)
                    {
                        if (columns.IndexOf(table.Columns[j]) < 0)
                            continue;

                        fields += "[" + table.Columns[j].ColumnName + "]" + ((j == fieldnum - 1) ? "" : ",");
                        Type fieldtype = table.Columns[j].DataType;
                        string val;
                        switch (fieldtype.Name)
                        {
                            case "DBNull":
                                val = "null";
                                values += val + ((j == fieldnum - 1) ? "null" : ",");
                                break;
                            case "Boolean":
                            case "DateTime":
                            case "String":
                                val = table.Rows[i][j].ToString().Trim().Replace("'", "''");
                                val = val.Replace("&nbsp;", "");
                                val = (val == null) ? "null" : val;
                                values += "'" + val + "'" + ((j == fieldnum - 1) ? "" : ",");
                                break;
                            case "Decimal":
                            case "Single":
                            case "Double":

                            case "SByte":
                            case "Int16":
                            case "Int32":
                            case "Int64":

                            case "Byte":
                            case "Char":
                            case "UInt16":
                            case "UInt32":
                            case "UInt64":
                                val = table.Rows[i][j].ToString().Trim();
                                val = ((val == "") || (val == null)) ? "null" : val;
                                values += val + ((j == fieldnum - 1) ? "" : ",");
                                break;
                        }
                    }
                    cmdInsert.CommandText += fields + ") Values(" + values + ")";
                    cmdInsert.Connection = _oleConn;
                    cmdInsert.ExecuteNonQuery();
                }
                sucess = true;
            }
            catch
            {
                sucess = false;
            }
            return sucess;
        }

        /// <summary>
        /// 根据DataTable自动创建并填充Excel文件的Sheet
        /// </summary>
        /// <param name="table"></param>
        public void CreateAndFillSheet(DataTable table, DataColumnCollection columns)
        {
            CreateSheet(GetHashtableByDataTable(columns));
            FillSheetCell(table, columns);
        }

        /// <summary>
        /// 根据数据集自动创建Excel文件
        /// </summary>
        /// <param name="dsXls"></param>
        public void ImportToExcel(DataSet dsXls)
        {
            for (int i = 0; i < dsXls.Tables.Count; i++)
            {
                CreateSheet(GetHashtableByDataTable(dsXls.Tables[i].Columns));
                FillSheetCell(dsXls.Tables[i], dsXls.Tables[i].Columns);
            }
        }

        public DataSet ExportToDataSet()
        {
            string[] sheets = GetSheetNames();
            OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
            DataSet dsXls = new DataSet();
            string strSelect;
            for (int i = 0; i < sheets.Length; i++)
            {
                if (sheets[i].Substring(sheets[i].Length - 1, 1) == "$")
                {
                    strSelect = string.Format(@"Select * From [{0}]", sheets[i]);
                    oleAdapter.SelectCommand = new OleDbCommand(strSelect, _oleConn);
                    DataSet dsSheet = new DataSet();
                    string dtname = sheets[i].Substring(0, sheets[i].Length - 1);
                    oleAdapter.Fill(dsSheet, dtname);
                    dsXls.Tables.Add(dsSheet.Tables[dtname].Copy());
                    if (dsXls.Tables[dtname].Columns[0].ColumnName == "F1") dsXls.Tables[dtname].Columns.Remove("F1");
                }
            }
            return dsXls;
        }

        public DataSet ExportToDataSet(int top)
        {
            string[] sheets = GetSheetNames();
            OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
            DataSet dsXls = new DataSet();
            string strSelect;
            for (int i = 0; i < sheets.Length; i++)
            {
                if (sheets[i].Substring(sheets[i].Length - 1, 1) == "$")
                {
                    strSelect = string.Format(@"Select top " + top + " * From [{0}]", sheets[i]);
                    oleAdapter.SelectCommand = new OleDbCommand(strSelect, _oleConn);
                    DataSet dsSheet = new DataSet();
                    string dtname = sheets[i].Substring(0, sheets[i].Length - 1);
                    oleAdapter.Fill(dsSheet, dtname);
                    dsXls.Tables.Add(dsSheet.Tables[dtname].Copy());
                }
            }
            return dsXls;
        }

    }
}
