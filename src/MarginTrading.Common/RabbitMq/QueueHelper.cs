﻿using Microsoft.Extensions.PlatformAbstractions;

namespace MarginTrading.Common.RabbitMq
{
    public static class QueueHelper
    {
        public static string BuildQueueName(string exchangeName, string env)
        {
            return
                $"{exchangeName}.{PlatformServices.Default.Application.ApplicationName}.{env ?? "DefaultEnv"}";
        }
    }
}
