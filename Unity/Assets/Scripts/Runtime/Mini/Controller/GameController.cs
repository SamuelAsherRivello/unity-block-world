using RMC.Mini.Controller;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Service;
using RMC.BlockWorld.Mini.Service.Storage;
using RMC.BlockWorld.Mini.View;
using RMC.Mini;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RMC.BlockWorld.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class GameController: BaseController // Extending 'base' is optional
        <BlockWorldModel, GameView, LocalDiskStorageService> 
    {
        public GameController(
            BlockWorldModel model, GameView view, LocalDiskStorageService service) 
            : base(model, view, service)
        {
        }

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Load the data as needed
                _service.OnLoadCompleted.AddListener(Service_OnLoadCompleted);
                if (!_model.HasLoadedService.Value)
                {
                    _service.Load();
                }
                else
                {
                    Service_OnLoadCompleted(null);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _service.OnLoadCompleted.RemoveListener(Service_OnLoadCompleted);
        }


        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
        private void Service_OnLoadCompleted(LocalDiskStorageServiceDto localDiskStorageServiceDto)
        {
            RequireIsInitialized();
            _model.HasLoadedService.Value = true;
            
            if (localDiskStorageServiceDto != null)
            {
                // Set FROM the saved data. Don't save again here.
                _model.CharacterData.Value = localDiskStorageServiceDto.CharacterData;
                _model.EnvironmentData.Value = localDiskStorageServiceDto.EnvironmentData;
            }
            else
            {
                _model.CharacterData.OnValueChangedRefresh();
                _model.EnvironmentData.OnValueChangedRefresh();
            }
        }
    }
}