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
            OscillateWithRpm(device, 60f / strokeInfo.DurationSecs);
            yield return WaitForSecondsUnscaled(strokeInfo.DurationSecs);
        }

        protected override IEnumerator HandleOrgasm(Device device)
        {
            OscillateWithRpm(device, OscillatorConfig.RpmLimit.Value);
            yield break;
        }

        protected override void HandleLevel(Device device, float level, float durationSecs)
        {}

        private void OscillateWithRpm(Device device, float rpm)
        {
            rpm = Mathf.Min(rpm, OscillatorConfig.RpmLimit.Value);
            var settings = device.Settings.OscillatorSettings;
            var feature = device.DeviceMessages.ScalarCmd
                .First(cmd => cmd.ActuatorType == Buttplug.Buttplug.Feature.Oscillate);
            int steps = feature.StepCount;
            float rate = Mathf.InverseLerp(settings.MinRpm, settings.MaxRpm, value: rpm);
            float speed = Mathf.Lerp(1f / steps, 1f, t: rate);
            Client.OscillateCmd(device, speed);
        }
    }
}