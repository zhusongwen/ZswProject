using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Zsw.Common.Util
{
    public class ReadExcel
    {
        //支持新老版本的excel
        //HDR=NO=读excel头部 YES=不读头部
        private const string XEXCELPROVIDER = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR={1};IMEX=1;\"";

        public DataTable ConvertExcelToDataTable(string fileFullName, bool needHeader = false, string sheetName = null)
        {
            string readHeader = "YES";// 不读头部
            if (needHeader) readHeader = "NO";
            string connectionString = string.Format(XEXCELPROVIDER, fileFullName, readHeader);
            //没有sheet名称就默认sheet1
            if (string.IsNullOrEmpty(sheetName))
                sheetName = "Sheet1";
            //验证文件是否存在
            if (!File.Exists(fileFullName))
                throw new APPException(string.Format("找不到文件{0}", fileFullName));
            //验证文件格式
            if (!IsExcelFile(fileFullName))
                throw new APPException("不是excel文件");
            //查询excel数据
            var adapter = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), connectionString);
            var ds = new DataSet();
            adapter.Fill(ds, "excelTable");
            var dataTable = ds.Tables["excelTable"];
            return dataTable;
        }

        private bool IsExcelFile(string path)
        {
            string fileExtension = Path.GetExtension(path);
            if (fileExtension == ".xls" || fileExtension == ".xlsx")
                return true;
            else
                return false;
        }
    }
}
