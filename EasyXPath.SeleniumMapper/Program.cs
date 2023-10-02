using EasyXPath;
using EasyXPath.SeleniumMapper;
using EasyXPath.XPathSteps;


Driver driver = new Driver("https://alternative-to-tests.eu.buddy.cloud/");

Console.WriteLine("Prepare view");
Console.ReadKey();

var root = new SeleniumPageObject(driver, "//body");
driver.SetupEvents(root.Element);

while (true)
{
    LoopIteration(driver, root);
}

List<SeleniumPageObject> BuildHierarchy(SeleniumPageObject root, SeleniumPageObject leaf)
{
    List<SeleniumPageObject> hierarchy = new(){leaf};
    while (hierarchy.First() != root) hierarchy.Insert(0, hierarchy.First().Parent);
    return hierarchy;
}

void LoopIteration(Driver driver, SeleniumPageObject root)
{
    Console.WriteLine("Press key in browser");

    var obj = new SeleniumPageObject(driver, driver.WaitForKeyDown());
    var hierarchy = BuildHierarchy(root, obj);
    for (int index = 0; index < hierarchy.Count; index++)
    {
        Console.WriteLine($"{index}.\t{hierarchy[index].TagName}");
        foreach(var (key, arguments) in hierarchy[index].GetFullDescription())
            Console.WriteLine($"\t\t{key}:\t{arguments}");
    }

    SeleniumPageObject tag;
    while (true)
    {
        Console.Write("@> ");
        string strOption = Console.ReadLine()!;
        if (!int.TryParse(strOption, out int option))
        {
            Console.WriteLine("Wybierz poprawną opcję");
            continue;
        }
        tag = hierarchy[option];
        break;
    }

    Console.WriteLine(tag.TagName);
    XPathBuilder builder = new(BuildHierarchy(root, tag).Select(x => new HtmlElement(x.TagName!, x.GetFullDescription())));
    builder.AddStep(new TokenizeElements());
    string xpath = builder.Build();
    if (driver.GetElements(xpath).Count() != 1)
    {
        Console.WriteLine("Nie można wygenerować xpath-a");
        return;
    }

    builder.AddStep(new RemoveAttributes());
    xpath = builder.Build();
    if (driver.GetElements(xpath).Count() != 1)
    {
        builder.RemoveLastStep();
    }

    string last = "";
    int i = 0;
    while (last != xpath)
    {
        last = xpath;
        builder.AddStep(new RemoteXPathAt(i));
        xpath = builder.Build();
        if (xpath == "" || driver.GetElements(xpath).Count() != 1 || !Equals(driver.GetElement(xpath), tag.Element))
        {
            builder.RemoveLastStep();
            break;
        }
    }
    Console.WriteLine(builder.Build());
}