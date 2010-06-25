namespace HtmlTags
{
    public class LinkTag : HtmlTag
    {
        public LinkTag(string text, string url, params string[] classes)
            : base("a")
        {
            Text(text);
            InitCommon(url, classes);
        }

        public LinkTag(HtmlTag child, string url, params string[] classes)
            : base("a")
        {
            Child(child);
            InitCommon(url, classes);
        }

        public string Href { get { return Attr("href"); } set { Attr("href", value); } }

        private void InitCommon(string url, string[] classes)
        {
            Href = url;
            classes.Each(x => AddClass(x));
        }
    }
}