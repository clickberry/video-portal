using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestExtension
{
    public static class CustomAssert
    {
        public static void IsThrown<T>(Action action) where T : Exception
        {
            var message = String.Format("The exception of type {0} should be thrown.", typeof(T));
            try
            {
                action.Invoke();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(T), message);
            }
        }

        public static void IsThrown<T>(Action action, string message) where T : Exception
        {
            try
            {
                action.Invoke();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(T), message);
            }
        }

        public static void IsNotThrown(Action action)
        {
            const string message = "The exception of type {0} was be thrown.";
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                var exType = ex.GetType();
                Assert.Fail(message, exType);
            }
        }

        public static void IsNotThrown(Action action, string message)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
                Assert.Fail(message);
            }
        }
    }

    //public static class MockExtension
    //{
    //    public static void Aggregate<T>(this Mock<T> moq, Action action)
    //    {
    //        try
    //        {
    //            action.Invoke();
    //        }
    //        catch (Exception ex)
    //        {
    //            new AggregateException();
    //        }
    //    }
    //}
}
