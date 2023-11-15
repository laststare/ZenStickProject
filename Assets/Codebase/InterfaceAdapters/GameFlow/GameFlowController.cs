using Codebase.Utilities;
using Cysharp.Threading.Tasks;
using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.GameFlow
{
    public class GameFlowController : DisposableBase, IGameFlow
    {
        
        public ReactiveProperty<LevelFlowState> levelFlowState { get; set; }
        public ReactiveEvent<LevelFlowState> changeLevelFlowState { get; set; }
        public ReactiveTrigger startLevel { get; set; }
        public ReactiveTrigger startGame { get; set; }
        public ReactiveTrigger finishLevel { get; set; }
        public GameFlowController()
        {
            levelFlowState = new ReactiveProperty<LevelFlowState>();
            changeLevelFlowState = new ReactiveEvent<LevelFlowState>();
            startLevel = new ReactiveTrigger();
            startGame = new ReactiveTrigger();
            finishLevel = new ReactiveTrigger();
            
             startLevel
                .Subscribe(() => levelFlowState.Value = LevelFlowState.PlayerIdle)
                .AddTo(_disposables);
            changeLevelFlowState.SubscribeWithSkip(x => levelFlowState.Value = x)
                .AddTo(_disposables);
            StartGame();
        }
        
        private async void StartGame()
        {
            await UniTask.DelayFrame(1); 
            startGame.Notify(); 
        }

       
    }
}