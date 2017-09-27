using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using unirest_net.request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ImplicitlyCapturedClosure

namespace unirest_net_tests.request
{
    [TestClass]
    public class HttpRequestTests
    {
        [TestMethod]
        public void HttpRequest_Should_Construct()
        {
            Action get = () => new HttpRequest(HttpMethod.Get, "http://localhost");
            Action post = () => new HttpRequest(HttpMethod.Post, "http://localhost");
            Action delete = () => new HttpRequest(HttpMethod.Delete, "http://localhost");
            Action patch = () => new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            Action put = () => new HttpRequest(HttpMethod.Put, "http://localhost");

            get.ShouldNotThrow();
            post.ShouldNotThrow();
            delete.ShouldNotThrow();
            patch.ShouldNotThrow();
            put.ShouldNotThrow();
        }

        [TestMethod]
        public void HttpRequest_Should_Not_Construct_With_Invalid_URL()
        {
            Action get = () => new HttpRequest(HttpMethod.Get, "http:///invalid");
            Action post = () => new HttpRequest(HttpMethod.Post, "http:///invalid");
            Action delete = () => new HttpRequest(HttpMethod.Delete, "http:///invalid");
            Action patch = () => new HttpRequest(new HttpMethod("PATCH"), "http:///invalid");
            Action put = () => new HttpRequest(HttpMethod.Put, "http:///invalid");

            get.ShouldThrow<ArgumentException>();
            post.ShouldThrow<ArgumentException>();
            delete.ShouldThrow<ArgumentException>();
            patch.ShouldThrow<ArgumentException>();
            put.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void HttpRequest_Should_Not_Construct_With_None_HTTP_URL()
        {
            Action get = () => new HttpRequest(HttpMethod.Get, "ftp://localhost");
            Action post = () => new HttpRequest(HttpMethod.Post, "mailto:localhost");
            Action delete = () => new HttpRequest(HttpMethod.Delete, "news://localhost");
            Action patch = () => new HttpRequest(new HttpMethod("PATCH"), "about:blank");
            Action put = () => new HttpRequest(HttpMethod.Put, "about:settings");

            get.ShouldThrow<ArgumentException>();
            post.ShouldThrow<ArgumentException>();
            delete.ShouldThrow<ArgumentException>();
            patch.ShouldThrow<ArgumentException>();
            put.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void HttpRequest_Should_Construct_With_Correct_Verb()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            get.HttpMethod.Should().Be(HttpMethod.Get);
            post.HttpMethod.Should().Be(HttpMethod.Post);
            delete.HttpMethod.Should().Be(HttpMethod.Delete);
            patch.HttpMethod.Should().Be(new HttpMethod("PATCH"));
            put.HttpMethod.Should().Be(HttpMethod.Put);
        }

        [TestMethod]
        public void HttpRequest_Should_Construct_With_Correct_URL()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            get.Url.OriginalString.Should().Be("http://localhost");
            post.Url.OriginalString.Should().Be("http://localhost");
            delete.Url.OriginalString.Should().Be("http://localhost");
            patch.Url.OriginalString.Should().Be("http://localhost");
            put.Url.OriginalString.Should().Be("http://localhost");
        }

        [TestMethod]
        public void HttpRequest_Should_Construct_With_Headers()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            get.Headers.Should().NotBeNull();
            post.Url.OriginalString.Should().NotBeNull();
            delete.Url.OriginalString.Should().NotBeNull();
            patch.Url.OriginalString.Should().NotBeNull();
            put.Url.OriginalString.Should().NotBeNull();
        }

        [TestMethod]
        public void HttpRequest_Should_Add_Headers()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            get.Header("User-Agent", "unirest-net/1.0");
            post.Header("User-Agent", "unirest-net/1.0");
            delete.Header("User-Agent", "unirest-net/1.0");
            patch.Header("User-Agent", "unirest-net/1.0");
            put.Header("User-Agent", "unirest-net/1.0");

