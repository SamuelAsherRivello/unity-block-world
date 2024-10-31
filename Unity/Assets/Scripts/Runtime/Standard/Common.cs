
using RMC.BlockWorld.Mini.View;
using UnityEngine;

namespace RMC.BlockWorld.Standard
{
    /// <summary>
    /// Holder of common Scene elements
    /// </summary>
    public class Common : MonoBehaviour
    {
        //  Properties ------------------------------------
        public HudView HudView { get { return _hudView; }}

        
        //  Fields ----------------------------------------
        [SerializeField]
        private HudView _hudView;

        
        //  Unity Methods   -------------------------------
        protected void Start ()
        {
          
        }
    }
}


