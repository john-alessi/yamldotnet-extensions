namespace YamlDotNetExtensions.CommentSerialization
{
    public interface ICommentWrapper
    {
        public IEnumerable<string> LeadingComments { get; }
    }
}
