﻿using ShockwaveGORN.Haptics;
using ShockwaveGORN.Setup.Devices;
using ShockwaveGORN.Setup.ConfigModels;
using UnityEngine;
using Tomlet.Attributes;

namespace ShockwaveGORN.Setup.Effects
{
    internal class E_Wobble : I_EffectBase
    {
        [TomlDoNotInlineObject]
        internal class CM_VelocityNew : CM_Velocity
        {
            public CM_VelocityNew()
            {
                Max = 1f;
                Multiplier = 0.0025f;
            }
        }
        internal I_Arm<CM_Intensity, CM_VelocityNew> UpperArmL;
        internal I_Arm<CM_Intensity, CM_VelocityNew> LowerArmL;
        internal I_Arm<CM_Intensity, CM_VelocityNew> UpperArmR;
        internal I_Arm<CM_Intensity, CM_VelocityNew> LowerArmR;

        internal E_Wobble(I_WeaponBase weaponBase, string basefolder) : base(weaponBase)
        {
            string className = "Wobble";

            UpperArmL = new I_Arm<CM_Intensity, CM_VelocityNew>(true, true, basefolder, className);
            LowerArmL = new I_Arm<CM_Intensity, CM_VelocityNew>(false, true, basefolder, className);
            UpperArmR = new I_Arm<CM_Intensity, CM_VelocityNew>(true, false, basefolder, className);
            LowerArmR = new I_Arm<CM_Intensity, CM_VelocityNew>(false, false, basefolder, className);
        }

        internal void Play(Vector3 velocity, bool is_left)
        {
            if (!Config.HapticEffects.Wobble)
                return;

            CheckArm(velocity, is_left ? UpperArmL : UpperArmR, is_left, true);
            CheckArm(velocity, is_left ? LowerArmL : LowerArmR, is_left, false);
        }

        private void CheckArm<T>(Vector3 velocity, T device, bool is_left, bool is_upper) where T : I_Arm<CM_Intensity, CM_VelocityNew>
        {
            if (!device.IsEnabled())
                return;

            var intensity = GetIntensityScale(velocity.magnitude, device.VelocityScaling);
            PlayPattern(intensity, is_upper ?
                 (is_left ? eHapticRegion.LEFT_UPPER_ARM : eHapticRegion.RIGHT_UPPER_ARM)
                 : (is_left ? eHapticRegion.LEFT_LOWER_ARM : eHapticRegion.RIGHT_LOWER_ARM));
        }

        private void PlayPattern(float intensity, eHapticRegion region)
        {
            HapticPlayback.PlayDotPattern(new ((int, float)[], double)[]
               {
				(new (int, float)[] {
                    (0, 1f),
                    (1, 1f),
                }, 150),
               }, intensity, region, eHapticSide.BOTH);
        }
    }
}