﻿using RMC.Mini.Features;
using RMC.BlockWorld.Mini.Controller;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Service;
using RMC.BlockWorld.Mini.View;

namespace RMC.BlockWorld.Mini.Feature
{
    /// <summary>
    /// Set up a collection of related <see cref="IConcern"/> instances
    /// </summary>
    public class MenuFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Get from mini
            ConfiguratorModel model = MiniMvcs.ModelLocator.GetItem<ConfiguratorModel>();
            ConfiguratorService service = MiniMvcs.ServiceLocator.GetItem<ConfiguratorService>();
            
            // Create new controller
            MenuController controller = 
                new MenuController(model, View as MenuView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            
            // Set Mode
            model.HasNavigationBack.Value = false;
            model.HasNavigationDeveloperConsole.Value = true;
        }

        
        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ViewLocator.HasItem<MenuView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<MenuView>();
            }
            
            if (MiniMvcs.ControllerLocator.HasItem<MenuController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<MenuController>();
            }
        }
    }
}