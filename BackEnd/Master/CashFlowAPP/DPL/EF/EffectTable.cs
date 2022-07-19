﻿using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class EffectTable
    {
        /// <summary>
        /// 影響資料表流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 受影響資料表名稱
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }
    }
}
