﻿using BLL.IServices;
using Common.Methods;
using Common.Model;
using DPL.EF;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Timers;
namespace API.Hubs
{
    /// <summary>
    /// 實作 WebSocket through SignalR ( 這裡應該只實作 Hub Like Controller 業務邏輯請交給 Service )
    /// </summary>
    public class Hubs : Hub
    {
        private IClientHubService _ClientHubService;
        private readonly IHubContext<Hubs> _hubContext;
        private readonly CashFlowDbContext _CashFlowDbContext;
        private System.Timers.Timer timer;
        public static Boolean TimerTrigger = false;
        public static List<string> ConnIDList = new List<string>(); // 連線ID清單
        public static List<UserInfo> UserList = new List<UserInfo>(); // 連線User清單
        public static UserInfo _UserObject = new UserInfo(); // 使用者物件 Id 信箱 姓名 ...
        public static List<RandomItem<int>> CardList = new List<RandomItem<int>>();
        public static List<Card> Cards = new List<Card>();

        /// <summary>
        /// 建構子
        /// </summary>
        public Hubs(IClientHubService ClientHubService, IHubContext<Hubs> hubContext, CashFlowDbContext cashFlowDbContext)
        {
            _ClientHubService = ClientHubService;
            _CashFlowDbContext = cashFlowDbContext;
            _hubContext = hubContext;

            Cards = _CashFlowDbContext.Cards.AsNoTracking().ToList();
            CardList = Cards.Select(x => new RandomItem<int>
            {
                SampleObj = x.Id,
                Weight = (decimal)x.Weight
            }).ToList();

            if (!TimerTrigger)
            {
                TimerInit();
                TimerTrigger = true;
            }
        }

        /// <summary>
        /// 連線事件
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var Token = Context.GetHttpContext().Request.Query["token"];
            if (Token.Count == 0)
            {
                var Stranger = Context.GetHttpContext().Request.Query["stranger"];
                _UserObject.Name = (string)Stranger + "$$$";
            }
            else
            {
                //string value = !string.IsNullOrEmpty(Token.ToString()) ? Token.ToString() : "default";
                _UserObject = Jose.JWT.Decode<UserInfo>(
                       Token, Encoding.UTF8.GetBytes("錢董"),
                       Jose.JwsAlgorithm.HS256);
            }

            if (ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault() == null)
            {
                ConnIDList.Add(Context.ConnectionId);

            }

            if (_UserObject != null)
            {
                UserList.Add(_UserObject);
            }

            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString, UserList.Select(x => x.Name).ToList());

            // 更新個人 ID
            await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId, UserList.Where(x => x.Id == _UserObject.Id).FirstOrDefault());

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "新連線玩家: " + _UserObject.Name);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string? id = ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault();
            var User = UserList.Where(user => user.Id == _UserObject.Id).FirstOrDefault();

            if (id != null)
            {
                UserList.Remove(User);
                ConnIDList.Remove(id);
            }

            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString, UserList.Select(x => x.Name).ToList());

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "已離線玩家: " + _UserObject.Name);

            await base.OnDisconnectedAsync(ex);
        }

        #region Invoke

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <returns></returns>
        public async Task SendMessage(FromClientChat package)
        {
            //string selfID, string message, string sendToID
            if (string.IsNullOrEmpty(package.sendToID))
            {
                await Clients.All.SendAsync("UpdContent", package.selfID + " 說: " + package.message);
            }
            else
            {
                // 接收人
                await Clients.Client(package.sendToID).SendAsync("UpdContent", package.selfID + " 私訊向你說: " + package.message);

                // 發送人
                await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + package.sendToID + " 私訊說: " + package.message);
            }
        }

        /// <summary>
        /// 計時器初始化
        /// </summary>
        public void TimerInit()
        {
            var flag = true;
            var Count = 0;
            // Create a timer with a two second interval.
            while (flag)
            {

                var time1 = DateTime.Now.Second;

                if (DateTime.Now.Second % 60 == 0)
                {
                    var time = DateTime.Now.Second;
                    timer = new System.Timers.Timer(60000);
                    Count++;
                    flag = false;
                }
            }
            var test = Count;

            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        /// <summary>
        /// 計時器觸發抽卡
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();

                foreach (var ID in ConnIDList)
                {
                    var CardByRandom = Method.RandomWithWeight(CardList);

                    var YourCard = Cards.FirstOrDefault(x => x.Id == CardByRandom);

                    await _hubContext.Clients.Client(ID).SendAsync("Okay", YourCard);
                }

                timer.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽卡
        /// </summary>
        /// <returns></returns>
        public void DrawCard()
        {
            // 寫一隻 Service 執行抽卡
        }

        /// <summary>
        /// 抽卡結果回傳
        /// </summary>
        /// <returns></returns>
        public async Task CardResult()
        {
            // 寫一隻 Service 執行抽卡結果判斷與影響
        }

        #endregion
    }
}
