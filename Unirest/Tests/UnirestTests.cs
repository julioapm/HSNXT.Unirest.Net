using System.Net.Http;
using FluentAssertions;
using unirest_net.http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unirest_net_tests.http
{
    [TestClass]
    public class UnirestTest
    {
        [TestMethod]
        public void Unirest_Patch_Should_Equal_Itself()
        {
            Unirest.PatchMethod.ShouldBeEquivalentTo(new HttpMethod("PATCH")); 
        }

        [TestMethod]
        public void Unirest_Should_Return_Correct_Verb()
        {
            Unirest.Get("http://localhost").HttpMethod.Should().Be(HttpMethod.Get);
            Unirest.Post("http://localhost").HttpMethod.Should().Be(HttpMethod.Post);
            Unirest.Delete("http://localhost").HttpMethod.Should().Be(HttpMethod.Delete);
            Unirest.Patch("http://localhost").HttpMethod.Should().Be(new HttpMethod("PATCH"));
            Unirest.Put("http://localhost").HttpMethod.Should().Be(HttpMethod.Put);
            Unirest.Options("http://localhost").HttpMethod.Should().Be(HttpMethod.Options);
            Unirest.Head("http://localhost").HttpMethod.Should().Be(HttpMethod.Head);
            Unirest.Trace("http://localhost").HttpMethod.Should().Be(HttpMethod.Trace);
            
            new GetRequest("http://localhost").HttpMethod.Should().Be(HttpMethod.Get);
            new PostRequest("http://localhost").HttpMethod.Should().Be(HttpMethod.Post);
            new DeleteRequest("http://localhost").HttpMethod.Should().Be(HttpMethod.Delete);
            new PatchRequest("http://localhost").HttpMethod.Should().Be(Unirest.PatchMethod);
            new PutRequest("http://localhost").HttpMethod.Should().Be(HttpMethod.Put);
            new OptionsRequest("http://localhost").HttpMethod.Should().Be(HttpMethod.Options);
            new HeadRequest("http://localhost").HttpMethod.Should().Be(HttpMethod.Head);
            new TraceRequest("http://localhost").HttpMethod.Should().Be(HttpMethod.Trace);
        }

        [TestMethod]
        public void Unirest_Should_Return_Correct_URL()
        {
            Unirest.Get("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            Unirest.Post("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            Unirest.Delete("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            Unirest.Patch("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            Unirest.Put("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            Unirest.Options("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            Unirest.Head("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            Unirest.Trace("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            
            new GetRequest("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            new PostRequest("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            new DeleteRequest("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            new PatchRequest("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            new PutRequest("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            new OptionsRequest("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            new HeadRequest("http://localhost").Url.OriginalString.Should().Be("http://localhost");
            new TraceRequest("http://localhost").Url.OriginalString.Should().Be("http://localhost");
        }
    }
}
