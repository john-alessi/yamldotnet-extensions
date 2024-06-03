using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectGraphVisitors;

namespace YamlDotNetExtensions.CommentSerialization
{
    public class CommentObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        public CommentObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor)
        : base(nextVisitor)
        {
        }

        public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
        {
            var inlineComments = (value.Value as ICommentWrapper)?.LeadingComments;
            if (inlineComments != null)
            {
                foreach (var comment in inlineComments)
                {
                    context.Emit(new Comment(comment, false));
                }
            }

            return base.EnterMapping(key, value, context);
        }
    }
}