using External.Reactive;

namespace Codebase.InterfaceAdapters.Stick
{
    public class StickViewModel
    {
        public readonly ReactiveTrigger StickIsDown = new();
        public readonly ReactiveTrigger StartStickGrow = new();
        public readonly ReactiveTrigger StartStickRotation = new();
    }
}