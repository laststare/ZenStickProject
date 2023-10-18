using UniRx;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public class LevelBuilderViewModel
    {
       // public ContentProvider contentProvider;
        public ReactiveCommand startLevel { get; } = new();
        public ReactiveProperty<float> actualColumnXPosition { get; set; } = new();
        public ReactiveProperty<float> nextColumnXPosition { get; set; } = new();
        public ReactiveProperty<LevelFlowState> levelFlowState { get; } = new();
        public ReactiveProperty<bool> columnIsReachable { get; } = new();
    }
}