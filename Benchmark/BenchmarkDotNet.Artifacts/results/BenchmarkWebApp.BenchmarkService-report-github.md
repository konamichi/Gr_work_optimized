```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700F, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  Job-SMXCYM : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

InvocationCount=1  IterationCount=1  RunStrategy=Monitoring  
UnrollFactor=1  

```
| Method                            | Mean       | Error |
|---------------------------------- |-----------:|------:|
| RunLoadArticles                   | 1,552.6 ms |    NA |
| RunGetAllArticles                 |   352.6 ms |    NA |
| RunGetAllCategories               |   340.6 ms |    NA |
| RunGetArticlesWithCategoriesModel |   360.2 ms |    NA |
| RunSearch                         |   357.5 ms |    NA |
| RunPublishArticle                 |   333.8 ms |    NA |
| RunDeleteArticle                  |   402.6 ms |    NA |
| RunEditArticle                    |   403.8 ms |    NA |
| RunGetCategoryByName              |   370.5 ms |    NA |
| RunGetCategoryById                |   369.8 ms |    NA |
