using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.Player;
using Codebase.Utilities;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Stick
{
    public class StickController : DisposableBase
    {
        private readonly StickViewModel _stickViewModel;
        private readonly IContentProvider _contentProvider;
        private readonly GameFlowViewModel _gameFlowViewModel;
        private readonly PlayerViewModel _playerViewModel;
        private readonly LevelBuilderViewModel _levelBuilderViewModel;
        private CompositeDisposable _clickHandlers;
        private readonly List<GameObject> _spawnedSticks = new List<GameObject>();

        public StickController(IContentProvider contentProvider, StickViewModel stickViewModel, GameFlowViewModel gameFlowViewModel, 
            PlayerViewModel playerViewModel, LevelBuilderViewModel levelBuilderViewModel)
        {
            _contentProvider = contentProvider;
            _stickViewModel = stickViewModel;
            _gameFlowViewModel = gameFlowViewModel;
            _playerViewModel = playerViewModel;
            _levelBuilderViewModel = levelBuilderViewModel;
            _gameFlowViewModel.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerIdle) TmpClickDownSubscription();
            }).AddTo(_disposables);

            _gameFlowViewModel.startLevel.Subscribe(DestroySticks).AddTo(_disposables);
            _stickViewModel.stickIsDown.Subscribe(
                () =>
                _gameFlowViewModel.changeLevelFlowState.Notify(LevelFlowState.PlayerRun))
                .AddTo(_disposables);
            _playerViewModel.columnIsReachable.Subscribe(x =>
            {
                if (x)
                    RemoveOneView();
            }).AddTo(_disposables);
        }

        private void CreateView()
        {
            var view = Object.Instantiate(_contentProvider.StickView(),
                new Vector2(_levelBuilderViewModel.actualColumnXPosition.Value + 1, Constant.PlayerYPosition - 0.5f),
                Quaternion.identity);
            view.Init(_stickViewModel, _gameFlowViewModel);
            _spawnedSticks.Add(view.gameObject);
        }


        private void TmpClickDownSubscription()
        {
            _clickHandlers = new CompositeDisposable();
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0)).Subscribe(
                    (_) =>
                    {
                        CreateView();
                        GrowStickUp();
                        TmpClickUpSubscription();
                    }).AddTo(_clickHandlers);
        }

        private void TmpClickUpSubscription()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0)).Subscribe(
                    (_) =>
                    {
                        RotateStick();
                        _clickHandlers.Dispose();
                    })
                .AddTo(_clickHandlers); 
        }

        private void GrowStickUp()
        {
            _gameFlowViewModel.changeLevelFlowState.Notify(LevelFlowState.StickGrowsUp);
            _stickViewModel.startStickGrow.Notify();
        }
        
        private void RotateStick()
        {
            _gameFlowViewModel.changeLevelFlowState.Notify(LevelFlowState.StickFalls);
            _stickViewModel.startStickRotation.Notify();
        }
        
        private void DestroySticks()
        {
            foreach (var stick in _spawnedSticks) 
                Object.Destroy(stick);
            _spawnedSticks.Clear();
        }
        private void RemoveOneView()
        {
            if (_spawnedSticks.Count <= 2) return;
            Object.Destroy(_spawnedSticks[0].gameObject);
            _spawnedSticks.RemoveAt(0);
        }
    }
}