// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.LocalizationHandler.cs
// Created on: 20221119
// -----------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Pony.Localization;

/// <summary>
///     Class for handling languages
/// </summary>
public static class LocalizationHandler
{
    private static CultureInfo _cul;

    private static ResourceManager _resMrg;

    public enum Language
    {
        English,
        Dutch,
        Chinese
    }

    static LocalizationHandler()
    {
        SetLanguage(Language.English);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static Dictionary<Language, string> Languages { get; } = new()
    {
        {Language.English, "en-US"}, {Language.Dutch, "nl-NL"}, {Language.Chinese, "zh-cn"}
    };

    /// <summary>
    ///     Get a specific resource string for the current language
    /// </summary>
    public static string GetString(string resName) => _resMrg.GetString(resName, _cul);

    public static void SetLanguage(Language lang)
    {
        string dictionaryName = $"{Assembly.GetExecutingAssembly().GetName().Name}.Localization.{Languages[lang]}";
        _resMrg = new ResourceManager(dictionaryName, Assembly.GetExecutingAssembly());
        _cul    = new CultureInfo(Languages[lang]);
    }
}