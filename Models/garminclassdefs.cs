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

// Root price class
public class ProductPrice
{
    public string? Pid { get; set; }

    public List<PricedSku>? PricedSkus { get; set; }
}

// PricedSku class
public class PricedSku
{
    public string? PartNumber { get; set; }

    public List<string>? AppliedPromotionGuids { get; set; }

    public decimal? SalePrice { get; set; }

    public ListPrice? ListPrice { get; set; }

    public decimal? Savings { get; set; }
}

public class ListPrice
{
    public decimal Price { get; set; }

    public string? WholeUnitAmount { get; set; }

    public string? CurrencySymbol { get; set; }

    public string? DecimalSeparator { get; set; }

    public int TenthDigit { get; set; }

    public int HundredthDigit { get; set; }

    public string? CurrencyCode { get; set; }

    public string? Template { get; set; }

    public string? FormattedPrice { get; set; }
}