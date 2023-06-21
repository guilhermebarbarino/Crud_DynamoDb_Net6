using Amazon.DynamoDBv2.DataModel;

namespace Dynamo_Manager.models
{

    [DynamoDBTable("students")]
    public class Students
    {
        [DynamoDBHashKey("id")]
        public int? Id { get; set; }

        [DynamoDBProperty("Nome")]
        public string? Nome { get; set; }

        [DynamoDBProperty("Idade")]
        public int? Idade { get; set; }

        [DynamoDBProperty("Pais")]
        public string? Pais { get; set; }
    }
}
