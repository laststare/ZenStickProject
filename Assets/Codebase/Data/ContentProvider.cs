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
            public GameObject levelcolumn;
            public CameraView cameraView;
            public PlayerView playerView;
            public StickView stickView;
            public RewardView rewardView;
        }
        
        [Serializable]
        public class SettingsContent
        {
            public RewardConfig rewardConfig;
        }
        
        public MainMenuView MainMenuView() => UIViews.mainMenuView;
        public ScoreCounterView ScoreCounterView() => UIViews.scoreCounterView;
        public GameObject LevelColumn()  => Views.levelcolumn;
        public CameraView CameraView()  => Views.cameraView;
        public PlayerView PlayerView()  => Views.playerView;
        public StickView StickView()  => Views.stickView;
        public RewardView RewardView()  => Views.rewardView;
        public RewardConfig RewardConfig()  => Settings.rewardConfig;
        
    }
}