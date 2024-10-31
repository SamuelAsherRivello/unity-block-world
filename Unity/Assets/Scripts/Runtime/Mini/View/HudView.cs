using System;
using RMC.Mini.View;
using RMC.BlockWorld.Mini.Controller;
using RMC.BlockWorld.Mini.Model;
using RMC.Mini;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.BlockWorld.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="HudController"/>
    /// 
    /// </summary>
    public class HudView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnBack = new UnityEvent();
        
        [HideInInspector] 
        public readonly UnityEvent OnDeveloperConsole = new UnityEvent();

        [HideInInspector] 
        public readonly UnityEvent OnQuit = new UnityEvent();
        
        [HideInInspector] 
        public readonly UnityEvent OnReset = new UnityEvent();
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        public Label StatusLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("StatusLabel"); }}
        public Button BackButton { get { return _uiDocument?.rootVisualElement.Q<Button>("BackButton"); }}
        public Button DeveloperConsoleButton { get { return _uiDocument?.rootVisualElement.Q<Button>("DeveloperConsoleButton"); }}
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        // Input
        private InputAction _quitInputAction;
        private InputAction _resetInputAction;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
                model.HasLoadedService.OnValueChanged.AddListener(ServiceHasLoaded_OnValueChanged);

                BackButton.clicked += BackButton_OnClicked;
                DeveloperConsoleButton.clicked += DeveloperConsoleButtonOnClicked;
                
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
        protected void Awake()
        {
            _quitInputAction = InputSystem.actions.FindAction("Quit");
            _resetInputAction = InputSystem.actions.FindAction("Reset");
        }

        
        protected void OnEnable()
        {
            _resetInputAction.Enable();
            _quitInputAction.Enable();
        }
        
        
        protected void OnDisable()
        {
            _resetInputAction.Disable();
            _quitInputAction.Disable();
        }
        
        
        protected void Update()
        {
            if (_resetInputAction.WasPerformedThisFrame())
            {
                OnReset.Invoke();
            }
            
            if (_quitInputAction.WasPerformedThisFrame())
            {
                OnQuit.Invoke();
            }
        }


        protected void OnDestroy()
        {
            ConfiguratorModel model = Context?.ModelLocator.GetItem<ConfiguratorModel>();
            if (model == null)
            {
                return;
            }
            model.HasLoadedService.OnValueChanged.RemoveListener(ServiceHasLoaded_OnValueChanged);

            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
            StatusLabel.text = SceneManager.GetActiveScene().name;
            BackButton.SetEnabled(model.HasLoadedService.Value && model.HasNavigationBack.Value);
            DeveloperConsoleButton.SetEnabled(model.HasLoadedService.Value && model.HasNavigationDeveloperConsole.Value);
        }
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void BackButton_OnClicked()
        {
            OnBack.Invoke();
        }

        private void DeveloperConsoleButtonOnClicked()
        {
            OnDeveloperConsole.Invoke();
        }
        
        
        private void ServiceHasLoaded_OnValueChanged(bool previousValue, bool currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
   
        }
    }
}