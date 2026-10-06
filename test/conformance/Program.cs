using System.Text.Json.Nodes;
using AnyOAuth;
JsonObject output;
try {var input=JsonNode.Parse(await Console.In.ReadToEndAsync())!.AsObject();var p=input["params"]?.AsObject()??new JsonObject();using var c=new Client(input["baseUrl"]?.GetValue<string>()??"https://api.anyoauth.com",input["clientId"]?.GetValue<string>()??"",input["clientSecret"]?.GetValue<string>()??"");JsonNode? result=input["action"]!.GetValue<string>() switch {
"exchangeCode" => await c.ExchangeCode(p),
"profile" => await c.Profile(p),
"revoke" => await c.Revoke(p),
"providers" => await c.Providers(p),
"pkceChallenge"=>JsonValue.Create(Client.PkceChallenge(p["codeVerifier"]!.GetValue<string>())),
"createTransaction"=>Client.CreateTransaction(),
"authorizationUrl"=>JsonValue.Create(c.AuthorizationUrl(p)),
"validateCallback"=>Client.ValidateCallback(p),
_=>throw new SDKException(0,"invalid_argument","Unknown action")};output=new(){["ok"]=true,["result"]=result};}
catch(Exception e){var error=e as SDKException??new SDKException(0,"invalid_argument","Invalid input");output=new(){["ok"]=false,["error"]=new JsonObject{["status"]=error.Status,["code"]=error.Code,["message"]=error.Message}};}
Console.WriteLine(output.ToJsonString());
