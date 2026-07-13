using System;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events.EventArgs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Lagrange.Milky.Logging;

public partial class LagrangeLoggingService(ILoggerFactory lf, BotContext lagrange) : IHostedService
{
    private readonly ILoggerFactory _lf = lf;

    private readonly BotContext _lagrange = lagrange;

    public Task StartAsync(CancellationToken ct)
    {
        _lagrange.EventInvoker.RegisterEvent<BotLogEvent>(OnLog);

        return Task.CompletedTask;
    }

    private void OnLog(BotContext ctx, BotLogEvent e)
    {
        var level = (LogLevel)e.Level;

        var logger = _lf.CreateLogger(e.Tag);
        if (logger.IsEnabled(level)) LoggerUtility.Log(logger, level, e.Message, e.Exception);
    }

    public Task StopAsync(CancellationToken ct)
    {
        _lagrange.EventInvoker.UnregisterEvent<BotLogEvent>(OnLog);

        return Task.CompletedTask;
    }

    private static partial class LoggerUtility
    {
        [LoggerMessage("{Message}", SkipEnabledCheck = true)]
        public static partial void Log(ILogger logger, LogLevel level, string message, Exception? e);
    }
}