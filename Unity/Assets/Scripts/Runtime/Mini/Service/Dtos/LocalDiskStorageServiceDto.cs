using RMC.Core.Attributes;
using RMC.BlockWorld.Mini.Model.Data;
using UnityEngine;

namespace RMC.BlockWorld.Mini.Service.Storage
{
    /// <summary>
    /// Represents a file entry for local disk storage
    /// </summary>
    [CustomFilePath("LocalDiskStorageServiceDto", CustomFilePathLocation.StreamingAssetsPath)]
    public class LocalDiskStorageServiceDto
    {
        //  Fields -----------------------------------------
        [SerializeField] public EnvironmentData EnvironmentData = Model.Data.EnvironmentData.FromDefaultValues();
        
        [SerializeField] 
        public CharacterData CharacterData = Model.Data.CharacterData.FromDefaultValues();
    }
}
