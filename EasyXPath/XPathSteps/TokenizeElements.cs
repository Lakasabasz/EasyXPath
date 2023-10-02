using System.Collections.Immutable;

namespace EasyXPath.XPathSteps;

public class TokenizeElements: IXPathBuildStep
{
    public Tuple<ImmutableList<HtmlElement>, IList<XPath>> MakeStep(ImmutableList<HtmlElement> elements, IList<XPath> xPaths)
    {
        if (elements.Count == 0) return new Tuple<ImmutableList<HtmlElement>, IList<XPath>>(elements, xPaths);
        List<XPath> list = new List<XPath>{new (elements.First().TagName, elements.First().Attributes)};
        foreach (var (tagName, attributes) in elements.Skip(1))
        {
            list.Add(new XPath(tagName, attributes).Prepend("/"));
        }

        return new Tuple<ImmutableList<HtmlElement>, IList<XPath>>(elements, list);
    }
}