using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Seo.Queries;

public record GetSitemapRequest : IRequest<OperationResult<string>>;

public class GetSitemapHandler(ISeoRepository repository, StorefrontSettings storefrontSettings)
    : IRequestHandler<GetSitemapRequest, OperationResult<string>>
{
    public async Task<OperationResult<string>> Handle(
        GetSitemapRequest request,
        CancellationToken cancellationToken)
    {
        var urls = await repository.GetPublicSitemapUrlsAsync(cancellationToken);
        var baseUrl = storefrontSettings.BaseUrl.TrimEnd('/');

        var body = string.Join(
            Environment.NewLine,
            urls.Select(url =>
            {
                var loc = XmlEscape($"{baseUrl}{url.Path}");
                var lastMod = url.LastModifiedUtc.HasValue
                    ? $"<lastmod>{url.LastModifiedUtc.Value:yyyy-MM-dd}</lastmod>"
                    : string.Empty;
                return $"  <url><loc>{loc}</loc>{lastMod}<changefreq>{url.ChangeFrequency}</changefreq><priority>{url.Priority:0.0}</priority></url>";
            }));

        var xml = $"""
                   <?xml version="1.0" encoding="UTF-8"?>
                   <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
                   {body}
                   </urlset>
                   """;

        return xml;
    }

    private static string XmlEscape(string value)
        => value
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
}
