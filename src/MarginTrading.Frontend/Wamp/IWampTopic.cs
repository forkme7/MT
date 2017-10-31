﻿using MarginTrading.Common.Documentation;
using MarginTrading.Contract.ClientContracts;
using MarginTrading.Contract.RabbitMqMessageModels;

namespace MarginTrading.Frontend.Wamp
{
    public interface IWampTopic
    {
        [DocMe(Name = "prices.update", Description = "sends prices for all instruments")]
        BidAskPairRabbitMqContract AllPricesUpdate();
        [DocMe(Name = "prices.update.{instrumentId}", Description = "sends prices for specific instrument")]
        BidAskPairRabbitMqContract InstumentPricesUpdate();
        [DocMe(Name = "user.{notificationId}", Description = "sends user updates on position, account changes and dictionaries")]
        NotifyResponse UserUpdates();

    }
}