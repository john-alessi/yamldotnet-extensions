using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNetExtensions.CommentSerialization
{
    public class CommentParser : IParser
    {
        private Parser InternalParser { get; set; }

        private CommentScanner Scanner { get; set; }

        public IEnumerable<string> ConsumeComments() => Scanner.ConsumeComments();

        public ParsingEvent? Current => InternalParser.Current;

        public CommentParser(string yaml)
        {
            Scanner = new CommentScanner(yaml);
            InternalParser = new Parser(Scanner);
        }

        public bool MoveNext() => InternalParser.MoveNext();
    }
}
