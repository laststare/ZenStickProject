using Codebase.Data;
using Codebase.InterfaceAdapters.DataSave;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.MainMenu;
using Codebase.Utilities;
using Codebase.Views;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.ScoreCounter
{
    public class ScoreCounterController : DisposableBase
    {
        private readonly IContentProvider _contentProvider;
        private readonly IDataSave _dataSave;
        private readonly IGameFlow _iGameFlow;
        private readonly ILevelBuilder _iLevelBuilder;
        private readonly IMainMenu _iMainMenu;
        private readonly Transform _uiRoot;
        private readonly ScoreCounterViewModel _scoreCounterViewModel;
        
        private ScoreCounterView _view;
        private int _currentScore, _bestScore;
        private readonly float _playerYPosition;

        public ScoreCounterController(IContentProvider contentProvider, IDataSave dataSave, Transform uiRoot, ScoreCounterViewModel scoreCounterViewModel, 
            IGameFlow iGameFlow, ILevelBuilder iLevelBuilder, IMainMenu iMainMenu)
        {
            _contentProvider = contentProvider;
            _dataSave = dataSave;
            _uiRoot = uiRoot;
            _scoreCounterViewModel = scoreCounterViewModel;
            _iGameFlow = iGameFlow;
            _iLevelBuilder = iLevelBuilder;
            _iMainMenu = iMainMenu;
            _playerYPosition = _contentProvider.LevelConfig().GetPlayerYPosition;
            
            _iGameFlow.StartGame.Subscribe(GetSavedScore).AddTo(_disposables);
            
            _iGameFlow.FinishLevel.Subscribe(() =>
            {
                UpdateBestScore();
                SendScoreToView();
                ClearScore();
            }).AddTo(_disposables);
            
            _iGameFlow.ColumnIsReachable.Subscribe(x =>
            {
                if (!x) return;
                UpdateScore();
            }).AddTo(_disposables);
            CreateView();
        }

        private void CreateView()
        {
            _view = Object.Instantiate(_contentProvider.ScoreCounterView(), _uiRoot);
            _view.Init(_scoreCounterViewModel, _iGameFlow, _iMainMenu);
        }
        private void GetSavedScore()
        {
            _bestScore = _dataSave.LoadBestScore();
            SendScoreToView();
        }
        private void SendScoreToView()
        {
            var bestText = $"Best score: {_bestScore}";
            var actualText =  $"Your score: {_currentScore}";
            _scoreCounterViewModel.ShowScore.Notify(bestText, actualText);
        }
        private void UpdateScore()
        {
            _currentScore += _contentProvider.RewardConfig().OneColumnReward;
            _scoreCounterViewModel.SpawnRewardView.Notify();
            SpawnRewardItem();
        }
        private void SpawnRewardItem()
        {
            var reward = Object.Instantiate(_contentProvider.Reward(),
                new Vector3(_iLevelBuilder.NextColumnXPosition, _playerYPosition, 0), Quaternion.identity);
            reward.DOMoveY(reward.position.y + 3, 2);
            var rewardText = reward.transform.GetChild(0).GetComponent<TMP_Text>();
            rewardText.DOFade(0, 2).OnComplete(() => { Object.Destroy(reward.gameObject);});
        }
        private void UpdateBestScore()
        {
            if (_currentScore <= _bestScore)
                return;
            _bestScore = _currentScore;
            _dataSave.SaveBestScore(_bestScore);
        }
        private void ClearScore() => _currentScore = 0;

    }
}