using Codebase.Data;
using Codebase.Utilities;
using Codebase.Views;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public class MainMenuController : DisposableBase
    {
        private readonly ContentProvider _contentProvider;
        private readonly Transform _uiRoot;
        private readonly MainMenuViewModel _mainMenuViewModel;
        private MainMenuView _view;
        
        public MainMenuController(ContentProvider contentProvider, Transform uiRoot, MainMenuViewModel mainMenuViewModel)
        {
            _contentProvider = contentProvider;
            _uiRoot = uiRoot;
            _mainMenuViewModel = mainMenuViewModel;
            _mainMenuViewModel.menuButtonClicked.SubscribeWithSkip(ButtonClickReceiver).AddTo(_disposables);
            CrateView();
        }
        
        private void CrateView()
        {
            _view = Object.Instantiate(_contentProvider.UIViews.MainMenuView, _uiRoot);
            _view.Init(_mainMenuViewModel);
        }
        
        private void ButtonClickReceiver(MainMenuButton button)
        {
            switch (button)
            {
                case MainMenuButton.StartGame:
                    StartLevel();
                    break;
                case MainMenuButton.RestartGame:
                    StartLevel();
                    break;
                case MainMenuButton.BackToStartScreen:
                   // _ctx.showStartMenu.Notify();
                    break;
            }
        }

        private void StartLevel()
        {
            Debug.Log("level started");
           // _ctx.startLevel.Notify();
        }
    }
}