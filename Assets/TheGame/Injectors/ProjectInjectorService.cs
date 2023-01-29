using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TheGame;
using TheGame.Data;

public class ProjectInjectorService : MonoInstaller
{
    [SerializeField] private PrefabsProvider _prefabsProvider;

    public override void InstallBindings()
    {
        Container.Bind<IPlayerService>().To<PlayerService>().AsSingle();
        Container.Bind<IDataService>().To<DataService>().AsSingle().NonLazy();
        Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
        Container.Bind<IMatchService>().To<MatchService>().AsSingle();
        Container.Bind<IPrefabsProvider>().To<PrefabsProvider>().FromInstance(_prefabsProvider).AsSingle();
    }
}
