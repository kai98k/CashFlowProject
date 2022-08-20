﻿

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class QustionEffectsService : IQustionEffectsService<
                    List<CreateQustionEffectArgs>,
                    List<ReadQustionEffectArgs>,
                    List<UpdateQustionEffectArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public QustionEffectsService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateQustionEffectArgs>> Req)
                    {
                            var qustionEffects = new List<QustionEffect>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var qustionEffect = new QustionEffect();        

                                qustionEffect.Id = Arg.Id;
qustionEffect.TableId = Arg.TableId;
qustionEffect.Description = Arg.Description;
qustionEffect.QuestionId = Arg.QuestionId;
qustionEffect.EffectTableId = Arg.EffectTableId;


                                qustionEffects.Add(qustionEffect);
                            }

                            _CashFlowDbContext.AddRange(qustionEffects);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = qustionEffects.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = "成功新增";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadQustionEffectArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var qustionEffects = _CashFlowDbContext.QustionEffects.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == "Id") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    qustionEffects = qustionEffects.Where(x => Ids.Contains(x.Id));
                                }
                            }

                            var Data = qustionEffects
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
                            Res.TotalDataCount = qustionEffects.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateQustionEffectArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var qustionEffect = _CashFlowDbContext.QustionEffects
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (qustionEffect == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                                }
                                else
                                {
                                    qustionEffect.Id = Arg.Id;
qustionEffect.TableId = Arg.TableId;
qustionEffect.Description = Arg.Description;
qustionEffect.QuestionId = Arg.QuestionId;
qustionEffect.EffectTableId = Arg.EffectTableId;
                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(qustionEffect.Id);
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
                                var qustionEffect = _CashFlowDbContext.QustionEffects
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (qustionEffect == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = "無此Id";
                                }
                                else
                                {
                                    _CashFlowDbContext.QustionEffects.Remove(qustionEffect);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(qustionEffect.Id);
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

