using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.GameFlow
{
    public interface IGameFlow
    {
        ReactiveProperty<LevelFlowState> LevelFlowState { get; set; }
        ReactiveEvent<LevelFlowState> ChangeLevelFlowState { get; set; }
        ReactiveProperty<bool> ColumnIsReachable { get; set; }
        ReactiveTrigger StartLevel { get; set; }
        ReactiveTrigger StartGame { get; set; }
        ReactiveTrigger FinishLevel { get; set; }
    }
}