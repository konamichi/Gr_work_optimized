using BenchmarkDotNet.Running;
using BenchmarkWebApp;

namespace Benchmark;

class Program
{
    static void Main(string[] args)
    {  
        var summary = BenchmarkRunner.Run<BenchmarkService>();      
        //service.RunEditArticle();
        //service.RunDeleteArticle();
    }
}

