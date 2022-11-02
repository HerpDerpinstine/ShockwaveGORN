using ShockwaveGORN.Haptics;
using ShockwaveGORN.Setup.Devices;
using ShockwaveGORN.Setup.ConfigModels;

namespace ShockwaveGORN.Setup.Effects
{
    internal class E_ShootString : I_EffectBase
    {
        internal I_Arm<CM_Intensity> UpperArmL;
        internal I_Arm<CM_Intensity> LowerArmL;
        internal I_Arm<CM_Intensity> UpperArmR;
        internal I_Arm<CM_Intensity> LowerArmR;

        internal E_ShootString(I_WeaponBase weaponBase, string basefolder) : base(weaponBase)
        {
            string className = "ShootString";

            UpperArmL = new I_Arm<CM_Intensity>(true, true, basefolder, className);
            LowerArmL = new I_Arm<CM_Intensity>(false, true, basefolder, className);
            UpperArmR = new I_Arm<CM_Intensity>(true, false, basefolder, className);
            LowerArmR = new I_Arm<CM_Intensity>(false, false, basefolder, className);
        }

        internal void Play(bool is_left)
        {
            if (!Config.HapticEffects.ShootString)
                return;

            CheckArm(is_left ? UpperArmL : UpperArmR, is_left, true);
            CheckArm(is_left ? LowerArmL : LowerArmR, is_left, false);
        }

        private void CheckArm<T>(T device, bool is_left, bool is_upper) where T : I_Arm<CM_Intensity>
        {
            if (!device.IsEnabled())
                return;

            var intensity = device.General.IntensityScale.ScaleIntensity();
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
                    (0f, 1f, 0, 1f),
                    (0.5f, 0.5f, 75, 1f),
                    (1f, 0f, 150, 1f),
                }, eHapticType.CONSTANT),

				// 1
				(new (float, float, double, float)[]
                {
                    (1f, 1f, 0, 1f),
                    (0.5f, 0.5f, 75, 1f),
                    (0f, 0f, 150, 1f),
                }, eHapticType.CONSTANT),

				// 2
				(new (float, float, double, float)[]
                {
                    (0.5f, 1f, 0, 1f),
                    (0.5f, 0.5f, 75, 1f),
                    (0.5f, 0f, 150, 1f),
                }, eHapticType.CONSTANT),
            },
            intensity, region, eHapticSide.BOTH);
        }
    }
}