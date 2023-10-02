using System.Collections.Immutable;

namespace EasyXPath.XPathSteps;

public class RemoveAttributes: IXPathBuildStep
{
    public Tuple<ImmutableList<HtmlElement>, IList<XPath>> MakeStep(ImmutableList<HtmlElement> elements, IList<XPath> xPaths)
    {
        List<XPath> list = xPaths.Select(xPath => new XPath(xPath.ToString()).Prepend(xPath.Prefix)).ToList();
        return new Tuple<ImmutableList<HtmlElement>, IList<XPath>>(elements, list);
    }
}