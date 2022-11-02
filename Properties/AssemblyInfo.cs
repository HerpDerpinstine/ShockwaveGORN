using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(ShockwaveGORN.BuildInfo.Name)]
[assembly: AssemblyCompany(ShockwaveGORN.BuildInfo.Company)]
[assembly: AssemblyProduct(ShockwaveGORN.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + ShockwaveGORN.BuildInfo.Author)]
[assembly: AssemblyTrademark(ShockwaveGORN.BuildInfo.Company)]
[assembly: Guid("8F18FDDA-4605-4566-992B-C1BB16349FA7")]
[assembly: AssemblyVersion(ShockwaveGORN.BuildInfo.Version)]
[assembly: AssemblyFileVersion(ShockwaveGORN.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(ShockwaveGORN.ShockwaveGORN), ShockwaveGORN.BuildInfo.Name, ShockwaveGORN.BuildInfo.Version, ShockwaveGORN.BuildInfo.Author, ShockwaveGORN.BuildInfo.DownloadLink)]
[assembly: MelonGame("Free Lives", "GORN")]
[assembly: VerifyLoaderVersion("0.5.7", true)]