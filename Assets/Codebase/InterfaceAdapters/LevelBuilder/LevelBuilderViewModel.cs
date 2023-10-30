using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public class LevelBuilderViewModel
    {
        public ReactiveProperty<float> actualColumnXPosition { get; set; } = new();
        public ReactiveProperty<float> nextColumnXPosition { get; set; } = new();
    }
}