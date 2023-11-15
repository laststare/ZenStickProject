﻿using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.Player;
using Codebase.Utilities;
using External.Reactive;
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
        private readonly IPlayer _iPlayer;
        private CompositeDisposable _clickHandlers;
        
        private readonly List<GameObject> _spawnedSticks = new List<GameObject>();
        public ReactiveProperty<float> stickLength { get; set; }
        public ReactiveTrigger stickIsDown { get; set; }
        public ReactiveTrigger startStickGrow { get; set; }
        public ReactiveTrigger startStickRotation { get; set; }

        protected StickController(IContentProvider contentProvider, StickViewModel stickViewModel, IGameFlow iGameFlow, 
            IPlayer iPlayer, ILevelBuilder iLevelBuilder)
        {

            stickLength = new ReactiveProperty<float>();
            stickIsDown = new ReactiveTrigger();
            startStickGrow = new ReactiveTrigger();
            startStickRotation = new ReactiveTrigger();
            
            _contentProvider = contentProvider;
            _stickViewModel = stickViewModel;
            _iGameFlow = iGameFlow;
            _iPlayer = iPlayer;
            _iLevelBuilder = iLevelBuilder;

            _stickViewModel.levelFlowState = _iGameFlow.levelFlowState;
            
            _iGameFlow.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerIdle) TmpClickDownSubscription();
            }).AddTo(_disposables);

            _iGameFlow.startLevel.Subscribe(DestroySticks).AddTo(_disposables);
            _stickViewModel.stickIsDown.Subscribe(
                () =>
                    _iGameFlow.changeLevelFlowState.Notify(LevelFlowState.PlayerRun))
                .AddTo(_disposables);
            
            _iPlayer.columnIsReachable.Subscribe(x =>
            {
                if (x)
                    RemoveOneView();
            }).AddTo(_disposables);
        }

        private void CreateView()
        {
            var view = Object.Instantiate(_contentProvider.StickView(),
                new Vector2(_iLevelBuilder.actualColumnXPosition + 1, Constant.PlayerYPosition - 0.5f),
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
            _iGameFlow.changeLevelFlowState.Notify(LevelFlowState.StickGrowsUp);
            _stickViewModel.startStickGrow.Notify();
        }
        
        private void RotateStick()
        {
            _iGameFlow.changeLevelFlowState.Notify(LevelFlowState.StickFalls);
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