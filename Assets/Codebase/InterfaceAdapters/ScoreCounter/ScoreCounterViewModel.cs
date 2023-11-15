using External.Reactive;

namespace Codebase.InterfaceAdapters.ScoreCounter
{
    public class ScoreCounterViewModel
    {
        public readonly ReactiveTrigger SpawnRewardView = new();
        public readonly ReactiveEvent<string, string> ShowScore = new();
    }
}