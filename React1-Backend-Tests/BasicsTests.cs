using React1_Backend.Contracts;
namespace React1_Backend_Tests;

[TestFixture]
public class BasicTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestPass()
    {
        Assert.Pass();
    }

    [Test]
    public void TestStartsWithA()
    {
        string[] words = ["Alphabet", "Aebra", "ABCD"];
        foreach (string word in words)
        {
            bool result = word.StartsWith('A');
            Assert.IsTrue(result, string.Format("Expected for '{0}': true; Actual: {1}", word, result));
        }
    }

    [Test]
    public void TestCloudFile()
    {
        CloudFile cloudFile = new();
        Assert.IsNotNull(cloudFile);
        Assert.That(cloudFile, Is.Not.Null);
    }
}
