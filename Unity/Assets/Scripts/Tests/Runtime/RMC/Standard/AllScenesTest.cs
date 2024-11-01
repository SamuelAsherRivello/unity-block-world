using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RMC.BlockWorld.Mini.Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace RMC.BlockWorld.Standard
{
    [TestFixture]
    [Category ("RMC.BlockWorld.Standard")]
    public class AllScenesTest
    {
        //NOTE: Keep here, I tried to move this to BlockWorldConstants, but the ValueSource fails when run.
        private static readonly List<string> SceneNames = new List<string>
        {
            BlockWorldConstants.Scene01_Menu,
            BlockWorldConstants.Scene02_CustomizeCharacter,
            BlockWorldConstants.Scene03_CustomizeEnvironment ,
            BlockWorldConstants.Scene04_Game,
            BlockWorldConstants.Scene05_DeveloperConsole
        };
        
        [SetUp]
        public void SetUp()
        {
            
        }
        
        [TearDown]
        public void TearDown()
        {
        }
        
        [UnityTest]
        public IEnumerator SceneName_ThrowsNoErrors_WhenLoadScene([ValueSource(nameof(SceneNames))] string sceneName)
        {
            // Arrange
            
            // Act
            Assert.DoesNotThrow(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
  
            // Assert
            yield return new WaitForSeconds(1);
        }
        
    }
}
