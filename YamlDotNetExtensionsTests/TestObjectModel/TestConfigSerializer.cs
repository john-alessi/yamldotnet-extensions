using YamlDotNet.Serialization;
using YamlDotNetExtensions.CommentSerialization;

namespace YamlDotNetExtensionsTests.TestObjectModel
{
    public class TestConfigSerializer
    {
        private ISerializer _serializer;
        private IDeserializer _deserializer;

        public TestConfigSerializer()
        {
            _serializer = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitEmptyCollections | DefaultValuesHandling.OmitNull)
                .WithIndentedSequences()
                .WithEmissionPhaseObjectGraphVisitor((e) => new CommentObjectGraphVisitor(e.InnerVisitor))
                .Build ();

            _deserializer = new DeserializerBuilder().Build();
        }

        public TestConfig Deserialize(string yaml)
        {
            var parser = new CommentParser(yaml);
            return _deserializer.Deserialize<TestConfig>(parser);
        }

        public string Serialize(TestConfig testConfig) => _serializer.Serialize(testConfig);
    }
}
