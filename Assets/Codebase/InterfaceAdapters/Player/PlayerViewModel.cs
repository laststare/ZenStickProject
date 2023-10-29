using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Player
{
    public class PlayerViewModel
    {
        public ReactiveProperty<float> actualColumnXPosition { get; } = new();
        public ReactiveProperty<float> nextColumnXPosition { get; } = new();
        public ReactiveEvent<float> movePlayerTo { get; set; } = new();
        public ReactiveProperty<LevelFlowState> levelFlowState { get; } = new();
        public ReactiveProperty<float> stickLength { get; } = new();
        public ReactiveTrigger playerFinishMoving { get; } = new();
        public ReactiveTrigger finishLevel { get; set; } = new();
        public ReactiveProperty<bool> columnIsReachable { get; set; } = new();
        public ReactiveEvent<LevelFlowState> changeLevelFlowState { get; set; } = new();
        public ReactiveTrigger startLevel  { get; } = new();
    }
}