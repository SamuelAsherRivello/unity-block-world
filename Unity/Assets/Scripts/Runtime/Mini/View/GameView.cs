using System;
using JetBrains.Annotations;
using RMC.Mini.View;
using RMC.BlockWorld.Mini.Controller;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Model.Data;
using RMC.BlockWorld.Standard;
using RMC.BlockWorld.Standard.Objects;
using RMC.Mini;
using UnityEngine;
using UnityEngine.UIElements;
using Environment = RMC.BlockWorld.Standard.Objects.Environment;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.BlockWorld.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="GameController"/>
    /// 
    /// </summary>
    public class GameView: MonoBehaviour, IView
    {
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        public Label TitleLabel { get { return _uiDocument?.rootVisualElement?.Q<Label>("TitleLabel"); }}
        public Label SubtitleLabel { get { return _uiDocument?.rootVisualElement?.Q<Label>("SubtitleLabel"); }}

        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;
        
        [Header("Gameplay")]
        [SerializeField]
        [CanBeNull]
        private Environment _environment;

        [SerializeField] 
        private Player _player;
                

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                _player.IsPlayerEnabled = true;
                
                BlockWorldModel model = Context.ModelLocator.GetItem<BlockWorldModel>();
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
            BlockWorldModel model = Context?.ModelLocator.GetItem<BlockWorldModel>();
            if (model == null)
            {
                return;
            }
            model.CharacterData.OnValueChanged.RemoveListener(CharacterData_OnValueChanged);

            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            RequireIsInitialized();
            
            // Optional: Update any text here...
            
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
    }
}