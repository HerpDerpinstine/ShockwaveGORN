using HarmonyLib;
using ShockwaveGORN.Managers;
using MelonLoader;

namespace ShockwaveGORN.Hooks
{
    internal static class H_CrossbowCaestus
    {
        private static AccessTools.FieldRef<CrossbowCaestus, Fist> CrossbowCaestus_ownerFist = null;

        internal static void Initialize()
        {
            Debug.LogFieldRefGet("CrossbowCaestus.ownerFist");
            CrossbowCaestus_ownerFist = AccessTools.FieldRefAccess<CrossbowCaestus, Fist>("ownerFist");
            Debug.LogFieldRefFound("CrossbowCaestus.ownerFist", CrossbowCaestus_ownerFist);

            Debug.LogPatchInit("CrossbowCaestus.Fire");
            ShockwaveGORN.ModHarmony.Patch(
                AccessTools.Method(typeof(CrossbowCaestus), "Fire"), 
                AccessTools.Method(typeof(H_CrossbowCaestus), "Fire_Prefix").ToNewHarmonyMethod());
        }

        private static void Fire_Prefix(CrossbowCaestus __instance)
        {
            if (__instance.crankedM < 1f)
                return;
            Fist ownerFist = CrossbowCaestus_ownerFist(__instance);
            if (ownerFist == null)
                return;
            M_HapticFeedback.WeaponsByCaestusType[CaestusType.Crossbow]?.OnShootString(ownerFist.left);
        }
    }
}
