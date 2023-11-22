using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.Stick;
using Codebase.Utilities;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Player
{
    public class PlayerController : DisposableBase
    {
        private readonly IContentProvider _contentProvider;
        private readonly IGameFlow _iGameFlow;
        private readonly ILevelBuilder _iLevelBuilder;
        private readonly IStick _iStick;
        private readonly int _columnOffset;
        private readonly float _playerOnColumnXOffset, _destinationOffset;
        private readonly Transform _player;
        
        public PlayerController(IContentProvider contentProvider, IGameFlow iGameFlow, 
            IStick iStick, ILevelBuilder iLevelBuilder)
        {
            _contentProvider = contentProvider;
            _iLevelBuilder = iLevelBuilder;
            _iStick = iStick;
            _iGameFlow = iGameFlow;
            _columnOffset = _contentProvider.LevelConfig().GetColumnOffset;
            _playerOnColumnXOffset = _contentProvider.LevelConfig().GetPlayerOnColumnXOffset;
            _destinationOffset = _contentProvider.LevelConfig().GetDestinationOffset;
            _player = Object.Instantiate(_contentProvider.Player());

            _iGameFlow.StartLevel.Subscribe(() =>
            {
                _player.position = new Vector2(_playerOnColumnXOffset,
                    _contentProvider.LevelConfig().GetPlayerYPosition);
                _player.gameObject.SetActive(true);
            }).AddTo(_disposables);
            
            _iGameFlow.LevelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerRun)
                    SetPlayerDestinationPoint();
            }).AddTo(_disposables);
        }

        private void SetPlayerDestinationPoint()
        {
            var moveDistance = _iLevelBuilder.ActualColumnXPosition + _destinationOffset + _iStick.StickLength;
            _iGameFlow.ColumnIsReachable.SetValueAndForceNotify(moveDistance >= _iLevelBuilder.NextColumnXPosition - _columnOffset &&
                                                          moveDistance <= _iLevelBuilder.NextColumnXPosition + _columnOffset);
            var playerDestination = _iGameFlow.ColumnIsReachable.Value
                ? _iLevelBuilder.NextColumnXPosition + _playerOnColumnXOffset
                : moveDistance;

            _player.DOMoveX(playerDestination, 2).OnComplete(PlayerOnNextColumn);
        }

        private void PlayerOnNextColumn()
        {
            if (_iGameFlow.ColumnIsReachable.Value)
                _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.CameraRun);
            else
                _iGameFlow.FinishLevel.Notify();
        }
        
        protected override void OnDispose()
        {
            base.OnDispose();
            if(_player != null)
                Object.Destroy(_player.gameObject);
        }
        
    }
}