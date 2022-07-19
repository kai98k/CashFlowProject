﻿using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class AssetCategory
    {
        /// <summary>
        /// 資產類別流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 資產類別名稱
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 父類別流水號
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }
    }
}
