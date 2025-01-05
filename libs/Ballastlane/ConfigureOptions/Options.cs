using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ballastlane.Filters;

namespace Ballastlane.ConfigureOptions;

public class Options : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.Filters.Add(typeof(ResultFilter));
        options.Filters.Add(typeof(ExceptionFilter));
    }
}
