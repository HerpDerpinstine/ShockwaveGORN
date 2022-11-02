﻿using System;
using HarmonyLib;
using UnityEngine;
using ShockwaveGORN.Managers;
using System.Reflection;
using MelonLoader;

namespace ShockwaveGORN.Hooks
{
    internal static class H_GrabHand
    {
        private static FieldInfo GrabHand_currentVelocity = null;

        internal static void Initialize()
        {
            Type Type_GrabHand = typeof(GrabHand);

            Debug.LogFieldRefGet("GrabHand.currentVelocity");
            GrabHand_currentVelocity = AccessTools.Field(Type_GrabHand, "currentVelocity");
            Debug.LogFieldRefFound("GrabHand.currentVelocity", GrabHand_currentVelocity);

            Debug.LogPatchInit("GrabHand.FixedUpdate");
            ShockwaveGORN.ModHarmony.Patch(AccessTools.Method(Type_GrabHand, "FixedUpdate"),
                null,
                AccessTools.Method(typeof(H_GrabHand), "FixedUpdate_Postfix").ToNewHarmonyMethod());
        }

        private static void FixedUpdate_Postfix(GrabHand __instance)
        {
            if (!Config.General.Allow_Player_Communication)
                return;

            if (!__instance.isPlayer
                || (__instance.GrabbedObject == null)
                || (__instance.ownerFist == null))
                return;

            if ((__instance.GrabbedObject.GetHeldWeaponType() == WeaponType.Arrow)
                && ((Arrow)__instance.GrabbedObject).IsNocked)
                return;

            Vector3 currentVelocity = (Vector3)GrabHand_currentVelocity.GetValue(__instance);
            if (currentVelocity.magnitude < 10f)
                return;

            M_HapticFeedback.fist.OnWobble(currentVelocity, __instance.ownerFist.left);
        }
    }
}