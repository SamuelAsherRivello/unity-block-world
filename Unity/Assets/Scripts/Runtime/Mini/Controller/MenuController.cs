using RMC.Audio;
using RMC.Mini.Controller;
using RMC.Mini.Features.SceneSystem;
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
    public class MenuController: BaseController // Extending 'base' is optional
        <BlockWorldModel, MenuView, LocalDiskStorageService> 
    {
        public MenuController(
            BlockWorldModel model, MenuView view, LocalDiskStorageService service) 
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
                _view.OnPlay.AddListener(View_OnPlay);
                _view.OnCustomizeCharacter.AddListener(View_OnCustomizeCharacter);
                _view.OnCustomizeEnvironment.AddListener(View_OnCustomizeEnvironment);
                Context.CommandManager.AddCommandListener<LoadSceneRequestCommand>(OnLoadSceneRequestCommand);
      
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
        public override void Dispose()
        {
            base.Dispose();
            Context.CommandManager.RemoveCommandListener<LoadSceneRequestCommand>(OnLoadSceneRequestCommand);
        }


        //  Event Handlers --------------------------------
        private void OnLoadSceneRequestCommand(LoadSceneRequestCommand loadSceneRequestCommand)
        {
            //Note: its optional to observe this command and toggle off the UI
            //THis is just a demo of how to do it
            _view.PlayGameButton.SetEnabled(false);
            _view.CustomizeCharacterButton.SetEnabled(false);
            _view.CustomizeEnvironmentButton.SetEnabled(false);
        }
        
        private async void View_OnCustomizeCharacter()
        {
            // Wait for button click
            await AudioManager.Instance.WhileIsPlayingAsync();
            
            // Change scene
            
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(BlockWorldConstants.Scene02_CustomizeCharacter));
        }
        

        private async void View_OnCustomizeEnvironment()
        {
            // Wait for button click
            await AudioManager.Instance.WhileIsPlayingAsync();
            
            // Change scene
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(BlockWorldConstants.Scene03_CustomizeEnvironment));
        }
        
        private async void View_OnPlay()
        {
            // Wait for button click
            await AudioManager.Instance.WhileIsPlayingAsync();
            
            // Change scene
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(BlockWorldConstants.Scene04_Game));
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