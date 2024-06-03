using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace YamlDotNetExtensions.CommentSerialization
{
    public class CommentWrapper<T> : IYamlConvertible, ICommentWrapper
    {
        public string? InlineComment { get; set; }

        public IEnumerable<string> LeadingComments { get; set; } = new List<string>();

        public T? Value { get; set; }

        public bool IsScalar { get; set; }

        public CommentWrapper(T value)
            : base()
        {
            Value = value;
        }

        public CommentWrapper()
            : base()
        {
        }

        public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
        {
            var commentParser = parser as CommentParser;
            if (commentParser is not null)
            {
                LeadingComments = commentParser.ConsumeComments();
            }

            if (parser.Accept<Scalar>(out var _))
            {
                IsScalar = true;
                var scalar = nestedObjectDeserializer(typeof(T));
                if (scalar != null)
                {
                    Value = (T)scalar;
                }
            }

            if (parser.TryConsume<Comment>(out var inlineComment))
            {
                InlineComment = inlineComment.Value;
            }

            if (parser.Accept<SequenceStart>(out var _))
            {
                var sequence = nestedObjectDeserializer(typeof(T));
                if (sequence != null)
                {
                    Value = (T)sequence;
                    if (parser.TryConsume<Comment>(out var inlineCommentOnFlowSequence))
                    {
                        LeadingComments = LeadingComments.Append(inlineCommentOnFlowSequence.Value);
                    }
                }
            }
            else if (parser.Accept<MappingStart>(out var _))
            {
                var sequence = nestedObjectDeserializer(typeof(T));
                if (sequence != null)
                {
                    Value = (T)sequence;
                }
            }
        }

        public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            if (IsScalar)
            {
                nestedObjectSerializer(Value, typeof(T));

                if (!string.IsNullOrEmpty(InlineComment))
                {
                    emitter.Emit(new Comment(InlineComment, true));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(InlineComment))
                {
                    emitter.Emit(new Comment(InlineComment, true));
                }

                nestedObjectSerializer(Value, typeof(T));
            }
        }
    }
}
