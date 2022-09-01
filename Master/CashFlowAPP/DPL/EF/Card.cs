﻿using System;
using System.Collections.Generic;

namespace DPL.EF
{
    public partial class Card
    {
        public Card()
        {
            CardEffects = new HashSet<CardEffect>();
        }

        /// <summary>
        /// 卡片流水號
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 卡片名稱
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 狀態 0. 停用 1. 啟用 2. 刪除
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// 權重
        /// </summary>
        public decimal? Weight { get; set; }

        public virtual ICollection<CardEffect> CardEffects { get; set; }
    }
}