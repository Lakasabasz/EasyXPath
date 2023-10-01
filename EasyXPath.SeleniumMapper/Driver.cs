using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace EasyXPath.SeleniumMapper;

internal class Driver
{
    private readonly IWebDriver _driver = new FirefoxDriver(new FirefoxOptions
    {
        AcceptInsecureCertificates = true
    });

    public Driver(string url)
    {
        _driver.Url = url;
    }

    public IWebElement GetElement(string xpath)
    {
        return _driver.FindElement(By.XPath(xpath));
    }

    public IEnumerable<IWebElement> GetElements(string xpath)
    {
        return _driver.FindElements(By.XPath(xpath));
    }

    public void SetupEvent(IWebElement element, Guid id)
    {
        ((IJavaScriptExecutor)_driver).ExecuteScript("""
                                                     var id = arguments[1];
                                                     arguments[0].addEventListener("click", function onchange(e) {
                                                        window.selenium_clicked.push(id);
                                                     });
                                                     """, element, id.ToString());
    }

    public void SetupEvents(IWebElement element)
    {
        ((IJavaScriptExecutor)_driver).ExecuteScript(
            """
            arguments[0].addEventListener("mousemove", (e) => {
                window.selenium_target = e.target;
            })
            window.addEventListener("keydown", () => {
                window.selenium_ready = true;
            })
            window.selenium_target = null;
            window.selenium_ready = false;
            """, element);
    }

    public void InitGlobalJsVariable(string name, string value)
    {
        ((IJavaScriptExecutor)_driver).ExecuteScript($"window.{name} = {value};");
    }

    public ReadOnlyCollection<object> WaitForClick()
    {
        while (true)
        {
            var x = ((IJavaScriptExecutor)_driver).ExecuteAsyncScript("""
                                                                      var callback = arguments[0];
                                                                      (function fn(){
                                                                          if(window.selenium_clicked.lenght !== 0){
                                                                              return callback(window.selenium_clicked);
                                                                              }
                                                                          setTimeout(fn, 60);
                                                                      })();
                                                                      """);
            if (x is ReadOnlyCollection<object> { Count: > 0 } list) return list;
        }
    }
    
    public IWebElement WaitForKeyDown()
    {
        while (true)
        {
            var x = ((IJavaScriptExecutor)_driver)
                .ExecuteAsyncScript("""
                                    var callback = arguments[0];
                                    (function fn(){
                                        if(window.selenium_ready){
                                            return callback(window.selenium_target);
                                        }
                                        setTimeout(fn, 50);
                                    })();
                                    """);
            return (IWebElement)x;
        }
    }

    public Dictionary<string, object> GetAllTags(IWebElement element)
    {
        return (Dictionary<string, object>)((IJavaScriptExecutor)_driver).ExecuteScript("""
            var items = {};
            for (index = 0; index < arguments[0].attributes.length; ++index) {
                items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value
            };
            return items;
            """, element);
    }
}