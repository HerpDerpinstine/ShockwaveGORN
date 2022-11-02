using System.IO;
using MelonLoader;
using ShockwaveGORN.Hooks;
using ShockwaveGORN.Managers;
using ShockwaveGORN.Haptics;

namespace ShockwaveGORN
{
    public class ShockwaveGORN : MelonMod
	{
        internal static HarmonyLib.Harmony ModHarmony = null;
        internal static MelonLogger.Instance Logger = null; 
        internal static string BaseUserDataDirectory = null;

        public override void OnInitializeMelon()
        {
            ModHarmony = HarmonyInstance;
            Logger = LoggerInstance;

            BaseUserDataDirectory = Path.Combine(MelonUtils.UserDataDirectory, "Shockwave");
            if (!Directory.Exists(BaseUserDataDirectory))
                Directory.CreateDirectory(BaseUserDataDirectory);

            string nativeLibraryPath = Path.Combine(MelonUtils.GameDirectory, "ShockWaveIMU.dll");
            if (File.Exists(nativeLibraryPath))
                File.Delete(nativeLibraryPath);
            File.WriteAllBytes(nativeLibraryPath, Properties.Resources.ShockWaveIMU);

            Config.I_General.Init();
            Config.I_HapticDevices.Init();
            Config.I_HapticEffects.Init();
            M_HapticFeedback.Setup();

            H_Bow.Initialize();
            H_CrossbowCaestus.Initialize();
            H_DamageRelay.Initialize();
            H_DamagerRigidbody.Initialize();
            H_GameController.Initialize();
            H_Gong.Initialize();
            H_GrabHand.Initialize();
            H_Grapple.Initialize();
            H_Gun.Initialize();
            H_PlayerDamageRelay.Initialize();
            H_SurpriseBox.Initialize();

            HapticPlayback.Instance.BeginInit();

            Logger?.Msg("Initialized!");
        }

        public override void OnApplicationQuit()
            => HapticPlayback.Instance.EndInit();
    }
}