using System;
using System.Collections;
using System.Linq;
using LoveMachine.Core.Buttplug;
using LoveMachine.Core.Config;
using LoveMachine.Core.Game;
using UnityEngine;

namespace LoveMachine.Core.Controller
{
    internal sealed class OscillatorController: ClassicButtplugController
    {
        public override string FeatureName => "Oscillation";

        public override bool IsDeviceSupported(Device device) => device.IsOscillator;

        protected override IEnumerator HandleAnimation(Device device, StrokeInfo strokeInfo)
        {
            var settings = device.Settings.OscillatorSettings;
            var feature = device.DeviceMessages.ScalarCmd
                .First(cmd => cmd.ActuatorType == Buttplug.Buttplug.Feature.Oscillate);
            int steps = feature.StepCount;
            float durationSecs = strokeInfo.DurationSecs;
            float rpm = Mathf.Min(60f / durationSecs, OscillatorConfig.RpmLimit.Value);
            float level = Mathf.InverseLerp(settings.MinRpm, settings.MaxRpm, rpm);
            float speed = Mathf.Lerp(1f / steps, 1f, level);
            Client.OscillateCmd(device, speed);
            yield return WaitForSecondsUnscaled(durationSecs);
        }

        protected override IEnumerator HandleOrgasm(Device device)
        {
            Client.OscillateCmd(device, 1f);
            yield break;
        }

        protected override void HandleLevel(Device device, float level, float durationSecs)
        {}
    }
}