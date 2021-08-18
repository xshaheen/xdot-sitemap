// Copyright (c) Mahmoud Shaheen, 2021. All rights reserved.
// Licensed under the Apache 2.0 license.
// See the LICENSE.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using JetBrains.Annotations;

namespace X.Sitemap {
    /// <summary>Sitemap index file builder.</summary>
    /// <remarks>https://developers.google.com/search/docs/advanced/sitemaps/large-sitemaps</remarks>
    [PublicAPI]
    public static class SitemapIndexBuilder {
        /// <summary>Write sitemap index file into the stream.</summary>
        public static async Task WriteToAsync(this IEnumerable<SitemapReference> sitemapReferences, Stream output) {
            await using var writer = XmlWriter.Create(output, SitemapConstants.WriterSettings);
            await writer.WriteStartDocumentAsync();

            await writer.WriteStartElementAsync(
                prefix: null,
                localName: "sitemapindex",
                ns: "http://www.sitemaps.org/schemas/sitemap/0.9");

            // Write sitemaps URL.
            foreach (var sitemapReference in sitemapReferences) {
                await _WriteSitemapRefNodeAsync(writer, sitemapReference);
            }

            await writer.WriteEndElementAsync();
        }

        private static async Task _WriteSitemapRefNodeAsync(XmlWriter writer, SitemapReference sitemapRef) {
            await writer.WriteStartElementAsync(localName: "sitemap", prefix: null, ns: null);
            await writer.WriteElementStringAsync(localName: "loc", value: sitemapRef.Location, prefix: null, ns: null);

            if (sitemapRef.LastModified.HasValue) {
                var value = sitemapRef.LastModified.Value.ToString(
                    SitemapConstants.SitemapDateFormat,
                    CultureInfo.InvariantCulture);

                await writer.WriteElementStringAsync(prefix: null, localName: "lastmod", ns: null, value);
            }

            await writer.WriteEndElementAsync();
        }
    }
}
