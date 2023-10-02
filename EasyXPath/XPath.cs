namespace EasyXPath;

public class XPath
{
    private Dictionary<string, object>? _attributes;
    public string Prefix { get; set; }

    private bool Equals(XPath other)
    {
        return _current == other._current;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((XPath)obj);
    }

    public override int GetHashCode()
    {
        return _current.GetHashCode();
    }

    private readonly string _current;

    public XPath(string xpath)
    {
        _current = xpath;
    }

    public XPath(string tagName, Dictionary<string, object> attributes)
    {
        _current = tagName;
        _attributes = attributes;
    }

    public static XPath operator +(XPath current, string xpath) => new (current._prepare() + xpath);
    public static XPath operator +(XPath current, XPath other) => new(current._prepare() + other._prepare());

    public static bool operator ==(XPath current, string xpath) => xpath.Equals(current._prepare());
    public static bool operator ==(XPath current, XPath other) => current._prepare().Equals(other._prepare());

    public static bool operator !=(XPath current, string xpath) => !(current == xpath);
    public static bool operator !=(XPath current, XPath other) => !(current == other);

    public XPath this[int index] => new($"({_current})[{index + 1}]");

    public override string ToString() => _current;

    public enum StringifyMode
    {
        None,
        Wherever,
        WhereverFromHere,
        Child,
        ChildFromHere
    }

    private string _prepare()
    {
        if (_attributes is null) return Prefix + _current;
        List<string> conditions = new();
        foreach (var (key, value) in _attributes) conditions.Add($"@{key}='{value}'");
        string combined = string.Join(" and ", conditions);
        return $"{Prefix}{_current}" + (combined.Length > 0 ? $"[{combined}]" : "");
    }

    public string ToString(StringifyMode mode)
    {
        string prepared = _prepare();
        return mode switch
        {
            StringifyMode.None => prepared,
            StringifyMode.Wherever when !(prepared.StartsWith(".") || prepared.StartsWith("/")) => "//" + prepared,
            StringifyMode.WhereverFromHere when !(prepared.StartsWith(".") || prepared.StartsWith("/")) => ".//" + prepared,
            StringifyMode.Child when !(prepared.StartsWith(".") || prepared.StartsWith("/")) => "/" + prepared,
            StringifyMode.ChildFromHere when !(prepared.StartsWith(".") || prepared.StartsWith("/")) => "./" + prepared,
            _ => prepared
        };
    }

    public XPath Prepend(string s)
    {
        Prefix = s;
        return this;
    }
}