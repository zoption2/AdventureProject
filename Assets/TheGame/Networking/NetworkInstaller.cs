using UnityEngine;
using Zenject;
using Mirror;

namespace TheGame
{
    public class NetworkInstaller : MonoInstaller
    {
        [SerializeField] private NetworkMatchService _matchService;
        [SerializeField] private CustomNetworkManager _networkManager;

        public override void InstallBindings()
        {
            BindServices();
        }

        private void BindServices()
        {
            Container.Bind<INetworkMatchService>().To<NetworkMatchService>().FromInstance(_matchService).AsSingle();
            
            Container.Bind<CustomNetworkManager>().To<CustomNetworkManager>().FromInstance(_networkManager).AsSingle();
        }
    }
}

