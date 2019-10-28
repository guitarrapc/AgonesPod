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
            readonly string _defaultFleetName;
            public AgonesConsole(IConfiguration config)
            {
                _defaultFleetName = config.GetValue("DEFAULT_FLEET_NAME", "");
            }

            [Command("getgameserver", "Get GameServer Info")]
            public void GetGameServer()
            {
                Context.Logger.LogInformation("# GetGameServer");
                foreach (var server in GameServer.Current)
                {
                    Context.Logger.LogInformation($"  Host:Port = {server.Host}:{server.Port}");
                    Context.Logger.LogInformation($"    {nameof(server.IsRunningOnKubernetes)} : {server.IsRunningOnKubernetes}");
                    Context.Logger.LogInformation($"    {nameof(server.State)} : {server.State}");
                }
            }

            [Command("allocategameserver", "Allocate Readied GameServer")]
            public async Task AllocateGameServer([Option("-f", "fleet name to allocate gameserver")]string fleetName = "")
            {
                Context.Logger.LogInformation("# AllocateGameServer");

                if (string.IsNullOrWhiteSpace(fleetName))
                {
                    if (!string.IsNullOrWhiteSpace(_defaultFleetName))
                    {
                        Context.Logger.LogDebug("fleetName arg was missing, using default fleet name retrieved from env DEFAULT_FLEET_NAME.");
                        fleetName = _defaultFleetName;
                    }
                    else
                    {
                        Context.Logger.LogWarning("fleetName arg was missing and default fleet name also missing. Please set env DEFAULT_FLEET_NAME to set default fleet name.");
                    }
                }

                var allocation = await GameServer.AllocateAsync(fleetName);
                Context.Logger.LogInformation($"  Host:Port = {nameof(allocation)} = {allocation}");
                Context.Logger.LogInformation($"    {nameof(allocation.IsAllocated)} = {allocation.IsAllocated}");
                Context.Logger.LogInformation($"    {nameof(allocation.Status)} = {allocation.Status}");
                Context.Logger.LogInformation($"    {nameof(allocation.Scheduling)} = {allocation.Scheduling}");
                Context.Logger.LogInformation($"    {nameof(allocation.GameServerName)} = {allocation.GameServerName}");
                Context.Logger.LogInformation($"    {nameof(allocation.Address)} = {allocation.Address}");
                Context.Logger.LogInformation($"    {nameof(allocation.Port)} = {allocation.Port}");
                Context.Logger.LogInformation($"    {nameof(allocation.NodeName)} = {allocation.NodeName}");
            }
        }
    }
}
