// Copyright (c) Mahmoud Shaheen, 2021. All rights reserved.
// Licensed under the Apache 2.0 license.
// See the LICENSE.txt file in the project root for full license information.

using System;
using JetBrains.Annotations;
using X.Sitemap.Internals;

namespace X.Sitemap {
    /// <summary>Represent a node that reference a sub-sitemap.</summary>
    [PublicAPI]
    public record SitemapReference {
        private readonly string? _location;

        /// <summary>
        /// Identifies the location of the Sitemap.
        /// This location can be a Sitemap, an Atom file, RSS file or a simple text file.
        /// </summary>
        public string Location {
            get => _location!;
            init => _location = Uri.EscapeUriString(value.ToLowerInvariant().RemoveHiddenChars());
        }

        /// <summary>
        /// Identifies the time that the corresponding Sitemap file was modified.
        /// It does not correspond to the time that any of the pages listed in that Sitemap were changed.
        /// By providing the last modification timestamp, you enable search engine crawlers to
        /// retrieve only a subset of the Sitemaps in the index i.e. a crawler may only retrieve
        /// Sitemaps that were modified since a certain date. This incremental Sitemap fetching
        /// mechanism allows for the rapid discovery of new URLs on very large sites.
        /// </summary>
        public DateTime? LastModified { get; init; }
    }
}
