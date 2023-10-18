using System.Collections.Generic;
using Codebase.Data;
using Codebase.Utilities;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.LevelBuilder
{
    public class LevelBuilderController : DisposableBase
    {

        private readonly LevelBuilderViewModel _levelBuilderViewModel;
        private readonly ContentProvider _contentProvider;
        private readonly List<GameObject> _levelColumns = new List<GameObject>();
        
        public LevelBuilderController(LevelBuilderViewModel levelBuilderViewModel, ContentProvider contentProvider)
        {
            _levelBuilderViewModel = levelBuilderViewModel;
            _contentProvider = contentProvider;
            _levelBuilderViewModel.startLevel.Subscribe(x => CreateFirstColumns()).AddTo(_disposables);
            CreateFirstColumns();
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
    }
}