using BTDB.IOC;
using NUnit.Framework;

namespace BTDBTest
{
    public interface ILogger
    {
    }

    public class Logger : ILogger
    {
    }

    [TestFixture]
    public class IOCTests
    {
        [Test]
        public void AlwaysNew()
        {
            var builder = new ContainerBuilder();
            builder.Register<Logger>().As<ILogger>();
            var container = builder.Build();
            var log1 = container.Resolve<ILogger>();
            Assert.NotNull(log1);
            var log2 = container.Resolve<ILogger>();
            Assert.NotNull(log2);
            Assert.AreNotSame(log1, log2);
        }

        [Test]
        public void Singleton()
        {
            var builder = new ContainerBuilder();
            builder.Register<Logger>().As<ILogger>().SingleInstance();
            var container = builder.Build();
            var log1 = container.Resolve<ILogger>();
            Assert.NotNull(log1);
            var log2 = container.Resolve<ILogger>();
            Assert.NotNull(log2);
            Assert.AreSame(log1, log2);
        }
    }

}