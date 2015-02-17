using Moq;

namespace TestExtension
{
    public static class AsRef
    {
        public static T Set<T>(T param) where T : class
        {
            return It.Is<T>(t => t == param);
        }
    }
}
