using System.Collections.Immutable;
using System.Collections.ObjectModel;
using EasyXPath.SeleniumMapper;
using OpenQA.Selenium;


Driver driver = new Driver("https://alternative-to-tests.eu.buddy.cloud/");

Console.WriteLine("Prepare view");
Console.ReadKey();

var root = new SeleniumPageObject(driver, "//body");
driver.SetupEvents(root.Element);
Console.WriteLine("Press key in browser");

var obj = new SeleniumPageObject(driver, driver.WaitForKeyDown());
List<SeleniumPageObject> hierarchy = new(){obj};
while (hierarchy.First() != root) hierarchy.Insert(0, hierarchy.First().Parent);
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
foreach (var (key, value) in tag.GetFullDescription())
{
    Console.WriteLine($"\t{key} => {value}");
}
