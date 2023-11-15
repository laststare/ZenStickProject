using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.MainMenu;
using Codebase.InterfaceAdapters.ScoreCounter;
using Codebase.Utilities;
using TMPro;
using UniRx;
using UnityEngine;

namespace Codebase.Views
{
    public class ScoreCounterView : ViewBase
    {
        [SerializeField] private TMP_Text bestScoreText, actualScoreText;

        public void Init(ScoreCounterViewModel scoreCounterViewModel, IGameFlow iGameFlow, IMainMenu iMainMenu)
        {
            scoreCounterViewModel.ShowScore.SubscribeWithSkip(ShowScore).AddTo(this);
            iMainMenu.ShowStartMenu.Subscribe(() =>
            {
                HideAll();
                ShowStart();
            }).AddTo(this);
            iGameFlow.StartGame.Subscribe(ShowStart).AddTo(this);
            iGameFlow.StartLevel.Subscribe(HideAll).AddTo(this);
            iGameFlow.FinishLevel.Subscribe(ShowFinish).AddTo(this);

        }

        private void ShowScore(string best, string actual)
        {
            gameObject.SetActive(true);
            bestScoreText.text = best;
            actualScoreText.text = actual;
        }

        private void ShowStart()
        {
            gameObject.SetActive(true);
            bestScoreText.gameObject.SetActive(true);
        }

        private void ShowFinish()
        {
            ShowStart();
            actualScoreText.gameObject.SetActive(true);
        }

        private void HideAll()
        {
            gameObject.SetActive(false);
            bestScoreText.gameObject.SetActive(false);
            actualScoreText.gameObject.SetActive(false);
        }
    }
}