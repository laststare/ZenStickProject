using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Stick
{
    public interface IStick
    {
        public ReactiveProperty<float> stickLength { get; set; }
        public ReactiveTrigger stickIsDown { get; set; }
        public ReactiveTrigger startStickGrow { get; set; }
        public ReactiveTrigger startStickRotation { get; set; }
    }
}