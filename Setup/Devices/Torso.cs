using System.IO;
using MelonLoader.Preferences;
using ShockwaveGORN.Setup.ConfigModels;

namespace ShockwaveGORN.Setup.Devices
{
    internal class I_Torso<T> where T : CM_Toggle, new()
    {
        private MelonPreferences_ReflectiveCategory GeneralCategory;
        internal T General;

        internal I_Torso(string basefolder, string className)
        {
            basefolder = Path.Combine(basefolder, "Torso");

            General = Config.SetupCategory<T>(ref GeneralCategory, "General", basefolder, className);
        }

        internal bool IsEnabled()
            => (Config.HapticDevices.VestFront 
            && Config.HapticDevices.VestBack 
            && General.Enabled);
    }
}
