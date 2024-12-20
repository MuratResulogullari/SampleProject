﻿namespace SampleProject.CatalogService.API.SeedWork;

public readonly struct PaginationRequest(int pageSize = 5, int pageIndex = 0)
{
    public readonly int PageSize { get; } = pageSize;

    public readonly int PageIndex { get; } = pageIndex;
}
