```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700F, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  Job-MIHALS : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

InvocationCount=1  IterationCount=1  RunStrategy=Monitoring  
UnrollFactor=1  

```
| Method                            | Mean       | Error |
|---------------------------------- |-----------:|------:|
| RunLoadArticles                   |   520.2 ms |    NA |
| RunGetAllArticles                 | 1,537.8 ms |    NA |
| RunGetAllCategories               |   345.0 ms |    NA |
| RunGetArticlesWithCategoriesModel | 1,643.9 ms |    NA |
| RunSearch                         | 2,083.6 ms |    NA |
| RunPublishArticle                 |   353.5 ms |    NA |
| RunDeleteArticle                  |   420.3 ms |    NA |
| RunEditArticle                    |   419.6 ms |    NA |
| RunGetCategoryByName              |   386.3 ms |    NA |
| RunGetCategoryById                |   383.5 ms |    NA |
