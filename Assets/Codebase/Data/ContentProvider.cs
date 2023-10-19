﻿using System;
using Codebase.Views;
using UnityEngine;

namespace Codebase.Data
{

    [Serializable]
    public class ContentProvider 
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
             [SerializeField] private MainMenuView mainMenuView;
            // [SerializeField] private ScoreCounterView scoreCounterView;
             public MainMenuView MainMenuView => mainMenuView;
            // public ScoreCounterView ScoreCounterView => scoreCounterView;
            
        }

        [Serializable]
        public class ViewsContent
        { 
            [SerializeField] private GameObject levelcolumn;
            // [SerializeField] private CameraView cameraView;
            // [SerializeField] private PlayerView playerView;
            // [SerializeField] private StickView stickView;
            // [SerializeField] private RewardView rewardView;
             public GameObject Levelcolumn => levelcolumn;
            // public CameraView CameraView => cameraView;
            // public PlayerView PlayerView => playerView;
            // public StickView StickView => stickView;
            // public RewardView RewardView => rewardView;
        }

        [Serializable]
        public class SettingsContent
        {
            // [SerializeField] private RewardConfig rewardConfig;
            // public RewardConfig RewardConfig => rewardConfig;
        }
    }
}