            get.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            post.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            delete.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            patch.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            put.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
        }

        [TestMethod]
        public void HttpRequest_Should_Add_Headers_Dictionary()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            get.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });
            post.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });
            delete.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });
            patch.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });
            put.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });

            get.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            post.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            delete.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            patch.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            put.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
        }

        [TestMethod]
        public void HttpRequest_Should_Return_String()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://www.google.com");
            var post = new HttpRequest(HttpMethod.Post, "http://www.google.com");
            var delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com");
            var put = new HttpRequest(HttpMethod.Put, "http://www.google.com");

            get.AsString().Body.Should().NotBeNullOrWhiteSpace();
            post.AsString().Body.Should().NotBeNullOrWhiteSpace();
            delete.AsString().Body.Should().NotBeNullOrWhiteSpace();
            patch.AsString().Body.Should().NotBeNullOrWhiteSpace();
            put.AsString().Body.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void HttpRequest_Should_Return_Stream()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://www.google.com");
            var post = new HttpRequest(HttpMethod.Post, "http://www.google.com");
            var delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com");
            var put = new HttpRequest(HttpMethod.Put, "http://www.google.com");

            get.AsBinary().Body.Should().NotBeNull();
            post.AsBinary().Body.Should().NotBeNull();
            delete.AsBinary().Body.Should().NotBeNull();
            patch.AsBinary().Body.Should().NotBeNull();
            put.AsBinary().Body.Should().NotBeNull();
        }

        [TestMethod]
        public void HttpRequest_Should_Return_Parsed_JSON()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://www.google.com");
            var post = new HttpRequest(HttpMethod.Post, "http://www.google.com");
            var delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com");
            var put = new HttpRequest(HttpMethod.Put, "http://www.google.com");

            get.AsJson<string>().Body.Should().NotBeNullOrWhiteSpace();
            post.AsJson<string>().Body.Should().NotBeNullOrWhiteSpace();
            delete.AsJson<string>().Body.Should().NotBeNullOrWhiteSpace();
            patch.AsJson<string>().Body.Should().NotBeNullOrWhiteSpace();
            put.AsJson<string>().Body.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void HttpRequest_Should_Return_String_Async()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://www.google.com").AsStringAsync();
            var post = new HttpRequest(HttpMethod.Post, "http://www.google.com").AsStringAsync();
            var delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com").AsStringAsync();
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com").AsStringAsync();
            var put = new HttpRequest(HttpMethod.Put, "http://www.google.com").AsStringAsync();

            Task.WaitAll(get, post, delete, patch, put);

            get.Result.Body.Should().NotBeNullOrWhiteSpace();
            post.Result.Body.Should().NotBeNullOrWhiteSpace();
            delete.Result.Body.Should().NotBeNullOrWhiteSpace();
            patch.Result.Body.Should().NotBeNullOrWhiteSpace();
            put.Result.Body.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void HttpRequest_Should_Return_Stream_Async()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://www.google.com").AsBinaryAsync();
            var post = new HttpRequest(HttpMethod.Post, "http://www.google.com").AsBinaryAsync();
            var delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com").AsBinaryAsync();
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com").AsBinaryAsync();
            var put = new HttpRequest(HttpMethod.Put, "http://www.google.com").AsBinaryAsync();

            Task.WaitAll(get, post, delete, patch, put);

            get.Result.Body.Should().NotBeNull();
            post.Result.Body.Should().NotBeNull();
            delete.Result.Body.Should().NotBeNull();
            patch.Result.Body.Should().NotBeNull();
            put.Result.Body.Should().NotBeNull();
        }

        [TestMethod]
        public void HttpRequest_Should_Return_Parsed_JSON_Async()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://www.google.com").AsJsonAsync<string>();
            var post = new HttpRequest(HttpMethod.Post, "http://www.google.com").AsJsonAsync<string>();
            var delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com").AsJsonAsync<string>();
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com").AsJsonAsync<string>();
            var put = new HttpRequest(HttpMethod.Put, "http://www.google.com").AsJsonAsync<string>();

            Task.WaitAll(get, post, delete, patch, put);

            get.Result.Body.Should().NotBeNullOrWhiteSpace();
            post.Result.Body.Should().NotBeNullOrWhiteSpace();
            delete.Result.Body.Should().NotBeNullOrWhiteSpace();
            patch.Result.Body.Should().NotBeNullOrWhiteSpace();
            put.Result.Body.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Construct()
        {
            Action post = () => new HttpRequest(HttpMethod.Post, "http://localhost");
            Action delete = () => new HttpRequest(HttpMethod.Delete, "http://localhost");
            Action patch = () => new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            Action put = () => new HttpRequest(HttpMethod.Put, "http://localhost");

            post.ShouldNotThrow();
            delete.ShouldNotThrow();
            patch.ShouldNotThrow();
            put.ShouldNotThrow();
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Not_Construct_With_Invalid_URL()
        {
            Action post = () => new HttpRequest(HttpMethod.Post, "http:///invalid");
            Action delete = () => new HttpRequest(HttpMethod.Delete, "http:///invalid");
            Action patch = () => new HttpRequest(new HttpMethod("PATCH"), "http:///invalid");
            Action put = () => new HttpRequest(HttpMethod.Put, "http:///invalid");

            post.ShouldThrow<ArgumentException>();
            delete.ShouldThrow<ArgumentException>();
            patch.ShouldThrow<ArgumentException>();
            put.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Not_Construct_With_None_HTTP_URL()
        {
            Action post = () => new HttpRequest(HttpMethod.Post, "mailto:localhost");
            Action delete = () => new HttpRequest(HttpMethod.Delete, "news://localhost");
            Action patch = () => new HttpRequest(new HttpMethod("PATCH"), "about:blank");
            Action put = () => new HttpRequest(HttpMethod.Put, "about:settings");

            post.ShouldThrow<ArgumentException>();
            delete.ShouldThrow<ArgumentException>();
            patch.ShouldThrow<ArgumentException>();
            put.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Construct_With_Correct_Verb()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            post.HttpMethod.Should().Be(HttpMethod.Post);
            delete.HttpMethod.Should().Be(HttpMethod.Delete);
            patch.HttpMethod.Should().Be(new HttpMethod("PATCH"));
            put.HttpMethod.Should().Be(HttpMethod.Put);
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Construct_With_Correct_URL()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            post.Url.OriginalString.Should().Be("http://localhost");
            delete.Url.OriginalString.Should().Be("http://localhost");
            patch.Url.OriginalString.Should().Be("http://localhost");
            put.Url.OriginalString.Should().Be("http://localhost");
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Construct_With_Headers()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            post.Url.OriginalString.Should().NotBeNull();
            delete.Url.OriginalString.Should().NotBeNull();
            patch.Url.OriginalString.Should().NotBeNull();
            put.Url.OriginalString.Should().NotBeNull();
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Add_Headers()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            post.Header("User-Agent", "unirest-net/1.0");
            delete.Header("User-Agent", "unirest-net/1.0");
            patch.Header("User-Agent", "unirest-net/1.0");
            put.Header("User-Agent", "unirest-net/1.0");

            post.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            delete.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            patch.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            put.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Add_Headers_Dictionary()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            post.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });
            delete.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });
            patch.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });
            put.SetHeaders(new Dictionary<string, object> { { "User-Agent", "unirest-net/1.0" } });

            post.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            delete.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            patch.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            put.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Encode_Fields()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            post.SetField("key", "value");
            delete.SetField("key", "value");
            patch.SetField("key", "value");
            put.SetField("key", "value");

            post.Body.Should().NotBe(string.Empty);
            delete.Body.Should().NotBe(string.Empty);
            patch.Body.Should().NotBe(string.Empty);
            put.Body.Should().NotBe(string.Empty);
        }

        [TestMethod]
        public void HttpRequest_With_Body_Should_Encode_File()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            var stream = new MemoryStream();

            post.SetField(stream);
            delete.SetField(stream);
            patch.SetField(stream);
            put.SetField(stream);

            post.Body.Should().NotBe(string.Empty);
            delete.Body.Should().NotBe(string.Empty);
            patch.Body.Should().NotBe(string.Empty);
            put.Body.Should().NotBe(string.Empty);
        }

        [TestMethod]
        public void HttpRequestWithBody_Should_Encode_Multiple_Fields()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            var dict = new Dictionary<string, object>
                {
                    {"key", "value"},
                    {"key2", "value2"},
                    {"key3", new MemoryStream()}
                };

            post.SetFields(dict);
            delete.SetFields(dict);
            patch.SetFields(dict);
            put.SetFields(dict);

            post.Body.Should().NotBe(string.Empty);
            delete.Body.Should().NotBe(string.Empty);
            patch.Body.Should().NotBe(string.Empty);
            put.Body.Should().NotBe(string.Empty);
        }

        [TestMethod]
        public void HttpRequestWithBody_Should_Add_String_Body()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            post.SetBody("test");
            delete.SetBody("test");
            patch.SetBody("test");
            put.SetBody("test");

            post.Body.Should().NotBe(string.Empty);
            delete.Body.Should().NotBe(string.Empty);
            patch.Body.Should().NotBe(string.Empty);
            put.Body.Should().NotBe(string.Empty);
        }

        [TestMethod]
        public void HttpRequestWithBody_Should_Add_JSON_Body()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            post.SetBody(new List<int> { 1, 2, 3 });
            delete.SetBody(new List<int> { 1, 2, 3 });
            patch.SetBody(new List<int> { 1, 2, 3 });
            put.SetBody(new List<int> { 1, 2, 3 });

            post.Body.Should().NotBe(string.Empty);
            delete.Body.Should().NotBe(string.Empty);
            patch.Body.Should().NotBe(string.Empty);
            put.Body.Should().NotBe(string.Empty);
        }

        [TestMethod]
        public void Http_Request_Shouldnt_Add_Fields_To_Get()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://localhost");
            Action addStringField = () => get.SetField("name", "value");
            Action addKeyField = () => get.SetField(new MemoryStream());
            Action addStringFields = () => get.SetFields(new Dictionary<string, object> {{"name", "value"}});
            Action addKeyFields = () => get.SetFields(new Dictionary<string, object> {{"key", new MemoryStream()}});

            addStringField.ShouldThrow<InvalidOperationException>();
            addKeyField.ShouldThrow<InvalidOperationException>();
            addStringFields.ShouldThrow<InvalidOperationException>();
            addKeyFields.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void Http_Request_Shouldnt_Add_Body_To_Get()
        {
            var get = new HttpRequest(HttpMethod.Get, "http://localhost");
            Action addStringBody = () => get.SetBody("string");
            Action addJsonBody = () => get.SetBody(new List<int> {1,2,3});

            addStringBody.ShouldThrow<InvalidOperationException>();
            addJsonBody.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void HttpRequestWithBody_Should_Not_Allow_Body_For_Request_With_Field()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            var stream = new MemoryStream();

            post.SetField(stream);
            delete.SetField(stream);
            patch.SetField(stream);
            put.SetField(stream);

            Action addBodyPost = () => post.SetBody("test");
            Action addBodyDelete = () => delete.SetBody("test");
            Action addBodyPatch = () => patch.SetBody("test");
            Action addBodyPut = () => put.SetBody("test");
            Action addObjectBodyPost = () => post.SetBody(1);
            Action addObjectBodyDelete = () => delete.SetBody(1);
            Action addObjectBodyPatch = () => patch.SetBody(1);
            Action addObjectBodyPut = () => put.SetBody(1);

            addBodyPost.ShouldThrow<InvalidOperationException>();
            addBodyDelete.ShouldThrow<InvalidOperationException>();
            addBodyPatch.ShouldThrow<InvalidOperationException>();
            addBodyPut.ShouldThrow<InvalidOperationException>();
            addObjectBodyPost.ShouldThrow<InvalidOperationException>();
            addObjectBodyDelete.ShouldThrow<InvalidOperationException>();
            addObjectBodyPatch.ShouldThrow<InvalidOperationException>();
            addObjectBodyPut.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void HttpRequestWithBody_Should_Not_Allow_Fields_For_Request_With_Body()
        {
            var post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var put = new HttpRequest(HttpMethod.Put, "http://localhost");

            var stream = new MemoryStream();

            post.SetBody("test");
            delete.SetBody("test");
            patch.SetBody("test");
            put.SetBody("lalala");

            Action addFieldPost = () => post.SetField("key", "value");
            Action addFieldDelete = () => delete.SetField("key", "value");
            Action addFieldPatch = () => patch.SetField("key", "value");
            Action addFieldPut = () => put.SetField("key", "value");
            Action addStreamFieldPost = () => post.SetField(stream);
            Action addStreamFieldDelete = () => delete.SetField(stream);
            Action addStreamFieldPatch = () => patch.SetField(stream);
            Action addStreamFieldPut = () => put.SetField(stream);
            Action addFieldsPost = () => post.SetFields(new Dictionary<string, object> {{"test", "test"}});
            Action addFieldsDelete = () => delete.SetFields(new Dictionary<string, object> {{"test", "test"}});
            Action addFieldsPatch = () => patch.SetFields(new Dictionary<string, object> {{"test", "test"}});
            Action addFieldsPut = () => put.SetFields(new Dictionary<string, object> {{"test", "test"}});

            addFieldPost.ShouldThrow<InvalidOperationException>();
            addFieldDelete.ShouldThrow<InvalidOperationException>();
            addFieldPatch.ShouldThrow<InvalidOperationException>();
            addFieldPut.ShouldThrow<InvalidOperationException>();
            addStreamFieldPost.ShouldThrow<InvalidOperationException>();
            addStreamFieldDelete.ShouldThrow<InvalidOperationException>();
            addStreamFieldPatch.ShouldThrow<InvalidOperationException>();
            addStreamFieldPut.ShouldThrow<InvalidOperationException>();
            addFieldsPost.ShouldThrow<InvalidOperationException>();
            addFieldsDelete.ShouldThrow<InvalidOperationException>();
            addFieldsPatch.ShouldThrow<InvalidOperationException>();
            addFieldsPut.ShouldThrow<InvalidOperationException>();
        }
    }
}
