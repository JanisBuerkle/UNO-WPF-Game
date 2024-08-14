using System;
using System.Globalization;
using System.Resources;
using UNO_Spielprojekt.Resources;
using UNO_Spielprojekt.Setting;

namespace UNO_Spielprojekt.Localization;

public static class LocalizationManager
{
    private static ResourceManager resourceManager;

    public static void SetLanguage(CultureInfo culture)
    {
        resourceManager = new ResourceManager("UNO_Spielprojekt.Resources.Resource", typeof(Resource).Assembly);

        var localizedResourceSet = resourceManager.GetResourceSet(culture, true, true);

        if (localizedResourceSet != null)
        {
            Console.WriteLine($@"Resources loaded for culture: {culture.Name}");
        }
        else
        {
            Console.WriteLine($@"No resources found for culture: {culture.Name}");
        }
    }

    public static string? GetLocalizedString(string key)
    {
        resourceManager ??= new ResourceManager("UNO_Spielprojekt.Resources.Resource", typeof(Resource).Assembly);

        return SettingsView.language != null ? resourceManager.GetString(key, SettingsView.language.LangCulture) : "DefaultLocalizedString";
    }
}