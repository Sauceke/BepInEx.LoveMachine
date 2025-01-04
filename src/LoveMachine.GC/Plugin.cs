using BepInEx;
using LoveMachine.Core;
using LoveMachine.Core.NonPortable;

namespace LoveMachine.GC;

[BepInPlugin(Globals.GUID, Globals.PluginName, Globals.Version)]
internal class Plugin : LoveMachinePlugin<GalsCollectorGame>
{
    protected override void Start()
    {
        base.Start();
        Globals.ManagerObject.AddComponent<HSceneMonitor>();
    }
}