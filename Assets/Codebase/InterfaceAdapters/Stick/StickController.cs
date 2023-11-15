using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.Utilities;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Stick
{
    public class StickController : DisposableBase, IStick
    {
        private readonly StickViewModel _stickViewModel;
        private readonly IContentProvider _contentProvider;
        private readonly IGameFlow _iGameFlow;
        private readonly ILevelBuilder _iLevelBuilder;
        private CompositeDisposable _clickHandlers;
        
        private readonly List<GameObject> _spawnedSticks = new List<GameObject>();
        public ReactiveProperty<float> StickLength { get; set; }

        protected StickController(IContentProvider contentProvider, StickViewModel stickViewModel, 
            IGameFlow iGameFlow, ILevelBuilder iLevelBuilder)
        {
            StickLength = new ReactiveProperty<float>();
            _contentProvider = contentProvider;
            _stickViewModel = stickViewModel;
            _iGameFlow = iGameFlow;
            _iLevelBuilder = iLevelBuilder;
            
            _iGameFlow.LevelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerIdle) TmpClickDownSubscription();
            }).AddTo(_disposables);

            _iGameFlow.StartLevel.Subscribe(DestroySticks).AddTo(_disposables);
            
            _stickViewModel.StickIsDown.Subscribe(
                () =>
                    _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.PlayerRun))
                .AddTo(_disposables);
            
            _iGameFlow.ColumnIsReachable.Subscribe(x =>
            {
                if (x)
                    RemoveOneView();
            }).AddTo(_disposables);

        }

        private void CreateView()
        {
            var view = Object.Instantiate(_contentProvider.StickView(),
                new Vector2(_iLevelBuilder.ActualColumnXPosition + 1, Constant.PlayerYPosition - 0.5f),
                Quaternion.identity);
            view.Init(_stickViewModel, _iGameFlow, this);
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
            _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.StickGrowsUp);
            _stickViewModel.StartStickGrow.Notify();
        }
        private void RotateStick()
        {
            _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.StickFalls);
            _stickViewModel.StartStickRotation.Notify();
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