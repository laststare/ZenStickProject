using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Player
{
    public class PlayerViewModel
    {
        public ReactiveEvent<float> movePlayerTo { get; set; } = new();
        public ReactiveTrigger playerFinishMoving { get; } = new();
        public ReactiveProperty<bool> columnIsReachable { get; set; } = new();
    }
}