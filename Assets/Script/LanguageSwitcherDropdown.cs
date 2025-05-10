using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using TMPro; // Jika Anda menggunakan TextMeshPro untuk teks Dropdown

public class LanguageSwitcherDropdown : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // Atau UnityEngine.UI.Dropdown jika tidak menggunakan TMP

    private void Start()
    {
        if (languageDropdown != null)
        {
            // Bersihkan opsi dropdown yang ada
            languageDropdown.ClearOptions();

            // Tambahkan opsi bahasa dari AvailableLocales
            foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                languageDropdown.AddOptions(new System.Collections.Generic.List<TMP_Dropdown.OptionData> { new TMP_Dropdown.OptionData(locale.Identifier.CultureInfo.DisplayName) });
            }

            // Tetapkan bahasa awal ke bahasa yang saat ini dipilih
            if (LocalizationSettings.SelectedLocale != null)
            {
                int index = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
                languageDropdown.SetValueWithoutNotify(index);
            }

            // Tambahkan listener untuk menangani perubahan nilai dropdown
            languageDropdown.onValueChanged.AddListener(ChangeLanguage);
        }
        else
        {
            Debug.LogError("Dropdown bahasa tidak ditemukan!");
        }
    }

    public void ChangeLanguage(int languageIndex)
    {
        if (languageIndex >= 0 && languageIndex < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
        }
    }
}