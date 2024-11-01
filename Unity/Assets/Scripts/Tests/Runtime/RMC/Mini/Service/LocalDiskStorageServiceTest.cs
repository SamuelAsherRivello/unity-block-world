using System.Threading.Tasks;
using NUnit.Framework;
using RMC.BlockWorld.Mini.Service.Storage;
using RMC.Mini;

namespace RMC.BlockWorld.Mini.Service
{
    
    [TestFixture]
    [Category ("RMC.BlockWorld.Mini")]
    public class LocalDiskStorageServiceTest
    {
        private LocalDiskStorageService _service;
        private IContext _context;
        private const int WaitDurationMS = 500;

        
        [SetUp]
        public void SetUp()
        {
            _context = new BaseContext();
            _service = new LocalDiskStorageService();
        }
        
        
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _context = null;
            
            _service.Dispose();
            _service = null;
        }

        
        [Test]
        public void Initialize_SetsDefaults()
        {
            // Arrange
            Assert.IsFalse(_service.IsInitialized);

            // Act
            _service.Initialize(_context);

            // Assert
            Assert.IsTrue(_service.IsInitialized);
        }
        
        
        [Test]
        public async Task Load_isOnLoadCompleted_True_WhenCalled()
        {
            // Arrange
            bool isOnLoadCompleted = false;
            _service.OnLoadCompleted.AddListener((LocalDiskStorageServiceDto data) =>
            {
                isOnLoadCompleted = true;
            });

            // Act
            _service.Initialize(_context);
            _service.Load();
            await Task.Delay(WaitDurationMS);

            // Assert
            Assert.IsTrue(isOnLoadCompleted);
        }
        
        
        [Test]
        public async Task Load_DataIsNotNull_WhenCalled()
        {
            // Arrange
            _service.OnLoadCompleted.AddListener((LocalDiskStorageServiceDto data) =>
            {
                // Assert
                Assert.IsNotNull(data);
                Assert.IsNotNull(data.CharacterData);
                Assert.IsNotNull(data.EnvironmentData);
            });

            // Act
            _service.Initialize(_context);
            _service.Load();
            await Task.Delay(WaitDurationMS);
        }
    }
}
