﻿

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class LogsService : ILogsService<
                    List<CreateLogArgs>,
                    List<ReadLogArgs>,
                    List<UpdateLogArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public LogsService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateLogArgs>> Req)
                    {
                            var logs = new List<Log>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var log = new Log();        

                                log.Id = Arg.Id;
log.UserId = Arg.UserId;
log.UserName = Arg.UserName;
log.TableId = Arg.TableId;
log.TableName = Arg.TableName;
log.Action = Arg.Action;
log.ActionDate = Arg.ActionDate;


                                logs.Add(log);
                            }

                            _CashFlowDbContext.AddRange(logs);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = logs.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = "成功新增";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadLogArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var logs = _CashFlowDbContext.Logs.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == "Id") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    logs = logs.Where(x => Ids.Contains(x.Id));
                                }
                            }

                            var Data = logs
                            // 後端分頁
                            // 省略幾筆 ( 頁數 * 每頁幾筆 )
                            .Skip(((int)Req.PageIndex -1) * (int)Req.PageSize)
                            // 取得幾筆，
                            .Take((int)Req.PageSize)
                            .ToList();

                            Res.Data = Data;
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = "成功讀取";
                            Res.TotalDataCount = logs.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateLogArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var log = _CashFlowDbContext.Logs
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (log == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                                }
                                else
                                {
                                    log.Id = Arg.Id;
log.UserId = Arg.UserId;
log.UserName = Arg.UserName;
log.TableId = Arg.TableId;
log.TableName = Arg.TableName;
log.Action = Arg.Action;
log.ActionDate = Arg.ActionDate;
                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(log.Id);
                                }
                            }

                            Res.Data = $@"SussList：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = "成功更改";

                            return Res;
                    }

                    public async Task<ApiResponse> Delete(ApiRequest<List<int?>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var log = _CashFlowDbContext.Logs
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (log == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = "無此Id";
                                }
                                else
                                {
                                    _CashFlowDbContext.Logs.Remove(log);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(log.Id);
                                }
                            }

                            Res.Data = $@"SussList：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = "成功刪除";

                            return Res;
                    }

            }
        }

