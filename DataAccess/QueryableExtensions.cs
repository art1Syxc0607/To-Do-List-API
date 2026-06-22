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


    public static IQueryable<UserTask> ApplySorting(
       this IQueryable<UserTask> query,
       string? sortBy,
       bool sortDesc = true)
    {
        return (sortBy?.ToLower() ?? "createdAt") switch
        {
            "title" => sortDesc
                ? query.OrderByDescending(t => t.Title)
                : query.OrderBy(t => t.Title),

            "status" => sortDesc
                ? query.OrderByDescending(t => t.Status)
                : query.OrderBy(t => t.Status),

            "priority" => sortDesc
                ? query.OrderByDescending(t => t.Priority)
                : query.OrderBy(t => t.Priority),

            "dueDate" => sortDesc
                ? query.OrderByDescending(t => t.DueDate)
                : query.OrderBy(t => t.DueDate),

            "updatedAt" => sortDesc
                ? query.OrderByDescending(t => t.UpdatedAt)
                : query.OrderBy(t => t.UpdatedAt),

            "createdAt" => sortDesc
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt),

            _ => query.OrderByDescending(t => t.CreatedAt) // по умолчанию
        };
    }

}


