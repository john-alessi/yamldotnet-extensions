using YamlDotNet.Serialization;
using YamlDotNetExtensions.CommentSerialization;

namespace YamlDotNetExtensionsTests.TestObjectModel
{
    [Serializable]
    public class TestConfig
    {
        [YamlMember(Alias = "version")]
        public CommentWrapper<int>? Version { get; set; }

        [YamlMember(Alias = "name")]
        public CommentWrapper<string>? Name { get; set; }
    }
}
