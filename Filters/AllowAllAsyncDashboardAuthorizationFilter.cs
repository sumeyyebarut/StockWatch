using Hangfire.Dashboard;

namespace StockWatch.Filters{
    public class AllowAllAsyncDashboardAuthorizationFilter : IDashboardAsyncAuthorizationFilter
{
    public async Task<bool> AuthorizeAsync(DashboardContext context)
    {
        return await Task.FromResult(true); // Herkese izin ver
    }
}

}