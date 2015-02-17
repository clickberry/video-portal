using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Portal.BackEnd.IoC.Tests
{
    [TestClass]
    public class BackEndInitializerTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            //Arrange
            var container = new Container();
            var initializer = new BackEndInitializer();
            
            //Act
            initializer.Initialize(container);

            //Assert
            container.Verify();
        }
    }
}
