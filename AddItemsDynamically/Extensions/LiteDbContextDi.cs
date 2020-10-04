using System;
using System.Collections.Generic;
using SIPVS.litedb;

namespace Microsoft.Extensions.DependencyInjection
{
    static class LiteDbContextDI
    {
        /// supportive extension function for dependency
        /// injection of the service
        public static IServiceCollection AddDataRepository(
            this IServiceCollection services)
        {
            return services.AddSingleton<ILiteDbContext, LiteDbContext>();
        }

    }
}