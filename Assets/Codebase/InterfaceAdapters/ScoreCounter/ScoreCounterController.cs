using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.DataSave;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.MainMenu;
using Codebase.Utilities;
using Codebase.Views;
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
        private readonly List<RewardView> _spawnedRewardViews = new();

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
            _iGameFlow.StartGame.Subscribe(GetSavedScore).AddTo(_disposables);
            
            _iGameFlow.FinishLevel.Subscribe(() =>
            {
                UpdateBestScore();
                SendScoreToView();
                ClearScore();
                DestroyRewardView();
            }).AddTo(_disposables);
            
            _iGameFlow.ColumnIsReachable.Subscribe(x =>
            {
                if (!x) return;
                UpdateScore();
                RemoveOneView();
            }).AddTo(_disposables);
            _scoreCounterViewModel.SpawnRewardView.Subscribe(CreateRewardView).AddTo(_disposables);
            
            CreateView();
        }

        private void CreateView()
        {
            _view = Object.Instantiate(_contentProvider.ScoreCounterView(), _uiRoot);
            _view.Init(_scoreCounterViewModel, _iGameFlow, _iMainMenu);
        }
        
        private void CreateRewardView()
        {
            var rewardView = Object.Instantiate(_contentProvider.RewardView(),
                new Vector3(_iLevelBuilder.NextColumnXPosition, Constant.PlayerYPosition, 0), Quaternion.identity);
            _spawnedRewardViews.Add(rewardView);
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
        }

        private void UpdateBestScore()
        {
            if (_currentScore <= _bestScore)
                return;
            _bestScore = _currentScore;
            _dataSave.SaveBestScore(_bestScore);
        }

        private void DestroyRewardView()
        {
            foreach (var view in _spawnedRewardViews) 
                Object.Destroy(view.gameObject);
            _spawnedRewardViews.Clear();
        }

        private void ClearScore() => _currentScore = 0;

        private void RemoveOneView()
        {
            if (_spawnedRewardViews.Count <= 2) return;
            Object.Destroy(_spawnedRewardViews[0].gameObject);
            _spawnedRewardViews.RemoveAt(0);
        }
   
    }
}