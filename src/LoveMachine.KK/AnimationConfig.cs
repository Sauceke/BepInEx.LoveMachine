﻿using BepInEx;
using BepInEx.Configuration;

namespace LoveMachine.KK
{
    public static class AnimationConfig
    {
        public static ConfigEntry<bool> TrackAnimationBlending { get; private set; }

        public static void Initialize(BaseUnityPlugin plugin)
        {
            const string animationSettingsTitle = "Animation Settings (KK/KKS only)";
            TrackAnimationBlending = plugin.Config.Bind(
                section: animationSettingsTitle,
                key: "Track Animation Blending",
                defaultValue: false,
                description: "Makes animation tracking more precise but also less uniform");
        }
    }
}