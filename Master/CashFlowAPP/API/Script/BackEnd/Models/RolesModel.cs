﻿

        using DPL.EF;
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;

        namespace Common.Model.AdminSide
        {
            public class CreateRoleArgs : Role
            {
                public int Id { get;set; } 
public string Name { get;set; } 
public byte Status { get;set; } 

            }

            public class UpdateRoleArgs : Role
            {
                public int Id { get;set; } 
public string Name { get;set; } 
public byte Status { get;set; } 

            }

            /// <summary>
            /// 查詢條件
            /// </summary>
            public class ReadRoleArgs
            {
                public string Key { get; set; } = "Id";
                public string JsonString { get; set; } = "[1,2,3]";
            }
        }

