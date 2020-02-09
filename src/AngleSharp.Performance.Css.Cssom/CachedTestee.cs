using AngleSharp.Css;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngleSharp.Performance.Css.Cssom
{
    class CachedTestee:ITestee
    {
        public string Name => "Cached";

        public Type Library => typeof(BrowsingContext);

        public void Run(string argument)
        {
            List<object> ls = new List<object>();
            var context = BrowsingContext.New(Configuration.Default.WithCss());
            var parser = context.GetService<IHtmlParser>();
            var document = parser.ParseDocument(argument);
            var cache = document.GenStyleCache();
            foreach (var one in document.DocumentElement.DescendentsAndSelf().OfType<IElement>())
            {
                ls.Add(cache.ComputeCurrentStyle(one));
            }
        }
    }
}
