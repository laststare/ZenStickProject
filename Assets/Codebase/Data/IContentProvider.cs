using Codebase.Views;
using UnityEngine;

namespace Codebase.Data
{
    public interface IContentProvider
    {
        public MainMenuView MainMenuView();
        public ScoreCounterView ScoreCounterView();
        public GameObject LevelColumn();
        public Transform Camera();
        public Transform Player();
        public Transform Stick();
        public Transform Reward();
        public RewardConfig RewardConfig();
        public LevelConfig LevelConfig();
    }
}