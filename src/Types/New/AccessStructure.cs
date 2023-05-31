namespace Concordium.Sdk.Types.New;

public record AccessStructure(
    ushort[] AuthorizedKeys,
    ushort Threshold);
