using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.Camera
{
    public class CameraViewModel
    {
        public ReactiveProperty<LevelFlowState> levelFlowState  { get; } = new();
        public ReactiveProperty<float> actualColumnXPosition  { get; } = new();
        public ReactiveEvent<float> moveCameraToNextColumn { get; set; } = new();
        public ReactiveTrigger cameraFinishMoving { get; set; } = new();
        public ReactiveEvent<LevelFlowState> changeLevelFlowState { get; set; } = new();
        public ReactiveTrigger startLevel { get; } = new();
    }
}