using System;
using System.Threading.Tasks;

namespace AgonesPod.ConsoleSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("# GetGameServer");
            foreach (var server in GameServer.Current)
            {
                Console.WriteLine($"  Host:Port = {server.Host}:{server.Port}");
                Console.WriteLine($"    {nameof(server.IsRunningOnKubernetes)} : {server.IsRunningOnKubernetes}");
                Console.WriteLine($"    {nameof(server.State)} : {server.State}");
            }

            Console.WriteLine("# AllocateGameServer");
            var allocation = await GameServer.AllocateAsync("magiconion-chatserver");
            Console.WriteLine($"  Host:Port = {nameof(allocation)} = {allocation}");
            Console.WriteLine($"    {nameof(allocation.IsAllocated)} = {allocation.IsAllocated}");
            Console.WriteLine($"    {nameof(allocation.Status)} = {allocation.Status}");
            Console.WriteLine($"    {nameof(allocation.Scheduling)} = {allocation.Scheduling}");
            Console.WriteLine($"    {nameof(allocation.GameServerName)} = {allocation.GameServerName}");
            Console.WriteLine($"    {nameof(allocation.Address)} = {allocation.Address}");
            Console.WriteLine($"    {nameof(allocation.Port)} = {allocation.Port}");
            Console.WriteLine($"    {nameof(allocation.NodeName)} = {allocation.NodeName}");
        }
    }
}
