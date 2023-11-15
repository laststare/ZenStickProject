using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Player
{
    public interface IPlayer
    {
        public ReactiveEvent<float> movePlayerTo { get; set; }
        public ReactiveTrigger playerFinishMoving { get; set; }
        public ReactiveProperty<bool> columnIsReachable  { get; set; }
    }
}