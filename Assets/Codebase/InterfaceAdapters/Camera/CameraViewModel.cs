using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Camera
{
    public class CameraViewModel
    {
        public ReactiveEvent<float> moveCameraToNextColumn = new();
        public ReactiveTrigger cameraFinishMoving = new();
    }
}