using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Core.Interface.Excels
{
    /// <summary>
    /// interface thao tac voi excel
    /// </summary>
    public interface IExcelInfra
    {
        /// <summary>
        /// xuat ra file excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// Created by: vdtien (27/6/2023)
        public byte[] ExportToExcel(DataTable data, string title);
    }
}
