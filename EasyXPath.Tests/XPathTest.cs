namespace EasyXPath.Tests;

public class XPathTests
{
    [Test]
    public void Comparison()
    {
        string value = "//main";
        XPath xPath = new(value);
        XPath second = new(value);
        Assert.Multiple(() =>
        {
            Assert.That(xPath.ToString(), Is.EqualTo(value), "ToString method provides wrong result");
            Assert.That(xPath == value, Is.True, "XPath and string comparison is invalid");
            Assert.That(xPath != value, Is.False, "XPath and string inequality comparison is invalid");
            Assert.That(xPath == second, Is.True, "XPath and XPath comparison is invalid");
            Assert.That(xPath != second, Is.False, "XPath and XPath inequality comparison is invalid");
        });
    }

    [Test]
    public void NElement()
    {
        string value = "//main/ul/li";
        XPath xPath = new(value);
        XPath nElement = xPath[1];
        Assert.That(nElement.ToString(), Is.EqualTo($"({value})[2]"), "Indexer returns invalid xPath");
    }

    [Test]
    public void ConcatenationWithString()
    {
        string baseXPath = "//main";
        string secondPart = "//h1";
        XPath xPath = new(baseXPath);
        XPath result = xPath + secondPart;
        Assert.That(result.ToString(), Is.EqualTo(baseXPath + secondPart));
    }
    
    [Test]
    public void ConcatenationWithXPath()
    {
        string baseXPath = "//main";
        string secondPart = "//h1";
        XPath xPath = new(baseXPath);
        XPath second = new(secondPart);
        XPath result = xPath + second;
        Assert.That(result.ToString(), Is.EqualTo(baseXPath + secondPart));
    }
}