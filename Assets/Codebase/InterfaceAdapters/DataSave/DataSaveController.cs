using Codebase.Utilities;
using UnityEngine;

namespace Codebase.InterfaceAdapters.DataSave
{
    public class DataSaveController : DisposableBase, IDataSave
    {
        public void SaveBestScore(int bestScore)
        {
            PlayerPrefs.SetInt(Constant.SavedScore, bestScore);
        }

        public int LoadBestScore()
        {
            return PlayerPrefs.GetInt(Constant.SavedScore);
        }
    }
}