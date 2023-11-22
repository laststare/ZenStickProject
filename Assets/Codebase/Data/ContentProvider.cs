﻿using System;
using Codebase.Views;
using UnityEngine;

namespace Codebase.Data
{

    [Serializable]
    public class ContentProvider : IContentProvider
    {
        public UIViewsContent UIViews => uiViews;
        public ViewsContent Views => views;
        public SettingsContent Settings => settings;
        
        [SerializeField] private ViewsContent views;
        [Space]
        [SerializeField] private UIViewsContent uiViews;
        [Space]
        [SerializeField] private SettingsContent settings;

        [Serializable]
        public class UIViewsContent
        {
             public MainMenuView mainMenuView;
             public ScoreCounterView scoreCounterView;
        }
        
        [Serializable]
        public class ViewsContent
        { 
            public GameObject levelColumn;
            public Transform camera;
            public Transform player;
            public Transform stick;
            public Transform reward;
        }
        
        [Serializable]
        public class SettingsContent
        {
            public RewardConfig rewardConfig;
            public LevelConfig levelConfig;
        }
        
        public MainMenuView MainMenuView() => UIViews.mainMenuView;
        public ScoreCounterView ScoreCounterView() => UIViews.scoreCounterView;
        public GameObject LevelColumn()  => Views.levelColumn;
        public Transform Camera()  => Views.camera;
        public Transform Player()  => Views.player;
        public Transform Stick()  => Views.stick;
        public Transform Reward()  => Views.reward;
        public RewardConfig RewardConfig()  => Settings.rewardConfig;
        public LevelConfig LevelConfig() => Settings.levelConfig;
    }
}