// Copyright (c) Mahmoud Shaheen, 2021. All rights reserved.
// Licensed under the Apache 2.0 license.
// See the LICENSE.txt file in the project root for full license information.

using System.Xml.Linq;
using FluentAssertions;

namespace Sitemap.Tests {
    public class TestBase {
        protected static void AssertEquivalentXml(string result, string expected) {
            result = XDocument.Parse(result).ToString();
            expected = XDocument.Parse(expected).ToString();
            result.Should().Be(expected);
        }
    }
}
