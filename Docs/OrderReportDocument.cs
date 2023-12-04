using HouseholdOnlineStore.Models.DocumentModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;

namespace HouseholdOnlineStore.Docs
{
	public class OrderReportDocument : IDocument
	{
		public OrderReportModel Model { get; }

		public OrderReportDocument(OrderReportModel model)
		{
			Model = model;
		}

		public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
		public DocumentSettings GetSettings() => DocumentSettings.Default;

		public void Compose(IDocumentContainer container)
		{
			container
			.Page(page =>
			{
				page.Margin(50);

				page.Header().Element(ComposeHeader);
				page.Content().Element(ComposeContent);


				page.Footer().AlignCenter().Text(x =>
				{
					x.CurrentPageNumber();
					x.Span(" / ");
					x.TotalPages();
				});
			});

			void ComposeHeader(IContainer container)
			{
				var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

				container.Row(row =>
				{
					row.RelativeItem().Column(column =>
					{
						column.Item().Text($"Order report").Style(titleStyle);

						column.Item().Text(text =>
						{
							text.Span("From ").SemiBold();
							text.Span($"{Model.StartDate:d}");
							text.Span(" to ").SemiBold();
							text.Span($"{Model.EndDate:d}");

						});

					});

					row.RelativeItem().Column(column =>
					{
						column.Spacing(2);

						column.Item().PaddingBottom(5).Text($"Total number of orders: {Model.Items.Count}").SemiBold();
						
						column.Item().BorderBottom(1).Text("Delivered by ").FontFamily(Fonts.Arial).SemiBold();
						column.Item().Text("Укрпошта: " + Model.UkrPostCount).FontFamily(Fonts.Arial);
						column.Item().Text("Нова пошта: " + Model.NovaPostCount).FontFamily(Fonts.Arial);
						column.Item().Text("Meest: " + Model.MeestCount).FontFamily(Fonts.Arial);
						column.Item().BorderBottom(1).Text("Paid using: ").FontFamily(Fonts.Arial).SemiBold();
						column.Item().Text("Credit card: " + Model.CardCount).FontFamily(Fonts.Arial);
						column.Item().Text("Cash: " + Model.CashCount).FontFamily(Fonts.Arial);
					});
				});

				
			}

			void ComposeContent(IContainer container)
			{
				container.PaddingVertical(40).Column(column =>
				{
					column.Spacing(5);

					column.Item().Element(ComposeTable);

					var totalPrice = Model.Items.Select(x => x.Products.Sum(o => o.Price * o.Quantity)).Sum();
					column.Item().AlignRight().Text($"Grand total: {totalPrice}").FontSize(14);
				});
			}

			void ComposeTable(IContainer container)
			{
				container.Table(table =>
				{
					// step 1
					table.ColumnsDefinition(columns =>
					{
						columns.ConstantColumn(25);
						columns.RelativeColumn(3);
						columns.RelativeColumn();
						columns.RelativeColumn();
					});

					// step 2
					table.Header(header =>
					{
						header.Cell().Element(CellStyle).Text("Id");
						header.Cell().Element(CellStyle).Text("Delivery number");
						header.Cell().Element(CellStyle).AlignRight().Text("Order date");
						header.Cell().Element(CellStyle).AlignRight().Text("Price");

						static IContainer CellStyle(IContainer container)
						{
							return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
						}
					});

					// step 3
					foreach (var item in Model.Items)
					{
						table.Cell().Element(CellStyle).Text(item.Id);
						table.Cell().Element(CellStyle).Text(item.OrderNumber).FontFamily(Fonts.Arial);
						table.Cell().Element(CellStyle).AlignRight().Text($"{item.OrderDateTime:yyyy-MM-dd HH:mm:ss}");
						table.Cell().Element(CellStyle).AlignRight().Text(item.Products.Sum(x => x.Quantity * x.Price));


						static IContainer CellStyle(IContainer container)
						{
							return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
						}
					}
				});
			}
		}
	}
}
