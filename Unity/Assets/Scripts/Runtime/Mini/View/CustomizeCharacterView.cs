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

// ReSharper disable Unity.NoNullPropagation
namespace RMC.BlockWorld.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="CustomizeCharacterController"/>
    /// 
    /// </summary>
    public class CustomizeCharacterView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnRandomize = new UnityEvent();
        
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        public Button RandomizeButton { get { return _uiDocument?.rootVisualElement.Q<Button>("RandomizeButton"); }}
        
        public Label TitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("TitleLabel"); }}
        
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        [Header("Gameplay")]
        [SerializeField] 
        private Player _player;
        
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                BlockWorldModel model = Context.ModelLocator.GetItem<BlockWorldModel>();
                model.CharacterData.OnValueChanged.AddListener(CharacterData_OnValueChanged);
                RandomizeButton.clicked += RandomizeButton_OnClicked;
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
            
            BlockWorldModel model = Context.ModelLocator.GetItem<BlockWorldModel>();
            RandomizeButton.SetEnabled(model.HasLoadedService.Value);
        }
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void RandomizeButton_OnClicked()
        {
            OnRandomize.Invoke();
        }
        
        private void CharacterData_OnValueChanged(CharacterData previousValue, CharacterData currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
            _player.CharacterData = currentValue;
        }
    }
}