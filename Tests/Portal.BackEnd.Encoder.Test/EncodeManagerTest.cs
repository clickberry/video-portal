using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Exceptions;
using Portal.BackEnd.Encoder.Interface;
using Wrappers;

namespace Portal.BackEnd.Encoder.Test
{
    [TestClass]
    public class EncodeManagerTest
    {
        [TestMethod]
        public void StartMethodTest()
        {
            //Arrange
            const int quantityMethodCall = 1;
            var count = 0;

            var taskStatic = new Mock<TaskStaticWrapper>();
            var pipeline = new Mock<IEncodePipeline>();
            var tokenSource = new CancellationTokenSource();
            var manager = new EncodeManager(pipeline.Object, tokenSource, taskStatic.Object);

            pipeline.Setup(m => m.Run()).Callback(() =>
                                                         {
                                                             count++;
                                                             if (quantityMethodCall == count)
                                                             {
                                                                tokenSource.Cancel();
                                                             }
                                                         });

            //Act
            var task = manager.Start();
            task.Wait();

            //Assert
            pipeline.Verify(m => m.Run(), Times.Exactly(quantityMethodCall));
        }

        [TestMethod]
        public void StartMethodExceptionHandleTest()
        {
            //Arrange
            var taskStatic = new Mock<TaskStaticWrapper>();
            var pipeline = new Mock<IEncodePipeline>();
            var tokenSource = new CancellationTokenSource();
            var manager = new EncodeManager(pipeline.Object, tokenSource, taskStatic.Object);

            pipeline.Setup(m => m.Run()).Callback(()=>
                                                        {
                                                            tokenSource.Cancel();
                                                            throw new Exception();
                                                        });

            //Act
            var task = manager.Start();
            task.Wait();

            //Assert
            Assert.IsFalse(task.IsFaulted);
        }

        [TestMethod]
        public void StartMethodSleeppWhenThrowNotContentExceptionTest()
        {
            //Arrange
            var taskStatic = new Mock<TaskStaticWrapper>();
            var pipeline = new Mock<IEncodePipeline>();
            var tokenSource = new CancellationTokenSource();
            var manager = new EncodeManager(pipeline.Object, tokenSource, taskStatic.Object);

            taskStatic.Setup(m => m.Delay(5000)).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    tcs.SetResult(null);
                    return tcs.Task;
                });

            pipeline.Setup(m => m.Run()).Callback(() =>
                {
                    tokenSource.Cancel();
                    throw new NoContentException();
                });

            //Act
            var task = manager.Start();
            task.Wait();

            //Assert
            taskStatic.Verify(m=>m.Delay(5000));
        }
    }
}
