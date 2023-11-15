using Codebase.Utilities;
using Cysharp.Threading.Tasks;
using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.GameFlow
{
    public class GameFlowController : DisposableBase, IGameFlow
    {
        public ReactiveProperty<LevelFlowState> LevelFlowState { get; set; }
        public ReactiveEvent<LevelFlowState> ChangeLevelFlowState { get; set; }
        public ReactiveProperty<bool> ColumnIsReachable { get; set; }
        public ReactiveTrigger StartLevel { get; set; }
        public ReactiveTrigger StartGame { get; set; }
        public ReactiveTrigger FinishLevel { get; set; }
        public GameFlowController()
        {
            LevelFlowState = new ReactiveProperty<LevelFlowState>();
            ChangeLevelFlowState = new ReactiveEvent<LevelFlowState>();
            StartLevel = new ReactiveTrigger();
            StartGame = new ReactiveTrigger();
            FinishLevel = new ReactiveTrigger();
            ColumnIsReachable = new ReactiveProperty<bool>();
            
             StartLevel
                .Subscribe(() => LevelFlowState.Value = Codebase.LevelFlowState.PlayerIdle)
                .AddTo(_disposables);
            ChangeLevelFlowState.SubscribeWithSkip(x => LevelFlowState.Value = x)
                .AddTo(_disposables);
            StartTheGame();
        }
        
        private async void StartTheGame()
        {
            await UniTask.DelayFrame(1); 
            StartGame.Notify(); 
        }

       
    }
}