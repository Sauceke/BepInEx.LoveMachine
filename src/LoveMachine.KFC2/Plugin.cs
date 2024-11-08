using BepInEx;
using LoveMachine.Core;
using LoveMachine.Core.NonPortable;

namespace LoveMachine.KFC2;

[BepInPlugin(Globals.GUID, Globals.PluginName, Globals.Version)]
internal class Plugin : LoveMachinePlugin<KinkyFightClub2Adapter>
{ }