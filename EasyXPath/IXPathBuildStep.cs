using System.Collections.Immutable;

namespace EasyXPath;

public interface IXPathBuildStep
{
    public Tuple<ImmutableList<HtmlElement>, IList<XPath>> MakeStep(ImmutableList<HtmlElement> elements,
        IList<XPath> xPaths);
}