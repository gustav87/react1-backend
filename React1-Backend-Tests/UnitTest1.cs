namespace React1_Backend_Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void TestStartsWithUpper()
    {
        // Tests that we expect to return true.
        string[] words = ["Alphabet", "Aebra", "ABC"];
        foreach (string word in words)
        {
            bool result = word.StartsWith('A');
            Assert.IsTrue(result, string.Format("Expected for '{0}': true; Actual: {1}", word, result));
        }
    }
}
