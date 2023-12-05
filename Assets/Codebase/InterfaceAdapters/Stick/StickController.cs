using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Stick
{
    public class StickController : DisposableBase, IStick
    {
        private readonly IContentProvider _contentProvider;
        private readonly IGameFlow _iGameFlow;
        private readonly ILevelBuilder _iLevelBuilder;
        private CompositeDisposable _clickHandlers;
        
        private readonly List<GameObject> _spawnedSticks = new List<GameObject>();
        public float StickLength { get; set; }

        protected StickController(IContentProvider contentProvider, IGameFlow iGameFlow, ILevelBuilder iLevelBuilder)
        {
            _contentProvider = contentProvider;
            _iGameFlow = iGameFlow;
            _iLevelBuilder = iLevelBuilder;
            
            _iGameFlow.LevelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerIdle) TmpClickDownSubscription();
            }).AddTo(_disposables);

            _iGameFlow.StartLevel.Subscribe(DestroySticks).AddTo(_disposables);
            
            _iGameFlow.ColumnIsReachable.Subscribe(x =>
            {
                if (x)
                    RemoveOneView();
            }).AddTo(_disposables);

        }

        private void CreateView()
        {
            var stick = Object.Instantiate(_contentProvider.Stick(),
                new Vector2(_iLevelBuilder.ActualColumnXPosition + 1, _contentProvider.LevelConfig().GetPlayerYPosition - 0.5f),
                Quaternion.identity);
            _spawnedSticks.Add(stick.gameObject);
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

        private async void GrowStickUp()
        {
            _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.StickGrowsUp);
            var stick = _spawnedSticks[^1];
            var stickHeight = 0f;
            var stickWidth = stick.transform.localScale.x;
            while (_iGameFlow.LevelFlowState.Value == LevelFlowState.StickGrowsUp)
            {
                stickHeight += Time.deltaTime * 6;
                stick.transform.localScale = new Vector3(stickWidth, stickHeight, 1);
                await UniTask.Yield();
            }
            StickLength = stickHeight;
        }
        private void RotateStick()
        {
            _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.StickFalls);
            var stick = _spawnedSticks[^1];
            stick.transform.DORotate(new Vector3(0, 0, -90f), 0.5f)
                .OnComplete(() => _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.PlayerRun));
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