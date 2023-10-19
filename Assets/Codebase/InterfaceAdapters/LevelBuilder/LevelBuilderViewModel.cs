using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public class LevelBuilderViewModel
    {
        public ReactiveTrigger startLevel { get; } = new();
        public ReactiveProperty<float> actualColumnXPosition { get; set; } = new();
        public ReactiveProperty<float> nextColumnXPosition { get; set; } = new();
        public ReactiveProperty<LevelFlowState> levelFlowState { get; } = new();
        public ReactiveProperty<bool> columnIsReachable { get; } = new();
    }
}