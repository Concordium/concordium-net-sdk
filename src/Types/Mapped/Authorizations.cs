namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Access structures for each of the different possible chain updates, together
/// with the context giving all the possible keys.
/// </summary>
/// <param name="Keys">The list of all keys that are currently authorized to perform updates.</param>
/// <param name="Emergency">Access structure for emergency updates.</param>
/// <param name="Protocol">Access structure for protocol updates.</param>
/// <param name="Consensus">Access structure for updating the consensus (former election difficulty.)</param>
/// <param name="EuroPerEnergy">Access structure for updating the euro to energy exchange rate.</param>
/// <param name="MicroCcdPerEuro">Access structure for updating the microccd per euro exchange rate.</param>
/// <param name="FoundationAccount">Access structure for updating the foundation account address.</param>
/// <param name="MintDistribution">Access structure for updating the mint distribution parameters.</param>
/// <param name="TransactionFeeDistribution">Access structure for updating the transaction fee distribution.</param>
/// <param name="GasRewards">Access structure for updating the gas reward distribution parameters.</param>
/// <param name="Pool">
/// Access structure for updating the pool parameters. For V0 this is only
/// the baker stake threshold, for V1 there are more.
/// </param>
/// <param name="AddAnonymityRevoker">Access structure for adding new anonymity revokers.</param>
/// <param name="AddIdentityProvider">Access structure for adding new identity providers.</param>
public record AuthorizationsV0(
    IList<UpdatePublicKey> Keys,
    AccessStructure Emergency,
    AccessStructure Protocol,
    AccessStructure Consensus,
    AccessStructure EuroPerEnergy,
    AccessStructure MicroCcdPerEuro,
    AccessStructure FoundationAccount,
    AccessStructure MintDistribution,
    AccessStructure TransactionFeeDistribution,
    AccessStructure GasRewards,
    AccessStructure Pool,
    AccessStructure AddAnonymityRevoker,
    AccessStructure AddIdentityProvider)
{
    internal static AuthorizationsV0 From(Grpc.V2.AuthorizationsV0 authorizationsV0) =>
        new(
            authorizationsV0.Keys.Select(UpdatePublicKey.From).ToList(),
            AccessStructure.From(authorizationsV0.Emergency),
            AccessStructure.From(authorizationsV0.Protocol),
            AccessStructure.From(authorizationsV0.ParameterConsensus),
            AccessStructure.From(authorizationsV0.ParameterEuroPerEnergy),
            AccessStructure.From(authorizationsV0.ParameterMicroCCDPerEuro),
            AccessStructure.From(authorizationsV0.ParameterFoundationAccount),
            AccessStructure.From(authorizationsV0.ParameterMintDistribution),
            AccessStructure.From(authorizationsV0.ParameterTransactionFeeDistribution),
            AccessStructure.From(authorizationsV0.ParameterGasRewards),
            AccessStructure.From(authorizationsV0.PoolParameters),
            AccessStructure.From(authorizationsV0.AddAnonymityRevoker),
            AccessStructure.From(authorizationsV0.AddIdentityProvider)
        );
}

/// <summary>
/// Access structures for each of the different possible chain updates, together
/// with the context giving all the possible keys.
/// </summary>
/// <param name="V0"><see cref="AuthorizationsV0"/></param>
/// <param name="CooldownParameters">Keys for changing cooldown periods related to baking and delegating.</param>
/// <param name="TimeParameters">Keys for changing the length of the reward period.</param>
public record AuthorizationsV1(
    AuthorizationsV0 V0,
    AccessStructure CooldownParameters,
    AccessStructure TimeParameters)
{
    internal static AuthorizationsV1 From(Grpc.V2.AuthorizationsV1 authorizationsV1) =>
        new(
            AuthorizationsV0.From(authorizationsV1.V0),
            AccessStructure.From(authorizationsV1.ParameterCooldown),
            AccessStructure.From(authorizationsV1.ParameterTime)
        );
}
