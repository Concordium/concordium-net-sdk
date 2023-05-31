namespace Concordium.Sdk.Types.New;

public record ProtocolUpdate(
    string Message,
    string SpecificationURL,
    string SpecificationHash,
    BinaryData SpecificationAuxiliaryData);