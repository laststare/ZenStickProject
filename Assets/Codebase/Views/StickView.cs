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
        private IGameFlow _iGameFlow;
        private CompositeDisposable _disposables;
        private IStick _iStick;

        public void Init(StickViewModel stickViewModel, IGameFlow iGameFlow, IStick iStick)
        {
            _stickViewModel = stickViewModel;
            _iGameFlow = iGameFlow;
            _iStick = iStick;
            _disposables = new CompositeDisposable();
            _stickViewModel.StartStickGrow.Subscribe(GrowStickUp).AddTo(_disposables);
            _stickViewModel.StartStickRotation.Subscribe(RotateStick).AddTo(_disposables);
        }   
        
        private async void GrowStickUp()
        {
            var stickHeight = 0f;
            var stickWidth = transform.localScale.x;
            while (_iGameFlow.LevelFlowState.Value == LevelFlowState.StickGrowsUp)
            {
                stickHeight += Time.deltaTime * 6;
                transform.localScale = new Vector3(stickWidth, stickHeight, 1);
                await UniTask.Yield();
            }
            _iStick.StickLength.Value = stickHeight;
        }
        
        private void RotateStick()
        {
            transform.DORotate(new Vector3(0, 0, -90f), 0.5f)
                .OnComplete(() => _stickViewModel.StickIsDown.Notify());
            _disposables.Dispose();
        }
        
    }
}