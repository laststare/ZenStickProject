using Codebase.Data;
using Codebase.InterfaceAdapters.DataSave;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.MainMenu;
using Codebase.InterfaceAdapters.Player;
using UnityEngine;
using Zenject;

namespace Codebase.Configuration.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private Transform uiRoot;
        [Inject] private IContentProvider _contentProvider;
        public override void InstallBindings()
        {
            CreateUIRoot();
            InjectLevelBuilder();
            InjectMainMenu();
            InjectDataSave();
        }

        private void CreateUIRoot()
        {
            Container.Bind<Transform>()
                .FromInstance(uiRoot)
                .AsSingle();
        }
        
        private void InjectLevelBuilder()
        {

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
        
        private void InjectDataSave()
        {
            Container.BindInterfacesAndSelfTo<DataSaveController>()
                .AsSingle()
                .NonLazy();
        }
        
        
    }
}
