// Copyright (c) Mahmoud Shaheen, 2021. All rights reserved.
// Licensed under the Apache 2.0 license.
// See the LICENSE.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using X.Sitemap;
using Xunit;

namespace Sitemap.Tests {
    public class SitemapUrlsTests : TestBase {
        public static readonly TheoryData<List<SitemapUrl>, string> TestData = new() {
            // basic
            {
                new List<SitemapUrl> {
                    new(location: "https://www.example.com"),
                    new(location: "https://www.example.com/contact-us"),
                },
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "    <loc>https://www.example.com</loc>" +
                "  </url>" +
                "  <url>" +
                "    <loc>https://www.example.com/contact-us</loc>" +
                "  </url>" +
                "</urlset>"
            },
            // with priority, last modified, change frequency
            {
                new List<SitemapUrl> {
                    new(
                        location: "https://www.example.com",
                        lastModified: new DateTime(year: 2021, month: 3, day: 15),
                        changeFrequency: ChangeFrequency.Daily,
                        priority: 0.8f),
                },
                "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "   <loc>https://www.example.com</loc>" +
                "   <priority>0.8</priority>" +
                "   <changefreq>daily</changefreq>" +
                "   <lastmod>2021-03-15</lastmod>" +
                "  </url>" +
                "</urlset>"
            },
            // Urls follow RFC-3986
            {
                new List<SitemapUrl> {
                    new(location: "https://www.Example.com/ümlaT.html"),
                    new(location: "https://www.example.com/اداره-اعلانات"),
                },
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "    <loc>https://www.example.com/%C3%BCmlat.html</loc>" +
                "  </url>" +
                "  <url>" +
                "    <loc>https://www.example.com/%D8%A7%D8%AF%D8%A7%D8%B1%D9%87-%D8%A7%D8%B9%D9%84%D8%A7%D9%86%D8%A7%D8%AA</loc>" +
                "  </url>" +
                "</urlset>"
            },
            // XML entity escape URLs
            {
                new List<SitemapUrl> { new(location: "https://www.example.com/ümlat.html&q=name") },
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "    <loc>https://www.example.com/%C3%BCmlat.html&amp;q=name</loc>" +
                "  </url>" +
                "</urlset>"
            },
        };

        [Theory]
        [MemberData(memberName: nameof(TestData))]
        public async Task write_to_stream_test(List<SitemapUrl> urls, string expected) {
            string result;

            await using (var stream = new MemoryStream()) {
                await urls.WriteToAsync(stream);
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            AssertEquivalentXml(result, expected);
        }

        [Fact]
        public async Task write_should_add_xhtml_namespace_when_define_alternatives() {
            var urls = new List<SitemapUrl> {
                new(alternateLocations: new SitemapAlternateUrl[] {
                    new() {
                        Location = "https://www.example.com/ar/page.html",
                        LanguageCode = "ar",
                    },
                    new() {
                        Location = "https://www.example.com/en/page.html",
                        LanguageCode = "en",
                    },
                }),
            };

            string result;

            await using (var stream = new MemoryStream()) {
                await urls.WriteToAsync(stream);
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            result.Should().Contain("xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"");
        }

        [Fact]
        public async Task write_should_write_alternative_urls_when_provide_any() {
            var urls = new List<SitemapUrl> {
                new(alternateLocations: new SitemapAlternateUrl[] {
                    new() {
                        Location = "https://www.example.com/english/page.html",
                        LanguageCode = "en",
                    },
                    new() {
                        Location = "https://www.example.com/deutsch/page.html",
                        LanguageCode = "de",
                    },
                    new() {
                        Location = "https://www.example.com/schweiz-deutsch/page.html",
                        LanguageCode = "de-ch",
                    },
                }),
            };

            const string expected =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<urlset xmlns:xhtml=\"http://www.w3.org/1999/xhtml\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "    <loc>https://www.example.com/english/page.html</loc>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.example.com/english/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"https://www.example.com/deutsch/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de-ch\" href=\"https://www.example.com/schweiz-deutsch/page.html\"/>" +
                "  </url>" +
                "  <url>" +
                "    <loc>https://www.example.com/deutsch/page.html</loc>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.example.com/english/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"https://www.example.com/deutsch/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de-ch\" href=\"https://www.example.com/schweiz-deutsch/page.html\"/>" +
                "  </url>" +
                "  <url>" +
                "    <loc>https://www.example.com/schweiz-deutsch/page.html</loc>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.example.com/english/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"https://www.example.com/deutsch/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de-ch\" href=\"https://www.example.com/schweiz-deutsch/page.html\"/>" +
                "  </url>" +
                "</urlset>";

            string result;

            await using (var stream = new MemoryStream()) {
                await urls.WriteToAsync(stream);
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            AssertEquivalentXml(result, expected);
        }
    }
}
