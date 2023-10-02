using System.Collections.Immutable;

namespace EasyXPath;

public class XPathBuilder
{
    private readonly ImmutableList<HtmlElement> _elements;
    private readonly List<IXPathBuildStep> _steps = new();

    public XPathBuilder(IEnumerable<HtmlElement> elements)
    {
        _elements = elements.ToImmutableList();
    }

    public XPathBuilder AddStep(IXPathBuildStep step)
    {
        _steps.Add(step);
        return this;
    }

    public XPathBuilder RemoveLastStep()
    {
        _steps.RemoveAt(_steps.Count - 1);
        return this;
    }

    public string Build(XPath.StringifyMode mode = XPath.StringifyMode.Wherever)
    {
        Tuple<ImmutableList<HtmlElement>, IList<XPath>> stepState =
            new Tuple<ImmutableList<HtmlElement>, IList<XPath>>(_elements, new List<XPath>());
        stepState = _steps.Aggregate(stepState, (current, xPathBuildStep) => xPathBuildStep.MakeStep(current.Item1, current.Item2));
        XPath? xPath = stepState.Item2.FirstOrDefault();
        if (xPath is null) return "";
        xPath = stepState.Item2.Skip(1).Aggregate(xPath, (current, path) => current + path);
        return xPath.ToString(mode);
    }
}

public record HtmlElement(string TagName, Dictionary<string, object> Attributes);