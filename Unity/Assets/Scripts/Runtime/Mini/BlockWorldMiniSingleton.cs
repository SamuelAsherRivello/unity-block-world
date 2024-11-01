using RMC.Core.DesignPatterns.Creational.SingletonMonobehaviour;
using UnityEngine;

namespace RMC.BlockWorld.Mini
{
    /// <summary>
    /// Here is a <see cref="BlockWorldMiniSingleton"/> that can be used to access the <see cref="BlockWorldMini"/>.
    /// Replace this with your own custom implementation.
    /// </summary>
    public class BlockWorldMiniSingleton: Singleton<BlockWorldMiniSingleton>
    {
        //  Fields ----------------------------------------
        public BlockWorldMini BlockWorldMini { get; set; }
       

        //  Unity Methods ---------------------------------
        public override void OnInitialized()
        {
            base.OnInitialized();
            
            // Create the mini as you typically do  
            // Store the mini on this singleton for easy access
            BlockWorldMini = new BlockWorldMini();
            BlockWorldMini.Initialize();
        }
    }
}