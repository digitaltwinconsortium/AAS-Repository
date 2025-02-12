using Newtonsoft.Json;
using System.Collections.Generic;

namespace AdminShell
{
    public class DynamicsBearerTokenResponse
    {
        [JsonProperty("access_token")]
        public string accessToken = string.Empty;

        [JsonProperty("token_type")]
        public string tokenType = string.Empty;

        [JsonProperty("expires_in")]
        public int expiresInSeconds = 0;
    }

    public class DynamicsQuery
    {
        public string tracingDirection { get; set; }

        public string trackingId { get; set; }

        public string company { get; set; }

        public string itemNumber { get; set; }

        public string serialNumber { get; set; }

        public string batchNumber { get; set; }

        public bool shouldIncludeEvents { get; set; }
    }


    public class DynamicsQueryResponse
    {
        public string tracingDirection { get; set; }

        public Root root { get; set; }
    }

    public class Root
    {
        public string trackingId { get; set; }

        public List<Next> next { get; set; }

        public List<Event> events { get; set; }
    }

    public class ConsumptionTransaction
    {
        public string transactionId { get; set; }

        public string itemId { get; set; }

        public string trackingId { get; set; }

        public string eventId { get; set; }

        public int quantity { get; set; }

        public string unitOfMeasure { get; set; }

        public string transactionType { get; set; }

        public string batchId { get; set; }
    }

    public class Details
    {
        public string datacollectionname { get; set; }
    }

    public class Event
    {
        public string eventId { get; set; }

        public string companyCode { get; set; }

        public string @operator { get; set; }

        public string description { get; set; }

        public string activityType { get; set; }

        public string activityCode { get; set; }

        public string datetime { get; set; }

        public Details details { get; set; }

        public List<ConsumptionTransaction> consumptionTransactions { get; set; }

        public List<ProductTransaction> productTransactions { get; set; }
    }

    public class Next
    {
        public string trackingId { get; set; }

        public List<object> next { get; set; }

        public List<object> events { get; set; }
    }

    public class ProductTransaction
    {
        public string transactionId { get; set; }

        public string itemId { get; set; }

        public string trackingId { get; set; }

        public Details details { get; set; }

        public string eventId { get; set; }

        public int quantity { get; set; }

        public string unitOfMeasure { get; set; }

        public string transactionType { get; set; }

        public string serialId { get; set; }
    }
}
