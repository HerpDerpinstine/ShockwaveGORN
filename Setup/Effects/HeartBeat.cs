using ShockwaveGORN.Haptics;
using ShockwaveGORN.Setup.Devices;
using ShockwaveGORN.Setup.ConfigModels;

namespace ShockwaveGORN.Setup.Effects
{
	internal class E_HeartBeat : I_EffectBase
	{
		internal I_Torso<CM_Intensity> Torso;

		internal E_HeartBeat()
		{
			string className = "HeartBeat";

			Torso = new I_Torso<CM_Intensity>("Player", className);
		}

		internal void Play()
		{
			if (!Config.HapticEffects.HeartBeat || !Torso.IsEnabled())
				return;

			float intensity = Torso.General.IntensityScale.ScaleIntensity();

			HapticPlayback.PlayDotPattern(new ((int, float)[], double)[]
			{
				// 0
				(new (int, float)[] {
				}, 88),
				
				// 1
				(new (int, float)[] {
					(0, 0.5f),
					(1, 0.4f),
					(5, 0.5f),
				}, 177),

				// 2
				(new (int, float)[] {
				}, 265),
			}, intensity, eHapticRegion.TORSO, eHapticSide.BOTH);

			HapticPlayback.PlayDotPattern(new ((int, float)[], double)[]
			{
				// 0
				(new (int, float)[] {
				}, 74),
				
				// 1
				(new (int, float)[] {
					(0, 0.5f),
					(1, 0.2f),
					(5, 0.5f),
				}, 148),

				// 2
				(new (int, float)[] {
				}, 223),
			}, intensity, eHapticRegion.TORSO, eHapticSide.BOTH, 265);
		}
	}
}