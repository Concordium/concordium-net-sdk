namespace Concordium.Sdk.Tests.IntegrationTests.Utils.LocalNode;

[CollectionDefinition(LocalNodes)]
public sealed class LocalNodeCollectionFixture : ICollectionFixture<LocalNodeFixture>
{
    internal const string LocalNodes = "Local Node collection";
}
