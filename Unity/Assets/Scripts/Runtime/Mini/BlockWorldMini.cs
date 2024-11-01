using RMC.Mini.Features;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Service;
using RMC.Mini;

namespace RMC.BlockWorld.Mini
{
    /// <summary>
    /// See <see cref="MiniMvcs{TContext,TModel,TView,TController,TService}"/>
    /// </summary>
    public class BlockWorldMini: MiniMvcs
            <Context, 
                IModel, 
                IView, 
                IController,
                IService>
    {
        
        //  Fields ----------------------------------------

        
        //  Properties ------------------------------------
        /// <summary>
        /// This is an optional addon that gives this <see cref="BlockWorldMini"/>
        /// the support of a <see cref="IFeatureBuilder{F}"/> to easily 
        /// add and remove <see cref="IFeature"/>. 
        /// </summary>
        private FeatureBuilder FeatureBuilder { get; set; }
        
        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                // Context
                _context = new Context();
                
                // Model
                BlockWorldModel model = new BlockWorldModel();
                model.Initialize(_context); //Added to locator inside
                
                // Service
                LocalDiskStorageService service = new LocalDiskStorageService();
                ServiceLocator.AddItem(service);
                service.Initialize(_context);
                
                //Feature
                FeatureBuilder = new FeatureBuilder();
                FeatureBuilder.Initialize(this);
                
            }
        }

        
        //  Methods  -------------------------------
        public bool HasFeature<TFeature>(string key = "") where TFeature : IFeature
        {
            return FeatureBuilder.HasFeature<TFeature>(key);
        }
        
        public void AddFeature<TFeature>(TFeature feature, string key = "") where TFeature : IFeature
        {
            FeatureBuilder.AddFeature<TFeature>(feature, key);
        }
        
        public void RemoveFeature<TFeature>(string key = "", bool willDispose = false) where TFeature : IFeature
        {
            FeatureBuilder.RemoveFeature<TFeature>(key, willDispose);
        }
        
        // Overload for automatically disposing
        public void RemoveAndDisposeFeature<TFeature>() where TFeature : IFeature
        {
            RemoveFeature<TFeature>("", true);
        }
        
    }
}