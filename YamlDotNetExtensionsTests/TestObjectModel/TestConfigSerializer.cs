using YamlDotNet.Serialization;
using YamlDotNetExtensions.CommentSerialization;

namespace YamlDotNetExtensionsTests.TestObjectModel
{
    public class ConfigSerializer<TConfig>
    {
        private ISerializer _serializer;
        private IDeserializer _deserializer;

        public ConfigSerializer()
        {
            _serializer = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitEmptyCollections | DefaultValuesHandling.OmitNull)
                .WithIndentedSequences()
                .WithEmissionPhaseObjectGraphVisitor((e) => new CommentObjectGraphVisitor(e.InnerVisitor))
                .Build ();

            _deserializer = new DeserializerBuilder().Build();
        }

        public TConfig Deserialize(string yaml)
        {
            var parser = new CommentParser(yaml);
            return _deserializer.Deserialize<TConfig>(parser);
        }

        public string Serialize(TestConfig testConfig) => _serializer.Serialize(testConfig);
    }
}
