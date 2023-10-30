using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Camera
{
    public class CameraViewModel
    {
        public ReactiveEvent<float> moveCameraToNextColumn { get; set; } = new();
        public ReactiveTrigger cameraFinishMoving { get; set; } = new();
    }
}