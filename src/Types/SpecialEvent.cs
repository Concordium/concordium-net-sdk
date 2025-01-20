using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// In addition to the user initiated transactions the protocol generates some
/// events which are deemed "Special outcomes". These are rewards for running
/// the consensus and finalization protocols.
/// </summary>
public interface ISpecialEvent
{ }

internal static class SpecialEventFactory
{
    internal static ISpecialEvent From(BlockSpecialEvent specialEvent) =>
            specialEvent.EventCase switch
            {
                BlockSpecialEvent.EventOneofCase.BakingRewards =>
                    BakingRewards.From(specialEvent.BakingRewards),
                BlockSpecialEvent.EventOneofCase.Mint =>
                    Mint.From(specialEvent.Mint),
                BlockSpecialEvent.EventOneofCase.FinalizationRewards =>
                    FinalizationRewards.From(specialEvent.FinalizationRewards),
                BlockSpecialEvent.EventOneofCase.BlockReward =>
                    BlockReward.From(specialEvent.BlockReward),
                BlockSpecialEvent.EventOneofCase.PaydayFoundationReward =>
                    PaydayFoundationReward.From(specialEvent.PaydayFoundationReward),
                BlockSpecialEvent.EventOneofCase.PaydayAccountReward =>
                    PaydayAccountReward.From(specialEvent.PaydayAccountReward),
                BlockSpecialEvent.EventOneofCase.BlockAccrueReward =>
                    BlockAccrueReward.From(specialEvent.BlockAccrueReward),
                BlockSpecialEvent.EventOneofCase.PaydayPoolReward =>
                    PaydayPoolReward.From(specialEvent.PaydayPoolReward),
                BlockSpecialEvent.EventOneofCase.ValidatorPrimedForSuspension =>
                    ValidatorPrimedForSuspension.From(specialEvent.ValidatorPrimedForSuspension),
                BlockSpecialEvent.EventOneofCase.ValidatorSuspended =>
                    ValidatorSuspended.From(specialEvent.ValidatorSuspended),
                BlockSpecialEvent.EventOneofCase.None => throw new MissingEnumException<BlockSpecialEvent.EventOneofCase>(specialEvent.EventCase),
                _ => throw new MissingEnumException<BlockSpecialEvent.EventOneofCase>(specialEvent.EventCase)
            };
}

/// <summary>
/// Reward issued to all the bakers at the end of an epoch for baking blocks
/// in the epoch.
/// </summary>
/// <param name="Rewards">The amount awarded to each baker.</param>
/// <param name="Remainder">The remaining balance of the baker reward account.</param>
public sealed record BakingRewards(IDictionary<AccountAddress, CcdAmount> Rewards, CcdAmount Remainder) : ISpecialEvent
{
    internal static BakingRewards From(BlockSpecialEvent.Types.BakingRewards bakingRewards) =>
        new(
            Rewards: bakingRewards
                .BakerRewards
                .Entries
                .ToDictionary(e => AccountAddress.From(e.Account), e => CcdAmount.From(e.Amount)),
            Remainder: CcdAmount.From(bakingRewards.Remainder)
        );
}

/// <summary>
/// Distribution of newly minted CCD.
/// </summary>
/// <param name="MintBakingReward">
/// The portion of the newly minted CCD that goes to the baking reward
/// account.
/// </param>
/// <param name="MintFinalizationReward">The portion that goes to the finalization reward account.  </param>
/// <param name="MintPlatformDevelopmentCharge">The portion that goes to the foundation, as foundation tax.</param>
/// <param name="FoundationAccount">
/// The address of the foundation account that the newly minted CCD goes
/// to.
/// </param>
public sealed record Mint(
        CcdAmount MintBakingReward,
        CcdAmount MintFinalizationReward,
        CcdAmount MintPlatformDevelopmentCharge,
        AccountAddress FoundationAccount
        )
    : ISpecialEvent
{
    internal static Mint From(BlockSpecialEvent.Types.Mint mint) =>
        new(
            MintBakingReward: CcdAmount.From(mint.MintBakingReward),
            MintFinalizationReward: CcdAmount.From(mint.MintFinalizationReward),
            MintPlatformDevelopmentCharge: CcdAmount.From(mint.MintPlatformDevelopmentCharge),
            FoundationAccount: AccountAddress.From(mint.FoundationAccount)
        );
}

