
namespace Shared.Enums;

public enum ProductType
{
    SimpleProduct = 5,
    GroupedProduct = 10,
    BundledProduct = 15
}

public enum ProductVisibility
{
    Full = 0,
    SearchResults = 10,
    ProductPage = 20,
    Hidden = 30
}

public enum ProductCondition
{
    New = 0,
    Refurbished = 10,
    Used = 20,
    Damaged = 30
}

public enum QuantityControlType
{
    Spinner = 0,
    Dropdown = 1
}

public enum AttributeChoiceBehaviour
{
    GrayOutUnavailable = 0,
    None = 30
}

public enum BackorderMode
{
    NoBackorders = 0,
    AllowQtyBelow0 = 1,
    AllowQtyBelow0OnBackorder = 2
}

public enum DownloadActivationType
{
    WhenOrderIsPaid = 1,
    Manually = 10
}

public enum LowStockActivity
{
    Nothing = 0,
    DisableBuyButton = 1,
    Unpublish = 2
}

public enum ManageInventoryMethod
{
    DontManageStock = 0,
    ManageStock = 1,
    ManageStockByAttributes = 2
}

public enum RecurringProductCyclePeriod
{
    Days = 0,
    Weeks = 10,
    Months = 20,
    Years = 30
}


public enum ProductSortingEnum
{
    Initial = 0,
    Relevance = 1,
    NameAsc = 5,
    NameDesc = 6,
    PriceAsc = 10,
    PriceDesc = 11,
    CreatedOn = 15,
    CreatedOnAsc = 16
}
