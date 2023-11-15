using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Player
{
    public class PlayerViewModel
    {
        public ReactiveEvent<float> movePlayerTo = new();
        public ReactiveTrigger playerFinishMoving  = new();
        public ReactiveProperty<bool> columnIsReachable  = new();
        public ReactiveTrigger startLevel { get; set; }


    }
}