/// <summary>
/// Distribution of finalization rewards.
/// </summary>
/// <param name="Rewards">The amount awarded to each finalizer.</param>
/// <param name="Remainder">
/// Remaining balance of the finalization reward account. It exists
/// since it is not possible to perfectly distribute the
/// accumulated rewards since amounts are represented as integers.
/// </param>
public sealed record FinalizationRewards(IDictionary<AccountAddress, CcdAmount> Rewards, CcdAmount Remainder) : ISpecialEvent
{
    internal static FinalizationRewards From(BlockSpecialEvent.Types.FinalizationRewards finalizationRewards) =>
        new(
            Rewards: finalizationRewards
                .FinalizationRewards_
                .Entries
                .ToDictionary(e => AccountAddress.From(e.Account), e => CcdAmount.From(e.Amount)),
            Remainder: CcdAmount.From(finalizationRewards.Remainder)
        );
}

/// <summary>
/// Reward for including transactions in a block.
/// </summary>
/// <param name="TransactionFees">Total amount of transaction fees in the block.</param>
/// <param name="OldGasAccount">Previous balance of the GAS account.</param>
/// <param name="NewGasAccount">New balance of the GAS account.</param>
/// <param name="BakerReward">The amount of CCD that goes to the baker.</param>
/// <param name="FoundationCharge">The amount of CCD that goes to the foundation.</param>
/// <param name="Baker">The account address where the baker receives the reward.</param>
/// <param name="FoundationAccount">The account address where the foundation receives the tax.</param>
public sealed record BlockReward(
    CcdAmount TransactionFees,
    CcdAmount OldGasAccount,
    CcdAmount NewGasAccount,
    CcdAmount BakerReward,
    CcdAmount FoundationCharge,
    AccountAddress Baker,
    AccountAddress FoundationAccount
    ) : ISpecialEvent
{
    internal static BlockReward From(BlockSpecialEvent.Types.BlockReward blockReward) =>
        new(
            TransactionFees: CcdAmount.From(blockReward.TransactionFees),
            OldGasAccount: CcdAmount.From(blockReward.OldGasAccount),
            NewGasAccount: CcdAmount.From(blockReward.NewGasAccount),
            BakerReward: CcdAmount.From(blockReward.BakerReward),
            FoundationCharge: CcdAmount.From(blockReward.FoundationCharge),
            Baker: AccountAddress.From(blockReward.Baker),
            FoundationAccount: AccountAddress.From(blockReward.FoundationAccount)
        );
}


/// <summary>
/// Payment for the foundation.
/// </summary>
/// <param name="FoundationAccount">Address of the foundation account.</param>
/// <param name="DevelopmentCharge">Amount rewarded.</param>
public sealed record PaydayFoundationReward(AccountAddress FoundationAccount, CcdAmount DevelopmentCharge) : ISpecialEvent
{
    internal static PaydayFoundationReward From(BlockSpecialEvent.Types.PaydayFoundationReward paydayFoundationReward) =>
        new(
            FoundationAccount: AccountAddress.From(paydayFoundationReward.FoundationAccount),
            DevelopmentCharge: CcdAmount.From(paydayFoundationReward.DevelopmentCharge)
        );
}

/// <summary>
/// Payment for a particular account.
/// When listed in a block summary, the delegated pool of the account is
/// given by the last PaydayPoolReward outcome included before this outcome.
/// </summary>
/// <param name="Account">The account that got rewarded.</param>
/// <param name="TransactionFees">The transaction fee reward at payday to the account.</param>
/// <param name="BakerReward">The baking reward at payday to the account.</param>
/// <param name="FinalizationReward">The finalization reward at payday to the account.</param>
public sealed record PaydayAccountReward(
    AccountAddress Account,
    CcdAmount TransactionFees,
    CcdAmount BakerReward,
    CcdAmount FinalizationReward
    ) : ISpecialEvent
{
    internal static PaydayAccountReward From(BlockSpecialEvent.Types.PaydayAccountReward paydayAccountReward) =>
        new(
            Account: AccountAddress.From(paydayAccountReward.Account),
            TransactionFees: CcdAmount.From(paydayAccountReward.TransactionFees),
            BakerReward: CcdAmount.From(paydayAccountReward.BakerReward),
            FinalizationReward: CcdAmount.From(paydayAccountReward.FinalizationReward)
        );
}

