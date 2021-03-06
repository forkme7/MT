﻿using System;
using MarginTrading.Backend.Core;
using MarginTrading.Contract.RabbitMqMessageModels;

namespace MarginTrading.Backend.Services
{
    class AccountMarginEventMessageConverter
    {
        public static AccountMarginEventMessage Create(IMarginTradingAccount account, bool isStopout, DateTime eventTime)
        {
            return new AccountMarginEventMessage
            {
                EventId = Guid.NewGuid().ToString("N"),
                EventTime = eventTime,
                IsEventStopout = isStopout,

                ClientId = account.ClientId,
                AccountId = account.Id,
                TradingConditionId = account.TradingConditionId,
                BaseAssetId = account.BaseAssetId,
                Balance = account.Balance,
                WithdrawTransferLimit = account.WithdrawTransferLimit,

                MarginCall = account.GetMarginCallLevel(),
                StopOut = account.GetStopOutLevel(),
                TotalCapital = account.GetTotalCapital(),
                FreeMargin = account.GetFreeMargin(),
                MarginAvailable = account.GetMarginAvailable(),
                UsedMargin = account.GetUsedMargin(),
                MarginInit = account.GetMarginInit(),
                PnL = account.GetPnl(),
                OpenPositionsCount = account.GetOpenPositionsCount(),
                MarginUsageLevel = account.GetMarginUsageLevel(),
            };
        }
    }
}
