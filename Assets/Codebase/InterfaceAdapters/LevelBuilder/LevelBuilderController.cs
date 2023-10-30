using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.Player;
using Codebase.Utilities;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public class LevelBuilderController : DisposableBase
    {

        private readonly LevelBuilderViewModel _levelBuilderViewModel;
        private readonly ContentProvider _contentProvider;
        private readonly GameFlowViewModel _gameFlowViewModel;
        private readonly PlayerViewModel _playerViewModel;
        private readonly List<GameObject> _levelColumns = new ();
        
        public LevelBuilderController(LevelBuilderViewModel levelBuilderViewModel, ContentProvider contentProvider, GameFlowViewModel gameFlowViewModel, PlayerViewModel playerViewModel)
        {
            _levelBuilderViewModel = levelBuilderViewModel;
            _contentProvider = contentProvider;
            _gameFlowViewModel = gameFlowViewModel;
            _playerViewModel = playerViewModel;
            _gameFlowViewModel.startLevel.Subscribe(CreateFirstColumns).AddTo(_disposables);
            _gameFlowViewModel.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun) NextColumn();
            }).AddTo(_disposables);
            _playerViewModel.columnIsReachable.Subscribe(x =>
            {
                if (x)
                    RemoveOneColumn();
            }).AddTo(_disposables);
        }
        
        private void CreateFirstColumns()
        {
            DestroyLevel();
            AddColumn(0);
            NextColumn();
        }
        
        private void DestroyLevel()
        {
            foreach (var column in _levelColumns) 
                Object.Destroy(column);
            _levelColumns.Clear();
            _levelBuilderViewModel.actualColumnXPosition.Value = 0;
            _levelBuilderViewModel.nextColumnXPosition.Value = 0;
        }
        
        private void AddColumn(float xPosition)
        {
            var column = Object.Instantiate(_contentProvider.Views.Levelcolumn,
                new Vector3(xPosition, 0, 0),
                Quaternion.identity);
            _levelColumns.Add(column); 
        }
        
        private void NextColumn()
        {
            _levelBuilderViewModel.actualColumnXPosition.Value = _levelBuilderViewModel.nextColumnXPosition.Value;
            _levelBuilderViewModel.nextColumnXPosition.Value = _levelBuilderViewModel.actualColumnXPosition.Value + 5 + Random.Range(0, 4);
            AddColumn(_levelBuilderViewModel.nextColumnXPosition.Value);
        }
        
        private void RemoveOneColumn()
        {
            if (_levelColumns.Count <= 2) return;
            Object.Destroy(_levelColumns[0].gameObject);
            _levelColumns.RemoveAt(0);
        }
    }
}