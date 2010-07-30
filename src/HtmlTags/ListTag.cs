using System;

namespace HtmlTags
{
    public class ListTag : HtmlTag
    {

        public ListTag() : base("ul")
        {
            
        }

        public ListItemTag AddItem(string itemText)
        {
            var item = Child<ListItemTag>();
            item.Text(itemText);
            return item;
        }

        public ListItemTag AddItem(HtmlTag itemTag)
        {
            var item = Child<ListItemTag>();
            item.Child(itemTag);
            return item;
        }

        public ListItemTag AddItem(Action<ListItemTag> configure)
        {
            var item = Child<ListItemTag>();
            configure(item);
            return item;
        }

    }

    public class ListItemTag : HtmlTag
    {
        public ListItemTag() : base("li"){}
    }
}