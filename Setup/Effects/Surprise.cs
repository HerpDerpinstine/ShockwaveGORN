using ShockwaveGORN.Haptics;
using ShockwaveGORN.Setup.Devices;
using ShockwaveGORN.Setup.ConfigModels;

namespace ShockwaveGORN.Setup.Effects
{
	internal class E_Surprise : I_EffectBase
	{
		internal I_Torso<CM_Intensity> Torso;

		internal E_Surprise()
		{
			string className = "Surprise";

			Torso = new I_Torso<CM_Intensity>("Player", className);
		}

		internal void Play()
		{
			if (!Config.HapticEffects.Surprise || !Torso.IsEnabled())
				return;

			float intensity = Torso.General.IntensityScale.ScaleIntensity();

			HapticPlayback.PlayPathPattern(new ((float, float, double, float)[], eHapticType)[]
			{
				// 0
				(new (float, float, double, float)[]
				{
					(0.35f, 1f, 0, 0.4f),
					(0.25f, 0.95f, 250, 0.4f),
					(0f, 0.75f, 500, 0.4f),
				}, eHapticType.FADE_OUT),

				// 1
				(new (float, float, double, float)[]
				{
					(0.65f, 1f, 0, 0.4f),
					(0.75f, 0.95f, 250, 0.4f),
					(1f, 0.75f, 500, 0.4f),
				}, eHapticType.FADE_OUT),

				// 2
				(new (float, float, double, float)[]
				{
					(0f, 1f, 0, 0.4f),
					(0f, 0.95f, 250, 0.4f),
					(0.15f, 0.75f, 500, 0.4f),
				}, eHapticType.FADE_OUT),

				// 3
				(new (float, float, double, float)[]
				{
					(1f, 1f, 0, 0.4f),
					(0.95f, 0.95f, 250, 0.4f),
					(0.85f, 0.75f, 500, 0.4f),
				}, eHapticType.FADE_OUT),
			},
			intensity, eHapticRegion.TORSO, eHapticSide.BOTH);
		}
	}
}