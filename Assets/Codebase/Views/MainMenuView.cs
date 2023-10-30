using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.MainMenu;
using Codebase.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Codebase.Views
{
    public class MainMenuView : ViewBase
    {
        private MainMenuViewModel _mainMenuViewModel;
        private GameFlowViewModel _gameFlowViewModel;
        [SerializeField] private Button startGameBtn, restartGameBtn, backStartScreenBtn;
        
        public void Init(MainMenuViewModel mainMenuViewModel, GameFlowViewModel gameFlowViewModel)
        {
            _mainMenuViewModel = mainMenuViewModel;
            _gameFlowViewModel = gameFlowViewModel;
            _gameFlowViewModel.startGame.Subscribe(() => startGameBtn.gameObject.SetActive(true)).AddTo(this);
            
            _gameFlowViewModel.startLevel.Subscribe(() => { 
                HideStartScreen();
                HideFinishScreen();
            });
            
            _gameFlowViewModel.finishLevel.Subscribe(ShowFinishScreen).AddTo(this);
            
            
            startGameBtn.onClick.AddListener(() =>
            {
                _mainMenuViewModel.menuButtonClicked.Notify(MainMenuButton.StartGame);
            });
            
            restartGameBtn.onClick.AddListener(() =>
            {
                _mainMenuViewModel.menuButtonClicked.Notify(MainMenuButton.RestartGame);
            });
            
            backStartScreenBtn.onClick.AddListener(() =>
            {
                _mainMenuViewModel.menuButtonClicked.Notify(MainMenuButton.BackToStartScreen);
                ShowStartScreen();
            });
        }
        
        private void ShowStartScreen()
        {
            startGameBtn.gameObject.SetActive(true);
            HideFinishScreen();
        }

        private void ShowFinishScreen()
        {
            restartGameBtn.gameObject.SetActive(true); 
            backStartScreenBtn.gameObject.SetActive(true);
        }
        
        private void HideStartScreen() => startGameBtn.gameObject.SetActive(false);

        private void HideFinishScreen()
        {
            restartGameBtn.gameObject.SetActive(false); 
            backStartScreenBtn.gameObject.SetActive(false);
        }
    }
}