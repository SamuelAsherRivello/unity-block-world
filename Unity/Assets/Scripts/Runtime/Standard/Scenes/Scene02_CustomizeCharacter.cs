using RMC.Mini.Features.SceneSystem;
using RMC.BlockWorld.Mini;
using RMC.BlockWorld.Mini.Feature;
using RMC.BlockWorld.Mini.View;
using UnityEngine;

namespace RMC.BlockWorld.Standard
{
    /// <summary>
    /// This is the main entry point to one of the scenes
    /// </summary>
    public class Scene02_CustomizeCharacter : MonoBehaviour
    {
        //  Fields ----------------------------------------
        [SerializeField] 
        private Common _common;
        
        [SerializeField] 
        private CustomizeCharacterView _customizeCharacterView;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            AddFeature();
        }
        
        protected void OnDestroy()
        {
            RemoveFeature();
            
            // Optional: Handle any cleanup here...
        }

        
        //  Methods ---------------------------------------
        private void AddFeature()
        {
            ConfiguratorMini mini = ConfiguratorMiniSingleton.Instance.ConfiguratorMini;
            
            //  Scene-Specific ----------------------------
            CustomizeCharacterFeature customizeCharacterFeature = new CustomizeCharacterFeature();
            customizeCharacterFeature.AddView(_customizeCharacterView);
            mini.AddFeature<CustomizeCharacterFeature>(customizeCharacterFeature);
            
            //  Scene-Agnostic ----------------------------
            if (!mini.HasFeature<SceneSystemFeature>())
            {
                SceneSystemFeature sceneSystemFeature = new SceneSystemFeature();
                mini.AddFeature<SceneSystemFeature>(sceneSystemFeature);
            }
            
            HudFeature hudFeature = new HudFeature();
            hudFeature.AddView(_common.HudView);
            mini.AddFeature<HudFeature>(hudFeature);
        }
        
        private void RemoveFeature()
        {
            if (ConfiguratorMiniSingleton.IsShuttingDown)
            {
                return;
            }
            
            ConfiguratorMini mini = ConfiguratorMiniSingleton.Instance.ConfiguratorMini;
            
            if (mini == null)
            {
                return;
            }
            
            //  Scene-Specific ----------------------------
            mini.RemoveAndDisposeFeature<CustomizeCharacterFeature>();
            
            //  Scene-Agnostic ----------------------------
            mini.RemoveAndDisposeFeature<HudFeature>();
        }
        
        //  Event Handlers --------------------------------
    }
}