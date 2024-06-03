using FluentAssertions;
using YamlDotNetExtensionsTests.TestObjectModel;

namespace YamlDotNetExtensionsTests
{
    [TestClass]
    public class CommentSerializationTests
    {
        private const string SampleYaml = """
# comment before version
version: 2

# comment before name
name: 'John' # inline comment on name
""";

        [DataTestMethod]
        [DataRow(SampleYaml)]
        public void TestCommentSerialization(string yaml)
        {
            var serializer = new TestConfigSerializer();
            var config = serializer.Deserialize(yaml);
            var reseralizedYaml = serializer.Serialize(config);
            reseralizedYaml.Should().Contain("inline comment on name");
        }
    }
}