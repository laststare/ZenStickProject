using Codebase.Views;
using UnityEngine;

namespace Codebase.Data
{
    public interface IContentProvider
    {
        public MainMenuView MainMenuView();
        public ScoreCounterView ScoreCounterView();
        public GameObject LevelColumn();
        public CameraView CameraView();
        public PlayerView PlayerView();
        public Transform Stick();
        public RewardView RewardView();
        public RewardConfig RewardConfig();
    }
}