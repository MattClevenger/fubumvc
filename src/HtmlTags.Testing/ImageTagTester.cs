using System;
using NUnit.Framework;

namespace HtmlTags.Testing
{
    [TestFixture]
    public class ImageTagTester
    {
        private ImageTag imageTag;
        private string source;
        private string altText;

        [SetUp]
        public void SetUp()
        {
            source = "foo.jpg";
            altText = "foo";
            imageTag = new ImageTag(source, altText);
        }

        [Test]
        public void should_have_img_as_top_level_element()
        {
            imageTag.TagName().ShouldEqual("img");
        }

        [Test]
        public void should_have_src_attribute()
        {
            imageTag.Attr("src").ShouldEqual(source);
        }

        [Test]
        public void should_have_alt_attribute()
        {
            imageTag.Attr("alt").ShouldEqual(altText);
        }

        [Test]
        public void should_produce_fully_formed_img_html()
        {
            var img = imageTag.ToPrettyString();
//            Console.WriteLine(img);

            img.ShouldEqual(string.Format("<img alt=\"{0}\" src=\"{1}\" />", altText, source));
        }

    }
}
