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
        private ScoreCounterViewModel _scoreCounterViewModel;
        private IGameFlow _iGameFlow;
        [SerializeField] private TMP_Text bestScoreText, actualScoreText;

        public void Init(ScoreCounterViewModel scoreCounterViewModel)
        {
            _scoreCounterViewModel = scoreCounterViewModel;
            _scoreCounterViewModel.showScore.SubscribeWithSkip(ShowScore).AddTo(this);
            _scoreCounterViewModel.showStartMenu.Subscribe(() =>
            {
                HideAll();
                ShowStart();
            }).AddTo(this);
            _iGameFlow.startGame.Subscribe(ShowStart).AddTo(this);
            _iGameFlow.startLevel.Subscribe(HideAll).AddTo(this);
            _iGameFlow.finishLevel.Subscribe(ShowFinish).AddTo(this);

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