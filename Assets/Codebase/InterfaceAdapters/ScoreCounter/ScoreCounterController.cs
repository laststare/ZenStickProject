using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.MainMenu;
using Codebase.InterfaceAdapters.Player;
using Codebase.Utilities;
using Codebase.Views;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.ScoreCounter
{
    public class ScoreCounterController : DisposableBase
    {
        private readonly ContentProvider _contentProvider;
        private readonly Transform _uiRoot;
        private readonly ScoreCounterViewModel _scoreCounterViewModel;
        private readonly GameFlowViewModel _gameFlowViewModel;
        private readonly PlayerViewModel _playerViewModel;
        private readonly LevelBuilderViewModel _levelBuilderViewModel;
        private readonly MainMenuViewModel _mainMenuViewModel;
        private ScoreCounterView _view;
        private int _currentScore, _bestScore;
        private readonly List<RewardView> _spawnedRewardViews = new();

        public ScoreCounterController(ContentProvider contentProvider, Transform uiRoot, ScoreCounterViewModel scoreCounterViewModel, 
            GameFlowViewModel gameFlowViewModel, PlayerViewModel playerViewModel, LevelBuilderViewModel levelBuilderViewModel, MainMenuViewModel mainMenuViewModel)
        {
            _contentProvider = contentProvider;
            _uiRoot = uiRoot;
            _scoreCounterViewModel = scoreCounterViewModel;
            _gameFlowViewModel = gameFlowViewModel;
            _playerViewModel = playerViewModel;
            _levelBuilderViewModel = levelBuilderViewModel;
            _mainMenuViewModel = mainMenuViewModel;
            _gameFlowViewModel.startGame.Subscribe(GetSavedScore).AddTo(_disposables);
            _gameFlowViewModel.finishLevel.Subscribe(() =>
            {
                UpdateBestScore();
                SendScoreToView();
                ClearScore();
                DestroyRewardView();
            }).AddTo(_disposables);
            
            _playerViewModel.columnIsReachable.Subscribe(x =>
            {
                if (!x) return;
                UpdateScore();
                RemoveOneView();
            }).AddTo(_disposables);
            _scoreCounterViewModel.spawnRewardView.Subscribe(CreateRewardView).AddTo(_disposables);
            
            CreateView();
        }

        private void CreateView()
        {
            _view = Object.Instantiate(_contentProvider.UIViews.ScoreCounterView, _uiRoot);
            _view.Init(_scoreCounterViewModel, _gameFlowViewModel, _mainMenuViewModel);
        }
        
        private void CreateRewardView()
        {
            var rewardView = Object.Instantiate(_contentProvider.Views.RewardView,
                new Vector3(_levelBuilderViewModel.nextColumnXPosition.Value, Constant.PlayerYPosition, 0), Quaternion.identity);
            _spawnedRewardViews.Add(rewardView);
        }
        

        private void GetSavedScore()
        {
            _bestScore = PlayerPrefs.GetInt(Constant.SavedScore);
            SendScoreToView();
        }

        private void SendScoreToView()
        {
            var bestText = $"Best score: {_bestScore}";
            var actualText =  $"Your score: {_currentScore}";
            _scoreCounterViewModel.showScore.Notify(bestText, actualText);
        }

        private void UpdateScore()
        {
            _currentScore += _contentProvider.Settings.RewardConfig.OneColumnReward;
            _scoreCounterViewModel.spawnRewardView.Notify();
        }

        private void UpdateBestScore()
        {
            if (_currentScore <= _bestScore)
                return;
            _bestScore = _currentScore;
            PlayerPrefs.SetInt(Constant.SavedScore, _bestScore);
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