<Query Kind="Program">
  <Connection>
    <ID>8dca33ac-f255-4e47-9dc1-c625af5e0aae</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Database>Contoso</Database>
    <Server>DESKTOP-GGJISR9\SQLEXPRESS</Server>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

// For database Contoso

void Main()
{
	CodeBehind_GetProductCategories();
	
	CodeBehind_GetInvoicesWithDetails("Torres").Dump();
}


// Question 1: Nested Queries
// Context: The product management team at Contoso Corporation is preparing for an upcoming product catalog restructuring.
// To ensure consistency and accuracy, the team needs to organize and review all product categories and their associated 
// subcategories using strongly typed lists.This approach will allow them to manage the data with greater type safety and 
// clarity in their codebase.

// The team needs a hierarchical list where each product category is represented by a strongly typed object (ProductCategorySummaryView), 
// and each associated subcategory is represented by another strongly typed object (ProductSubcategorySummaryView). The results should be 
// ordered alphabetically by both category and subcategory names, making the data easy to navigate and utilize in their planning processes.

// Objective: "How would you create a strongly typed query that retrieves all product categories and their associated subcategories as 
// strongly typed lists? The query should return a list of ProductCategorySummaryView & ProductSubcategorySummaryView objects, where each 
// category includes its name and a list of subcategories. Each subcategory should include its name and description. The results should be 
// ordered first by product category name and then by subcategory name."
public List<ProductCategorySummaryView> CodeBehind_GetProductCategories()
{
	// TODO: Any business rules
	
	return System_GetProductCategories();
}

public List<ProductCategorySummaryView> System_GetProductCategories()
{
	return ProductCategories
				.OrderBy(category => category.ProductCategoryName)
				.Select(category => new ProductCategorySummaryView {
					ProductCategoryName = category.ProductCategoryName,
					SubCategories = category.ProductSubcategories
													.OrderBy(sub => sub.ProductSubcategoryName)
													.Select(sub => new ProductSubcategorySummaryView {
														SubCategoryName = sub.ProductSubcategoryName,
														Description = sub.ProductSubcategoryDescription
													})
													.ToList()
											
				})
				.ToList();
}

public class ProductCategorySummaryView
{
	public string ProductCategoryName { get; set; }
	public List<ProductSubcategorySummaryView> SubCategories { get; set; }
}

public class ProductSubcategorySummaryView
{
	public string SubCategoryName { get; set; }
	public string Description { get; set; }
}


// Question 2: Nested Queries
// Context: "The customer service department at Contoso Corporation is frequently tasked with generating detailed invoice reports 
// for customers. These reports are essential for resolving billing inquiries and ensuring accurate customer service. A typical 
// request involves retrieving all invoices for a customer with a specific last name. Each report must include the main invoice 
// details and a line-by-line breakdown of the products involved, quantities, prices, and any applicable discounts. The customer 
// service team needs this information to be presented in a strongly typed format for consistency and ease of use in various systems."

// Objective: "Create a method that retrieves a detailed view of all invoices for a customer based on a specified last name. 
// The method should return a strongly typed list of InvoiceView objects, where each InvoiceView contains the invoice number, 
// invoice date, customer name, total amount, and a strongly typed list of InvoiceLineView objects. Each InvoiceLineView should 
// represent a line item on the invoice, including the product name, quantity, price, discount, and extended price. The invoice 
// lines should be ordered by their line reference."

public List<InvoiceView> CodeBehind_GetInvoicesWithDetails(string lastName)
{
	// TODO: Add any business rules here!!!
	
	return System_GetInvoicesWithDetails(lastName);
}


public List<InvoiceView> System_GetInvoicesWithDetails(string lastName)
{
	return Invoices.OrderBy(invoice => invoice.InvoiceID)
				   .Where(invoice => invoice.Customer.LastName == lastName)
				   .Select(invoice => new InvoiceView {
				   		InvocieNo = invoice.InvoiceID,
						InvoiceDate = invoice.DateKey.ToShortDateString(),
						Customer = invoice.Customer.FirstName + " " + invoice.Customer.LastName,
						Amount = invoice.TotalAmount,
						Details = invoice.InvoiceLines
												.OrderBy(line => line.InvoiceLineID)
												.Select(line => new InvoiceLineView {
													LineReference = line.InvoiceLineID,
													ProductName = line.Product.ProductName,
													Qty = line.SalesQuantity - line.ReturnQuantity,
													//Price = line.UnitPrice ?? 0,
													Price = line.UnitPrice == null ? 0 : 
															(line.SalesQuantity - line.ReturnQuantity) < 0 ? 
															(decimal)-line.UnitPrice : (decimal)line.UnitPrice,
													//Discount = line.DiscountAmount ?? 0,
													Discount = line.DiscountAmount == null ? 0 :
															(line.SalesQuantity - line.ReturnQuantity) < 0 ?
															(decimal)-line.DiscountAmount : (decimal)line.DiscountAmount,
													ExtPrice = (line.UnitPrice + line.DiscountAmount) * line.SalesQuantity == null ? 0 :
															(decimal)(line.SalesAmount - line.ReturnAmount)
													
												})
												.ToList()
				   })
				   .ToList();
}

public class InvoiceView 
{
	public int InvocieNo { get; set; }
	public string InvoiceDate { get; set; }
	public string Customer { get; set; }
	public decimal Amount { get; set; }
	public List<InvoiceLineView> Details { get; set; }
}

public class InvoiceLineView
{
	public int LineReference { get; set; }
	public string ProductName { get; set; }
	public int Qty { get; set; }
	public decimal Price { get; set; }
	public decimal Discount { get; set; }
	public decimal ExtPrice { get; set; }
}

