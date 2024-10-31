using System.Collections.Generic;
using RMC.BlockWorld.Mini.Model.Data;
using UnityEngine;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace RMC.BlockWorld.Standard.Objects
{
    /// <summary>
    /// Represents the 3D Graphics behind the <see cref="Player"/>
    /// </summary>
    public class Environment : MonoBehaviour
    {
        //  Properties ------------------------------------

        public EnvironmentData EnvironmentData
        {
            get { return _environmentData; }
            set
            {
                _environmentData = value;

                if (_floor == null)
                {
                    return;
                }

                //Make the BG **always** a bit darker, so it works well behind the player
                Color darker1 = CustomColorUtility.TintOrShadeColor(_environmentData.FloorColor, -50);
                Color darker2 =  CustomColorUtility.TintOrShadeColor(_environmentData.BackgroundColor, -50);
                Color darker3 =  CustomColorUtility.TintOrShadeColor(_environmentData.DecorationColor, -50);
                    
                CustomColorUtility.SetColorAsync(_floor.material, darker1, CustomColorUtility.DefaultDuration);
                
                foreach (Renderer background in _backgrounds)
                {
                    CustomColorUtility.SetColorAsync(background.material, darker2, CustomColorUtility.DefaultDuration);
                }
                
                foreach (Renderer decoration in _decorations)
                {
                    CustomColorUtility.SetColorAsync(decoration.material, darker3, CustomColorUtility.DefaultDuration);
                }
            }
        }

        //  Fields ----------------------------------------
        [SerializeField]
        private Renderer _floor;

        [SerializeField]
        private List<Renderer> _backgrounds;

        [SerializeField]
        private List<Renderer> _decorations;

        private EnvironmentData _environmentData;

        
        //  Unity Methods ---------------------------------
        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }
    }
}
