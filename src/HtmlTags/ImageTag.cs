namespace HtmlTags
{
    public class ImageTag : HtmlTag
    {
        public ImageTag(string source, string altText) : base("img")
        {
            Alt = altText;
            Src = source;
        }

        public string Src { get{ return Attr("src");} set { Attr("src", value); }}
        public string Alt { get { return Attr("alt"); } set { Attr("alt", value); } }
    }
}