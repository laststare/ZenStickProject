using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.GameFlow
{
    public interface IGameFlow
    {
        ReactiveProperty<LevelFlowState> levelFlowState { get; set; }
        ReactiveEvent<LevelFlowState> changeLevelFlowState { get; set; }
        ReactiveTrigger startLevel { get; set; }
        ReactiveTrigger startGame { get; set; }
        ReactiveTrigger finishLevel { get; set; }
    }
}