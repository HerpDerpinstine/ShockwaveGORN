using ShockwaveGORN.Haptics;
using ShockwaveGORN.Setup.Devices;
using ShockwaveGORN.Setup.ConfigModels;
using Tomlet.Attributes;
using UnityEngine;

namespace ShockwaveGORN.Setup.Effects
{
    internal class E_Blunt2 : I_EffectBase
    {
        [TomlDoNotInlineObject]
        internal class CM_VelocityNew : CM_Velocity
        {
            public CM_VelocityNew()
                => Multiplier = 0.004f;
        }
        internal I_Arm<CM_Intensity, CM_VelocityNew> UpperArmL;
        internal I_Arm<CM_Intensity, CM_VelocityNew> LowerArmL;
        internal I_Arm<CM_Intensity, CM_VelocityNew> UpperArmR;
        internal I_Arm<CM_Intensity, CM_VelocityNew> LowerArmR;

        internal E_Blunt2(I_WeaponBase weaponBase, string basefolder) : base(weaponBase)
        {
            string className = "Blunt";

            UpperArmL = new I_Arm<CM_Intensity, CM_VelocityNew>(true, true, basefolder, className);
            LowerArmL = new I_Arm<CM_Intensity, CM_VelocityNew>(false, true, basefolder, className);
            UpperArmR = new I_Arm<CM_Intensity, CM_VelocityNew>(true, false, basefolder, className);
            LowerArmR = new I_Arm<CM_Intensity, CM_VelocityNew>(false, false, basefolder, className);
        }

        internal void Play(Vector3 velocity, bool is_left)
        {
            if (!Config.HapticEffects.Blunt)
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
                    (0.5f, 0f, 0, 1f),
                    (0.5f, 0.25f, 71, 1f),
                    (0.5f, 0.45f, 150, 1f),
                }, eHapticType.FADE_OUT),

				// 1
				(new (float, float, double, float)[]
                {
                    (0.8f, 0f, 0, 1f),
                    (0.8f, 0.25f, 71, 1f),
                    (0.8f, 0.45f, 150, 1f),
                }, eHapticType.FADE_OUT),

				// 2
				(new (float, float, double, float)[]
                {
                    (0.2f, 0f, 0, 1f),
                    (0.2f, 0.25f, 71, 1f),
                    (0.2f, 0.45f, 150, 1f),
                }, eHapticType.FADE_OUT),
            },
            intensity, region, eHapticSide.BOTH);
        }
    }
}