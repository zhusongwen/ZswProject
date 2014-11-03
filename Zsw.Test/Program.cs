using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zsw.Common.Util;

namespace Zsw.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadExcel readExcel = new ReadExcel();
            readExcel.ConvertExcelToDataTable(@"D:\1.xlsx", true);



            Console.Read();
        }
    }
}
