using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TheGame;
using TheGame.Data;
using TheGame.Massager;

public class ProjectInjectorService : MonoInstaller
{
    [SerializeField] private PrefabsProvider _prefabsProvider;
    [SerializeField] private InternalDatabase _baseDataProvider;

    public override void InstallBindings()
    {
        Container.Bind<IPlayerService>().To<PlayerService>().AsSingle();
        Container.Bind<IDataService>().To<DataService>().AsSingle().NonLazy();
        Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
        Container.Bind<IMatchService>().To<MatchService>().AsSingle();
        Container.Bind<IMassageService>().To<MassagesService>().AsSingle();

        Container.Bind<IPrefabsProvider>().To<PrefabsProvider>().FromInstance(_prefabsProvider).AsSingle();
        Container.Bind<IDatabase>().To<InternalDatabase>().FromInstance(_baseDataProvider).AsSingle();

        BindDataServiceComponents();
    }

    private void BindDataServiceComponents()
    {
        Container.Bind<CharacterDataMediator>().To<CharacterDataMediator>().AsSingle();

        Container.Bind<UserDataMediator>().To<UserDataMediator>().AsSingle();

    }
}
