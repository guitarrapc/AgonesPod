using System;
using System.Threading.Tasks;
using MicroBatchFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AgonesPod.ConsoleSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await BatchHost.CreateDefaultBuilder()
                .RunBatchEngineAsync<AgonesConsole>(args);
        }

        public class AgonesConsole : BatchBase
        {
            readonly ILogger<AgonesConsole> _logger;
            readonly string _defaultFleetName;

            public AgonesConsole(IConfiguration config, ILogger<AgonesConsole> logger)
            {
                _defaultFleetName = config.GetValue("DEFAULT_FLEET_NAME", "");
                _logger = logger;
            }

            [Command("getgameserver", "Get GameServer Info")]
            public void GetGameServer()
            {
                _logger.LogInformation($"# {nameof(GetGameServer)}");
                foreach (var server in GameServer.Current)
                {
                    _logger.LogInformation($"  Host:Port = {server.Address}:{server.Port}");
                    _logger.LogInformation($"    {nameof(server.IsRunningOnKubernetes)} : {server.IsRunningOnKubernetes}");
                    _logger.LogInformation($"    {nameof(server.IsAllocated)} : {server.IsAllocated}");
                    _logger.LogInformation($"    {nameof(server.Name)} : {server.Name}");
                    _logger.LogInformation($"    {nameof(server.Address)} : {server.Address}");
                    _logger.LogInformation($"    {nameof(server.Port)} : {server.Port}");
                    _logger.LogInformation($"    {nameof(server.State)} : {server.State}");
                }
            }

            [Command("allocate", "Allocate Readied GameServer")]
            public async Task AllocateGameServer([Option("-f", "fleet name to allocate gameserver")]string fleetName = "")
            {
                _logger.LogInformation($"# {nameof(AllocateGameServer)}");

                if (string.IsNullOrWhiteSpace(fleetName))
                {
                    if (string.IsNullOrWhiteSpace(_defaultFleetName))
                        throw new ArgumentNullException("fleetName arg was missing and default fleet name also missing. Please set env DEFAULT_FLEET_NAME to set default fleet name.");

                    _logger.LogDebug("fleetName arg was missing, using default fleet name retrieved from env DEFAULT_FLEET_NAME.");
                    fleetName = _defaultFleetName;
                }

                var allocation = await GameServer.AllocateAsync(fleetName);
                _logger.LogInformation($"  Host:Port = {nameof(allocation)} = {allocation}");
                _logger.LogInformation($"    {nameof(allocation.IsAllocated)} = {allocation.IsAllocated}");
                _logger.LogInformation($"    {nameof(allocation.IsRunningOnKubernetes)} = {allocation.IsRunningOnKubernetes}");
                _logger.LogInformation($"    {nameof(allocation.Name)} = {allocation.Name}");
                _logger.LogInformation($"    {nameof(allocation.Address)} = {allocation.Address}");
                _logger.LogInformation($"    {nameof(allocation.Port)} = {allocation.Port}");
                _logger.LogInformation($"    {nameof(allocation.State)} = {allocation.State}");
                _logger.LogInformation($"    {nameof(allocation.NodeName)} = {allocation.NodeName}");
                _logger.LogInformation($"    {nameof(allocation.Scheduling)} = {allocation.Scheduling}");
            }
        }
    }
}
