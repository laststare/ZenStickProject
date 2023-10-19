using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public class MainMenuViewModel
    {
        public ReactiveEvent<MainMenuButton> menuButtonClicked = new ();
        public ReactiveTrigger startGame = new ();
        public ReactiveTrigger finishLevel = new();
        public ReactiveTrigger startLevel = new();
    }
    
}