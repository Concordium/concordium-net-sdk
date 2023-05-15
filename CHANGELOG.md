## Unreleased changes
- Added `GetBlockItemStatus` endpoint to clients and added helper classes `BlockItemSummary` and `TransactionStatus`.
- Rewrite the SDK to use the Concordium Node gRPC API version 2. This
  rewrite does not preserve any of the old API and therefore deprecates
  earlier versions of the SDK.

## 1.0.0
- Initial release.
