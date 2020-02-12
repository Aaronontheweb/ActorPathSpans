using System;
using System.Linq;
using Akka.Actor;
using FluentAssertions;
using Xunit;

namespace ActorPathSpans.Tests
{
    public class StringWithSpansSpecs
    {
        [Theory]
        [InlineData("aaron loves pizza", ' ', new[]{ "aaron", "loves", "pizza" })]
        [InlineData("aaron/loves/pizza", ' ', new[] { "aaron/loves/pizza" })]
        [InlineData("aaronlovespizza ", ' ', new[] { "aaronlovespizza" })]
        [InlineData(" aaronlovespizza", ' ', new[] { "aaronlovespizza" })]
        [InlineData(" aaron                   loves             pizza", ' ', new[] { "aaron", "loves", "pizza" })]
        public void ShouldSplitCorrectly(string inputStr, char separator, string[] expected)
        {
            var results = inputStr.SplitWithSpan(separator).ToList();

            results.Select(x => x.ToString()).Should().BeEquivalentTo(expected);
        }
    }
}
