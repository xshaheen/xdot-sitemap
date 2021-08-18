// Copyright (c) Mahmoud Shaheen, 2021. All rights reserved.
// Licensed under the Apache 2.0 license.
// See the LICENSE.txt file in the project root for full license information.

using System.Xml.Linq;
using FluentAssertions;
using X.Core.Extensions;

namespace Sitemap.Tests {
    public class TestBase {
        protected static void AssertEquivalentXml(string result, string expected) {
            result = XDocument.Parse(result).ToInvariantString();
            expected = XDocument.Parse(expected).ToInvariantString();
            result.Should().Be(expected);
        }
    }
}
