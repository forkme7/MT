﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MarginTrading.Backend.Contracts.AssetPairSettings;
using Refit;

namespace MarginTrading.Backend.Contracts
{
    [PublicAPI]
    public interface IAssetPairSettingsReadingApi
    {
        /// <summary>
        /// Get all settings
        /// </summary>
        [Get("/api/AssetPairSettings/")]
        Task<List<AssetPairSettings.AssetPairSettingsContract>> List();

        /// <summary>
        /// Get settings by id
        /// </summary>
        [Get("/api/AssetPairSettings/{assetPairId}")]
        Task<AssetPairSettings.AssetPairSettingsContract> Get(string assetPairId);

        /// <summary>
        /// Get settings by MatchingEngineMode
        /// </summary>
        [Get("/api/AssetPairSettings/by-mode/{matchingEngineMode}")]
        Task<List<AssetPairSettingsContract>> Get(MarginTrading.Backend.Contracts.AssetPairSettings.MatchingEngineModeContract matchingEngineMode);
    }
}