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

// Question 1: Group By (Simple)
// Context: The product management team at Contoso Corporation is working on a project to better organize their product catalog.
// They need to categorize all product subcategories under their respective categories to streamline their catalog structure, 
// making it easier for customers to find and purchase products. The team requires a comprehensive list that groups product 
// subcategories by their parent category and orders both categories and subcategories alphabetically.
// Objective: "How would you write a query to group product subcategories by their parent category? 
// The query should return the CategoryName and a list of ProductSubcategories that includes the SubCategoryName. 
// Each subcategory should be ordered alphabetically by category name and subcategory name."
ProductSubcategories
	.GroupBy(sub => new {sub.ProductCategory.ProductCategoryName})
	.Select(group => new {
		CategoryName = group.Key,
		ProductSubCategories = group.Select(sub => new {
			SubCategoryName = sub.ProductSubcategoryName
		})
		.OrderBy(anon => anon.SubCategoryName)
		.ToList()
	})
	.Dump();



// Question 2: Group By (Complex)
// Context: The product management team at Contoso Corporation is analyzing sales data to understand which products contribute 
// most to each category and subcategory.By grouping invoice lines by product category and subcategory, the team aims to identify 
// trends and patterns in product sales, which will help them make informed decisions about future product development and inventory 
// management.
// Objective: "How would you write a query to group invoice lines by product category and subcategory? 
// The query should return the CategoryName, SubcategoryName, and a list of invoices that include the InvoiceID, Product, and Amount.
// For each product, order by category name, subcategory name, and finally by product name."
InvoiceLines
	.GroupBy(line => new { line.Product.ProductSubcategory.ProductCategory.ProductCategoryName,
					       line.Product.ProductSubcategory.ProductSubcategoryName
					 	})
	.Select(group => new {
		CategoryName = group.Key.ProductCategoryName,
		SubCategoryName = group.Key.ProductSubcategoryName,
		Invoices = group.Select(invoice => new {
			InvoiceID = invoice.InvoiceID,
			Product = invoice.Product.ProductName,
			Amount = invoice.SalesQuantity-invoice.ReturnQuantity
		})
		.OrderBy(anon => anon.Product)
		.ToList()
	})
	.OrderBy(anon => anon.CategoryName)
	.ThenBy(anon => anon.SubCategoryName)
	.Dump();




