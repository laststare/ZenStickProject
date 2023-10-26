using Codebase.InterfaceAdapters.Camera;
using Codebase.Utilities;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Codebase.Views
{
    public class CameraView : ViewBase
    {
        private CameraViewModel _cameraViewModel;
        
        public void Init(CameraViewModel cameraViewModel)
        {
            _cameraViewModel = cameraViewModel;
            
            _cameraViewModel.moveCameraToNextColumn.SubscribeWithSkip(x => transform.DOMoveX(x, 1).OnComplete(() =>
            {
                _cameraViewModel.cameraFinishMoving.Notify();
            })).AddTo(this);

            _cameraViewModel.startLevel.Subscribe(() =>
            {
                transform.position = new Vector3(Constant.CameraOnColumnXOffset, transform.position.y,
                    transform.position.z);
            });
        }
    }
}