using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class LandingPageTest : PageTest
{
    [Test]
    public async Task PageDisplayedCorrectly()
    {
        //Navigate to the page
        await Page.GotoAsync("https://www.verisksequel.com/");

        // Accept cookies
        var button = Page.GetByRole(AriaRole.Button).And(Page.GetByText("Allow all"));
        await button.ClickAsync();

        //Logo check
        await Expect(Page.GetByLabel("home")).ToBeVisibleAsync();

        //Array of elements that needs to be checked
        var menuItems = new[]
        {
            "Solutions",    
            "Products",
            "News",
            "Company",
            "Careers",
            "Contact"
        };

        // Check each menu item is displayed
        foreach (var itemText in menuItems)
        {
            var element = Page.GetByRole(AriaRole.Banner).GetByRole(AriaRole.Link, new() { Name = $"{itemText}" });

            if (element == null)
            {
                Console.WriteLine($"Element with text: {itemText} NOT found.");
            }
            else
            {
                await Expect(element).ToBeVisibleAsync();
                Console.WriteLine($"{itemText} is displayed.");
            }
        }
    }
}