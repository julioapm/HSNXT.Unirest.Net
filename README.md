# HSNXT.Unirest.Net

Improved version of Kong's Unirest.Net library.

This library fixes a couple problems and improves upon Kong's library:
* Fixed methods not using proper C# code style
* Added real async (async methods simply used GetResult, so they ran synchronously)
* Added shared HttpClient (this can lead to running out of file descriptors)
* Authentication support
* Support for uploading files (multipart requests)
* Ported to .NET Standard
* Added support for OPTIONS, HEAD and TRACE methods
* Added an object initializer-based pattern for creating requests

Documentation is coming soon. In the meanwhile, you can use these test methods as reference:
```cs

        [TestMethod]
        public void BasicTextTest()
        {
            new GetRequestUrl("http://localhost:8080/test/text").AsString().Body
                .ShouldBeEquivalentTo("The quick brown fox jumps over the lazy dog");
        }
        
        [TestMethod]
        public async Task BasicTextTestAsync()
        {
            (await new GetRequestUrl("http://localhost:8080/test/text").AsStringAsync())
                .Body
                .ShouldBeEquivalentTo("The quick brown fox jumps over the lazy dog");
        }
        
        [TestMethod]
        public void ContentType()
        {
            new PostRequest("http://localhost:8080/test/content-type")
                {
                    Headers =
                    {
                        ContentType = "text/html; charset=utf-8"
                    },
                    BodyString = ExampleHtml
                }.AsString().Body
                .ShouldBeEquivalentTo("text/html; charset=utf-8");
        }
        
        [TestMethod]
        public void Accept()
        {
            new GetRequestUrl("http://localhost:8080/test/accept")
                {
                    Headers =
                    {
                        Accept = "application/json"
                    }
                }.AsString().Body
                .ShouldBeEquivalentTo("application/json");
        }

        [TestMethod]
        public void NoContentTypeInGet()
        {
            new Action(() =>
            {
                new GetRequestUrl("http://localhost:8080/test/accept")
                {
                    Headers =
                    {
                        ContentType = "text/html; charset=utf-8"
                    }
                }.AsString();
            }).ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void QueryParams1Entry()
        {
            new GetRequestUrl("http://localhost:8080/test/echo")
                .SetField("key", "value")
                .AsString().Body
                .ShouldBeEquivalentTo("/test/echo?key=value");
        }

        [TestMethod]
        public void QueryParams2Entry()
        {
            new GetRequestUrl("http://localhost:8080/test/echo")
                .SetField("key", "value")
                .SetField("key2", "value2")
                .AsString().Body
                .ShouldBeEquivalentTo("/test/echo?key=value&key2=value2");
        }
        
        [TestMethod]
        public void QueryParams2RandomObjects()
        {
            new GetRequestUrl("http://localhost:8080/test/echo")
                .SetField("key", typeof(TestMethodAttribute))
                .SetField("key2", new object())
                .SetField("key3", null)
                .AsString().Body
                .ShouldBeEquivalentTo("/test/echo" +
                                      "?key=Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute" +
                                      "&key2=System.Object" +
                                      "&key3=null");
        }
```
