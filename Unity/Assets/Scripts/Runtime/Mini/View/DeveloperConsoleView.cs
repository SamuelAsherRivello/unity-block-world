using System;
using RMC.Mini.View;
using RMC.BlockWorld.Mini.Controller;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Model.Data;
using RMC.BlockWorld.Standard;
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
        
        [HideInInspector] 
        public readonly UnityEvent OnRandomizeLanguage = new UnityEvent();
        
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public Button ResetButton { get { return _uiDocument?.rootVisualElement.Q<Button>("ResetButton"); }}
        public Button NextLanguageButton { get { return _uiDocument?.rootVisualElement.Q<Button>("NextLanguageButton"); }}
        
        public Label TitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("TitleLabel"); }}
        public Label SubtitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("SubtitleLabel"); }}
        
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
        
        // Audio
        private AudioBinding _audioBinding;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ResetButton.clicked += ResetButton_OnClicked;
                NextLanguageButton.clicked += NextLanguageButton_OnClicked;
                
                BlockWorldModel model = Context.ModelLocator.GetItem<BlockWorldModel>();
                model.CharacterData.OnValueChanged.AddListener(CharacterData_OnValueChanged);
                model.EnvironmentData.OnValueChanged.AddListener(EnvironmentData_OnValueChanged);

                //Audio for every button
                _audioBinding = CustomAudioUtility.CreateNewAudioBinding();
                _audioBinding.RegisterButton(ResetButton);
                _audioBinding.RegisterButton(NextLanguageButton);
               
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
            BlockWorldModel model = Context.ModelLocator.GetItem<BlockWorldModel>();
            if (model == null)
            {
                return;
            }
            model.CharacterData.OnValueChanged.RemoveListener(CharacterData_OnValueChanged);
            model.EnvironmentData.OnValueChanged.RemoveListener(EnvironmentData_OnValueChanged);

            if (_audioBinding != null)
            {
                _audioBinding.Dispose();
            }
            
            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------
        private async void RefreshUI()
        {
            RequireIsInitialized();

            BlockWorldModel model = Context.ModelLocator.GetItem<BlockWorldModel>();
            ResetButton.SetEnabled(model.HasLoadedService.Value && 
                                   (!model.CharacterDataIsDefault() || !model.EnvironmentDataIsDefault()));

            bool hasMultipleLanguages = await CustomLocalizationUtility.GetAvailableLocalesCountAsync() > 1;
            NextLanguageButton.SetEnabled(model.HasLoadedService.Value && hasMultipleLanguages);

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
        
        private void NextLanguageButton_OnClicked()
        {
            OnRandomizeLanguage.Invoke();
            RefreshUI();
        }
        
        
        private void ResetButton_OnClicked()
        {
            OnReset.Invoke();
            RefreshUI();
        }
    }
}