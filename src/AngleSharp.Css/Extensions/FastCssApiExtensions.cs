using System;
using System.Linq;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Svg.Dom;
using AngleSharp.Css;
using System.Collections.Generic;

namespace AngleSharp.Css
{
    /// <summary>
    /// Non-general CSS API extension methods.
    /// </summary>
    public static class FastCssApiExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static CssInfoCache GenStyleCache(this IDocument document)
        {
            var view = document?.DefaultView;
            if (view != null)
            {
                return new CssInfoCache(view);
            }
            return null;
        }
    }
    /// <summary>
    /// Cache for Css info
    /// </summary>
    public class CssInfoCache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public CssInfoCache(IWindow window)
        {
            Window = window;
            cssStyleRules = new Lazy<IStyleCollection>(() =>
              {
                  return Window.GetStyleCollection();
              }, false);
        }

        readonly Dictionary<(IElement, ICssStyleDeclaration), ICssStyleDeclaration> computeCascadedStyleCache = new Dictionary<(IElement, ICssStyleDeclaration), ICssStyleDeclaration>();
        ICssStyleDeclaration ComputeCascadedStyle(IStyleCollection styleCollection, IElement element, ICssStyleDeclaration parent = null)
        {
            var key = (element, parent);
            if (computeCascadedStyleCache.TryGetValue(key,out var result))
            {
                return result;
            }
            else
            {
                result = styleCollection.ComputeCascadedStyle(element, parent);
                computeCascadedStyleCache[key] = result;
                return result;
            }
        }

        ICssStyleDeclaration ComputeDeclarations(IStyleCollection rules, IElement element, String pseudoSelector = null)
        {
            var computedStyle = new CssStyleDeclaration(element.Owner?.Context);
            var nodes = element.GetAncestors().OfType<IElement>();

            if (!String.IsNullOrEmpty(pseudoSelector))
            {
                var pseudoElement = element?.Pseudo(pseudoSelector.TrimStart(':'));

                if (pseudoElement != null)
                {
                    element = pseudoElement;
                }
            }

            computedStyle.SetDeclarations(ComputeCascadedStyle(rules, element));

            foreach (var node in nodes)
            {
                computedStyle.UpdateDeclarations(ComputeCascadedStyle(rules, node));
            }

            return computedStyle;
        }

        readonly Lazy<IStyleCollection> cssStyleRules;
        /// <summary>
        /// Gets the computed style of the given element from the current view.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public ICssStyleDeclaration ComputeCurrentStyle(IElement element)
        {
            return ComputeDeclarations(cssStyleRules.Value, element);
        }

        IWindow Window { get; }
    }
}
