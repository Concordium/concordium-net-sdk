﻿using Newtonsoft.Json;
using Concordium.V2;
using ConcordiumNetSdk.Client;
using AccountAddress = ConcordiumNetSdk.Types.AccountAddress;
using ConcordiumNetSdk.Wallets;

var genesisWallet =
    @"
{
  ""accountKeys"": {
    ""keys"": {
      ""0"": {
        ""keys"": {
          ""0"": {
            ""signKey"": ""443c20439711361b6870c1679be33860d10cf7cded240e4a567e31ec3a56ecf5"",
            ""verifyKey"": ""514dd55ccc851b067f5f82ec2991b2f7313e82dadfd265de8e808a0bbd9f3539""
          }
        },
        ""threshold"": 1
      }
    },
    ""threshold"": 1
  },
  ""aci"": {
    ""credentialHolderInformation"": {
      ""idCredSecret"": ""22d0023ba339bc134ebc44daed45bf65b30910e2d26e32edd19e02d8a712869d""
    },
    ""prfKey"": ""4a9ed840929186e729b1f75bc66e2b12349956eb291a7e9a9409c07d43f87c2a""
  },
  ""address"": ""3h9rPiZoQVsfPMFv3s1xReZHPPJ7f7jJfRFnfuhZHNTL7LWqGG"",
  ""credentials"": {
    ""v"": 0,
    ""value"": {
      ""0"": {
        ""contents"": {
          ""arData"": {
            ""1"": {
              ""encIdCredPubShare"": ""98dde3f35587822f130c5e446c1179b225c3fa28752ac9eb1a8a094831f7bd502162679a689484f61539d1365ae46747ae1ac93c1dd1848a159bdd3bea39f4607e9fd4971c9ad3a0a981ffb1100deed28838ce1c841c73d40acd23c9b953963a""
            }
          },
          ""commitments"": {
            ""cmmAttributes"": {},
            ""cmmCredCounter"": ""998749aed65f17576c58cfcfd1a8b60eb8873b789172e40d0fb6c9023c20233543cd82bc771e1ca880564ca6a32c8ea3"",
            ""cmmIdCredSecSharingCoeff"": [
              ""adaf01a8891811352aae281eba75484ed9f2be4c8656b3ef829b01d7c9367232ae3f1ecfeb86e73a29b11405a52e47c8""
            ],
            ""cmmMaxAccounts"": ""a1fc107066fed534855f12b79a86a8ccf916265af7908c3c24d08c072685369a3001e072f33831f3b81987f8f6f46c00"",
            ""cmmPrf"": ""8855d00e72d033efdb94f4a04c6aadf554bb7f85445ebfe55a9d3b88c5878992e9ca062e82e9185b68fd5664dc0acdb5""
          },
          ""credId"": ""a4c32e92df5d140904dbed4ddae0091df930bcfc0dc9279d9ae47292892b93d0dc91c357c5859d6497418aeaad2f8ecd"",
          ""credentialPublicKeys"": {
            ""keys"": {
              ""0"": {
                ""schemeId"": ""Ed25519"",
                ""verifyKey"": ""514dd55ccc851b067f5f82ec2991b2f7313e82dadfd265de8e808a0bbd9f3539""
              }
            },
            ""threshold"": 1
          },
          ""ipIdentity"": 0,
          ""policy"": {
            ""createdAt"": ""202211"",
            ""revealedAttributes"": {},
            ""validTo"": ""202311""
          },
          ""revocationThreshold"": 1
        },
        ""type"": ""normal""
      }
    }
  },
  ""encryptionPublicKey"": ""b14cbfe44a02c6b1f78711176d5f437295367aa4f2a8c2551ee10d25a03adc69d61a332a058971919dad7312e1fc94c5a4c32e92df5d140904dbed4ddae0091df930bcfc0dc9279d9ae47292892b93d0dc91c357c5859d6497418aeaad2f8ecd"",
  ""encryptionSecretKey"": ""b14cbfe44a02c6b1f78711176d5f437295367aa4f2a8c2551ee10d25a03adc69d61a332a058971919dad7312e1fc94c5506ebe5a69ceb9135dddb9bc93293eb23396e2e09b52246cfb50c992b34cd7ca""
} 
";

WalletAccount myGenesisWalletAccount = WalletAccount.FromGenesisWalletExportFormat(genesisWallet);

Console.WriteLine("Hello!");
/*
// Create the client.
Uri url = new Uri("https://127.0.0.1/");

//Uri url = new Uri("https://172.31.16.30/");
UInt16 port = 8169;
ConcordiumClient concordiumNodeClient = new ConcordiumClient(url, port, 30, false);

var address = "3EqkGQ7NvakjpbUeocRiGzao62ZEEvB6A5rjGsezL8bQBKQGU8";
var mySender = AccountAddress.From(address).ToProto();
var best = new BlockHashInput() { Best = new Empty() };

var myAccountInfoRequest = new AccountInfoRequest()
{
    BlockHash = best,
    AccountIdentifier = new AccountIdentifierInput() { Address = mySender }
};

var moduleListRes = concordiumNodeClient.GetModuleList(best);
var accountInfoRes = concordiumNodeClient.GetAccountInfo(myAccountInfoRequest);

Console.WriteLine("Module list result:");
await foreach (var module in moduleListRes)
{
    Console.WriteLine("Got a module entry in module list stream.");
}

Console.WriteLine("Account info result:");
Console.WriteLine(JsonConvert.SerializeObject(accountInfoRes));
*/
