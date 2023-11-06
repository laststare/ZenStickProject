using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.Utilities;
using Codebase.Views;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public class MainMenuController : DisposableBase
    {
        private readonly IContentProvider _contentProvider;
        private readonly Transform _uiRoot;
        private readonly MainMenuViewModel _mainMenuViewModel;
        private readonly GameFlowViewModel _gameFlowViewModel;
        private MainMenuView _view;
        
        public MainMenuController(IContentProvider contentProvider, Transform uiRoot, MainMenuViewModel mainMenuViewModel, GameFlowViewModel gameFlowViewModel)
        {
            _contentProvider = contentProvider;
            _uiRoot = uiRoot;
            _mainMenuViewModel = mainMenuViewModel;
            _gameFlowViewModel = gameFlowViewModel;
            _mainMenuViewModel.menuButtonClicked.SubscribeWithSkip(ButtonClickReceiver).AddTo(_disposables);
            CrateView();
        }
        
        private void CrateView()
        {
            _view = Object.Instantiate(_contentProvider.MainMenuView(), _uiRoot);
            _view.Init(_mainMenuViewModel, _gameFlowViewModel);
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
                    _mainMenuViewModel.showStartMenu.Notify();
                    break;
            }
        }

        private void StartLevel()
        {
            Debug.Log("level started");
            _gameFlowViewModel.startLevel.Notify();
        }
    }
}