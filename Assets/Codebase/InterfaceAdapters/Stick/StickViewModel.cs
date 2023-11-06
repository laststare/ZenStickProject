using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Stick
{
    public class StickViewModel
    {
        public ReactiveProperty<float> stickLength = new();
        public ReactiveTrigger stickIsDown = new();
        public ReactiveTrigger startStickGrow = new();
        public ReactiveTrigger startStickRotation = new();
    }
}