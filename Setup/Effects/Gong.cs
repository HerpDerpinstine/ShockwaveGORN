using ShockwaveGORN.Haptics;
using ShockwaveGORN.Setup.Devices;
using ShockwaveGORN.Setup.ConfigModels;
using UnityEngine;

namespace ShockwaveGORN.Setup.Effects
{
	internal class E_Gong : I_EffectBase
	{
		internal I_Torso<CM_Intensity2> Torso;

		internal E_Gong()
		{
			string className = "Gong";

			Torso = new I_Torso<CM_Intensity2>("Player", className);
		}

		internal enum Strength
        {
			Hard,
			Medium,
			Soft
        }

		internal void Play(Strength strength)
		{
			if (!Config.HapticEffects.Gong || !Torso.IsEnabled())
				return;

			float strengthScale = Torso.General.IntensityScale_Hard;
			switch (strength)
			{
				case Strength.Soft:
					strengthScale = Torso.General.IntensityScale_Soft;
					break;
				case Strength.Medium:
					strengthScale = Torso.General.IntensityScale_Medium;
					break;
				default:
					break;
			}

			float intensity = strengthScale.ScaleIntensity();

			HapticPlayback.PlayPathPattern(new ((float, float, double, float)[], eHapticType)[]
			{
				// 0
				(new (float, float, double, float)[]
				{
					(0.33f, 0.37f, 0, 1f),
					(0.33f, 0.5f, 290, 1f),
					(0f, 0.75f, 1150, 1f),
				}, eHapticType.FADE_OUT),

				// 1
				(new (float, float, double, float)[]
				{
					(0.67f, 0.37f, 0, 1f),
					(0.67f, 0.5f, 274, 1f),
					(1f, 0.75f, 1150, 1f),
				}, eHapticType.FADE_OUT),

				// 2
				(new (float, float, double, float)[]
				{
					(0.33f, 0.37f, 0, 1f),
					(0.33f, 0.75f, 552, 1f),
					(0f, 1f, 1150, 1f),
				}, eHapticType.FADE_OUT),

				// 3
				(new (float, float, double, float)[]
				{
					(0.67f, 0.37f, 0, 1f),
					(0.67f, 0.75f, 552, 1f),
					(1f, 1f, 1150, 1f),
				}, eHapticType.FADE_OUT),

				// 4
				(new (float, float, double, float)[]
				{
					(0.67f, 0.37f, 0, 1f),
					(1f, 1f, 1150, 1f),
					(0.67f, 1f, 754, 1f),
				}, eHapticType.FADE_OUT),

				// 5
				(new (float, float, double, float)[]
				{
					(0.33f, 0.37f, 0, 1f),
					(0.33f, 1f, 754, 1f),
					(0f, 1f, 1150, 1f),
				}, eHapticType.FADE_OUT),
			},
			intensity, eHapticRegion.TORSO, eHapticSide.BOTH);
		}
	}
}