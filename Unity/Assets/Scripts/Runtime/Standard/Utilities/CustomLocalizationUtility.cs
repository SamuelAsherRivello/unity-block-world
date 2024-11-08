
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace RMC.BlockWorld.Standard
{
    public static class CustomLocalizationUtility
    {
        //  Fields ----------------------------------------
        
        //  Methods ---------------------------------------
        
        /// <summary>
        /// TODO: This is experimental. I'm not sure this is a proper
        /// idea to load this way. It's a bit of a hack.
        /// NOTE: Does NOT yet fix the 0.25-ish second flicker of text upon startup
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void RuntimeInitializeOnLoadMethod()
        {
            await EnsureInitializedAsync();
            await SetSelectedLocaleToIndexAsync(0);
        }
        
        
        private static async Task EnsureInitializedAsync()
        {
            while (!LocalizationSettings.InitializationOperation.IsDone)
            {
                await Task.Yield(); 
            }
        }
        
        
        public static async Task<int> GetAvailableLocalesCountAsync ()
        {
            await EnsureInitializedAsync();
            return LocalizationSettings.AvailableLocales.Locales.Count;
        }

        public static async Task SetSelectedLocaleToIndexAsync(int index)
        {
            await EnsureInitializedAsync();
            
            var locales = LocalizationSettings.AvailableLocales.Locales;

            if (locales.Count == 0)
                return;

            LocalizationSettings.SelectedLocale = locales[index];
    
        }
        
        public static async Task SetSelectedLocaleToNextAsync()
        {
            await EnsureInitializedAsync();
            
            var locales = LocalizationSettings.AvailableLocales.Locales;

            if (locales.Count == 0)
                return;

            var currentLocale = LocalizationSettings.SelectedLocale;
            int currentIndex = locales.IndexOf(currentLocale);

            // Calculate the index of the next locale
            int nextIndex = (currentIndex + 1) % locales.Count;
            LocalizationSettings.SelectedLocale = locales[nextIndex];
    
        }
    }
}
