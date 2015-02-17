using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Settings;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test
{
    [TestClass]
    public class WatchDogTimerTest
    {
        private Mock<ThreadingTimerWrapper> _timer;
        private WatchDogTimerSettings _settings;
        private WatchDogTimer _watchDog;
        private Mock<CancellationTokenSourceWrapper> _tokenSource;

        [TestInitialize]
        public void Initialize()
        {
            _timer = new Mock<ThreadingTimerWrapper>();
            _settings=new WatchDogTimerSettings(10000);
            _watchDog = new WatchDogTimer(_timer.Object, _settings);
            _tokenSource=new Mock<CancellationTokenSourceWrapper>();
        }

        [TestMethod]
        public void StartWatchDogTimerTest()
        {
            //Act
            _watchDog.Start(_tokenSource.Object);

            //Assert
            _timer.Verify(m=>m.SetCallback(It.IsAny<TimerCallback>()), Times.Once());
        }

        [TestMethod]
        public void StopWatchDogTimerTest()
        {
            //Act
            _watchDog.Stop();

            //Assert
            Assert.AreEqual(false, _watchDog.IsOverflowing);
            _timer.Verify(m => m.Dispose(), Times.Once());
        }


        [TestMethod]
        public void ResetTest()
        {
            //Act
            _watchDog.Reset();

            //Assert
            _timer.Verify(m => m.Change(_settings.Period, _settings.Period), Times.Once());
        }

        [TestMethod]
        public void TimerOverflowTest()
        {
            //Arrange
            TimerCallback callback = null;
            _timer.Setup(m => m.SetCallback(It.IsAny<TimerCallback>())).Callback<TimerCallback>(c => callback = c);

            _watchDog.Start(_tokenSource.Object);
           
            //Act
            callback(null);
            var isOverflowing = _watchDog.IsOverflowing;
            _watchDog.Start(_tokenSource.Object);
            var isNoOverflowing = _watchDog.IsOverflowing;

            //Assert
            Assert.AreEqual(true, isOverflowing);
            Assert.AreEqual(false, isNoOverflowing);
            _tokenSource.Verify(m => m.Cancel(), Times.Once());
        }
    }
}
