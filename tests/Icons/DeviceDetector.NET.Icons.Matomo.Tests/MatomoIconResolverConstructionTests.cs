using System;
using System.Collections.Generic;
using DeviceDetectorNET.Icons.Matomo;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Matomo.Tests
{
    public class MatomoIconResolverConstructionTests
    {
        [Fact]
        public void ThrowsWhenOptionsIsNull()
        {
            Should.Throw<ArgumentNullException>(() => new MatomoIconResolver(null));
        }

        [Fact]
        public void ThrowsWhenNeitherPhysicalRootPathNorFileExistsIsSet()
        {
            var options = new MatomoIconResolverOptions();

            Should.Throw<ArgumentException>(() => new MatomoIconResolver(options));
        }

        [Fact]
        public void ThrowsWhenExtensionPriorityIsEmpty()
        {
            var options = new MatomoIconResolverOptions
            {
                PhysicalRootPath = "C:\\icons",
                ExtensionPriority = new List<string>()
            };

            Should.Throw<ArgumentException>(() => new MatomoIconResolver(options));
        }

        [Fact]
        public void DoesNotThrowWhenPhysicalRootPathIsSet()
        {
            var options = new MatomoIconResolverOptions { PhysicalRootPath = "C:\\icons" };

            Should.NotThrow(() => new MatomoIconResolver(options));
        }

        [Fact]
        public void DoesNotThrowWhenFileExistsDelegateIsSetWithoutPhysicalRootPath()
        {
            var options = new MatomoIconResolverOptions { FileExists = _ => false };

            Should.NotThrow(() => new MatomoIconResolver(options));
        }
    }
}
