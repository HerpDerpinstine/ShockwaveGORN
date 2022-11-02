using ShockwaveGORN.Haptics;
using ShockwaveGORN.Setup.Devices;
using ShockwaveGORN.Setup.ConfigModels;
using UnityEngine;
using Tomlet.Attributes;

namespace ShockwaveGORN.Setup.Effects
{
    internal class E_Cut : I_EffectBase
    {
        [TomlDoNotInlineObject]
        internal class CM_VelocityNew : CM_Velocity
        {
            public CM_VelocityNew()
                => Multiplier = 0.00575f;
        }
        internal I_Arm<CM_Intensity, CM_VelocityNew> UpperArmL;
        internal I_Arm<CM_Intensity, CM_VelocityNew> LowerArmL;
        internal I_Arm<CM_Intensity, CM_VelocityNew> UpperArmR;
        internal I_Arm<CM_Intensity, CM_VelocityNew> LowerArmR;

        internal E_Cut(I_WeaponBase weaponBase, string basefolder) : base(weaponBase)
        {
            string className = "Cut";

            UpperArmL = new I_Arm<CM_Intensity, CM_VelocityNew>(true, true, basefolder, className);
            LowerArmL = new I_Arm<CM_Intensity, CM_VelocityNew>(false, true, basefolder, className);
            UpperArmR = new I_Arm<CM_Intensity, CM_VelocityNew>(true, false, basefolder, className);
            LowerArmR = new I_Arm<CM_Intensity, CM_VelocityNew>(false, false, basefolder, className);
        }

        internal void Play(Vector3 velocity, bool is_left)
        {
            if (!Config.HapticEffects.Cut)
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
            HapticPlayback.PlayPathPattern(new ((float, float, double, float)[], eHapticType)[]
            {
				// 0
				(new (float, float, double, float)[]
                {
                    (1f, 0f, 0, 1f),
                    (0.5f, 0.5f, 75, 1f),
                    (0f, 1f, 150, 1f),
                }, eHapticType.CONSTANT),

				// 1
				(new (float, float, double, float)[]
                {
                    (0f, 0f, 0, 1f),
                    (0.5f, 0.5f, 75, 1f),
                    (1f, 1f, 150, 1f),
                }, eHapticType.CONSTANT),

				// 2
				(new (float, float, double, float)[]
                {
                    (0.5f, 0f, 0, 1f),
                    (0.5f, 0.5f, 75, 1f),
                    (0.5f, 1f, 150, 1f),
                }, eHapticType.CONSTANT),
            },
            intensity, region, eHapticSide.BOTH);
        }
    }
}