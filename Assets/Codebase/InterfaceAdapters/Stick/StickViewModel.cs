using Codebase.Data;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Stick
{
    public class StickViewModel
    {
        public ReactiveProperty<float> actualColumnXPosition { get; } = new();
        public ReactiveProperty<LevelFlowState> levelFlowState { get; } = new();
        public ReactiveProperty<float> stickLength { get; set; } = new();
        public ReactiveTrigger startLevel { get; } = new();
        public ReactiveProperty<bool> columnIsReachable { get; } = new();
        public ReactiveEvent<LevelFlowState> changeLevelFlowState { get; set; } = new();
        public ReactiveTrigger stickIsDown { get; set; } = new();
        public ReactiveTrigger startStickGrow { get; } = new();
        public ReactiveTrigger startStickRotation { get; } = new();
        public ReactiveTrigger createView { get; set; } = new();
    }
}