using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bookify.API.OpenApi;

public sealed class ConfigureSwaggerOptions:IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var descrption in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(descrption.GroupName,CreateVersionInfo(descrption));

        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
       Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = $"Bookify.Api v{description.ApiVersion}",
            Version = description.ApiVersion.ToString()
        };
        if (description.IsDeprecated)
        {
        openApiInfo.Description += " This API version has been deprecated.";
        }
        return openApiInfo;
    }
}