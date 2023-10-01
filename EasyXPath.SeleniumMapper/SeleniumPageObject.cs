using OpenQA.Selenium;

namespace EasyXPath.SeleniumMapper;

class SeleniumPageObject
{
    private readonly string? _baseXPath;
    private readonly Driver _driver;
    private IWebElement? _webElement;

    public SeleniumPageObject(Driver driver, string baseXPath)
    {
        _baseXPath = baseXPath;
        _driver = driver;
        _webElement = null;
    }

    public SeleniumPageObject(Driver driver, IWebElement webElement)
    {
        _driver = driver;
        _webElement = webElement;
        _baseXPath = null;
    }

    public IWebElement Element => _webElement ?? _driver.GetElement(_baseXPath ??
                                                                    throw new ArgumentNullException(nameof(_baseXPath),
                                                                        "_baseXPath has to be not null if _webElement is null"));

    public string? TagName
    {
        get
        {
            try
            {
                return Element.TagName;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }
    }

    public List<WebElementHierarchical> SetupEvents(SeleniumPageObject? parent = null, string? baseXPath = null)
    {
        parent ??= this;
        baseXPath ??= _baseXPath;
        var children = _driver.GetElements($"{baseXPath}/*").ToList();
        var ret = new List<WebElementHierarchical>();
        for(int i = 0; i < children.Count; i++)
        {
            var child = new SeleniumPageObject(_driver, $"({baseXPath}/*)[{i + 1}]");
            var guid = Guid.NewGuid();
            ret.Add(new WebElementHierarchical(guid, child, parent));
            _driver.SetupEvent(children[i], guid);
            var t = SetupEvents(child, $"({baseXPath}/*)[{i + 1}]");
            ret.AddRange(t);
        }

        return ret;
    }

    public Dictionary<string, object> GetFullDescription()
    {
        return _driver.GetAllTags(Element);
    }

    public SeleniumPageObject Parent => _baseXPath is not null
        ? new SeleniumPageObject(_driver, $"{_baseXPath}/..") 
        : new SeleniumPageObject(_driver, _webElement!.FindElement(By.XPath("./..")));
    
    public static bool operator==(SeleniumPageObject a, SeleniumPageObject b)
    {
        return a.Element.Equals(b.Element);
    }

    public static bool operator !=(SeleniumPageObject a, SeleniumPageObject b)
    {
        return !(a == b);
    }
}

record WebElementHierarchical(Guid Id, SeleniumPageObject Element, SeleniumPageObject? Parent);