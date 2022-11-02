using ShockwaveGORN.Haptics;

namespace ShockwaveGORN
{
    internal static class ShockwaveTranslator
    {
		internal static ShockwaveManager.HapticRegion Region(eHapticRegion region)
        {
			switch (region)
            {
				case eHapticRegion.LEFT_UPPER_ARM:
					return ShockwaveManager.HapticRegion.LEFTUPPERARM;
				case eHapticRegion.LEFT_LOWER_ARM:
					return ShockwaveManager.HapticRegion.LEFTLOWERARM;
				case eHapticRegion.RIGHT_UPPER_ARM:
					return ShockwaveManager.HapticRegion.RIGHTUPPERARM;
				case eHapticRegion.RIGHT_LOWER_ARM:
					return ShockwaveManager.HapticRegion.RIGHTLOWERARM;
				case eHapticRegion.LEFT_UPPER_LEG:
					return ShockwaveManager.HapticRegion.LEFTUPPERLEG;
				case eHapticRegion.LEFT_LOWER_LEG:
					return ShockwaveManager.HapticRegion.LEFTLOWERLEG;
				case eHapticRegion.RIGHT_UPPER_LEG:
					return ShockwaveManager.HapticRegion.RIGHTUPPERLEG;
				case eHapticRegion.RIGHT_LOWER_LEG:
					return ShockwaveManager.HapticRegion.RIGHTLOWERLEG;

				case eHapticRegion.TORSO:
				default:
					return ShockwaveManager.HapticRegion.TORSO;
			}
        }

		private static int[] torso_front = new int[] { 16, 17, 22, 23 };
		private static int[] torso_back = new int[] { 18, 19, 20, 21 };
		internal static int Index(int index, eHapticRegion region, bool backwards)
		{
			ShockwaveManager.HapticGroup hapticGroup = ShockwaveManager.HapticGroup.ALL;
			switch (region)
			{
				case eHapticRegion.TORSO:
					index = CongealdTorsoIndex(index);
					return backwards ? torso_back[index] : torso_front[index];

				case eHapticRegion.LEFT_UPPER_ARM:
					hapticGroup = backwards ? ShockwaveManager.HapticGroup.LEFT_BICEP_BACK : ShockwaveManager.HapticGroup.LEFT_BICEP_FRONT;
					goto default;
				case eHapticRegion.LEFT_LOWER_ARM:
					hapticGroup = backwards ? ShockwaveManager.HapticGroup.LEFT_FOREARM_BACK : ShockwaveManager.HapticGroup.LEFT_FOREARM_FRONT;
					goto default;

				case eHapticRegion.RIGHT_UPPER_ARM:
					hapticGroup = backwards ? ShockwaveManager.HapticGroup.RIGHT_BICEP_BACK : ShockwaveManager.HapticGroup.RIGHT_BICEP_FRONT;
					goto default;
				case eHapticRegion.RIGHT_LOWER_ARM:
					hapticGroup = backwards ? ShockwaveManager.HapticGroup.RIGHT_FOREARM_BACK : ShockwaveManager.HapticGroup.RIGHT_FOREARM_FRONT;
					goto default;

				case eHapticRegion.LEFT_UPPER_LEG:
					hapticGroup = backwards ? ShockwaveManager.HapticGroup.LEFT_THIGH_BACK : ShockwaveManager.HapticGroup.LEFT_THIGH_FRONT;
					goto default;
				case eHapticRegion.LEFT_LOWER_LEG:
					hapticGroup = backwards ? ShockwaveManager.HapticGroup.LEFT_LEG_BACK : ShockwaveManager.HapticGroup.LEFT_LEG_FRONT;
					goto default;

				case eHapticRegion.RIGHT_UPPER_LEG:
					hapticGroup = backwards ? ShockwaveManager.HapticGroup.RIGHT_THIGH_BACK : ShockwaveManager.HapticGroup.RIGHT_THIGH_FRONT;
					goto default;
				case eHapticRegion.RIGHT_LOWER_LEG:
					hapticGroup = backwards ? ShockwaveManager.HapticGroup.RIGHT_LEG_BACK : ShockwaveManager.HapticGroup.RIGHT_LEG_FRONT;
					goto default;

				default:
					break;
			}

			return ShockwaveManager.ShockGroup(hapticGroup)[index];
		}

		private static int CongealdTorsoIndex(int index)
		{
			switch (index)
			{
				case 0:
				case 1:
				case 4:
				case 5:
					index = 0;
					goto default;

				case 2:
				case 3:
				case 6:
				case 7:
					index = 1;
					goto default;

				case 12:
				case 13:
				case 16:
				case 17:
					index = 2;
					goto default;

				case 14:
				case 15:
				case 18:
				case 19:
					index = 3;
					goto default;

				default:
					break;
			}
			return index;
		}
	}
}
