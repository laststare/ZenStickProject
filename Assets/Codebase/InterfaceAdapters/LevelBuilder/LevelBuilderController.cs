using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.Player;
using Codebase.Utilities;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public class LevelBuilderController : DisposableBase, ILevelBuilder
    {
        private readonly IContentProvider _contentProvider;
        private readonly IGameFlow _iGameFlow;
        private readonly IPlayer _iPlayer;
        private readonly List<GameObject> _levelColumns = new ();
        public float actualColumnXPosition { get; set; }
        public float nextColumnXPosition { get; set; }

        protected LevelBuilderController(IContentProvider contentProvider, 
            IPlayer iPlayer, IGameFlow iGameFlow)
        {
            _contentProvider = contentProvider;
            _iGameFlow = iGameFlow;
            _iPlayer = iPlayer;
            
            _iGameFlow.startLevel.Subscribe(CreateFirstColumns).AddTo(_disposables);
            _iGameFlow.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun) NextColumn();
            }).AddTo(_disposables);
            
            _iPlayer.columnIsReachable.Subscribe(x =>
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
            actualColumnXPosition = 0;
            nextColumnXPosition = 0;
        }
        
        private void AddColumn(float xPosition)
        {
            var column = Object.Instantiate(_contentProvider.LevelColumn(),
                new Vector3(xPosition, 0, 0),
                Quaternion.identity);
            _levelColumns.Add(column); 
        }
        
        private void NextColumn()
        { 
            actualColumnXPosition= nextColumnXPosition;
            nextColumnXPosition = actualColumnXPosition + 5 + Random.Range(0, 4);
            AddColumn(nextColumnXPosition);
        }
        
        private void RemoveOneColumn()
        {
            if (_levelColumns.Count <= 2) return;
            Object.Destroy(_levelColumns[0].gameObject);
            _levelColumns.RemoveAt(0);
        }
        
    }
}