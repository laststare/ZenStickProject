using System.Collections.Generic;
using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.Utilities;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public class LevelBuilderController : DisposableBase, ILevelBuilder
    {
        private readonly IContentProvider _contentProvider;
        private readonly List<GameObject> _levelColumns = new ();
        public float ActualColumnXPosition { get; set; }
        public float NextColumnXPosition { get; set; }

        protected LevelBuilderController(IContentProvider contentProvider, IGameFlow iGameFlow)
        {
            _contentProvider = contentProvider;
            
            iGameFlow.StartLevel.Subscribe(CreateFirstColumns).AddTo(_disposables);
            iGameFlow.LevelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun) NextColumn();
            }).AddTo(_disposables);
            
            iGameFlow.ColumnIsReachable.Subscribe(x =>
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
            ActualColumnXPosition = 0;
            NextColumnXPosition = 0;
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
            ActualColumnXPosition= NextColumnXPosition;
            NextColumnXPosition = ActualColumnXPosition + 5 + Random.Range(0, 4);
            AddColumn(NextColumnXPosition);
        }
        
        private void RemoveOneColumn()
        {
            if (_levelColumns.Count <= 2) return;
            Object.Destroy(_levelColumns[0].gameObject);
            _levelColumns.RemoveAt(0);
        }
        
    }
}