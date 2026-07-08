using Shouldly;
using System.Collections.Generic;
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
            ch.IsMobile().ShouldBeFalse();

            ch.GetOperatingSystem().ShouldBe("Windows");
            ch.GetOperatingSystemVersion().ShouldBe("14.0.0");
            ch.GetBrandList().ShouldBe(new Dictionary<string, string>{
                        { "Opera", "83" },
                        { " Not;A Brand", "99" },
                        { "Chromium", "98" },
                    });
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
                    { "HTTP_SEC_CH_UA_FULL_VERSION"    , "98.0.14335.105"},
                    { "HTTP_SEC_CH_UA_FORM_FACTORS"    , "\"Desktop\""}
                };

            var ch = ClientHints.Factory(headers);
            ch.IsMobile().ShouldBeTrue();
            ch.GetOperatingSystem().ShouldBe("Ubuntu");
            ch.GetOperatingSystemVersion().ShouldBe("3.7");

            ch.GetBrandList().ShouldBe(new Dictionary<string, string>{
                        { " Not A;Brand", "99.0.0.0" },
                        { "Chromium", "98.0.4758.82" },
                        { "Opera", "98.0.4758.82" },
                    });
            ch.GetModel().ShouldBe("DN2103");
            ch.GetFormFactors().ShouldBe(new[] { "desktop" });
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
                {"formFactors"          , "Desktop" },
                {"mobile"          , "false" },
                {"model"           , "" },
                {"platform"        , "Windows" },
                { "platformVersion" , "10.0.0" },
            };

            var ch = ClientHints.Factory(headers);
            ch.IsMobile().ShouldBeFalse();
            ch.GetOperatingSystem().ShouldBe("Windows");
            ch.GetOperatingSystemVersion().ShouldBe("10.0.0");

            //ch.GetBrandList().ShouldBe(new[] { //    new Dictionary<string, string>{
            //            { " Not A;Brand", "99.0.0.0" },
            //            { "Chromium", "99.0.4844.51" },
            //            { "Google Chrome", "99.0.4844.51" },
            //        }
            // });

            ch.GetModel().ShouldBe("");
            ch.FormFactors.ShouldBe(new[] { "desktop" });

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
            ch.IsMobile().ShouldBeFalse();
            ch.GetOperatingSystem().ShouldBe("Windows");
            ch.GetOperatingSystemVersion().ShouldBe("10.0.0");

            //ch.GetBrandList().ShouldBe(new[] { //    new Dictionary<string, string>{
            //            { " Not A;Brand", "99.0.0.0" },
            //            { "Chromium", "99.0.4844.51" },
            //            { "Google Chrome", "99.0.4844.51" },
            //        }
            // });

            ch.GetModel().ShouldBe("");

        }
    }
}