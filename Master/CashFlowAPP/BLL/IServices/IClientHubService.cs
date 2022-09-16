﻿using Common.Model;
using DPL.EF;

namespace BLL.IServices
{
    public interface IClientHubService
    {
        Task<CardInfo> ProcessCardInfo(Card YourCard, List<UserInfo> UsersInfos, int YourUserId, string ConnectId);
        Task<ApiResponse> ReadFiInfo(int UserId, string ConnectId);
        Task<ApiResponse> ChoiceOfCard(int UserId, string ConnectId);
        Task<ApiResponse> AssetSale(int UserId, string ConnectId, AssetAndCategoryModel Asset);
        Task<ApiResponse> LiabilitieSale(int UserId, string ConnectId, AssetAndCategoryModel Liabilitie);
        FiInfo FiInfoAccounting(FiInfo YourFiInfo);
    }
}
