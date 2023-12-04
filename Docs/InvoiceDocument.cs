using HouseholdOnlineStore.Models.DocumentModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;

namespace HouseholdOnlineStore.Docs
{
	public class InvoiceDocument : IDocument
	{
		public InvoiceModel Model { get; }

		public InvoiceDocument(InvoiceModel model)
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
						column.Item().Text($"Invoice #{Model.InvoiceNumber}").Style(titleStyle);

						column.Item().Text(text =>
						{
							text.Span("Order date: ").SemiBold();
							text.Span($"{Model.OrderDate:d}");
						});

					});

					row.RelativeItem().Column(column =>
					{
						column.Spacing(2);

						column.Item().BorderBottom(1).PaddingBottom(5).Text("Customer credentials").SemiBold();

						column.Item().Text("Name: " + Model.Name);
						column.Item().Text("Surname: " + Model.Surname);
						column.Item().Text("Phone number: " + Model.PhoneNumber);
						column.Item().Text("Address: " + Model.Address);
					});
				});

				
			}

			void ComposeContent(IContainer container)
			{
				container.PaddingVertical(40).Column(column =>
				{
					column.Spacing(5);

					column.Item().Element(ComposeTable);

					var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
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
						columns.RelativeColumn();
					});

					// step 2
					table.Header(header =>
					{
						header.Cell().Element(CellStyle).Text("#");
						header.Cell().Element(CellStyle).Text("Product");
						header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
						header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
						header.Cell().Element(CellStyle).AlignRight().Text("Total");

						static IContainer CellStyle(IContainer container)
						{
							return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
						}
					});

					// step 3
					foreach (var item in Model.Items)
					{
						table.Cell().Element(CellStyle).Text(Model.Items.IndexOf(item) + 1);
						table.Cell().Element(CellStyle).Text(item.Name).FontFamily(Fonts.Arial);
						table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}");
						table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity);
						table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity}");

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
