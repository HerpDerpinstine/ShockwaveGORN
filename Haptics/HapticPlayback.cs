using System;
using System.Threading;
using UnityEngine;

namespace ShockwaveGORN.Haptics
{
    internal class HapticPlayback : ThreadedTask
    {
		private static int HapticFeedbackDurationMillis = 10;
		private ThreadSafeQueue<HapticTask> TaskQueue = new ThreadSafeQueue<HapticTask>();
        private ThreadSafeQueue<HapticTask> TempTaskQueue = new ThreadSafeQueue<HapticTask>();
        private bool ShouldRun;

        private static HapticPlayback _instance;
        internal static HapticPlayback Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HapticPlayback();
                return _instance;
            }
        }

        internal override bool BeginInitInternal()
        {
            if (IsAlive())
                EndInit();

			ShockwaveManager.Instance.SetMaximumIntensity(4f);
			ShockwaveManager.Instance.MaximumIntensity = 4f;
			ShockwaveManager.Instance.IntensityDropOff = 0;
			ShockwaveManager.Instance.enableBodyTracking = false;

			ShockwaveManager.Instance.InitializeSuit();
			ShockwaveManager.Instance.InitSequence();

			ShockwaveManager.Instance.StopPositionComputation();

			ShouldRun = true;
            return true;
        }

        internal override bool EndInitInternal()
        {
            if (!IsAlive())
                return false;

            ShouldRun = false;
            while (IsAlive()) { Thread.Sleep(1); }

            TaskQueue.Clear();
            TempTaskQueue.Clear();
			
            ShockwaveManager.Instance.DisconnectSuit();

			return true;
        }

        internal override void WithinThread()
        {
            while (ShouldRun)
			{
				if (ShockwaveManager.Instance.Ready)
				{
					HapticTask task;

					while ((task = TempTaskQueue.Dequeue()) != null)
						TaskQueue.Enqueue(task);

					while ((task = TaskQueue.Dequeue()) != null)
						if (task.Invoke())
							TempTaskQueue.Enqueue(task);

					while ((task = TempTaskQueue.Dequeue()) != null)
						TaskQueue.Enqueue(task);
				}

				if (ShouldRun)
                    Thread.Sleep(1);
            }
        }

		private static void Enqueue(Func<DateTime, DateTime, bool> callback)
            => Instance.TempTaskQueue.Enqueue(new HapticTask(callback));

		// (Index, Intensity)[], Time
		internal static void PlayDotPattern(((int, float)[], double)[] pattern,
			float intensity,
			eHapticRegion region,
			eHapticSide side,
			double startDelay = 0)
		{
			if ((side == eHapticSide.FRONT) || (side == eHapticSide.BOTH))
				DotPulse(pattern, intensity, region, false, startDelay);
			if ((side == eHapticSide.BACK) || (side == eHapticSide.BOTH))
				DotPulse(pattern, intensity, region, true, startDelay);
		}

		// (Index, Intensity)[], Time
		private static void DotPulse(((int, float)[], double)[] pattern,
			float intensity,
			eHapticRegion region,
			bool backwards,
			double startDelay)
		{
			double totalDuration = 0;
			foreach (var point in pattern)
				if (point.Item2 > totalDuration)
					totalDuration = point.Item2;

			if (startDelay < 0)
				startDelay = 0;

			int pointsLength = pattern.Length;
			Enqueue((DateTime creationTime, DateTime invokeTime) =>
			{
				double timeSpent = (invokeTime - creationTime).TotalMilliseconds;
				if (timeSpent < startDelay)
					return true;
				timeSpent -= startDelay;

				for (int i = 0; i < pointsLength; i++)
				{
					var currentPoint = pattern[i];
					if (timeSpent > currentPoint.Item2)
						continue;

					foreach (var ind in currentPoint.Item1)
					{
						int index = ind.Item1;
						
						// Congeald Torso
						if ((region == eHapticRegion.TORSO)
										&& (index >= 8)
										&& (index <= 11))
						{
							ShockwaveManager.Instance.sendHapticsPulse(
								ShockwaveTranslator.Index(index - 4, region, backwards),
								intensity,
								HapticFeedbackDurationMillis);

							index += 4;
						}

						ShockwaveManager.Instance.sendHapticsPulse(
							ShockwaveTranslator.Index(index, region, backwards),
							intensity,
							HapticFeedbackDurationMillis);
					}
					break;
				}

				return (timeSpent <= totalDuration);
			});
		}

		// (X, Y, Time, IntensityScale)[], PulseType
		internal static void PlayPathPattern(((float, float, double, float)[], eHapticType)[] pattern,
			float intensity,
			eHapticRegion region,
			eHapticSide side,
			double startDelay = 0)
			=> PlayPathPattern(pattern, intensity, region, side, (0f, 0f), startDelay);

		// (X, Y, Time, IntensityScale)[], PulseType
		internal static void PlayPathPattern(((float, float, double, float)[], eHapticType)[] pattern,
			float intensity,
			eHapticRegion region,
			eHapticSide side,
			(float, float) additionalRotation,
			double startDelay = 0)
		{
			foreach (var pointsPair in pattern)
			{
				if ((side == eHapticSide.FRONT) || (side == eHapticSide.BOTH))
					PathPulse(pointsPair, intensity, region, false, additionalRotation, startDelay);
				if ((side == eHapticSide.BACK) || (side == eHapticSide.BOTH))
					PathPulse(pointsPair, intensity, region, true, additionalRotation, startDelay);
			}
		}

		// (X, Y, Time, IntensityScale)[], PulseType
		private static void PathPulse(((float, float, double, float)[], eHapticType) pointsPair,
			float intensity,
			eHapticRegion region,
			bool backwards,
			(float, float) additionalRotation,
			double startDelay)
		{
			GameController.Player.GetBodyForwardPos(out Vector3 centerPosition, out (Vector3, Vector3, Vector3) directions, out (float, float) size);

			Vector3 colliderPos = centerPosition;

			if (region == eHapticRegion.TORSO)
				colliderPos = centerPosition.CenterBodyPosToTorso(directions.Item3, size.Item2);

			var posBounds = colliderPos.GetHapticBounds(directions, size, backwards);

			double pointsTotalDuration = 0;
			foreach (var point in pointsPair.Item1)
				if (point.Item3 > pointsTotalDuration)
					pointsTotalDuration = point.Item3;

			if (startDelay < 0)
				startDelay = 0;

			int pointsLength = pointsPair.Item1.Length;
			Enqueue((DateTime creationTime, DateTime invokeTime) =>
			{
				double timeSpent = (invokeTime - creationTime).TotalMilliseconds;
				if (timeSpent < startDelay)
					return true;
				timeSpent -= startDelay;

				float playbackIntensity = ApplyIntensityFade(pointsPair.Item2, intensity, timeSpent, pointsTotalDuration);

				for (int i = 0; i < pointsLength; i++)
				{
					if ((i + 1) >= pointsLength)
						continue;

					var currentPoint = pointsPair.Item1[i];
					if (timeSpent < currentPoint.Item3)
						continue;

					var nextPoint = pointsPair.Item1[i + 1];
					if (timeSpent > nextPoint.Item3)
						continue;

					Vector3 currentPointPos = posBounds.GetPositionFromPoint((currentPoint.Item1, currentPoint.Item2));
					Vector3 nextPointPos = posBounds.GetPositionFromPoint((nextPoint.Item1, nextPoint.Item2));

					double pointToPointDuration = nextPoint.Item3 - currentPoint.Item3;
					double spentToPointDuration = timeSpent - currentPoint.Item3;
					float pointToPointRatio = (float)(spentToPointDuration / pointToPointDuration);

					Vector3 pointToPointDistance = nextPointPos - currentPointPos;
					Vector3 contactPos = currentPointPos + (pointToPointDistance * pointToPointRatio);

					(float, float) angle = contactPos.GetPositionAngle(centerPosition, directions.Item1);
					angle.Item1 += additionalRotation.Item1;
					angle.Item2 += additionalRotation.Item2;
					angle.AngleClamp();

					ShockwaveManager.Instance.sendHapticsPulsewithPositionInfo(
						ShockwaveTranslator.Region(region),
						intensity,
						angle.Item1,
						angle.Item2,
						size.Item2,
						HapticFeedbackDurationMillis);
				}

				return (timeSpent <= pointsTotalDuration);
			});
		}

		private static float ApplyIntensityFade(eHapticType pulseType, float intensity, double timeSpent, double duration)
		{
			float ratio = (float)(timeSpent / duration);
			float newIntensity = intensity;
			switch (pulseType)
			{
				case eHapticType.FADE_IN:
					newIntensity = intensity * ratio;
					goto default;

				case eHapticType.FADE_OUT:
					newIntensity = intensity * (1f - ratio);
					goto default;

				case eHapticType.FADE_IN_AND_OUT:
					if (ratio >= 0.5f)
						goto case eHapticType.FADE_OUT;
					goto case eHapticType.FADE_IN;

				default:
					break;
			}
			return newIntensity;
		}

		private class HapticTask
        {
            private DateTime CreationTime;
            private Func<DateTime, DateTime, bool> Callback;

            internal HapticTask(Func<DateTime, DateTime, bool> callback)
            {
                CreationTime = DateTime.Now;
                Callback = callback;
			}

            internal bool Invoke()
                => Callback.Invoke(CreationTime, DateTime.Now);
        }
    }
}
