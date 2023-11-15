using External.Reactive;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public interface IMainMenu
    {
        public ReactiveTrigger showStartMenu  { get; set; }
    }
}