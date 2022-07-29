﻿using Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling;
using System.Net;

namespace API.Filter
{
    /// <summary>
    /// 全域分析器
    /// </summary>
    public class MiniProfilerActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext _ActionExecutingContext)
        {
            if (!_ActionExecutingContext.ModelState.IsValid)
            {
                // 參考:https://code-maze.com/aspnetcore-modelstate-validation-web-api/
                _ActionExecutingContext.Result = new UnprocessableEntityObjectResult(_ActionExecutingContext.ModelState);
            }

            var Context = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 當前執行:{_ActionExecutingContext.ActionDescriptor.DisplayName}";
            var step = MiniProfiler.Current.CustomTimingIf("MiniProfiler", Context, 5);
            _ActionExecutingContext.HttpContext.Items["step"] = step;
        }

        public void OnActionExecuted(ActionExecutedContext _ActionExecutingContext)
        {
            var step = _ActionExecutingContext.HttpContext.Items["step"] as IDisposable;
            if (step != null)
            {
                step.Dispose();
            }
        }
    }
}
