using RMC.Mini.Service;
using RMC.Core.IO;
using RMC.BlockWorld.Mini.Model.Data;
using RMC.BlockWorld.Mini.Service.Storage;
using RMC.Mini;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.BlockWorld.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class LocalDiskStorageServiceEvent : UnityEvent<LocalDiskStorageServiceDto> {}

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class LocalDiskStorageService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly LocalDiskStorageServiceEvent OnLoadCompleted = new LocalDiskStorageServiceEvent();
        
        
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        private LocalDiskStorageServiceDto _localDiskStorageServiceDto;
        
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                bool hasData = LocalDiskStorage.Instance.Has<LocalDiskStorageServiceDto>();
                if (!hasData)
                {
                    var defaultData = new LocalDiskStorageServiceDto();
                    LocalDiskStorage.Instance.Save<LocalDiskStorageServiceDto>(defaultData);
                }
            }
        }

        
        //  Methods ---------------------------------------
        public void Load ()
        {
            RequireIsInitialized();
            
            bool hasData = LocalDiskStorage.Instance.Has<LocalDiskStorageServiceDto>();

            if (!hasData)
            {
                Debug.LogError("Error: LoadScore failed.");
                return;
            }
            
            _localDiskStorageServiceDto =  LocalDiskStorage.Instance.Load<LocalDiskStorageServiceDto>();

            OnLoadCompleted.Invoke(_localDiskStorageServiceDto);
        }
        
        
        public void SaveEnvironmentData (EnvironmentData environmentData)
        {
            RequireIsInitialized();
            
            bool hasData = LocalDiskStorage.Instance.Has<LocalDiskStorageServiceDto>();

            if (!hasData)
            {
                Debug.LogError("Error: SaveCharacterData failed.");
                return;
            }
            
            _localDiskStorageServiceDto = LocalDiskStorage.Instance.Load<LocalDiskStorageServiceDto>();
            _localDiskStorageServiceDto.EnvironmentData = environmentData;
            LocalDiskStorage.Instance.Save<LocalDiskStorageServiceDto>(_localDiskStorageServiceDto);

            OnLoadCompleted.Invoke(_localDiskStorageServiceDto);
        }
        
        
        public void SaveCharacterData (CharacterData characterData)
        {
            RequireIsInitialized();
            
            bool hasData = LocalDiskStorage.Instance.Has<LocalDiskStorageServiceDto>();

            if (!hasData)
            {
                Debug.LogError("Error: SaveCharacterData failed.");
                return;
            }
            
            _localDiskStorageServiceDto = LocalDiskStorage.Instance.Load<LocalDiskStorageServiceDto>();
            _localDiskStorageServiceDto.CharacterData = characterData;
            LocalDiskStorage.Instance.Save<LocalDiskStorageServiceDto>(_localDiskStorageServiceDto);

            OnLoadCompleted.Invoke(_localDiskStorageServiceDto);
        }
        
        
        //  Event Handlers --------------------------------
    }
}