using Codebase.Data;
using Codebase.InterfaceAdapters.Camera;
using UnityEngine;
using Zenject;

namespace Codebase.Configuration.Installers
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        [SerializeField] private Transform uiRoot;
        [Inject] private ContentProvider _contentProvider;
        public override void InstallBindings()
        {
            InjectCamera();
        }
        
        private void InjectCamera()
        {
            Container.Bind<CameraViewModel>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<CameraController>()
                .AsSingle()
                .NonLazy();
        }
    }
}