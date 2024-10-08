using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class ContactPageTest : PageTest
{
    [SetUp]
    public async Task TestSetup()
    {
        //Navigate to the page
        await Page.GotoAsync("https://www.verisksequel.com/");

        // Accept cookies
        await Page.GetByRole(AriaRole.Button).And(Page.GetByText("Allow all")).ClickAsync();

        //Go to Contact
        await Page.GetByRole(AriaRole.Banner).GetByRole(AriaRole.Link, new() { Name = "Contact" }).ClickAsync();

        // Make sure url is correct
        await Expect(Page).ToHaveURLAsync(new Regex("/contact/"));
    }

    [Test]
    public async Task AllMandatoryFieldsHaveAsteriks()
    {
        //Define mandatory fields
        var mandatoryFields = new[]
        {
            "Full Name",
            "Work email",
            "Message",
            "Enquiry Type"
        };

        //Check each menu item is displayed and has *
        foreach (var field in mandatoryFields)
        {
            var label = Page.GetByRole(AriaRole.Group).GetByText(field);
            var labelText = await label.InnerTextAsync();

            if (label == null)
            {
                Console.WriteLine($"Element with text: {field} not found.");
            }
            else
            {
                await Expect(label).ToBeVisibleAsync();
                Console.WriteLine($"{field} is displayed.");
                Console.WriteLine($"Full label text is : {labelText}.");

                // Assert that the label text contains '*'
                Assert.IsTrue(labelText.Contains("*"), $"Field '{labelText}' does not contain an asterisk (*).");
            }
        }
    }

    [Test]
    public async Task CheckEnquiryTypeDropDown ()
    {
        //Find the "Enquiry type" dropdown
        var dropdown =  Page.GetByLabel("Enquiry Type *");
        Assert.IsNotNull(dropdown, "Enquiry type dropdown is not found.");

        //Get option
        var optionLocator = dropdown.Locator("option");
        var actualOptions = await optionLocator.AllInnerTextsAsync();

        List<string> expectedOptions = new List<string>
            {
                "Product suite",
                "Digital Solutions",
                "Blueprint Two",
                "Partnership",
                "Recruitment",
                "Events/Marketing",
                "Other"
            };

        //NOTE: Test fails because there is empty option at the first place in dropdown, in the task that empty option is not mentioned
        CollectionAssert.AreEquivalent(expectedOptions, actualOptions, "The Enquiry type dropdown values do not match the expected values.");
    }

    [Test]
    public async Task CheckErrorMessagesValidation()
    {
        //Submit empty form
        await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

        var fullNameValidationMessage = Page.GetByText("Please provide a value for Full Name");
        await Expect(fullNameValidationMessage).ToBeVisibleAsync();

        var workEmailValidationMessage = Page.GetByText("Please provide a value for Work email");
        await Expect(workEmailValidationMessage).ToBeVisibleAsync();

        var messageValidationMessage = Page.GetByText("Please provide a value for Message");
        await Expect(messageValidationMessage).ToBeVisibleAsync();

        var enquiryTypeValidationMessage = Page.GetByText("Please provide a value for Enquiry Type");
        await Expect(enquiryTypeValidationMessage).ToBeVisibleAsync();
    }

    [Test]
    public async Task CheckEmailValidationErrorMessage()
    {
        //Navigate to the page
        await Page.GetByLabel("Work email *").FillAsync("invalidemail.com");
        var validationMessage = Page.GetByText("Please provide a valid email");

        //Assert that validation message is visible
        await Expect(validationMessage).ToBeVisibleAsync();

        //Check that the text content of the validation message matches expected text
        await Expect(validationMessage).ToHaveTextAsync("Please provide a valid email address");
    }
}