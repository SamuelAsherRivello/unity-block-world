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
    public class Scene01_Menu : MonoBehaviour
    {
        //  Fields ----------------------------------------
        [SerializeField] 
        private Common _common;
       
        [SerializeField] 
        private MenuView _menuView;
        
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
            BlockWorldMini mini = BlockWorldMiniSingleton.Instance.BlockWorldMini;
            
            //  Scene-Specific ----------------------------
            MenuFeature menuFeature = new MenuFeature();
            menuFeature.AddView(_menuView);
            mini.AddFeature<MenuFeature>(menuFeature);
            
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
            if (BlockWorldMiniSingleton.IsShuttingDown)
            {
                return;
            }
            
            BlockWorldMini mini = BlockWorldMiniSingleton.Instance.BlockWorldMini;
            
            if (mini == null)
            {
                return;
            }
            
            //  Scene-Specific ----------------------------
            mini.RemoveAndDisposeFeature<MenuFeature>();
            
            //  Scene-Agnostic ----------------------------
            mini.RemoveAndDisposeFeature<HudFeature>();
        }
        
        //  Event Handlers --------------------------------
    }
}