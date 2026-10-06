using Backend.Db;
using Backend.DTOs.Analytics;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class RevenueAnalyticsService : IRevenueAnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public RevenueAnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RevenueResponse> GetRevenueAsync(
            RevenueRequest request)
        {
            if (request.Period.ToLower() != "day" &&
                request.Period.ToLower() != "month")
            {
                throw new ArgumentException(
                    "Period must be 'day' or 'month'.");
            }

            if (request.Year <= 0)
            {
                throw new ArgumentException("Invalid year.");
            }

            if (request.Period.ToLower() == "day")
            {
                if (request.Month == null ||
                    request.Month < 1 ||
                    request.Month > 12)
                {
                    throw new ArgumentException(
                        "Month is required for daily revenue.");
                }

                int month = request.Month.Value;

                var startDate = new DateTime(
                    request.Year,
                    month,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc);

                var endDate = startDate.AddMonths(1);

                var transactions = await _context.Transactions
                    .Where(t =>
                        t.PaymentStatus == PaymentStatus.PAID &&
                        t.PaidAt != null &&
                        t.PaidAt >= startDate &&
                        t.PaidAt < endDate)
                    .ToListAsync();

                var groupedRevenue = transactions
                    .GroupBy(t => t.PaidAt!.Value.Date)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(t => t.Amount)
                    );

                int daysInMonth =
                    DateTime.DaysInMonth(request.Year, month);

                var data = new List<RevenueDataResponse>();

                for (int day = 1; day <= daysInMonth; day++)
                {
                    var date = new DateTime(
                        request.Year,
                        month,
                        day);

                    groupedRevenue.TryGetValue(
                        date,
                        out decimal revenue);

                    data.Add(new RevenueDataResponse
                    {
                        Label = date.ToString("MMM dd"),
                        Revenue = revenue
                    });
                }

                return new RevenueResponse
                {
                    Period = "day",
                    Year = request.Year,
                    Month = month,
                    TotalRevenue = data.Sum(x => x.Revenue),
                    Data = data
                };
            }


            var yearStart = new DateTime(
                request.Year,
                1,
                1);

            var yearEnd = yearStart.AddYears(1);

            var yearlyTransactions = await _context.Transactions
                .Where(t =>
                    t.PaymentStatus == PaymentStatus.PAID &&
                    t.PaidAt != null &&
                    t.PaidAt >= yearStart &&
                    t.PaidAt < yearEnd)
                .ToListAsync();

            var monthlyRevenue = yearlyTransactions
                .GroupBy(t => t.PaidAt!.Value.Month)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(t => t.Amount)
                );

            var monthlyData = new List<RevenueDataResponse>();

            for (int month = 1; month <= 12; month++)
            {
                monthlyRevenue.TryGetValue(
                    month,
                    out decimal revenue);

                var date = new DateTime(
                    request.Year,
                    month,
                    1);

                monthlyData.Add(new RevenueDataResponse
                {
                    Label = date.ToString("MMM"),
                    Revenue = revenue
                });
            }

            return new RevenueResponse
            {
                Period = "month",
                Year = request.Year,
                Month = null,
                TotalRevenue = monthlyData.Sum(x => x.Revenue),
                Data = monthlyData
            };
        }
    }
}