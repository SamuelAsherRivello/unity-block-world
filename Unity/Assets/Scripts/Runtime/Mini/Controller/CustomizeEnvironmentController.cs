using RMC.Mini.Controller;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Model.Data;
using RMC.BlockWorld.Mini.Service;
using RMC.BlockWorld.Mini.Service.Storage;
using RMC.BlockWorld.Mini.View;
using RMC.Mini;

namespace RMC.BlockWorld.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class CustomizeEnvironmentController: BaseController // Extending 'base' is optional
        <BlockWorldModel, CustomizeEnvironmentView, LocalDiskStorageService> 
    {
        public CustomizeEnvironmentController(
            BlockWorldModel model, CustomizeEnvironmentView view, LocalDiskStorageService service) 
            : base(model, view, service)
        {
        }

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                //
                _view.OnRandomize.AddListener(View_OnRandomize);

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


        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
        private void View_OnRandomize()
        {
            RequireIsInitialized();

            // Set from Random. Then save here.
            _model.EnvironmentData.Value = EnvironmentData.FromRandomValues();
            _service.SaveEnvironmentData(_model.EnvironmentData.Value);
        }
        
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