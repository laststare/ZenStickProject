using Codebase.Data;
using Codebase.InterfaceAdapters.LevelBuilder;
using Zenject;

namespace Codebase.Configuration.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [Inject] private ContentProvider _contentProvider;
        public override void InstallBindings()
        {
            InjectLevelBuilder();
        }

        private void InjectLevelBuilder()
        {
            Container.Bind<LevelBuilderViewModel>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<LevelBuilderController>()
                .AsSingle()
                .NonLazy();
        }
    }
}
