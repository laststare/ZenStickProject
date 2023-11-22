using Codebase.InterfaceAdapters.Camera;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.Player;
using Codebase.InterfaceAdapters.ScoreCounter;
using Codebase.InterfaceAdapters.Stick;
using Zenject;

namespace Codebase.Configuration.Installers
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        public override void InstallBindings()
        {
            InjectGameFlow();
            InjectCamera();
            InjectScoreCounter();
            InjectPlayer();
            InjectStick();
        }
        
        private void InjectGameFlow()
        {
            Container.BindInterfacesAndSelfTo<GameFlowController>()
                 .AsSingle()
                 .NonLazy();
        }

        private void InjectCamera()
        {
            Container.BindInterfacesAndSelfTo<CameraController>()
                .AsSingle()
                .NonLazy();
        }
        
        private void InjectScoreCounter()
        {
            Container.Bind<ScoreCounterViewModel>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<ScoreCounterController>()
                .AsSingle()
                .NonLazy();  
        }
        
        private void InjectPlayer()
        {
            Container.Bind<PlayerController>()
                .AsSingle()
                .NonLazy();
        }
        
        private void InjectStick()
        {
            Container.BindInterfacesAndSelfTo<StickController>()
                .AsSingle()
                .NonLazy();
        }

    }
}