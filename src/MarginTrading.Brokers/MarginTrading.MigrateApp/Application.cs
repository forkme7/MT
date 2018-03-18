using System;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage.Tables;
using Common.Log;
using Lykke.SettingsReader;
using Lykke.SlackNotifications;
using MarginTrading.AccountReportsBroker.Repositories.AzureRepositories.Entities;
using MarginTrading.AzureRepositories;
using MarginTrading.BrokerBase;
using MarginTrading.BrokerBase.Settings;
using MarginTrading.Contract.RabbitMqMessageModels;
using MoreLinq;

namespace MarginTrading.MigrateApp
{
    internal class Application : BrokerApplicationBase<BidAskPairRabbitMqContract>
    {
        private readonly Settings _settings;
        private readonly IReloadingManager<Settings> _reloadingManager;

        public Application(ILog logger, Settings settings,
            CurrentApplicationInfo applicationInfo,
            ISlackNotificationsSender slackNotificationsSender,
            IReloadingManager<Settings> reloadingManager)
            : base(logger, slackNotificationsSender, applicationInfo)
        {
            _settings = settings;
            _reloadingManager = reloadingManager;
        }

        protected override BrokerSettingsBase Settings => _settings;
        protected override string ExchangeName => "Fake";

        protected override Task HandleMessage(BidAskPairRabbitMqContract message)
        {
            throw new NotSupportedException();
        }

        public override void Run()
        {
            WriteInfoToLogAndSlack("Starting MigrateApp");

            try
            {
                Task.WaitAll(
                    Task.Run(ProcessAccounts),
                    Task.Run(ProcessTradingConditions),
                    Task.Run(ProcessAccountsReports),
                    Task.Run(ProcessAccountsStatReports)
                );
                WriteInfoToLogAndSlack("MigrateApp finished");
            }
            catch (Exception ex)
            {
                _logger.WriteErrorAsync(ApplicationInfo.ApplicationFullName, "Application.RunAsync", null, ex)
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private async Task ProcessAccounts()
        {
            var repository = AzureTableStorage<MarginTradingAccountEntity>.Create(
                _reloadingManager.Nested(s => s.Db.MarginTradingConnString),
                "MarginTradingAccounts", _logger);
            var tasks = (await repository.GetDataAsync())
                .Where(a => a.LegalEntity == null)
                .GroupBy(a => a.PartitionKey)
                .SelectMany(g => g.Batch(500))
                .Select(batch => repository.InsertOrMergeBatchAsync(batch.Pipe(a => a.LegalEntity = "LYKKEVU")));
            await Task.WhenAll(tasks);
        }

        private async Task ProcessTradingConditions()
        {
            var repository = AzureTableStorage<TradingConditionEntity>.Create(
                _reloadingManager.Nested(s => s.Db.MarginTradingConnString),
                "MarginTradingConditions", _logger);
            var tasks = (await repository.GetDataAsync())
                .Where(a => a.LegalEntity == null)
                .Batch(500)
                .Select(batch => repository.InsertOrMergeBatchAsync(batch.Pipe(a => a.LegalEntity = "LYKKEVU")));
            await Task.WhenAll(tasks);
        }

        private async Task ProcessAccountsReports()
        {
            var repository = AzureTableStorage<AccountsReportEntity>.Create(
                _reloadingManager.Nested(s => s.Db.ReportsConnString),
                "ClientAccountsReports", _logger);
            var tasks = (await repository.GetDataAsync())
                .Where(a => a.LegalEntity == null)
                .GroupBy(a => a.PartitionKey)
                .SelectMany(g => g.Batch(500))
                .Select(batch => repository.InsertOrMergeBatchAsync(batch.Pipe(a => a.LegalEntity = "LYKKEVU")));
            await Task.WhenAll(tasks);
        }

        private async Task ProcessAccountsStatReports()
        {
            var repository = AzureTableStorage<AccountsStatReportEntity>.Create(
                _reloadingManager.Nested(s => s.Db.ReportsConnString),
                "ClientAccountsStatusReports", _logger);
            var tasks = (await repository.GetDataAsync())
                .Where(a => a.LegalEntity == null)
                .GroupBy(a => a.PartitionKey)
                .SelectMany(g => g.Batch(500))
                .Select(batch => repository.InsertOrMergeBatchAsync(batch.Pipe(a => a.LegalEntity = "LYKKEVU")));
            await Task.WhenAll(tasks);
        }
    }
}