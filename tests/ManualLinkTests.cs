using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using Xunit;

namespace MetalGearRedirector.Tests;

public class ManualLinkTests
{
    private const BindingFlags Statics = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

    [Theory]
    [InlineData("mg1_msx", "en", "xx")]
    [InlineData("mg2", "en", "xx")]
    [InlineData("mg2", "fr", "fr")]
    public void SendsEveryManualToTheHostAndTheEnglishOneToTheLanguage(string game, string language, string expected)
    {
        var link = $"https://metalgear.konami.net/manual/mc1/{game}/pc/{language}/index.html";

        Assert.Equal($"https://manuals.example/manual/mc1/{game}/pc/{expected}/index.html",
            ManualLink.Redirect(link, "manuals.example", "xx"));
    }

    [Fact]
    public void LauncherBuildsTheLinksTheRedirectExpects()
    {
        var folder = Environment.GetEnvironmentVariable("METALGEAR_GAME_FOLDER");
        Assert.SkipWhen(string.IsNullOrEmpty(folder), "METALGEAR_GAME_FOLDER does not name the game's folder");
        var managed = Path.Combine(folder, "launcher_Data", "Managed");
        AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
        {
            var path = Path.Combine(managed, new AssemblyName(args.Name).Name + ".dll");
            return File.Exists(path) ? Assembly.LoadFrom(path) : null;
        };

        var builder = Type.GetType(ManualLink.BuilderType, true);
        var buildLink = builder.GetMethod(ManualLink.BuilderMethod, Type.EmptyTypes);
        var launcher = FormatterServices.GetUninitializedObject(builder);
        // The launcher reads the selected game and its own language from static fields of this class.
        var selection = builder.Assembly.GetType("Def", true);
        var languageField = selection.GetField("languageLauncher", Statics);

        foreach (var (number, game) in new[] { (0, "mg1_msx"), (1, "mg2") })
        {
            foreach (var (language, code) in new[] { ("English", "en"), ("French", "fr") })
            {
                selection.GetField("SelectStartUpNumber", Statics).SetValue(null, number);
                languageField.SetValue(null, Enum.Parse(languageField.FieldType, language));

                Assert.Equal($"https://metalgear.konami.net/manual/mc1/{game}/pc/{code}/index.html",
                    buildLink.Invoke(launcher, null));
            }
        }
    }
}
