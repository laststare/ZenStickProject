using External.Reactive;

namespace Codebase.InterfaceAdapters.ScoreCounter
{
    public class ScoreCounterViewModel
    {
        public ReactiveTrigger spawnRewardView = new();
        public ReactiveEvent<string, string> showScore = new();
        public ReactiveTrigger showStartMenu  { get; set; }
    }
}