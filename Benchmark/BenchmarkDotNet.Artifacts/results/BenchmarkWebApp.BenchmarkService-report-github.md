```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700F, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  Job-HYWXUF : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

InvocationCount=1  IterationCount=1  RunStrategy=Monitoring  
UnrollFactor=1  

```
| Method                            | Mean       | Error |
|---------------------------------- |-----------:|------:|
| RunLoadArticles                   |   545.7 ms |    NA |
| RunGetAllArticles                 | 2,335.4 ms |    NA |
| RunGetAllCategories               |   348.2 ms |    NA |
| RunGetArticlesWithCategoriesModel | 2,397.7 ms |    NA |
| RunSearch                         | 2,498.7 ms |    NA |
| RunPublishArticle                 |   353.6 ms |    NA |
| RunDeleteArticle                  |   425.2 ms |    NA |
| RunEditArticle                    |   424.5 ms |    NA |
| RunGetCategoryByName              |   345.0 ms |    NA |
| RunGetCategoryById                |   344.9 ms |    NA |
