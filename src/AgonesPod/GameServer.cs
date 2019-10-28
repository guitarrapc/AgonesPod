using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AgonesPod
{
    public class GameServer
    {
        public void Read(string json)
        {

        }
        // lock
        static readonly object _syncObject = new object();

        static IKubernetesServiceProvider _serviceProvider = GetDefaultProvider();
        static IGameServerInfo[] _current;

        public static IGameServerInfo[] Current
        {
            get
            {
                if (_current == null)
                {
                    Initialize();
                }
                return _current;
            }
        }

        public static void Initialize(bool throwOnFail = false)
        {
            lock (_syncObject)
            {
                if (_current != null) return;

                if (!_serviceProvider.IsRunningOnKubernetes)
                {
                    Debug.WriteLine("current environment is not k8s.");
                    _current = new[] { new PseudoGameServerInfo() };
                    return;
                }

                try
                {
                    Debug.WriteLine("Try Get GameServersAsync.");
                    _current = _serviceProvider.GetGameServersAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to get gameserver. {ex.Message} {ex.StackTrace} \nInner: {ex.InnerException}");
                    if (throwOnFail)
                    {
                        throw;
                    }
                    else
                    {
                        _current = new[] { new PseudoGameServerInfo() };
                    }
                }
            }
        }

        public static async ValueTask<IGameServerAllocationInfo> AllocateAsync(string fleetName)
        {
            return await _serviceProvider.AllocateGameServersAsync(fleetName);
        }

        public static void RegisterServiceProvider(IKubernetesServiceProvider provider)
            => _serviceProvider = provider ?? throw new ArgumentNullException(nameof(provider));

        private static IKubernetesServiceProvider GetDefaultProvider()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix
                ? (IKubernetesServiceProvider)new UnixKubernetesServiceProvider()
                : (IKubernetesServiceProvider)new WindowsKubernetesServiceProvider();
        }
    }
}
