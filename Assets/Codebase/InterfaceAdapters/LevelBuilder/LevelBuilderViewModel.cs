using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public class LevelBuilderViewModel
    {
        public ReactiveProperty<float> actualColumnXPosition = new();
        public ReactiveProperty<float> nextColumnXPosition = new();
    }
}