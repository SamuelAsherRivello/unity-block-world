
using System;
using System.Collections.Generic;
using RMC.Audio;
using UnityEngine.UIElements;

namespace RMC.BlockWorld.Standard
{
    
    /// <summary>
    /// Easily add/remove audio to MANY buttons in the game
    ///
    ///     TODO: Refactor this into partly abstracted and partly games-specific
    /// 
    /// </summary>
    public class AudioBinding : IDisposable
    {
        //  Fields ----------------------------------------
        private List<Button> _buttons = new List<Button>();
        
        //  Methods ---------------------------------------
        internal AudioBinding()
        {
        }

        
        public void RegisterButton(Button button)
        {
            _buttons.Add(button);
            button.RegisterCallback<PointerOverEvent>(Button_OnPointerOverEvent);
            button.RegisterCallback<PointerOutEvent>(Button_OnPointerOutEvent);
            button.RegisterCallback<ClickEvent>(Button_OnPointerDownEvent);
        }
        
        
        public void UnRegisterButton(Button button)
        {
            button.UnregisterCallback<PointerOverEvent>(Button_OnPointerOverEvent);
            button.UnregisterCallback<PointerOutEvent>(Button_OnPointerOutEvent);
            button.UnregisterCallback<ClickEvent>(Button_OnPointerDownEvent);
            _buttons.Remove(button);
        }
        
        
        public void Dispose()
        {
            //Loop backwards so we can remove items from the list
            for (int i = _buttons.Count -1; i >= 0; i--)
            {
                UnRegisterButton(_buttons[i]);
            }
            _buttons.Clear();
        }
        
        /// <summary>
        /// Only play sounds for a NON-disabled button
        /// </summary>
        private bool IsInteractableButton(IEventHandler target)
        {
            Button button = target as UnityEngine.UIElements.Button;
            return button != null && button.enabledSelf;
        }

        
        //  Event Handlers  -------------------------------------
        private void Button_OnPointerOverEvent(PointerOverEvent evt)
        {
            if (IsInteractableButton(evt.target))
            {
                AudioManager.Instance.PlayAudioClip("PointerOver01");
            }
        }


        private void Button_OnPointerOutEvent(PointerOutEvent evt)
        {
            if (IsInteractableButton(evt.target))
            {
                AudioManager.Instance.PlayAudioClip("PointerOut01");
            }
        }

        
        private void Button_OnPointerDownEvent(ClickEvent evt)
        {
            if (IsInteractableButton(evt.target))
            {
                AudioManager.Instance.PlayAudioClip("PointerDown01");
            }
        }
    }
    
    
    public static class CustomAudioUtility
    {
        //  Fields ----------------------------------------
        
        //  Methods ---------------------------------------
        public static AudioBinding CreateNewAudioBinding()
        {
            return new AudioBinding();
        }
    }
}
