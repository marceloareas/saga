namespace saga.Models.DTOs.Common

{
     public sealed class PagedResult<T>
     {
         public required IReadOnlyList<T> Items { get; init; }
         public int Page { get; init; }
         public int PageSize { get; init; }
         public int TotalCount { get; init; }
         public int TotalPages => (int)System.Math.Ceiling((double)TotalCount / PageSize);
     }
}
