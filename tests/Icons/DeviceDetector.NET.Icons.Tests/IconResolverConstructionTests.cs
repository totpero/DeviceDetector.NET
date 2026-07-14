using System;
using System.Collections.Generic;
using DeviceDetectorNET.Icons;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Tests
{
    public class IconResolverConstructionTests
    {
        [Fact]
        public void ThrowsWhenOptionsIsNull()
        {
            Should.Throw<ArgumentNullException>(() => new IconResolver(null));
        }

        [Fact]
        public void ThrowsWhenNeitherPhysicalRootPathNorFileExistsIsSet()
        {
            var options = new IconResolverOptions();

            Should.Throw<ArgumentException>(() => new IconResolver(options));
        }

        [Fact]
        public void ThrowsWhenExtensionPriorityIsEmpty()
        {
            var options = new IconResolverOptions
            {
                PhysicalRootPath = "C:\\icons",
                ExtensionPriority = new List<string>()
            };

            Should.Throw<ArgumentException>(() => new IconResolver(options));
        }

        [Fact]
        public void DoesNotThrowWhenPhysicalRootPathIsSet()
        {
            var options = new IconResolverOptions { PhysicalRootPath = "C:\\icons" };

            Should.NotThrow(() => new IconResolver(options));
        }

        [Fact]
        public void DoesNotThrowWhenFileExistsDelegateIsSetWithoutPhysicalRootPath()
        {
            var options = new IconResolverOptions { FileExists = _ => false };

            Should.NotThrow(() => new IconResolver(options));
        }
    }
}
