using DataAccess.Entities;
using System.Linq.Expressions;

namespace DataAccess.Repositories;
public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    public static IQueryable<UserTask> ApplyDueDateFilter(
this IQueryable<UserTask> query,
string? dueDateRange)
    {
        if (string.IsNullOrWhiteSpace(dueDateRange))
            return query;

        var today = DateTime.Today;

        return dueDateRange.ToLower() switch
        {
            "null" => query.Where(t => !t.DueDate.HasValue),
            "today" => query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == today),
            "week" => query.Where(t => t.DueDate.HasValue &&
                t.DueDate.Value.Date <= today.AddDays(7 - (int)today.DayOfWeek)),
            "overdue" => query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date < today),
            _ => throw new InvalidOperationException($"Неизвестный диапазон: {dueDateRange}")
        };
    }

}


