# Xdot.Sitemap

A package for .NET to help you build a sitemap according to the google recommendations. see [this link to learn about sitemaps][sitemap-google-docs].

**if you like this work, please consider give the project star 🌟**

## Installation

Using the [.NET CLI tools][dotnet-core-cli-tools]:

```sh
dotnet add package Xdot.Sitemap
```

Using the [NuGet CLI][nuget-cli]:

```sh
nuget install Xdot.Sitemap
```

Using the [Package Manager Console][package-manager-console]:

```powershell
Install-Package Xdot.Sitemap
```

## Usage

### Write a sitemap to stream

```c#
var urls = new List<SitemapUrl> 
{
    new("https://www.example.com"),
    new("https://www.example.com/contact-us"),
}

await using var stream = new MemoryStream();
await urls.WriteToAsync(stream);
```

This code will write the following to the memory stream:

```xml
<?xml version="1.0" encoding="utf-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url>
    <loc>https://www.example.com</loc>
  </url>
  <url>
    <loc>https://www.example.com/contact-us</loc>
  </url>
</urlset>
```

### Write localized versions for a URL

```c#
var urls = new List<SitemapUrl> 
{
    new(new SitemapAlternateUrl[] 
    {
        new() 
        {
            Location = "https://www.example.com/english/page.html",
            LanguageCode = "en",
        },
        new() 
        {
            Location = "https://www.example.com/deutsch/page.html",
            LanguageCode = "de",
        },
        new() 
        {
            Location = "https://www.example.com/schweiz-deutsch/page.html",
            LanguageCode = "de-ch",
        },
    }),
    
    // ...
};

await using var stream = new MemoryStream();
await urls.WriteToAsync(stream);
```

This code will write the following to the memory stream:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url>
    <loc>https://www.example.com/english/page.html</loc>
    <xhtml:link rel="alternate" hreflang="en" href="https://www.example.com/english/page.html" />
    <xhtml:link rel="alternate" hreflang="de" href="https://www.example.com/deutsch/page.html" />
    <xhtml:link rel="alternate" hreflang="de-ch" href="https://www.example.com/schweiz-deutsch/page.html" />
  </url>
  <url>
    <loc>https://www.example.com/deutsch/page.html</loc>
    <xhtml:link rel="alternate" hreflang="en" href="https://www.example.com/english/page.html" />
    <xhtml:link rel="alternate" hreflang="de" href="https://www.example.com/deutsch/page.html" />
    <xhtml:link rel="alternate" hreflang="de-ch" href="https://www.example.com/schweiz-deutsch/page.html" />
  </url>
  <url>
    <loc>https://www.example.com/schweiz-deutsch/page.html</loc>
    <xhtml:link rel="alternate" hreflang="en" href="https://www.example.com/english/page.html" />
    <xhtml:link rel="alternate" hreflang="de" href="https://www.example.com/deutsch/page.html" />
    <xhtml:link rel="alternate" hreflang="de-ch" href="https://www.example.com/schweiz-deutsch/page.html" />
  </url>
</urlset>
```


### Write Sitemap Index

```c#
var sitemapReferences = new List<SitemapReference> 
{
    new() { Location = "https://www.Example.com/ümlaT-sitemap.xml" },
    new() { Location = "https://www.example.com/اداره-اعلانات" },
}

await using var stream = new MemoryStream()
await references.WriteToAsync(stream);
```
This code will write the following to the memory stream:

```xml
<?xml version="1.0" encoding="utf-8"?>
<sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <sitemap>
    <loc>https://www.example.com/%C3%BCmlat-sitemap.xml</loc>
  </sitemap>
  <sitemap>
    <loc>https://www.example.com/%D8%A7%D8%AF%D8%A7%D8%B1%D9%87-%D8%A7%D8%B9%D9%84%D8%A7%D9%86%D8%A7%D8%AA</loc>
  </sitemap>
</sitemapindex>
```

## License

This project is licensed under the Apache 2.0 license.

## Contact

If you have any suggestions, comments or questions, please feel free to contact me on:

Email: mxshaheen@gmail.com


[sitemap-google-docs]: https://developers.google.com/search/docs/advanced/sitemaps/overview
[dotnet-core-cli-tools]: https://docs.microsoft.com/en-us/dotnet/core/tools/
[nuget-cli]: https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference
[package-manager-console]: https://docs.microsoft.com/en-us/nuget/tools/package-manager-console
