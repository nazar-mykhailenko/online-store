using HouseholdOnlineStore.Models.DocumentModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;

namespace HouseholdOnlineStore.Docs
{
	public class ProductsReportDocument : IDocument
	{
		public ProductsReportModel Model { get; }

		public ProductsReportDocument(ProductsReportModel model)
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
				var titleStyle = TextStyle.Default.FontSize(20).SemiBold();

				container.Row(row =>
				{
					row.RelativeItem().Column(column =>
					{
						column.Item().AlignCenter().Text($"Report on  {Model.ReportDate}").Style(titleStyle);

					});
				});

				
			}

			void ComposeContent(IContainer container)
			{
				container.PaddingVertical(40).Column(column =>
				{
					column.Spacing(5);

					column.Item().Element(ComposeTable);

				});
			}

			void ComposeTable(IContainer container)
			{
				container.Table(table =>
				{
					// step 1
					table.ColumnsDefinition(columns =>
					{
						columns.RelativeColumn(3);
						columns.RelativeColumn();
						columns.RelativeColumn();
						columns.RelativeColumn();
					});

					// step 2
					table.Header(header =>
					{
						header.Cell().Element(CellStyle).Text("Product");
						header.Cell().Element(CellStyle).AlignRight().Text("Manufacturer");
						header.Cell().Element(CellStyle).AlignRight().Text("Price");
						header.Cell().Element(CellStyle).AlignRight().Text("Left");

						static IContainer CellStyle(IContainer container)
						{
							return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
						}
					});

					// step 3
					foreach (var item in Model.Items)
					{
						table.Cell().Element(CellStyle).Text(item.Name).FontFamily(Fonts.Arial);
						table.Cell().Element(CellStyle).Text(item.Manufacturer).FontFamily(Fonts.Arial);
						table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}");
						table.Cell().Element(CellStyle).AlignRight().Text(item.Left);

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
