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
    public class DeveloperConsoleFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Get from mini
            BlockWorldModel model = MiniMvcs.ModelLocator.GetItem<BlockWorldModel>();
            LocalDiskStorageService service = MiniMvcs.ServiceLocator.GetItem<LocalDiskStorageService>();
            
            // Create new controller
            DeveloperConsoleController controller = 
                new DeveloperConsoleController(model, View as DeveloperConsoleView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            
            // Set Mode
            model.HasNavigationBack.Value = true;
            model.HasNavigationDeveloperConsole.Value = false;
        }

        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<DeveloperConsoleController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<DeveloperConsoleController>();
            }
            
            if (MiniMvcs.ViewLocator.HasItem<DeveloperConsoleView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<DeveloperConsoleView>();
            }
        }
    }
}