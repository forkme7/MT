﻿using JetBrains.Annotations;
using MarginTrading.Backend.Contracts;
using MarginTrading.Backend.Contracts.AccountAssetPair;
using MarginTrading.Backend.Contracts.Common;
using MarginTrading.Backend.Contracts.TradingConditions;
using MarginTrading.Backend.Core.TradingConditions;
using MarginTrading.Backend.Services.TradingConditions;
using MarginTrading.Common.Middleware;
using MarginTrading.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarginTrading.Backend.Controllers
{
    [Authorize]
    [Route("api/tradingConditions")]
    [MiddlewareFilter(typeof(RequestLoggingPipeline))]
    public class TradingConditionsController : Controller, ITradingConditionsEditingApi
    {
        private readonly TradingConditionsManager _tradingConditionsManager;
        private readonly AccountGroupManager _accountGroupManager;
        private readonly AccountAssetsManager _accountAssetsManager;
        private readonly IConvertService _convertService;

        public TradingConditionsController(TradingConditionsManager tradingConditionsManager,
            AccountGroupManager accountGroupManager,
            AccountAssetsManager accountAssetsManager,
            IConvertService convertService)
        {
            _tradingConditionsManager = tradingConditionsManager;
            _accountGroupManager = accountGroupManager;
            _accountAssetsManager = accountAssetsManager;
            _convertService = convertService;
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("AddOrReplaceTradingCondition")]
        public async Task<BackendResponse<TradingConditionContract>> InsertOrUpdate(
            [FromBody] TradingConditionContract model)
        {
            var tradingCondition = await _tradingConditionsManager.AddOrReplaceTradingConditionAsync(Convert(model));
            
            return BackendResponse<TradingConditionContract>.Ok(Convert(tradingCondition));
        }

        [HttpPost]
        [Route("accountGroups")]
        [SwaggerOperation("AddOrReplaceAccountGroup")]
        public async Task<BackendResponse<AccountGroupContract>> InsertOrUpdateAccountGroup(
            [FromBody] AccountGroupContract model)
        {
            var accountGroup = await _accountGroupManager.AddOrReplaceAccountGroupAsync(Convert(model));

            return BackendResponse<AccountGroupContract>.Ok(Convert(accountGroup));
        }

        [HttpPost]
        [Route("accountAssets/assignInstruments")]
        [SwaggerOperation("AssignInstruments")]
        public async Task<BackendResponse<List<AccountAssetPairContract>>> AssignInstruments(
            [FromBody] AssignInstrumentsContract model)
        {
            try
            {
                var assetPairs = await _accountAssetsManager.AssignInstruments(model.TradingConditionId, model.BaseAssetId,
                    model.Instruments);

                return BackendResponse<List<AccountAssetPairContract>>.Ok(
                    assetPairs.Select(a => Convert(a))
                    .ToList());
            }
            catch (Exception ex)
            {
                return BackendResponse<List<AccountAssetPairContract>>.Error(ex.Message);
            }
        }

        [HttpPost]
        [Route("accountAssets")]
        [SwaggerOperation("AddOrReplaceAccountAsset")]
        public async Task<BackendResponse<AccountAssetPairContract>> InsertOrUpdateAccountAsset([FromBody]AccountAssetPairContract model)
        {
            var assetPair = await _accountAssetsManager.AddOrReplaceAccountAssetAsync(Convert(model));
            return BackendResponse<AccountAssetPairContract>.Ok(Convert(assetPair));
        }

        private ITradingCondition Convert(TradingConditionContract tradingCondition)
        {
            return _convertService.Convert<TradingConditionContract, TradingCondition>(tradingCondition);
        }
        private TradingConditionContract Convert([CanBeNull] ITradingCondition tradingCondition)
        {
            if (tradingCondition == null)
            {
                return null;
            }
            return _convertService.Convert<ITradingCondition, TradingConditionContract>(tradingCondition);
        }

        private IAccountGroup Convert(AccountGroupContract accountGroup)
        {
            return _convertService.Convert<AccountGroupContract, AccountGroup>(accountGroup);
        }
        private AccountGroupContract Convert([CanBeNull] IAccountGroup accountGroup)
        {
            if (accountGroup == null)
            {
                return null;
            }
            return _convertService.Convert<IAccountGroup, AccountGroupContract>(accountGroup);
        }

        private AccountAssetPairContract Convert(IAccountAssetPair accountAssetPair)
        {
            return _convertService.Convert<IAccountAssetPair, AccountAssetPairContract>(accountAssetPair);
        }
        private IAccountAssetPair Convert(AccountAssetPairContract accountAssetPair)
        {
            return _convertService.Convert<AccountAssetPairContract, AccountAssetPair>(accountAssetPair);
        }
    }
}