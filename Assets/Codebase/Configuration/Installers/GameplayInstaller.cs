using Codebase.Data;
using Codebase.InterfaceAdapters.Camera;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.Player;
using Codebase.InterfaceAdapters.ScoreCounter;
using Codebase.InterfaceAdapters.Stick;
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
            InjectGameFlow();
            InjectCamera();
            InjectScoreCounter();
            InjectPlayer();
            InjectStick();
        }
        
        private void InjectGameFlow()
        {
            Container.Bind<GameFlowViewModel>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<GameFlowController>()
                .AsSingle()
                .NonLazy();
        }

        private void InjectCamera()
        {
            Container.Bind<CameraViewModel>()
                .AsSingle();
            
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
            Container.Bind<PlayerViewModel>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerController>()
                .AsSingle()
                .NonLazy();
        }
        
        private void InjectStick()
        {
            Container.Bind<StickViewModel>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<StickController>()
                .AsSingle()
                .NonLazy();
        }

    }
}