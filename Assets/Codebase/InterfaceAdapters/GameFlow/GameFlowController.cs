using Codebase.Utilities;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Codebase.InterfaceAdapters.GameFlow
{
    public class GameFlowController : DisposableBase
    {
        private readonly GameFlowViewModel _gameFlowViewModel;
        
        public GameFlowController(GameFlowViewModel gameFlowViewModel )
        {
            _gameFlowViewModel = gameFlowViewModel;
            _gameFlowViewModel.startLevel
                .Subscribe(() => _gameFlowViewModel.levelFlowState.Value = LevelFlowState.PlayerIdle)
                .AddTo(_disposables);
            _gameFlowViewModel.changeLevelFlowState.SubscribeWithSkip(x => _gameFlowViewModel.levelFlowState.Value = x)
                .AddTo(_disposables);
            StartGame();
        }
        
        private async void StartGame()
        {
            await UniTask.DelayFrame(1); 
            _gameFlowViewModel.startGame.Notify(); 
        }
    }
}