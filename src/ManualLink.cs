namespace MetalGearRedirector;

/// <summary>Where the launcher builds its manual links and how the mod redirects them.</summary>
internal static class ManualLink
{
    /// <summary>The launcher's type that builds the manual links, named so that the mod builds without the game.</summary>
    internal const string BuilderType = "DefManager, Assembly-CSharp";

    /// <summary>The method of <see cref="BuilderType"/> that returns the link to the selected game's manual.</summary>
    internal const string BuilderMethod = "GetURLManual";

    /// <summary>Points a manual link to another host, and the English manual to another language.</summary>
    /// <param name="link">The link the launcher built.</param>
    /// <param name="host">The host that serves the manuals in place of metalgear.konami.net.</param>
    /// <param name="language">The language code of the manual opened in place of the English one.</param>
    internal static string Redirect(string link, string host, string language) =>
        link.Replace("metalgear.konami.net", host).Replace("/en/", $"/{language}/");
}
