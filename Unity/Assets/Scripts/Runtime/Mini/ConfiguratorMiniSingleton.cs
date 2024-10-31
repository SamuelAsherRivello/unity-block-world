using RMC.Core.DesignPatterns.Creational.SingletonMonobehaviour;
using UnityEngine;

namespace RMC.BlockWorld.Mini
{
    /// <summary>
    /// Here is a <see cref="ConfiguratorMiniSingleton"/> that can be used to access the <see cref="ConfiguratorMini"/>.
    /// Replace this with your own custom implementation.
    /// </summary>
    public class ConfiguratorMiniSingleton: Singleton<ConfiguratorMiniSingleton>
    {
        //  Fields ----------------------------------------
        public ConfiguratorMini ConfiguratorMini { get; set; }
       

        //  Unity Methods ---------------------------------
        public override void OnInitialized()
        {
            base.OnInitialized();
            
            // Create the mini as you typically do  
            // Store the mini on this singleton for easy access
            ConfiguratorMini = new ConfiguratorMini();
            ConfiguratorMini.Initialize();
        }
    }
}