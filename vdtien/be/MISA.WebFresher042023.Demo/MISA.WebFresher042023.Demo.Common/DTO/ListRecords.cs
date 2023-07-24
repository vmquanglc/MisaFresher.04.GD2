using MISA.WebFresher042023.Demo.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Common.DTO
{
    public class ListRecords<T>
    {
        #region Property
        public int TotalRecord { get; set; }

        public int TotalRoot { get;set; }

        public List<T> Data { get; set; }
    #endregion
    }
}
