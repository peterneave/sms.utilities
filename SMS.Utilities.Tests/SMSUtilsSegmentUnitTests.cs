using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using static SMS.Utilities.SegmentCalculator;

namespace SMS.Utilities.Tests
{
    [TestClass]
    public class SMSUtilsSegmentUnitTests
    {
        [DataTestMethod]
        [DataRow("hello world", 1, Charset.GSM)]
        [DataRow("[hello world]", 1, Charset.GSM)]
        [DataRow("“hello world”", 1, Charset.Unicode)]
        [DataRow("This is the \"song\" that doesn't end. Yes, it goes on and on my friend. Some people started singing it, not knowing what it was, And now they are still singing it just because...", 2, Charset.GSM)]
        [DataRow("This is the “song” that doesn't end. Yes, it goes on and on my friend. Some people started singing it, not knowing what it was, And now they are still singing it just because... ", 3, Charset.Unicode)]
        [DataRow("x", 1, Charset.GSM)]
        [DataRow("😊", 1, Charset.Unicode)]
        public void GivenMessage_GetSegments(string message, int expectedSegments, Charset expectedCharset)
        {
            if (message == "hello world") Debugger.Break();

            var segmentCalc = new SegmentCalculator();

            (var actualSegments, var actualCharset) = segmentCalc.GetSegmentsCount(message);
            Assert.AreEqual(expectedSegments, actualSegments, $"Expected {expectedSegments} but got {actualSegments}.");

            Assert.AreEqual(expectedCharset, actualCharset, $"Expected {expectedCharset} but got character set {actualCharset}");
        }
    }
}