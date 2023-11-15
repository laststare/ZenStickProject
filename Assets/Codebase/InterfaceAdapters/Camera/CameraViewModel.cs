using External.Reactive;

namespace Codebase.InterfaceAdapters.Camera
{
    public class CameraViewModel
    {
        public readonly ReactiveEvent<float> MoveCameraToNextColumn = new();
        public readonly ReactiveTrigger CameraFinishMoving = new();
    }
}