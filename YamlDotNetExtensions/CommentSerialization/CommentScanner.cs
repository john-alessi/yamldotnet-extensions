using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace YamlDotNetExtensions.CommentSerialization
{
    public class CommentScanner : IScanner
    {
        private Scanner InternalScanner { get; set; }

        private Queue<string> FoundComments { get; set; } = new Queue<string>();

        public Mark CurrentPosition => InternalScanner.CurrentPosition;

        public Token? Current => InternalScanner.Current;

        public Queue<Queue<string>> CommentBlocks { get; set; } = new Queue<Queue<string>>();

        public CommentScanner(string yaml)
        {
            InternalScanner = new Scanner(new StringReader(yaml), skipComments: false);
        }

        public IEnumerable<string> ConsumeComments()
        {
            if (CommentBlocks.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                return CommentBlocks.Dequeue();
            }
        }

        public bool MoveNextWithoutConsuming()
        {
            bool tokensRemaining = InternalScanner.MoveNextWithoutConsuming();

            while (Current is Comment && !((Comment)Current).IsInline && tokensRemaining)
            {
                FoundComments.Enqueue(((Comment)Current).Value);
                tokensRemaining = MoveNext();
            }

            if (Current is Scalar)
            {
                PushCommentsToQueue();
            }

            return tokensRemaining;
        }

        public bool MoveNext()
        {
            return InternalScanner.MoveNext();
        }

        public void ConsumeCurrent()
        {
            InternalScanner.ConsumeCurrent();
        }

        private void PushCommentsToQueue()
        {
            if (FoundComments.Any())
            {
                CommentBlocks.Enqueue(FoundComments);
                FoundComments = new Queue<string>();
            }
        }
    }
}
