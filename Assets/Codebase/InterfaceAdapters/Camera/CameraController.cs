using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.Utilities;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Camera
{
    public class CameraController : DisposableBase
    {
        private readonly IContentProvider _contentProvider;
        private readonly IGameFlow _iGameFlow;
        private readonly ILevelBuilder _iLevelBuilder;
        private readonly Transform _camera;
        
        public CameraController(IContentProvider contentProvider, IGameFlow iGameFlow, ILevelBuilder iLevelBuilder)
        {
            _contentProvider = contentProvider;
            _iGameFlow = iGameFlow;
            _iLevelBuilder = iLevelBuilder;
            
            _camera = Object.Instantiate(_contentProvider.Camera());
            
            _iGameFlow.LevelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun)
                {
                    var cameraPosition = _iLevelBuilder.ActualColumnXPosition + _contentProvider.LevelConfig().GetCameraColumnXOffset;
                    _camera.DOMoveX(cameraPosition, 1).OnComplete(() =>
                    {
                        _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.PlayerIdle);
                    });
                }
            }).AddTo(_disposables);

            _iGameFlow.StartLevel.Subscribe(() =>
            {
                _camera.position = new Vector3(_contentProvider.LevelConfig().GetCameraColumnXOffset, _camera.position.y,
                    _camera.position.z);
            }).AddTo(_disposables);
        }
        
        protected override void OnDispose()
        {
            base.OnDispose();

            if (_camera != null)
            {
                Object.Destroy(_camera.gameObject);
            }
        }

    }
}