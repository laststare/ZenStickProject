using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.MainMenu;
using Codebase.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Views
{
    public class MainMenuView : ViewBase
    {
        [SerializeField] private Button startGameBtn, restartGameBtn, backStartScreenBtn;
        [SerializeField] private GameObject gameTitle;
        
        public void Init(MainMenuViewModel mainMenuViewModel, IGameFlow iGameFlow)
        {
            iGameFlow.StartGame.Subscribe(() => startGameBtn.gameObject.SetActive(true)).AddTo(this);
            
            iGameFlow.StartLevel.Subscribe(() => { 
                HideStartScreen();
                HideFinishScreen();
            });
            
            iGameFlow.FinishLevel.Subscribe(ShowFinishScreen).AddTo(this);
            
            
            startGameBtn.onClick.AddListener(() =>
            {
                mainMenuViewModel.MenuButtonClicked.Notify(MainMenuButton.StartGame);
            });
            
            restartGameBtn.onClick.AddListener(() =>
            {
                mainMenuViewModel.MenuButtonClicked.Notify(MainMenuButton.RestartGame);
            });
            
            backStartScreenBtn.onClick.AddListener(() =>
            {
                mainMenuViewModel.MenuButtonClicked.Notify(MainMenuButton.BackToStartScreen);
                ShowStartScreen();
            });
        }
        
        private void ShowStartScreen()
        {
            startGameBtn.gameObject.SetActive(true);
            gameTitle.SetActive(true);
            HideFinishScreen();
        }

        private void ShowFinishScreen()
        {
            restartGameBtn.gameObject.SetActive(true); 
            backStartScreenBtn.gameObject.SetActive(true);
        }

        private void HideStartScreen()
        {
            gameTitle.SetActive(false);
            startGameBtn.gameObject.SetActive(false);
        }

        private void HideFinishScreen()
        {
            restartGameBtn.gameObject.SetActive(false); 
            backStartScreenBtn.gameObject.SetActive(false);
        }
    }
}