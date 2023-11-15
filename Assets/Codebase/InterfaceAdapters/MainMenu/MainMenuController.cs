using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.Utilities;
using Codebase.Views;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.MainMenu
{
    public class MainMenuController : DisposableBase , IMainMenu
    {
        private readonly IContentProvider _contentProvider;
        private readonly Transform _uiRoot;
        private readonly MainMenuViewModel _mainMenuViewModel;
        private readonly IGameFlow _iGameFlow;
        private MainMenuView _view;
        public ReactiveTrigger ShowStartMenu { get; set; }

        protected MainMenuController(IContentProvider contentProvider, Transform uiRoot, IGameFlow iGameFlow, MainMenuViewModel mainMenuViewModel)
        {
            ShowStartMenu = new ReactiveTrigger();
            _contentProvider = contentProvider;
            _uiRoot = uiRoot;

            _mainMenuViewModel = mainMenuViewModel;

            _iGameFlow = iGameFlow;
            _mainMenuViewModel.MenuButtonClicked.SubscribeWithSkip(ButtonClickReceiver).AddTo(_disposables);
            CrateView();
        }
        
        private void CrateView()
        {
            _view = Object.Instantiate(_contentProvider.MainMenuView(), _uiRoot);
            _view.Init(_mainMenuViewModel, _iGameFlow);
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
                    ShowStartMenu.Notify();
                    break;
            }
        }

        private void StartLevel()
        {
            _iGameFlow.StartLevel.Notify();
        }

     
    }
}