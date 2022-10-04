using Mayotech.CloudCode;
using Newtonsoft.Json;

public class TestRequest : CloudCodeRequest<TestResponse>
{
    public override string RpcName => "TestScript";
}

public class TestResponse : CloudCodeResponse
{
    [JsonProperty("sides")]
    protected int sides;
    [JsonProperty("roll")]
    protected int roll;
}
