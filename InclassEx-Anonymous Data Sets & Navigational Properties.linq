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

// Question 1: Where Clause with OrderBy and Anonymous Data Set
// Context: "The pricing team analyzes low-cost items and must identify all products priced below $10. They want to see the product label, name, and unit price for these items, ordered alphabetically by price and product name."
// Question: "How would you filter the Product table to retrieve products with a unit price less than $10, order them by price and product name in ascending order, and return the results as an anonymous data set that includes the product label, product name, and unit price?"

Products
	.Where(product => product.UnitPrice < 10)
	.Select(product => new  {
		product.ProductLabel,
		product.ProductName,
		product.UnitPrice
	})
	.OrderBy(anon => anon.UnitPrice)
	.ThenBy(anon => anon.ProductName)
	.Dump();
	

// Question 2: Where Clause with Anonymous Data Set and Navigational Property
// Context: "Our marketing team needs to identify customers who live in British Columbia, Canada, to target them for a new regional promotion. We want to include their first name, last name, and city name in the order of the results by city then by last name."
// Question: "How would you filter the Customer table to retrieve customers in British Columbia, Canada, and include their first name, last name, and associated city name ordered by city then by last name (from the Geography table) as an anonymous data set?"
Customers
	.Where(customer => customer.Geography.StateProvinceName.Equals("British Columbia") &&
					   customer.Geography.RegionCountryName.Equals("Canada"))
	.Select(customer => new {
		FirstName = customer.FirstName,
		LastName = customer.LastName,
		City = customer.Geography.CityName
	})
	.OrderBy(anon => anon.City)
	.ThenBy(anon => anon.LastName)
	.Dump();


// Question 3: Navigational Properties & Anonymous Data Set
// Context: "The product management team focuses on audio-related products and wants to analyze those that fall under the 'Audio' category. Specifically, they want products within the 'Recording Pen' or 'Bluetooth Headphones' subcategory that are 'Pink.' The list should include the category name, subcategory name, and product name."
// Question: "How would you filter the Products table to retrieve products that are 'Pink' and belong to the 'Audio' category and any of the subcategories 'Recording Pen' or 'Bluetooth Headphones' and return the results as an anonymous data set with the product name, subcategory name, and category name?"
Products
	.Where(product => product.ColorName.Equals("Pink") &&
					  product.ProductSubcategory.ProductCategory.ProductCategoryName.Equals("Audio") &&
					  (product.ProductSubcategory.ProductSubcategoryName.Equals("Recording Pen") ||
					   product.ProductSubcategory.ProductSubcategoryName.Equals("Bluetooth Headphones")))
	.Select(product => new {
		CategoryName = product.ProductSubcategory.ProductCategory.ProductCategoryName,
		SubCategoryName = product.ProductSubcategory.ProductSubcategoryName,
		ProductName = product.ProductName
	})
	.OrderBy(anon => anon.SubCategoryName)
	.ThenBy(anon => anon.ProductName)
	.Dump();


// Question 4: Navigational Properties & Anonymous Data Set
// Context: "The sales department is analyzing invoices from European customers. They need a list of all invoices for European customers, along with the invoice number, date, customer name, city, and country. The results should be ordered alphabetically by city."
// Question: "How would you filter the Invoices table to retrieve invoices for customers located in Europe and return the results as an anonymous data set that includes the invoice number, invoice date, customer name, city, and country, ordered by city?"
Invoices
	.Where(invoice => invoice.Customer.Geography.ContinentName.Equals("Europe"))
	.Select(invoice => new {
		InvoiceNo = invoice.InvoiceID,
		InvoiceDate = invoice.DateKey,
		Customername = invoice.Customer.FirstName + "," + invoice.Customer.LastName,
		City = invoice.Customer.Geography.CityName,
		Country = invoice.Customer.Geography.RegionCountryName
	})
	.OrderBy(anon => anon.City)
	.Dump();









