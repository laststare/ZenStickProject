using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.Stick;
using Codebase.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Codebase.Views
{
    public class StickView : ViewBase
    {
        private StickViewModel _stickViewModel;
        private CompositeDisposable _disposables;

        public void Init(StickViewModel stickViewModel)
        {
            _stickViewModel = stickViewModel;
            _disposables = new CompositeDisposable();
            _stickViewModel.startStickGrow.Subscribe(GrowStickUp).AddTo(_disposables);
            _stickViewModel.startStickRotation.Subscribe(RotateStick).AddTo(_disposables);
        }   
        
        private async void GrowStickUp()
        {
            var stickHeight = 0f;
            var stickWidth = transform.localScale.x;
            while (_stickViewModel.levelFlowState.Value == LevelFlowState.StickGrowsUp)
            {
                stickHeight += Time.deltaTime * 6;
                transform.localScale = new Vector3(stickWidth, stickHeight, 1);
                await UniTask.Yield();
            }
            _stickViewModel.stickLength.Value = stickHeight;
        }
        
        private void RotateStick()
        {
            transform.DORotate(new Vector3(0, 0, -90f), 0.5f)
                .OnComplete(() => _stickViewModel.stickIsDown.Notify());
            _disposables.Dispose();
        }
        
    }
}