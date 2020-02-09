using AngleSharp.Html.Parser;
using AngleSharp.Css;
using AngleSharp.Css.Dom;

using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AngleSharp.Performance.Css.Cssom
{
    class NonCacheTestee : ITestee
    {
        public string Name => "NonCache";

        public Type Library => typeof(BrowsingContext);

        public void Run(string argument)
        {
            List<object> ls = new List<object>();
            var context = BrowsingContext.New(Configuration.Default.WithCss());
            var parser = context.GetService<IHtmlParser>();
            var document = parser.ParseDocument(argument);
            foreach(var one in document.DocumentElement.DescendentsAndSelf().OfType<IElement>())
            {
                ls.Add(one.ComputeCurrentStyle());
            }
        }
    }
}
