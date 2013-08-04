public sealed class RequestType
{
    private readonly string name;
    private readonly int value;

    public static readonly RequestType Search                           = new RequestType(1,  "Search.ff");
    public static readonly RequestType Suggest                          = new RequestType(2,  "Suggest.ff");
    public static readonly RequestType ShoppingCartInformationCollector = new RequestType(3,  "Scic.ff");
    public static readonly RequestType WhatsHot                         = new RequestType(4,  "WhatsHot.ff");
    public static readonly RequestType Recommendation                   = new RequestType(5,  "Recommender.ff");
    public static readonly RequestType ProductCampaign                  = new RequestType(6,  "ProductCampaign.ff");
    public static readonly RequestType SimilarRecords                   = new RequestType(7,  "SimilarRecords.ff");
    public static readonly RequestType Compare                          = new RequestType(8,  "Compare.ff");
    public static readonly RequestType Import                           = new RequestType(9,  "Import.ff");
    public static readonly RequestType TagCloud                         = new RequestType(10, "TagCloud.ff");
    public static readonly RequestType Tracking                         = new RequestType(11, "Tracking.ff");

    private RequestType(int value, string name)
    {
        this.name = name;
        this.value = value;
    }

    public override string ToString()
    {
 	     return name;
    }
}
