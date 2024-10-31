using RMC.Mini.Features;
using RMC.BlockWorld.Mini.Controller;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Service;
using RMC.BlockWorld.Mini.View;

namespace RMC.BlockWorld.Mini.Feature
{
    /// <summary>
    /// Set up a collection of related <see cref="IConcern"/> instances
    /// </summary>
    public class CustomizeEnvironmentFeature: BaseFeature // Extending 'base' is optional
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
            CustomizeEnvironmentController controller = 
                new CustomizeEnvironmentController(model, View as CustomizeEnvironmentView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            
            // Set Mode
            model.HasNavigationBack.Value = true;
            model.HasNavigationDeveloperConsole.Value = true;
        }

        
        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<CustomizeEnvironmentController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<CustomizeEnvironmentController>();
            }
            
            if (MiniMvcs.ViewLocator.HasItem<CustomizeEnvironmentView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<CustomizeEnvironmentView>();
            }
        }
    }
}