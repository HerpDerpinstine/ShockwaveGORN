using System.IO;
using MelonLoader.Preferences;
using ShockwaveGORN.Setup.ConfigModels;

namespace ShockwaveGORN.Setup.Devices
{
    internal class I_Arm<T, L> where T : CM_Toggle, new() where L : new()
    {
        private bool IsUpper;
        private bool IsLeft;
        private MelonPreferences_ReflectiveCategory GeneralCategory;
        private MelonPreferences_ReflectiveCategory VelocityScalingCategory;
        internal T General;
        internal L VelocityScaling;

        internal I_Arm(bool is_upper, bool is_left, string basefolder, string className)
        {
            IsUpper = is_upper;
            IsLeft = is_left;
            
            string ArmName = $"{(IsUpper ? "Upper" : "Lower")}Arm{(IsLeft ? "L" : "R")}";
            basefolder = Path.Combine(basefolder, ArmName);

            General = Config.SetupCategory<T>(ref GeneralCategory, "General", basefolder, className);
            VelocityScaling = Config.SetupCategory<L>(ref VelocityScalingCategory, "VelocityScaling", basefolder, className);
        }

        internal bool IsEnabled()
            => (IsLeft
            ? (IsUpper ? Config.HapticDevices.UpperArmL : Config.HapticDevices.LowerArmL)
            : (IsUpper ? Config.HapticDevices.UpperArmR : Config.HapticDevices.LowerArmR))
            && General.Enabled;
    }
}