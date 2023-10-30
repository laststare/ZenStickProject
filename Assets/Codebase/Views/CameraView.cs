using Codebase.InterfaceAdapters.Camera;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.Utilities;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Codebase.Views
{
    public class CameraView : ViewBase
    {
        private CameraViewModel _cameraViewModel;
        private GameFlowViewModel _gameFlowViewModel;

        
        public void Init(CameraViewModel cameraViewModel, GameFlowViewModel gameFlowViewModel)
        {
            _cameraViewModel = cameraViewModel;
            _gameFlowViewModel = gameFlowViewModel;
            
            _cameraViewModel.moveCameraToNextColumn.SubscribeWithSkip(x => 
                transform.DOMoveX(x, 1).OnComplete(() =>
            {
                _cameraViewModel.cameraFinishMoving.Notify();
            })).AddTo(this);

            _gameFlowViewModel.startLevel.Subscribe(() =>
            {
                transform.position = new Vector3(Constant.CameraOnColumnXOffset, transform.position.y,
                    transform.position.z);
            });
        }
    }
}