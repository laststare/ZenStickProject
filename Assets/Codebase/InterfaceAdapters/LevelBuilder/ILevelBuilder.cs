using UniRx;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public interface ILevelBuilder
    {
        float actualColumnXPosition { get; set; }
        float  nextColumnXPosition { get; set; }
    }
}