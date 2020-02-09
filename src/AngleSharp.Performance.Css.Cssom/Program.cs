using System;
using System.Collections.Generic;

namespace AngleSharp.Performance.Css.Cssom
{
    class Program
    {
        static void Main(string[] args)
        {
            var stylesheets = new UrlTests(
                extension: ".css",
                withBuffer: true);

            stylesheets.Include("https://github.com/AngleSharp/AngleSharp.Css").Wait();

            var parsers = new List<ITestee>
            {
                new NonCacheTestee(),
                new CachedTestee(),
            };

            var testsuite = new TestSuite(parsers, stylesheets.Tests, new Output())
            {
                NumberOfRepeats = 5,
                NumberOfReRuns = 1,
            };

            testsuite.Run();
            ;
        }
    }
}
