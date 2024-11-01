using System.Reflection;
using NUnit.Framework;
using RMC.BlockWorld.Mini.Model;
using RMC.BlockWorld.Mini.Service;
using RMC.BlockWorld.Mini.View;
using RMC.Mini;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.BlockWorld.Mini.Controller
{
    
    [TestFixture]
    [Category ("RMC.BlockWorld.Mini")]
    public class HudControllerTest
    {
        private HudController _hudController;
        private BlockWorldModel _model;
        private HudView _view;
        private LocalDiskStorageService _service;
        private IContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new BaseContext();
            _model = new BlockWorldModel();
            _view = new GameObject().AddComponent<HudView>();
            _service = new LocalDiskStorageService();
            _hudController = new HudController(_model, _view, _service);
        }

        [Test]
        public void Initialize_SetsDefaults()
        {
            // Arrange
            Assert.IsFalse(_hudController.IsInitialized);

            // Act
            _hudController.Initialize(_context);

            // Assert
            Assert.IsTrue(_hudController.IsInitialized);
            Assert.IsTrue(HasListeners(_view.OnBack));
        }
        
        private bool HasListeners(UnityEvent unityEvent)
        {
            var field = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);
            var calls = field.GetValue(unityEvent);
            var countProperty = calls.GetType().GetProperty("Count");
            int count = (int)countProperty.GetValue(calls);
            return count > 0;
        }

        [Test]
        public void View_OnBack_ThrowsException_WhenNotInitialized()
        {
            // Arrange
            Assert.IsFalse(_hudController.IsInitialized);

            // Act & Assert
            Assert.DoesNotThrow(() => _view.OnBack.Invoke());
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_view.gameObject);
        }
    }
}
