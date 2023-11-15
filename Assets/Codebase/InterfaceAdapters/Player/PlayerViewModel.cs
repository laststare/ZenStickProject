using External.Reactive;

namespace Codebase.InterfaceAdapters.Player
{
    public class PlayerViewModel
    {
        public readonly ReactiveEvent<float> MovePlayerTo = new();
        public readonly ReactiveTrigger PlayerFinishMoving  = new();
    }
}