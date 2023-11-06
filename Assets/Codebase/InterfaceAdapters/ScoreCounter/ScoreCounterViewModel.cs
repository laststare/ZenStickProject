using Codebase.Data;
using Codebase.Views;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.ScoreCounter
{
    public class ScoreCounterViewModel
    {
        public ReactiveTrigger spawnRewardView = new();
        public ReactiveEvent<string, string> showScore = new();
    }
}