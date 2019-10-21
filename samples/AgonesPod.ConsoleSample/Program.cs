using System;

namespace AgonesPod.ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var server in GameServer.Current)
            {
                Console.WriteLine($"Host:Port = {server.Host}:{server.Port} ({server.IsRunningOnKubernetes})");
            }
        }
    }
}
