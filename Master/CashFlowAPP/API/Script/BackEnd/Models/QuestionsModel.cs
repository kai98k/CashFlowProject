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
            public class CreateQuestionArgs : Question
            {
                public int Id { get;set; } 
public byte Type { get;set; } 
public string Name { get;set; } 
public string Answer { get;set; } 
public byte Status { get;set; } 

            }

            public class UpdateQuestionArgs : Question
            {
                public int Id { get;set; } 
public byte Type { get;set; } 
public string Name { get;set; } 
public string Answer { get;set; } 
public byte Status { get;set; } 

            }

            /// <summary>
            /// 查詢條件
            /// </summary>
            public class ReadQuestionArgs
            {
                public string Key { get; set; } = "Id";
                public string JsonString { get; set; } = "[1,2,3]";
            }
        }

