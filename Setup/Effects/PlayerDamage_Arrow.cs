using UnityEngine;
using ShockwaveGORN.Haptics;
using ShockwaveGORN.Setup.Devices;
using ShockwaveGORN.Setup.ConfigModels;
using Tomlet.Attributes;

namespace ShockwaveGORN.Setup.Effects
{
	internal class E_PlayerDamage_Arrow : I_EffectBase
	{
		[TomlDoNotInlineObject]
		internal class CM_VelocityNew : CM_Velocity
		{
			public CM_VelocityNew()
				=> Multiplier = 0.01f;
		}
		internal I_Torso<CM_Intensity, CM_VelocityNew> Torso;

		internal E_PlayerDamage_Arrow()
		{
			string className = "PlayerDamage_Arrow";

			Torso = new I_Torso<CM_Intensity, CM_VelocityNew>("Player", className);
		}

		internal void Play(Vector3 contactPos, Vector3 velocity)
		{
			if (!Config.HapticEffects.PlayerDamage_Arrow || !Torso.IsEnabled())
				return;

			float intensity = GetIntensityScale(velocity.magnitude, Torso.VelocityScaling);

			GameController.Player.GetBodyForwardPos(out Vector3 centerPosition, out (Vector3, Vector3, Vector3) directions, out (float, float) size);
			(float, float) angle = contactPos.GetPositionAngle(centerPosition, directions.Item1);

			PlayPattern(eHapticSide.FRONT, intensity, angle);
			PlayPattern(eHapticSide.BACK, intensity, angle);
		}

		private void PlayPattern(eHapticSide pulseFace,
			float intensity,
			(float, float) additionalRotation)
		{
			HapticPlayback.PlayPathPattern(new ((float, float, double, float)[], eHapticType)[]
			{
				// 0
				(new (float, float, double, float)[]
				{
					(0.5f, 0.5f, 0, 1f),
					(0.75f, 0.25f, 76, 1f),
					(1f, 0f, 160, 1f)
				}, eHapticType.CONSTANT),

				// 1
				(new (float, float, double, float)[]
				{
					(0.5f, 0.5f, 0, 1f),
					(0.25f, 0.25f, 76, 1f),
					(0f, 0f, 160, 1f)
				}, eHapticType.CONSTANT),

				// 2
				(new (float, float, double, float)[]
				{
					(0.5f, 0.5f, 0, 1f),
					(0.5f, 0.25f, 76, 1f),
					(0.5f, 0f, 160, 1f)
				}, eHapticType.CONSTANT),

				// 3
				(new (float, float, double, float)[]
				{
					(0.5f, 0.5f, 0, 1f),
					(0.5f, 0.75f, 76, 1f),
					(0.5f, 1f, 160, 1f)
				}, eHapticType.CONSTANT),

				// 4
				(new (float, float, double, float)[]
				{
					(0.5f, 0.5f, 0, 1f),
					(0.75f, 0.75f, 76, 1f),
					(1f, 1f, 160, 1f)
				}, eHapticType.CONSTANT),

				// 5
				(new (float, float, double, float)[]
				{
					(0.5f, 0.5f, 0, 1f),
					(0.75f, 0.5f, 76, 1f),
					(1f, 0.5f, 160, 1f)
				}, eHapticType.CONSTANT),

				// 6
				(new (float, float, double, float)[]
				{
					(0.5f, 0.5f, 0, 1f),
					(0.25f, 0.5f, 76, 1f),
					(0f, 0.5f, 160, 1f)
				}, eHapticType.CONSTANT),

				// 7
				(new (float, float, double, float)[]
				{
					(0.5f, 0.5f, 0, 1f),
					(0.25f, 0.75f, 76, 1f),
					(0f, 1f, 160, 1f)
				}, eHapticType.CONSTANT),
			},
			intensity, eHapticRegion.TORSO, pulseFace, additionalRotation);
		}
	}
}