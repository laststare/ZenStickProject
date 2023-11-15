using External.Reactive;
using UniRx;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public class MainMenuViewModel
    {
        public ReactiveEvent<MainMenuButton> menuButtonClicked = new ();
        public ReactiveTrigger finishLevel { get; set; }
        public ReactiveTrigger startLevel { get; set; }
        public ReactiveTrigger startGame { get; set; }

    }
    
}