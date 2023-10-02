using System.Collections.Immutable;

namespace EasyXPath.XPathSteps;

public class RemoteXPathAt: IXPathBuildStep
{
    private readonly int _indexToRemove;
    public RemoteXPathAt(int i)
    {
        _indexToRemove = i;
    }
    
    public Tuple<ImmutableList<HtmlElement>, IList<XPath>> MakeStep(ImmutableList<HtmlElement> elements, IList<XPath> xPaths)
    {
        if (_indexToRemove > xPaths.Count - 1)
            return new Tuple<ImmutableList<HtmlElement>, IList<XPath>>(elements, xPaths);
        List<XPath> list = xPaths.Take(_indexToRemove).ToList();
        if (_indexToRemove == xPaths.Count - 1)
            return new Tuple<ImmutableList<HtmlElement>, IList<XPath>>(elements, list);
        list.Add(xPaths.ElementAt(_indexToRemove + 1).Prepend("//"));
        list.AddRange(xPaths.Skip(_indexToRemove + 2));
        return new Tuple<ImmutableList<HtmlElement>, IList<XPath>>(elements, list);
    }
}