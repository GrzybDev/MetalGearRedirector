using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MelonLoader;

namespace MetalGearRedirector;

/// <summary>Registers the mod with MelonLoader and reads its settings, which its patch of the launcher applies.</summary>
public class Core : MelonMod
{
    private static MelonPreferences_Entry<string> _host;
    private static MelonPreferences_Entry<string> _language;

    /// <inheritdoc/>
    public override void OnInitializeMelon()
    {
        var settings = MelonPreferences.CreateCategory(nameof(MetalGearRedirector));
        _host = settings.CreateEntry("Host", BuildDefault("ManualHost"));
        _language = settings.CreateEntry("Language", BuildDefault("ManualLanguage"));
    }

    // The defaults come from the build configuration, so that other builds can point elsewhere.
    private static string BuildDefault(string key) =>
        typeof(Core).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(metadata => metadata.Key == key).Value;

    [HarmonyPatch]
    private static class ManualLinkPatch
    {
        private static MethodInfo TargetMethod() =>
            AccessTools.Method(Type.GetType(ManualLink.BuilderType, true), ManualLink.BuilderMethod);

        private static void Postfix(ref string __result) =>
            __result = ManualLink.Redirect(__result, _host.Value, _language.Value);
    }
}
