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
        public void Init(CameraViewModel cameraViewModel, IGameFlow iGameFlow)
        {
            cameraViewModel.MoveCameraToNextColumn.SubscribeWithSkip(x => 
                transform.DOMoveX(x, 1).OnComplete(() =>
            {
                cameraViewModel.CameraFinishMoving.Notify();
            })).AddTo(this);

            iGameFlow.StartLevel.Subscribe(() =>
            {
                transform.position = new Vector3(Constant.CameraOnColumnXOffset, transform.position.y,
                    transform.position.z);
            });
        }
    }
}