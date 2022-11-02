using ShockwaveGORN.Managers;
using HarmonyLib;
using MelonLoader;

namespace ShockwaveGORN.Hooks
{
    internal static class H_Grapple
    {
        internal static void Initialize()
        {
            Debug.LogPatchInit("Grapple.FireGrapple");
            ShockwaveGORN.ModHarmony.Patch(
                AccessTools.Method(typeof(Grapple), "FireGrapple"),
                AccessTools.Method(typeof(H_Grapple), "FireGrapple_Prefix").ToNewHarmonyMethod());
        }

        private static void FireGrapple_Prefix(Grapple __instance)
        {
            if (__instance.currentGrappleHead != null)
                return;
            M_HapticFeedback.WeaponsByCaestusType[CaestusType.Grapple]?.OnShoot(__instance.grabHand.ownerFist.left);
        }
    }
}
