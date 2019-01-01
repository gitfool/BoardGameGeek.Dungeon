using FakeItEasy;
using FluentAssertions;
using Xunit;
// ReSharper disable ConvertToConstant.Local

namespace BoardGameGeek.Dungeon
{
    public class ProcessorTests
    {
        public class IsHighlightMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("Highlight.")]
            [InlineData("Highlight 2014")]
            [InlineData("Highlight: 2014.")]
            [InlineData("Highlights 2014 2018")]
            [InlineData("Highlights: 2014, 2018.")]
            [InlineData(@"Highlight 2014\nHighlight 2018")]
            [InlineData(@"Highlight: 2014.\nHighlight: 2018.")]
            public void Should_ReturnFalse_WhenHighlightYear_IsNotInComments(string comments)
            {
                var processor = new Processor(A.Fake<IBggService>());

                var isHighlight = processor.IsHighlight(comments, 2016);

                isHighlight.Should().BeFalse();
            }

            [Theory]
            [InlineData("Highlight 2016")]
            [InlineData("Highlight: 2016.")]
            [InlineData("Highlights 2014 2016 2018")]
            [InlineData("Highlights: 2014, 2016, 2018.")]
            [InlineData(@"Highlight 2014\nHighlight 2016")]
            [InlineData(@"Highlight: 2014.\nHighlight: 2016.")]
            [InlineData(@"Highlights 2010 2012\nHighlights 2014 2016 2018")]
            [InlineData(@"Highlights: 2010, 2012.\nHighlights: 2014, 2016, 2018.")]
            public void Should_ReturnTrue_WhenHighlightYear_IsInComments(string comments)
            {
                var processor = new Processor(A.Fake<IBggService>());

                var isHighlight = processor.IsHighlight(comments, 2016);

                isHighlight.Should().BeTrue();
            }
        }

        public class IsSessionMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("Session.")]
            [InlineData("Session ?/?")]
            [InlineData("Session: ?/?.")]
            public void Should_ReturnFalse_WhenSessionStats_AreNotInComments(string comments)
            {
                var session = Processor.IsSession(comments);

                session.Should().BeFalse();
            }

            [Theory]
            [InlineData("Session 1/?")]
            [InlineData("Session: 1/?.")]
            [InlineData("Session 1/2")]
            [InlineData("Session: 1/2.")]
            [InlineData("Session 2/2")]
            [InlineData("Session: 2/2.")]
            public void Should_ReturnTrue_WhenSessionStats_AreInComments(string comments)
            {
                var session = Processor.IsSession(comments);

                session.Should().BeTrue();
            }
        }
    }
}
