using System;
using RMC.Mini.View;
using RMC.BlockWorld.Mini.Controller;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Model.Data;
using RMC.BlockWorld.Standard.Objects;
using RMC.Mini;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Gameplay_Environment = RMC.BlockWorld.Standard.Objects.Environment;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.BlockWorld.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="DeveloperConsoleController"/>
    /// 
    /// </summary>
    public class DeveloperConsoleView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnReset = new UnityEvent();
        
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public Button ResetButton { get { return _uiDocument?.rootVisualElement.Q<Button>("ResetButton"); }}
        public Label StatusLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("StatusLabel"); }}
        public Label SubstatusLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("SubstatusLabel"); }}
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        [Header("Gameplay")]
        [SerializeField] 
        private Gameplay_Environment _environment;

        [SerializeField] 
        private Player _player;

        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ResetButton.clicked += ResetButtonOnClicked;
                
                ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
                model.CharacterData.OnValueChanged.AddListener(CharacterData_OnValueChanged);
                model.EnvironmentData.OnValueChanged.AddListener(EnvironmentData_OnValueChanged);
                RefreshUI();
                
            }
        }


        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Unity Methods ---------------------------------
        protected void OnDestroy()
        {
            ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
            if (model == null)
            {
                return;
            }
            model.CharacterData.OnValueChanged.RemoveListener(CharacterData_OnValueChanged);
            model.EnvironmentData.OnValueChanged.RemoveListener(EnvironmentData_OnValueChanged);
            
            // Optional: Handle any cleanup here...
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
            ResetButton.SetEnabled(model.HasLoadedService.Value && 
                                   (!model.CharacterDataIsDefault() || !model.EnvironmentDataIsDefault()));
            
            StatusLabel.text = $"Reset The\nColors";
            SubstatusLabel.text = $"Keys:\nUse [R] to reload any Scene\nUse [Q] to quit from any Scene";
        }
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        
        //  Event Handlers --------------------------------
        private void CharacterData_OnValueChanged(CharacterData previousValue, CharacterData currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
            
            _player.CharacterData = currentValue;
        }
        
        private void EnvironmentData_OnValueChanged(EnvironmentData previousValue, EnvironmentData currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
            _environment.EnvironmentData = currentValue;
        }
        
        private void ResetButtonOnClicked()
        {
            OnReset.Invoke();
            RefreshUI();
        }
    }
}