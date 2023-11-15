using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.Utilities;
using Codebase.Views;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Camera
{
    public class CameraController : DisposableBase
    {
        private readonly IContentProvider _contentProvider;
        private readonly IGameFlow _iGameFlow;
        private readonly ILevelBuilder _iLevelBuilder;
        private readonly CameraViewModel _cameraViewModel;
        private CameraView _view;
        
        public CameraController(IContentProvider contentProvider, CameraViewModel cameraViewModel,
            IGameFlow iGameFlow, ILevelBuilder iLevelBuilder)
        {
            _contentProvider = contentProvider;
            _cameraViewModel = cameraViewModel;
            _iGameFlow = iGameFlow;
            _iLevelBuilder = iLevelBuilder;
            
            _iGameFlow.LevelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun)
                    SetCameraDestinationPointToColumn();
            }).AddTo(_disposables);
            
            _cameraViewModel.CameraFinishMoving.Subscribe(() =>
            {
                _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.PlayerIdle);
            }).AddTo(_disposables);
            CreateCameraView();
        }
        
        private void CreateCameraView()
        {
            _view = Object.Instantiate(_contentProvider.CameraView());
            _view.Init(_cameraViewModel, _iGameFlow);
        }
        
        private void SetCameraDestinationPointToColumn()
        {
            _cameraViewModel.MoveCameraToNextColumn.Notify(
                _iLevelBuilder.ActualColumnXPosition + Constant.CameraOnColumnXOffset);
        }
        
    }
}