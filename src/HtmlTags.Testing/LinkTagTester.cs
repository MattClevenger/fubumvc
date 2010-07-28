using System;
using System.Linq;
using NUnit.Framework;

namespace HtmlTags.Testing
{
    [TestFixture]
    public class LinkTagTester
    {
        private string _txt;
        private string _href;
        private LinkTag _textLink;
        private string _class1;
        private string _class2;
        private ImageTag _imgTag;
        private LinkTag _imgLink;

        [SetUp]
        public void SetUp()
        {
            _txt = "some text";
            _href = "href";
            _class1 = "class1";
            _class2 = "class2";
            _textLink = new LinkTag(_txt, _href, _class1, _class2);


            _imgTag = new ImageTag("image.jpg", "its an image");
            _imgLink = new LinkTag(_imgTag, _href, _class1, _class2);
        }

        [Test]
        public void should_have_text()
        {
            _textLink.Text().ShouldEqual(_txt);
        }

        [Test]
        public void should_have_href()
        {
            _textLink.Href.ShouldEqual(_href);
            _textLink.Attr("href").ShouldEqual(_href);
        }

        [Test]
        public void should_have_classes()
        {
            _textLink.GetClasses().Where(c => c == _class1).ShouldHaveCount(1);
            _textLink.GetClasses().Where(c => c == _class2).ShouldHaveCount(1);
        }

        [Test]
        public void should_contain_child()
        {
            _imgLink.FirstChild().ShouldEqual(_imgTag);
        }

        [Test]
        public void should_produce_fully_formed_link_html_with_text()
        {
            var link = _textLink.ToPrettyString();
//            Console.WriteLine(link);

            link.ShouldEqual(string.Format("<a href=\"{2}\" class=\"{0} {1}\">{3}</a>", 
                _class1, _class2, _href, _txt));
        }

        [Test]
        public void should_produce_fully_formed_link_html_with_child()
        {
            var link = _imgLink.ToPrettyString();
//            Console.WriteLine(link);

            link.ShouldEqual(string.Format("<a href=\"{2}\" class=\"{0} {1}\">{3}</a>", 
                _class1, _class2, _href, _imgTag.ToPrettyString()));
        }

    }
}
