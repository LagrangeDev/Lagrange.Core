﻿using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lagrange.OneBot.Core.Network.Service;

public sealed class ReverseWSServiceFactory(IServiceProvider services) : LagrangeWebServiceFactory(services), ILagrangeWebServiceFactory<ReverseWSService>
{
    public override ILagrangeWebService Create()
    {
        var config = _config ?? throw new InvalidOperationException("Configuration must be provided");
        var options = _services.GetRequiredService<IOptionsSnapshot<ReverseWSServiceOptions>>();
        config.Bind(options.Value);

        return _services.GetRequiredService<ReverseWSService>();
    }
}