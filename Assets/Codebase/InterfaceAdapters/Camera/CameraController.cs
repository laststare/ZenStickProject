using Codebase.Data;
using Codebase.Utilities;
using Codebase.Views;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Camera
{
    public class CameraController : DisposableBase
    {
        private readonly ContentProvider _contentProvider;
        private readonly Transform _uiRoot;
        private readonly CameraViewModel _cameraViewModel;
        private CameraView _view;
        
        public CameraController(ContentProvider contentProvider, Transform uiRoot, CameraViewModel cameraViewModel)
        {
            _contentProvider = contentProvider;
            _uiRoot = uiRoot;
            _cameraViewModel = cameraViewModel;

            _cameraViewModel.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun)
                    SetCameraDestinationPointToColumn();
            }).AddTo(_disposables);
            
            _cameraViewModel.cameraFinishMoving.Subscribe(() =>
            {
                _cameraViewModel.changeLevelFlowState.Notify(LevelFlowState.PlayerIdle);
            }).AddTo(_disposables);
            CreateCameraView();
        }
        
        private void CreateCameraView()
        {
            _view = Object.Instantiate(_contentProvider.Views.CameraView);
            _view.Init(_cameraViewModel);
        }
        
        private void SetCameraDestinationPointToColumn()
        {
            _cameraViewModel.moveCameraToNextColumn.Notify(_cameraViewModel.actualColumnXPosition.Value + Constant.CameraOnColumnXOffset);
        }
        
    }
}