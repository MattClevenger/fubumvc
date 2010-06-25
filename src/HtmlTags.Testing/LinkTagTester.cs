using System;
using System.Linq;
using NUnit.Framework;

namespace HtmlTags.Testing
{
    [TestFixture]
    public class LinkTagTester
    {
        private string txt;
        private string href;
        private LinkTag textLink;
        private string class1;
        private string class2;
        private ImageTag imgTag;
        private LinkTag imgLink;

        [SetUp]
        public void SetUp()
        {
            txt = "some text";
            href = "href";
            class1 = "class1";
            class2 = "class2";
            textLink = new LinkTag(txt, href, class1, class2);


            imgTag = new ImageTag("image.jpg", "its an image");
            imgLink = new LinkTag(imgTag, href, class1, class2);
        }

        [Test]
        public void should_have_text()
        {
            textLink.Text().ShouldEqual(txt);
        }

        [Test]
        public void should_have_href()
        {
            textLink.Href.ShouldEqual(href);
            textLink.Attr("href").ShouldEqual(href);
        }

        [Test]
        public void should_have_classes()
        {
            textLink.GetClasses().Where(c => c == class1).ShouldHaveCount(1);
            textLink.GetClasses().Where(c => c == class2).ShouldHaveCount(1);
        }

        [Test]
        public void should_contain_child()
        {
            imgLink.FirstChild().ShouldEqual(imgTag);
        }

        [Test]
        public void should_produce_fully_formed_link_html_with_text()
        {
            var link = textLink.ToPrettyString();
            Console.WriteLine(link);

            link.ShouldEqual(string.Format("<a href=\"{2}\" class=\"{0} {1}\">{3}</a>", 
                class1, class2, href, txt));
        }

        [Test]
        public void should_produce_fully_formed_link_html_with_child()
        {
            var link = imgLink.ToPrettyString();
            Console.WriteLine(link);

            link.ShouldEqual(string.Format("<a href=\"{2}\" class=\"{0} {1}\">{3}</a>", 
                class1, class2, href, imgTag.ToPrettyString()));
        }

    }
}
