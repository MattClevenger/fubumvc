using System.Linq;
using NUnit.Framework;

namespace HtmlTags.Testing
{
    [TestFixture]
    public class ListTagTester
    {
        private ListTag _listTag;

        [SetUp]
        public void SetUp()
        {
            _listTag = new ListTag();
        }

        [Test]
        public void should_have_ul_as_top_level_element()
        {
            _listTag.TagName().ShouldEqual("ul");
        }

        [Test]
        public void should_have_li_child_element_when_add_item_text()
        {
            var text = "an item";
            _listTag.AddItem(text);

            _listTag.Children.ShouldHaveCount(1);
            var item = _listTag.Children.First();
            item.TagName().ShouldEqual("li");
            item.Text().ShouldEqual(text);
        }

        [Test]
        public void should_have_li_child_element_when_add_item_child()
        {
            var child = new LinkTag("foo", "url");
            _listTag.AddItem(child);

            _listTag.Children.ShouldHaveCount(1);
            var item = _listTag.Children.First();
            item.TagName().ShouldEqual("li");
            item.Children.ShouldHaveCount(1);
            item.Children.First().ShouldEqual(child);
        }

        [Test]
        public void should_configure_child()
        {
            var text = "text";
            _listTag.AddItem(li => li.Text(text));

            _listTag.Children.ShouldHaveCount(1);
            var item = _listTag.Children.First();
            item.Text().ShouldEqual(text);
        }

    }
}