/// <summary>
/// Amounts accrued to accounts for each baked block.
/// </summary>
/// <param name="TransactionFees">The total fees paid for transactions in the block.</param>
/// <param name="OldGasAccount">The old balance of the GAS account.</param>
/// <param name="NewGasAccount">The new balance of the GAS account.</param>
/// <param name="BakerReward">The amount awarded to the baker.</param>
/// <param name="PassiveReward">The amount awarded to the passive delegators.</param>
/// <param name="FoundationCharge">The amount awarded to the foundation.</param>
/// <param name="BakerId">The baker of the block, who will receive the award.</param>
public sealed record BlockAccrueReward(
    CcdAmount TransactionFees,
    CcdAmount OldGasAccount,
    CcdAmount NewGasAccount,
    CcdAmount BakerReward,
    CcdAmount PassiveReward,
    CcdAmount FoundationCharge,
    BakerId BakerId
) : ISpecialEvent
{
    internal static BlockAccrueReward From(BlockSpecialEvent.Types.BlockAccrueReward blockAccrueReward) =>
        new(
            TransactionFees: CcdAmount.From(blockAccrueReward.TransactionFees),
            OldGasAccount: CcdAmount.From(blockAccrueReward.OldGasAccount),
            NewGasAccount: CcdAmount.From(blockAccrueReward.NewGasAccount),
            BakerReward: CcdAmount.From(blockAccrueReward.BakerReward),
            PassiveReward: CcdAmount.From(blockAccrueReward.PassiveReward),
            FoundationCharge: CcdAmount.From(blockAccrueReward.FoundationCharge),
            BakerId: new BakerId(new AccountIndex(blockAccrueReward.Baker.Value))
        );
}

/// <summary>
/// Payment distributed to a pool or passive delegators.
/// </summary>
/// <param name="PoolOwner">The pool owner (passive delegators when null).</param>
/// <param name="TransactionFees">Accrued transaction fees for pool.</param>
/// <param name="BakerReward">Accrued baking rewards for pool.</param>
/// <param name="FinalizationReward">Accrued finalization rewards for pool.</param>
public sealed record PaydayPoolReward(
    ulong? PoolOwner,
    CcdAmount TransactionFees,
    CcdAmount BakerReward,
    CcdAmount FinalizationReward
) : ISpecialEvent
{
    internal static PaydayPoolReward From(BlockSpecialEvent.Types.PaydayPoolReward paydayPoolReward) =>
        new(
            PoolOwner: paydayPoolReward.PoolOwner?.Value,
            TransactionFees: CcdAmount.From(paydayPoolReward.TransactionFees),
            BakerReward: CcdAmount.From(paydayPoolReward.BakerReward),
            FinalizationReward: CcdAmount.From(paydayPoolReward.FinalizationReward)
        );
}

/// <summary>
/// A validator that is primed for suspension at the next snapshot epoch due to too
/// many missed rounds.
/// </summary>
/// <param name="BakerId">The id of the primed validator.</param>
/// <param name="Account">The account of the primed validator.</param>
public sealed record ValidatorPrimedForSuspension(
    BakerId BakerId,
    AccountAddress Account
) : ISpecialEvent
{
    internal static ValidatorPrimedForSuspension From(BlockSpecialEvent.Types.ValidatorPrimedForSuspension primed) =>
        new(
            BakerId: BakerId.From(primed.BakerId),
            Account: AccountAddress.From(primed.Account)
        );
}

/// <summary>
/// The id of a validator that got suspended due to too many missed rounds.
/// </summary>
/// <param name="BakerId">The id of the suspended validator.</param>
/// <param name="Account">The account of the suspended validator.</param>
public sealed record ValidatorSuspended(
    BakerId BakerId,
    AccountAddress Account
) : ISpecialEvent
{
    internal static ValidatorSuspended From(BlockSpecialEvent.Types.ValidatorSuspended suspended) =>
        new(
            BakerId: BakerId.From(suspended.BakerId),
            Account: AccountAddress.From(suspended.Account)
        );
}
