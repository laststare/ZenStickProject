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
        private readonly ContentProvider _contentProvider;
        private readonly CameraViewModel _cameraViewModel;
        private readonly GameFlowViewModel _gameFlowViewModel;
        private readonly LevelBuilderViewModel _levelBuilderViewModel;
        private CameraView _view;
        
        public CameraController(ContentProvider contentProvider, CameraViewModel cameraViewModel,
            GameFlowViewModel gameFlowViewModel, LevelBuilderViewModel levelBuilderViewModel)
        {
            _contentProvider = contentProvider;
            _cameraViewModel = cameraViewModel;
            _gameFlowViewModel = gameFlowViewModel;
            _levelBuilderViewModel = levelBuilderViewModel;

            _gameFlowViewModel.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun)
                    SetCameraDestinationPointToColumn();
            }).AddTo(_disposables);
            
            _cameraViewModel.cameraFinishMoving.Subscribe(() =>
            {
                _gameFlowViewModel.changeLevelFlowState.Notify(LevelFlowState.PlayerIdle);
            }).AddTo(_disposables);
            CreateCameraView();
        }
        
        private void CreateCameraView()
        {
            _view = Object.Instantiate(_contentProvider.Views.CameraView);
            _view.Init(_cameraViewModel, _gameFlowViewModel);
        }
        
        private void SetCameraDestinationPointToColumn()
        {
            _cameraViewModel.moveCameraToNextColumn.Notify(
                _levelBuilderViewModel.actualColumnXPosition.Value + Constant.CameraOnColumnXOffset);
        }
        
    }
}