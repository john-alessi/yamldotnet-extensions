using YamlDotNet.Serialization;
using YamlDotNetExtensions.CommentSerialization;
using YamlDotNetExtensions.ExtendedObjectModel;

namespace YamlDotNetExtensionsTests.TestObjectModel
{
    [Serializable]
    public class TestConfig
    {
        [YamlMember(Alias = "version")]
        public int? Version { get; set; }

        [YamlMember(Alias = "name")]
        public string? Name { get; set; }
    }
}
