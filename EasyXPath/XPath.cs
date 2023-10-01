﻿namespace EasyXPath;

public class XPath
{
    private bool Equals(XPath other)
    {
        return _current == other._current;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
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

    public static XPath operator +(XPath current, string xpath) => new (current._current + xpath);
    public static XPath operator +(XPath current, XPath other) => new(current._current + other._current);

    public static bool operator ==(XPath current, string xpath) => xpath.Equals(current._current);
    public static bool operator ==(XPath current, XPath other) => current._current.Equals(other._current);

    public static bool operator !=(XPath current, string xpath) => !(current == xpath);
    public static bool operator !=(XPath current, XPath other) => !(current == other);

    public XPath this[int index] => new($"({_current})[{index + 1}]");

    public override string ToString()
    {
        return _current;
    }
}