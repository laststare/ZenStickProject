using Codebase.Data;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.MainMenu;
using UnityEngine;
using Zenject;

namespace Codebase.Configuration.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private Transform uiRoot;
        [Inject] private ContentProvider _contentProvider;
        public override void InstallBindings()
        {
            CreateUIRoot();
            InjectLevelBuilder();
            InjectMainMenu();
        }

        private void CreateUIRoot()
        {
            Container.Bind<Transform>()
                .FromInstance(uiRoot)
                .AsSingle();
        }
        
        private void InjectLevelBuilder()
        {
            Container.Bind<LevelBuilderViewModel>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<LevelBuilderController>()
                .AsSingle()
                .NonLazy();
        }

        private void InjectMainMenu()
        {
            Container.Bind<MainMenuViewModel>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<MainMenuController>()
                .AsSingle()
                .NonLazy();
        }
        
    }
}
