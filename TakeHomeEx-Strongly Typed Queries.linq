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
	GetInventorySummary(5, "Cell").Dump();
	
	GetInventorySummary(2, 10000).Dump();
}

// You can define other methods, fields, classes and namespaces here


// Question 1: Strongly Typed Queries
// Context: "The inventory management team at Contoso Corporation is conducting a detailed review of specific stores to ensure that inventory levels are adequately maintained for high-demand product categories. They need to focus on individual stores and specific product categories to determine whether stock levels are sufficient or if reordering is necessary. For this task, the team will specify the store ID and a product category name as search criteria. The report should list the store details, product names, whether a reorder is needed, and the category name. The results should be ordered by the store ID."
// Objective: "Create a method that retrieves inventory records based on both a specified storeID and a categoryName. The method should return a strongly typed list of InventorySummaryView objects, containing the store ID, store name, product name, whether a reorder is necessary, and the category name, ordered by store ID."
public List<InventorySummaryView> GetInventorySummary(Int32 storeID, String categoryName)
{
	List<InventorySummaryView> inventories = Inventories
												.Where(inventory => inventory.StoreID == storeID &&
																	inventory.Product.ProductSubcategory.ProductCategory.ProductCategoryName.Contains(categoryName))
												.Select(inventory => new InventorySummaryView
												{
													StoreID = inventory.StoreID,
													StoreName = inventory.Store.StoreName,
													ProductName = inventory.Product.ProductName,
													Reorder = (inventory.OnHandQuantity + inventory.OnOrderQuantity < inventory.SafetyStockQuantity) ? "Yes" : "No",
													CategoryName = inventory.Product.ProductSubcategory.ProductCategory.ProductCategoryName
												})
												.OrderBy(anon => anon.StoreID)
												.ToList();
	return inventories;
}

public class InventorySummaryView
{
	public Int32 StoreID { get; set; }
	public String StoreName { get; set; }
	public String ProductName { get; set; }
	public String Reorder { get; set; }
	public String CategoryName { get; set; }
}

// Question 2: Strongly Typed Queries
// Context: "The finance department at Contoso Corporation is conducting a review of high-priority invoices for specific stores. They are focusing on invoices where the total amount exceeds a certain threshold, as these invoices may require special attention. The finance team will specify a store ID to narrow down the search to a particular store and set a total amount threshold to identify high-priority invoices. The report should include invoice details, customer names, store information, and whether each invoice is categorized as high or low priority based on the total amount. The results should be ordered by the customer's last name."
// Objective: "Create a method that retrieves invoice records based on both a specified storeID and a totalAmount threshold. The method should return a strongly typed list of CustomerCollectionView objects, containing the invoice number, invoice date, total amount, customer name, store name, manager's city, and priority status, ordered by the customer's last name." NOTE: We have added a new field called "Amount"
public List<CustomerCollectionView> GetInventorySummary(Int32 storeID, decimal totalAmount)
{
	List<CustomerCollectionView> invoices = Invoices
												.Where(invoice => invoice.StoreID == storeID)
												.OrderBy(invoice => invoice.Customer.LastName)
												.Select(invoice => new CustomerCollectionView
												{
													InvoiceID = invoice.InvoiceID,
													InvoiceDate = invoice.DateKey.ToShortDateString(),
													Amount = invoice.TotalAmount,
													Name = invoice.Customer.FirstName + " " + invoice.Customer.LastName,
													StoreName = invoice.Store.StoreName,
													Manager = invoice.Store.Geography.CityName,
													Priority = invoice.TotalAmount < totalAmount ? "Low Priority" : "High Priority"
												})
												.ToList();
	return invoices;
}


public class CustomerCollectionView
{
	public Int32 InvoiceID { get; set; }
	public String InvoiceDate { get; set; }
	public Decimal Amount { get; set; }
	public String Name { get; set; }
	public String StoreName { get; set; }
	public String Manager { get; set; }
	public String Priority { get; set; }
}