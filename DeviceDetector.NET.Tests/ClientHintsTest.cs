using FluentAssertions;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using Xunit;

namespace DeviceDetectorNET.Tests
{
    [Trait("Category", "ClientHints")]
    public class ClientHintsTest
    {
        [Fact]
        public void TestHeaders()
        {
            //IDictionary<string, StringValues>
            var headers = new Dictionary<string, string>
            {
                {"sec-ch-ua",@"""Opera"";v=""83"", "" Not;A Brand"";v=""99"", ""Chromium"";v=""98""" },
                {"sec-ch-ua-mobile","?0" },
                {"sec-ch-ua-platform","Windows" },
                {"sec-ch-ua-platform-version","14.0.0" },
            };

            var ch = ClientHints.Factory(headers);
            ch.IsMobile().Should().BeFalse();

            ch.GetOperatingSystem().Should().Be("Windows");
            ch.GetOperatingSystemVersion().Should().Be("14.0.0");
            ch.GetBrandList().Should().Equal(
                new Dictionary<string, string>{ 
                        { "Opera", "83" },
                        { " Not;A Brand", "99" },
                        { "Chromium", "98" },
                    }
                );
        }

        [Fact]
        public void TestHeadersHttp()
        {
            var headers = new Dictionary<string, string>
                {
                    { "HTTP_SEC_CH_UA_FULL_VERSION_LIST" , @""" Not A;Brand"";v=""99.0.0.0"", ""Chromium"";v=""98.0.4758.82"", ""Opera"";v=""98.0.4758.82""" },
                    { "HTTP_SEC_CH_UA"                  , @""" Not A;Brand"";v=""99"", ""Chromium"";v=""98"", ""Opera"";v=""84"""},
                    { "HTTP_SEC_CH_UA_MOBILE"           , "?1"},
                    { "HTTP_SEC_CH_UA_MODEL"            , "DN2103"},
                    { "HTTP_SEC_CH_UA_PLATFORM"         , "Ubuntu"},
                    { "HTTP_SEC_CH_UA_PLATFORM_VERSION" , "3.7"},
                    { "HTTP_SEC_CH_UA_FULL_VERSION"    , "98.0.14335.105"}
                };

            var ch = ClientHints.Factory(headers);
            ch.IsMobile().Should().BeTrue();
            ch.GetOperatingSystem().Should().Be("Ubuntu");
            ch.GetOperatingSystemVersion().Should().Be("3.7");

            ch.GetBrandList().Should().Equal(
                new Dictionary<string, string>{
                        { " Not A;Brand", "99.0.0.0" },
                        { "Chromium", "98.0.4758.82" },
                        { "Opera", "98.0.4758.82" },
                    }
                );
            ch.GetModel().Should().Be("DN2103");
        }

        [Fact]
        public void TestHeadersJavascript()
        {
            var headers = new Dictionary<string, string>
            {
                //'fullVersionList' => [
                //    ['brand' => ' Not A;Brand', 'version' => '99.0.0.0'],
                //    ['brand' => 'Chromium', 'version' => '99.0.4844.51'],
                //    ['brand' => 'Google Chrome', 'version' => '99.0.4844.51'],
                //],
                {"mobile"          , "false" },
                {"model"           , "" },
                {"platform"        , "Windows" },
                { "platformVersion" , "10.0.0" },
            };

            var ch = ClientHints.Factory(headers);
            ch.IsMobile().Should().BeFalse();
            ch.GetOperatingSystem().Should().Be("Windows");
            ch.GetOperatingSystemVersion().Should().Be("10.0.0");

            //ch.GetBrandList().Should().Equal(
            //    new Dictionary<string, string>{
            //            { " Not A;Brand", "99.0.0.0" },
            //            { "Chromium", "99.0.4844.51" },
            //            { "Google Chrome", "99.0.4844.51" },
            //        }
            //    );

            ch.GetModel().Should().Be("");

        }

        [Fact]
        public void TestIncorrectVersionListIsDiscarded()
        {
            var headers = new Dictionary<string, string>
            {
                //'fullVersionList' => [
                //    ['brand' => ' Not A;Brand', 'version' => '99.0.0.0'],
                //    ['brand' => 'Chromium', 'version' => '99.0.4844.51'],
                //    ['brand' => 'Google Chrome', 'version' => '99.0.4844.51'],
                //],
                {"mobile"          , "false" },
                {"model"           , "" },
                {"platform"        , "Windows" },
                { "platformVersion" , "10.0.0" },
            };

            var ch = ClientHints.Factory(headers);
            ch.IsMobile().Should().BeFalse();
            ch.GetOperatingSystem().Should().Be("Windows");
            ch.GetOperatingSystemVersion().Should().Be("10.0.0");

            //ch.GetBrandList().Should().Equal(
            //    new Dictionary<string, string>{
            //            { " Not A;Brand", "99.0.0.0" },
            //            { "Chromium", "99.0.4844.51" },
            //            { "Google Chrome", "99.0.4844.51" },
            //        }
            //    );

            ch.GetModel().Should().Be("");

        }
    }
}