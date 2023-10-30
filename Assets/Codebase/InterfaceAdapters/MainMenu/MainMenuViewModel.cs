using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public class MainMenuViewModel
    {
        public ReactiveEvent<MainMenuButton> menuButtonClicked = new ();
        public ReactiveTrigger showStartMenu = new();
    }
    
}