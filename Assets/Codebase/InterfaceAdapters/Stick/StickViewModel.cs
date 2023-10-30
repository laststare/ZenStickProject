using Codebase.Data;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Stick
{
    public class StickViewModel
    {
        public ReactiveProperty<float> stickLength { get; set; } = new();
        public ReactiveTrigger stickIsDown { get; set; } = new();
        public ReactiveTrigger startStickGrow { get; } = new();
        public ReactiveTrigger startStickRotation { get; } = new();
    }
}