using System.Collections.Generic;
using Codebase.Data;
using Codebase.Utilities;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Stick
{
    public class StickController : DisposableBase
    {
        private readonly StickViewModel _stickViewModel;
        private readonly ContentProvider _contentProvider;
        private CompositeDisposable _clickHandlers;
        private readonly List<GameObject> _spawnedSticks = new List<GameObject>();

        public StickController(ContentProvider contentProvider, StickViewModel stickViewModel)
        {
            _contentProvider = contentProvider;
            _stickViewModel = stickViewModel;
            _stickViewModel.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerIdle) TmpClickDownSubscription();
            }).AddTo(_disposables);

            _stickViewModel.startLevel.Subscribe(DestroySticks).AddTo(_disposables);
            _stickViewModel.stickIsDown.Subscribe(() =>
                _stickViewModel.changeLevelFlowState.Notify(LevelFlowState.PlayerRun)).AddTo(_disposables);
            _stickViewModel.columnIsReachable.Subscribe(x =>
            {
                if (x)
                    RemoveOneView();
            }).AddTo(_disposables);
            _stickViewModel.createView.Subscribe(CreateView).AddTo(_disposables);
        }

        private void CreateView()
        {
            var view = Object.Instantiate(_contentProvider.Views.StickView,
                new Vector2(_stickViewModel.actualColumnXPosition.Value + 1, Constant.PlayerYPosition - 0.5f),
                Quaternion.identity);
            view.Init(_stickViewModel);
            _spawnedSticks.Add(view.gameObject);
        }


        private void TmpClickDownSubscription()
        {
            _clickHandlers = new CompositeDisposable();
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0)).Subscribe(
                    (_) =>
                    {
                        _stickViewModel.createView.Notify();
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
            _stickViewModel.changeLevelFlowState.Notify(LevelFlowState.StickGrowsUp);
            _stickViewModel.startStickGrow.Notify();
        }
        
        private void RotateStick()
        {
            _stickViewModel.changeLevelFlowState.Notify(LevelFlowState.StickFalls);
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