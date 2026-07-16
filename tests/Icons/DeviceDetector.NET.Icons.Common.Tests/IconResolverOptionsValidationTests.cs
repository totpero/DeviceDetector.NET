using System;
using System.Collections.Generic;
using System.IO;
using DeviceDetectorNET.Icons.Common;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Common.Tests
{
    public class IconResolverOptionsValidationTests
    {
        [Fact]
        public void ThrowsWhenNeitherPhysicalRootPathNorFileExistsIsSet()
        {
            Should.Throw<ArgumentException>(() =>
                IconResolverOptionsValidation.Validate(null, null, new List<string> { "svg" }));
        }

        [Fact]
        public void ThrowsWhenExtensionPriorityIsEmpty()
        {
            Should.Throw<ArgumentException>(() =>
                IconResolverOptionsValidation.Validate("C:\\icons", null, new List<string>()));
        }

        [Fact]
        public void ThrowsWhenExtensionPriorityIsNull()
        {
            Should.Throw<ArgumentException>(() =>
                IconResolverOptionsValidation.Validate("C:\\icons", null, null));
        }

        [Fact]
        public void DoesNotThrowWhenPhysicalRootPathIsSet()
        {
            Should.NotThrow(() =>
                IconResolverOptionsValidation.Validate("C:\\icons", null, new List<string> { "svg" }));
        }

        [Fact]
        public void DoesNotThrowWhenFileExistsDelegateIsSetWithoutPhysicalRootPath()
        {
            Should.NotThrow(() =>
                IconResolverOptionsValidation.Validate(null, _ => false, new List<string> { "svg" }));
        }

        [Fact]
        public void DefaultFileExistsChecksDiskRelativeToPhysicalRootPath()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "IconResolverOptionsValidationTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path.Combine(tempDir, "device", "type"));
            File.WriteAllText(Path.Combine(tempDir, "device", "type", "tv.svg"), string.Empty);

            try
            {
                var fileExists = IconResolverOptionsValidation.DefaultFileExists(tempDir);

                fileExists("/device/type/tv.svg").ShouldBeTrue();
                fileExists("/device/type/missing.svg").ShouldBeFalse();
            }
            finally
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }
    }
}
