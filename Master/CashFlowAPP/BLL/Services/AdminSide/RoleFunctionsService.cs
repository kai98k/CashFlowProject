﻿

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class RoleFunctionsService : IRoleFunctionsService<
                    List<CreateRoleFunctionArgs>,
                    List<ReadRoleFunctionArgs>,
                    List<UpdateRoleFunctionArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public RoleFunctionsService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateRoleFunctionArgs>> Req)
                    {
                            var roleFunctions = new List<RoleFunction>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var roleFunction = new RoleFunction();        

                                roleFunction.Id = Arg.Id;
roleFunction.RoleId = Arg.RoleId;
roleFunction.FunctionId = Arg.FunctionId;


                                roleFunctions.Add(roleFunction);
                            }

                            _CashFlowDbContext.AddRange(roleFunctions);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = roleFunctions.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = "成功新增";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadRoleFunctionArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var roleFunctions = _CashFlowDbContext.RoleFunctions.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == "Id") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    roleFunctions = roleFunctions.Where(x => Ids.Contains(x.Id));
                                }
                            }

                            var Data = roleFunctions
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
                            Res.TotalDataCount = roleFunctions.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateRoleFunctionArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var roleFunction = _CashFlowDbContext.RoleFunctions
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (roleFunction == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                                }
                                else
                                {
                                    roleFunction.Id = Arg.Id;
roleFunction.RoleId = Arg.RoleId;
roleFunction.FunctionId = Arg.FunctionId;
                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(roleFunction.Id);
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
                                var roleFunction = _CashFlowDbContext.RoleFunctions
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (roleFunction == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = "無此Id";
                                }
                                else
                                {
                                    _CashFlowDbContext.RoleFunctions.Remove(roleFunction);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(roleFunction.Id);
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

