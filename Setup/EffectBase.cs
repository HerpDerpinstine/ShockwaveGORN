using UnityEngine;
using ShockwaveGORN.Setup.ConfigModels;

namespace ShockwaveGORN.Setup
{
    public abstract class I_EffectBase
    {
        internal I_WeaponBase WeaponBase;

        internal I_EffectBase() { }
        internal I_EffectBase(I_WeaponBase weaponBase)
            => WeaponBase = weaponBase;

        internal float GetIntensityScale(float magnitude, CM_Velocity velocityScalingValues)
            => GetIntensityScale(magnitude, velocityScalingValues.Enabled, velocityScalingValues.Multiplier, velocityScalingValues.Min, velocityScalingValues.Max);
        internal float GetIntensityScale(float magnitude, bool enabled, float multiplier, float min, float max)
        {
            if (!enabled)
                return 0f;
            return Mathf.Clamp(magnitude * multiplier, min, max).ScaleIntensity();
        }
    }
}
