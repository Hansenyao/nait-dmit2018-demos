<Query Kind="Statements">
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

// Question 1: Ternary Operator
// Context: "The inventory management team at Contoso Corporation is focusing on the 'Cell phones' category to ensure that stores maintain adequate stock levels. They need to identify which stores are at risk of running out of inventory and may require a reorder. The team wants a report that lists each store, the specific cell phone products they carry, and whether a reorder is necessary. The reorder is required if the sum of the current on-hand quantity and on-order quantity is less than the safety stock quantity. This report should be organized by store ID to help the team prioritize which locations need immediate attention."
// Question: "How would you filter the Inventories table to retrieve records for products in the 'Cell phones' category, and return the results as an anonymous data set that includes the store ID, store name, product name, and whether a reorder is necessary, ordered by store ID, then by ProductName?"
Inventories
	.Where(inventory => inventory.Product.ProductSubcategory.ProductCategory.ProductCategoryName == "Cell phones")
	.Select(inventory => new {
		StoreID = inventory.StoreID,
		StoreName = inventory.Store.StoreName,
		ProductName = inventory.Product.ProductName,
		Reorder = (inventory.OnHandQuantity + inventory.OnOrderQuantity < inventory.SafetyStockQuantity) ? "Yes" : "No"
	})
	.OrderBy(anon => anon.StoreID)
	.ThenBy(anon => anon.ProductName)
	.Dump();

// Question 2: Ternary Operator
// Context: "The finance department at Contoso Corporation is conducting an analysis of customer invoices to prioritize collection efforts. They are particularly interested in identifying high-value invoices—those with amounts exceeding $5,000—as these should be flagged as 'High Priority' for immediate follow-up. The department wants a report that lists all invoices, sorted alphabetically by the customer's last name, to ensure easy navigation. The report should include the invoice number, date, customer name, store name, the manager's location (represented by the city), and whether the invoice is considered high or low priority based on the total amount."
// Question: "How would you filter the Invoices table to retrieve all invoices, and return the results as an anonymous data set that includes the invoice number, date, customer name, store name, manager's city, and priority status, ordered by the customer's last name?"
Invoices
	.OrderBy(invoice => invoice.Customer.LastName)
	.ThenBy(invoice => invoice.DateKey)
	.Select(invoice => new {
		InvoiceNo = invoice.InvoiceID,
		InvoiceDate = invoice.DateKey.ToShortDateString(),
		Name = invoice.Customer.FirstName + " " + invoice.Customer.LastName,
		StoreName = invoice.Store.StoreName,
		Manager = invoice.Store.Geography.CityName,
		Priority = invoice.TotalAmount < 5_000 ? "Low Priority" : "High Priority"
	})
	.Dump();


