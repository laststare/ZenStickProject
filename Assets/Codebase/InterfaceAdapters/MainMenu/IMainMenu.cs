using External.Reactive;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public interface IMainMenu
    {
        public ReactiveTrigger ShowStartMenu  { get; set; }
    }
}