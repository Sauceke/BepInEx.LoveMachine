﻿using System.Collections;
using UnityEngine;

namespace LoveMachine.Core
{
    public abstract class ClassicButtplugController : ButtplugController
    {
        protected abstract IEnumerator HandleAnimation(Device device, StrokeInfo strokeInfo);

        protected abstract IEnumerator HandleOrgasm(Device device);

        protected override IEnumerator Run(Device device)
        {
            while (true)
            {
                if (game.IsOrgasming(device.Settings.GirlIndex))
                {
                    var orgasm = HandleCoroutine(HandleOrgasm(device));
                    yield return new WaitForSecondsRealtime(game.MinOrgasmDurationSecs);
                    yield return WaitWhile(() => game.IsOrgasming(device.Settings.GirlIndex));
                    StopCoroutine(orgasm);
                    continue;
                }
                if (game.IsIdle(device.Settings.GirlIndex))
                {
                    client.StopDeviceCmd(device);
                    while (game.IsIdle(device.Settings.GirlIndex))
                    {
                        yield return new WaitForSecondsRealtime(0.1f);
                    }
                    continue;
                }
                if (TryGetCurrentStrokeInfo(device, out var strokeInfo))
                {
                    yield return HandleCoroutine(HandleAnimation(device, strokeInfo));
                }
                else
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                }
            }
        }
    }
}