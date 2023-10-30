using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.GameFlow
{
    public class GameFlowViewModel
    {
        public ReactiveProperty<LevelFlowState> levelFlowState { get; set; } = new();
        public ReactiveEvent<LevelFlowState> changeLevelFlowState { get; } = new();
        public ReactiveTrigger startLevel { get; set; } = new();
        public ReactiveTrigger startGame { get; set; } = new();
        public ReactiveTrigger finishLevel { get; set; } = new();
    }
}