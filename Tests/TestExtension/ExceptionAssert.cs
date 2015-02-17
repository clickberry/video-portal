using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestExtension
{
    public static class ExceptionAssert
    {
        public static T Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex,typeof(T));
                return ex as T;
            }

            Assert.Fail(string.Format("Expected exception of type {0} but no exception was thrown.", typeof(T)));
            return null;
        }

        public static async Task<T> ThrowsAsync<T>(Func<Task> func) where T : Exception
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(T));
                return ex as T;
            }

            Assert.Fail(string.Format("Expected exception of type {0} but no exception was thrown.", typeof(T)));
            return null;
        }

        public static void NotThrows<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                Assert.Fail(string.Format("Exception of type {0} was thrown.", ex.GetType()));
            }
        }
    }
    

}
