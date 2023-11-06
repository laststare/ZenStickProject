using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.GameFlow
{
    public class GameFlowViewModel
    {
        public ReactiveProperty<LevelFlowState> levelFlowState = new();
        public ReactiveEvent<LevelFlowState> changeLevelFlowState = new();
        public ReactiveTrigger startLevel = new();
        public ReactiveTrigger startGame = new();
        public ReactiveTrigger finishLevel = new();
    }
}