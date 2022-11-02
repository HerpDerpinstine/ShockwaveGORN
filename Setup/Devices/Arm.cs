using System.IO;
using MelonLoader.Preferences;
using ShockwaveGORN.Setup.ConfigModels;

namespace ShockwaveGORN.Setup.Devices
{
    internal class I_Arm<T> where T : CM_Toggle, new()
    {
        private bool IsUpper;
        private bool IsLeft;
        private MelonPreferences_ReflectiveCategory GeneralCategory;
        internal T General;
        
        internal I_Arm(bool is_upper, bool is_left, string basefolder, string className)
        {
            IsUpper = is_upper;
            IsLeft = is_left;
            string ArmName = $"{(IsUpper ? "Upper" : "Lower")}Arm{(IsLeft ? "L" : "R")}";
            basefolder = Path.Combine(basefolder, ArmName);

            General = Config.SetupCategory<T>(ref GeneralCategory, "General", basefolder, className);
        }

        internal bool IsEnabled()
            => (IsLeft
            ? (IsUpper ? Config.HapticDevices.UpperArmL : Config.HapticDevices.LowerArmL)
            : (IsUpper ? Config.HapticDevices.UpperArmR : Config.HapticDevices.LowerArmR))
            && General.Enabled;
    }
}
