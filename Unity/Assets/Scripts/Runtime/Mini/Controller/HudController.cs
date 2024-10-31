using RMC.Mini.Controller;
using RMC.Mini.Features.SceneSystem;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Model.Data;
using RMC.BlockWorld.Mini.Service;
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
    public class HudController: BaseController // Extending 'base' is optional
        <ConfiguratorModel, HudView, ConfiguratorService> 
    {
        public HudController(
            ConfiguratorModel model, HudView view, ConfiguratorService service) 
            : base(model, view, service)
        {
            
        }

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                _view.OnReset.AddListener(View_OnReset);
                _view.OnQuit.AddListener(View_OnQuit);
                _view.OnBack.AddListener(View_OnBack);
                _view.OnDeveloperConsole.AddListener(View_OnDeveloperConsole);
            }
        }

        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private void View_OnQuit()
        {
            RequireIsInitialized();
            
#if UNITY_EDITOR
            Debug.Log("Application.Quit() applies only to builds.");
#else
            Application.Quit();
#endif
            
        }
        
        
        private void View_OnReset()
        {
            RequireIsInitialized();
            
            //Reload current scene
            //Great workflow for development
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        
        private void View_OnBack()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(ConfiguratorConstants.Scene01_Menu));
        }
        
        
        private void View_OnDeveloperConsole()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(ConfiguratorConstants.Scene05_DeveloperConsole));
        }
    }
}