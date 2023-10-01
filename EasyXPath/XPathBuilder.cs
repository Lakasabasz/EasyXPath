using System.Collections.Immutable;

namespace EasyXPath;

class XPathBuilder
{
    private readonly ImmutableList<HtmlElement> _elements;

    public XPathBuilder(IEnumerable<HtmlElement> elements)
    {
        _elements = elements.ToImmutableList();
    }

    public string Build()
    {
        return "";
    }
}

public record HtmlElement(string TagName, Dictionary<string, object> Attributes);