using Codebase.Data;
using Codebase.Views;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.ScoreCounter
{
    public class ScoreCounterViewModel
    {
        public ReactiveTrigger startGame { get; } = new();
        public ReactiveTrigger startLevel { get; } = new();
        public ReactiveTrigger finishLevel { get; } = new();
        public ReactiveTrigger showStartMenu { get; } = new();
        public ReactiveProperty<bool> columnIsReachable { get; set; } = new();
        public ReactiveProperty<float> nextColumnXPosition { get; set; } = new();
        public ReactiveTrigger spawnRewardView { get; set; } = new();
        public ReactiveEvent<string, string> showScore { get; set; } = new();
    }
}