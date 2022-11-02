using System.IO;
using MelonLoader.Preferences;
using ShockwaveGORN.Setup.ConfigModels;

namespace ShockwaveGORN.Setup.Devices
{
    internal class I_Torso<T, L> where T : CM_Toggle, new() where L : new()
    {
        private MelonPreferences_ReflectiveCategory GeneralCategory;
        private MelonPreferences_ReflectiveCategory VelocityScalingCategory;
        internal T General;
        internal L VelocityScaling;
        internal I_Torso(string basefolder, string className)
        {
            basefolder = Path.Combine(basefolder, "Torso");

            General = Config.SetupCategory<T>(ref GeneralCategory, "General", basefolder, className);
            VelocityScaling = Config.SetupCategory<L>(ref VelocityScalingCategory, "VelocityScaling", basefolder, className);
        }

        internal bool IsEnabled()
            => (Config.HapticDevices.VestFront
            && Config.HapticDevices.VestBack
            && General.Enabled);
    }
}
