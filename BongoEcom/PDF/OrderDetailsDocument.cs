using QuestPDF.Infrastructure;
using Application.Features.Orders.Queries;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Drawing;
using Application.Features.Attributes.DTOs;
using Newtonsoft.Json;
using Application.Features.CompanyInfoes.Queries;
using Shared.Enums;

namespace BongoEcom.PDF;
public class OrderPdfDocument : IDocument
{
    private readonly OrderDto _order;
    private readonly IWebHostEnvironment _env;
    private readonly CompanyInfoDto _company;

    public OrderPdfDocument(OrderDto order, IWebHostEnvironment env, CompanyInfoDto? company)
    {
        _order = order;
        _company = company ?? new();
        _env = env;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(1, Unit.Centimetre);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().AlignCenter().Text(x =>
            {
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });
        });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text(_company.CompanyName).FontSize(20).SemiBold();
            });

            row.ConstantItem(100).Height(50).Image(MapWwwrootPath(_company.LogoUrl), ImageScaling.FitArea);
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(10).Column(column =>
        {
            column.Spacing(10);

            // Customer Information
            column.Item().BorderBottom(1).PaddingBottom(10).Column(customerColumn =>
            {
                customerColumn.Item().Text($"Customer: {_order.CustomerName}").FontColor(Colors.Blue.Medium);
                customerColumn.Item().Text($"Order No: #{_order.OrderNo}");
                customerColumn.Item().Text($"Status: {(OrderStatus)_order.Status}");
                customerColumn.Item().Text($"Delivery Address: {_order.DeliveryAddress}");
                customerColumn.Item().Text($"Phone: {_order.CustomerPhone}");
                customerColumn.Item().Text($"Payment Method: {_order.PaymentMethod}");
                customerColumn.Item().Text($"Order Date: {_order.CreatedAt.ToString("f")}");
                if (_order.Coupon is not null)
                {
                    customerColumn.Item().Text($"Coupon : {_order.Coupon.Code} is applied");
                }
            });

            // Order Items
            column.Item().PaddingTop(10).Text("Order Items").FontSize(14).SemiBold();
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(2, Unit.Centimetre); // Image
                    columns.RelativeColumn(2); // Product
                    columns.RelativeColumn(2); // Attributes
                    columns.ConstantColumn(1.5f, Unit.Centimetre); // Qty
                    columns.ConstantColumn(2, Unit.Centimetre); // Unit Price
                    columns.ConstantColumn(2, Unit.Centimetre); // Total
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Text("Image").SemiBold();
                    header.Cell().Text("Product").SemiBold();
                    header.Cell().Text("Attributes").SemiBold();
                    header.Cell().Text("Qty").SemiBold();
                    header.Cell().Text("Unit Price").SemiBold();
                    header.Cell().Text("Total").SemiBold();

                    header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                });

                // Items
                foreach (var item in _order.Items)
                {
                    table.Cell().Padding(5).Image(MapWwwrootPath(item.ImageUrl), ImageScaling.FitArea);
                    table.Cell().Text(item.ProductName);
                    table.Cell().Text(txt =>
                    {
                        var items = JsonConvert.DeserializeObject<List<ItemAttribute>>(item.Attributes);
                        if (items is null)
                        {
                            txt.Span("");
                        }
                        else
                        {
                            foreach (var item in items)
                            {
                                txt.Span($"{item.Key} : ").SemiBold();
                                txt.Span(item.Value);
                                txt.Span(", ");
                            }
                        }
                    });
                    table.Cell().Text(item.Quantity.ToString());
                    table.Cell().Text($"৳{item.UnitPrice:N2}").FontFamily("Siyam Rupali");
                    table.Cell().Text($"৳{(item.Quantity * item.UnitPrice):N2}").FontFamily("Siyam Rupali"); ;
                }
            });

            // Summary
            column.Item().PaddingTop(10).Column(summaryColumn =>
            {
                summaryColumn.Item().Row(row =>
                {
                    row.RelativeItem().Text("Subtotal:").SemiBold();
                    row.ConstantItem(100).AlignRight().Text($"৳{_order.SubTotal:N2}").FontFamily("Siyam Rupali"); ;
                });

                if(_order.Discount > 0)
                {
                    summaryColumn.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Discount : ").SemiBold();
                        row.ConstantItem(100).AlignRight().Text($" - ৳{_order.Discount:N2}").FontFamily("Siyam Rupali"); ;
                    });
                }

                summaryColumn.Item().Row(row =>
                {
                    row.RelativeItem().Text("Delivery Charge:").SemiBold();
                    row.ConstantItem(100).AlignRight().Text($"৳{_order.DeliveryCharge:N2}").FontFamily("Siyam Rupali"); ;
                });

                summaryColumn.Item().Row(row =>
                {
                    row.RelativeItem().Text("Total:").FontSize(13).SemiBold();
                    row.ConstantItem(100).AlignRight().Text($"৳{_order.TotalAmount:N2}").FontSize(13).SemiBold().FontFamily("Siyam Rupali"); ;
                });
            });
        });
    }

    public string MapWwwrootPath(string relativePath)
    {
        var cleanPath = relativePath.TrimStart('~', '/');
        return Path.Combine(_env.WebRootPath, cleanPath);
    }
}