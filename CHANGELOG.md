## Unreleased changes
- Added
  - Add optional cancellation token parameter to all client calls.
- Obsolete
  - Made former constructors on `ConcordiumClient` and `RawClient` obsolete in favor of   new overload which takes `ConcordiumClientOptions`. Using this makes it easier to used  from configurations and extending with additional properties in the future.
- Breaking changes
  - Data structures have been aligned throughout the SDK which has resulted in some major   changes. Changes are:
    - Records are used when structures need immutability and equality by value.
    - Struct are used when data structures is below 16 bytes, and when they are not used  through any interfaces.
    - Sealed are added where inheritance isn't expected.
    - Structs are either records (and hence as default override) or override equality
    - Record structs has readonly where immutability is expected.
    - Implicit operators are removed to favor being explicit.
  - Add ConfigureAwait to enhance library uses from UI- or own syncronization context   usages.
  - `RawClient` asynchronous calls have been changed to return either `AsyncUnaryCall<T>`   or `AsyncServerStreamingCall` such that response header is available in   `ConcordiumClient`. 
    One can get the response be calling `.ResponseAsync` on `AsyncUnaryCall<T>` and   `ResponseStream` on `AsyncServerStreamingCall`.
  - Property `Timeout` moved into `Options` in class `RawClient`.


## 2.0.0
- Rewrite the SDK to use the Concordium Node gRPC API version 2. This
  rewrite does not preserve any of the old API and therefore deprecates
  earlier versions of the SDK.

## 1.0.0
- Initial release.