using System.Collections;
using LoveMachine.Core.NonPortable;
using UnityEngine;

namespace LoveMachine.GC;

public class HSceneMonitor : CoroutineHandler
{
    public void Start()
    {
        HandleCoroutine(MonitorHScene());
    }

    private IEnumerator MonitorHScene()
    {
        var ui = GameObject.Find("/UI").transform.Find("UI_Sex_Container").gameObject;
        while (true)
        {
            while (!ui.active)
            {
                yield return new WaitForSeconds(5f);
            }
            HSceneStarted();
            while (ui.active)
            {
                yield return new WaitForSeconds(5f);
            }
            HSceneEnded();
        }
    }
    
    public void HSceneStarted() {}
    
    public void HSceneEnded() {}
}