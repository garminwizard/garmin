using Newtonsoft.Json;

public class RootObject
{
    public List<Product>? Products { get; set; }
}

public class Product
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public Description? Description { get; set; }

    [JsonProperty("image")]
    public Image? Image { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("group")]
    public bool Group { get; set; }

    [JsonProperty("productIds")]
    public List<string>? ProductIds { get; set; }
}

public class Description
{
    [JsonProperty("shortText")]
    public string? ShortText { get; set; }

    [JsonProperty("longText")]
    public string? LongText { get; set; }
}

public class Image
{
    [JsonProperty("large")]
    public string? Large { get; set; }
}

public class Media
{
    public string? smallImage { get; set; }
    public string? mediumImage { get; set; }
    public string? largeImage { get; set; }
}

public class Value
{
    public string pid { get; set; } = string.Empty;
    public string specDisplayValue { get; set; } = string.Empty;
    public string specValue { get; set; } = string.Empty;
}

public class Spec
{
    public string specKey { get; set; } = string.Empty;
    public string specDisplayName { get; set; } = string.Empty;
    public bool differences { get; set; }
    public List<Value> values { get; set; } = new();
}

public class SpecGroup
{
    public string specGroupKey { get; set; } = string.Empty;
    public string specGroupKeyDisplayName { get; set; } = string.Empty;
    public List<Spec> specs { get; set; } = new();
}

public class ProductDetails
{
    public string pid { get; set; } = string.Empty;
    public string displayName { get; set; } = string.Empty;
    public Media? media { get; set; }
    public string productUrl { get; set; } = string.Empty;
    public bool sellable { get; set; }
    public string description { get; set; } = string.Empty;
}

public class ProductSpecs
{
    public List<SpecGroup>? specGroups { get; set; }
}

public class ProductRootObject
{
    public List<ProductDetails>? products { get; set; }
    public ProductSpecs? productSpecs { get; set; }
}

public class Unformatted
{
    public string? salePrice { get; set; }
    public string? listPrice { get; set; }
}

public class SkuPromotions
{
    // You can add properties specific to skuPromotions if needed
}

public class ProductPrice
{
    public string? id { get; set; }
    public string? salePrice { get; set; }
    public string? listPrice { get; set; }
    public Unformatted? unformatted { get; set; }
    public SkuPromotions? skuPromotions { get; set; }
}