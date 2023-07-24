﻿using MISA.WebFresher042023.Demo.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher042023.Demo.Common.Entity
{
    /// <summary>
    /// thuc the account
    /// </summary>
    /// created by: vdtiem (18/7/2023)
    public class Account:Base
    {
        /// <summary>
        /// id tai khoan
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// ma tai khoan
        /// </summary>
        public string? AccountCode { get; set; }
    
        /// <summary>
        /// ten tai khoan
        /// </summary>
        public string? AccountName { get; set; }

        /// <summary>
        /// ten tieng anh cua tai khoan
        /// </summary>
        public string? AccountNameEnglish { get; set; }
        
        /// <summary>
        /// id cha tai khoan
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// tinh chat tai khoan
        /// </summary>
        public AccountFeature? AccountFeature { get; set; }
        
        /// <summary>
        /// dien giai tai khoan
        /// </summary>
        public string? Explain { get; set; }

        /// <summary>
        /// Trang thai su dung
        /// </summary>
        public Status? Status { get; set; }

        /// <summary>
        /// node co phai la parent node khong
        /// </summary>
        public int? IsParent { get; set; }

        /// <summary>
        /// Cap cua node
        /// </summary>
        public int? Grade { get; set; }

        /// <summary>
        /// so luong con
        /// </summary>
        public int?NumberChilds { get; set; }

        /// <summary>
        /// doi tuong su dung
        /// </summary>
        public UserObject? UserObject { get; set; }

    }

}
