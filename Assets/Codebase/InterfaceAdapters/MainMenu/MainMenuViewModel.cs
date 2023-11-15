using External.Reactive;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public class MainMenuViewModel
    {
        public readonly ReactiveEvent<MainMenuButton> MenuButtonClicked = new ();

    }
    
}