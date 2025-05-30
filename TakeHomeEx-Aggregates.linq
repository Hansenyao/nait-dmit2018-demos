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

// Aggregates Take-Home exercise

// Take Home 1: Count()
// Context: The sales team at Contoso Corporation is analyzing store performance by region.To better understand the distribution of
// their stores, they need to count how many stores are located in Canada.This information will help them in regional planning and 
// in allocating resources for their Canadian operations.
// Objective: How would you write a query to count the number of stores located in Canada ? 
// The query should filter the Stores table to find those with a RegionCountryName value of 'Canada' and return the total count.
Stores
	.Where(store => store.Geography.RegionCountryName == "Canada")
	.Count()
	.Dump();

// Take Home 2: Count()
// Context: The product management team at Contoso Corporation is preparing a report on the structure of their product catalog. 
// They need to determine how many subcategories exist under each product category. This data will help them in assessing 
// the depth and organization of their product lines and making decisions about future expansions or restructurings.
// Objective: How would you write a query to retrieve each product category and count the number of subcategories within it? 
// The query should return the product category name and the count of its subcategories, ordered by category name.
ProductCategories
	.Select(category => new {
		Category = category.ProductCategoryName,
		SubCategories = category.ProductSubcategories.Count()
	})
	.OrderBy(anon => anon.Category)
	.Dump();
	
// Take Home 3: Sum()
// Context: The accounting department needs to calculate the total cost for each invoice to verify profitability against sales.
// Objective: How would you use LINQ to sum the total TotalCost for all invoice lines related to each invoice using 
// navigation properties in the Invoice table?
Invoices
	.Select(invoice => new {
		InvoiceID = invoice.InvoiceID,
		InvoiceDate = invoice.DateKey.ToShortDateString(),
		TotalCost = invoice.InvoiceLines.Sum(line => line.TotalCost)
	})
	.Dump();


// Take Home 4: Sum()
// Context: The financial analysis team at Contoso Corporation is conducting a study on the income distribution of customers 
// across different cities in Canada. They need to generate a report that shows the total yearly income of all customers 
// residing in each city. This information will be used to tailor financial products and services to the needs of customers 
// in various regions.
// Objective: How would you write a query to retrieve the name of each city in Canada and the total yearly income of 
// all customers living there? The query should filter the Geographies table to include only cities in Canada, 
// calculate the sum of YearlyIncome for each city, and order the results by city name. If there are no customers in a city,
// the total income should be returned as 0.
Geographies
	.Where(g => g.RegionCountryName == "Canada")
	.Select(g => new {
		City = g.CityName,
		TotalIncome = g.Customers.Count() == 0? 
					  0 : g.Customers.Sum(c => c.YearlyIncome)
	})
	.Where(anon => anon.City != null)
	.OrderBy(anon => anon.City)
	.Dump();


// Question 5: Min()
// Context: The finance department at Contoso Corporation is analyzing sales data to identify the smallest invoice 
// amounts recorded at each store.This information is critical for understanding the minimum sales transaction values 
// and for setting benchmarks for future sales targets.
// Objective: How would you write a query to retrieve the smallest invoice amount for each store? The query should return 
// the StoreID, StoreName, and the smallest invoice amount.If no invoices exist for a store, the result should display '0' 
// for the amount. The results should be ordered alphabetically by the store name.
Stores
	.Select(store => new {
		StoreID = store.StoreID,
		Name = store.StoreName,
		SmallInvoiceAmont = store.Invoices.Count() == 0 ? 
							0 : store.Invoices.Min(invoice => invoice.TotalAmount)
	})
	.OrderBy(anon => anon.Name)
	.Dump();


// Question 6: Min()
// Context: The sales analysis team at Contoso Corporation is evaluating product performance by examining the minimum
// sales quantities recorded for each product. This analysis will help identify products with low sales volume, 
// which may require strategic adjustments such as promotions or discontinuation.
// Objective: How would you write a query to retrieve the minimum sales quantity for each product? 
// The query should return the ProductName and the minimum SalesQuantity. If no sales records exist for a product, 
// the result should display '0' for the quantity.
Products
	.Select(product => new {
		ProductName = product.ProductName,
		SalesQuantity = product.InvoiceLines.Count() == 0 ?
						0 : product.InvoiceLines.Min(line => line.SalesQuantity)
	})
	.Dump();


// Question 7: Max()
// Context: The marketing team at Contoso Corporation is analyzing the effectiveness of various promotions. 
// They want to identify which promotions have led to the highest invoice return amounts. 
// This information is crucial for assessing the impact of promotions on customer returns and making informed decisions 
// about future campaigns.
// Objective: How would you write a query to retrieve the maximum return amount associated with each promotion? 
// The query should return the PromotionID, PromotionName, and the MaxReturnAmount for each promotion.
Promotions
	.Select(p => new {
		PromotionID = p.PromotionID,
		PromotionName = p.PromotionName,
		MaxReturnAmount = p.InvoiceLines.Max(line => line.ReturnAmount)
	})
	.Dump();


// Question 8: Max()
// Context: The inventory management team at Contoso Corporation is reviewing product stock longevity to identify 
// which products have remained in inventory for the most extended periods. Understanding the maximum number of days 
// each product has been in stock will help the team make decisions regarding inventory turnover and product lifecycle management.
// Objective: How would you write a query to retrieve the maximum number of days a product has been in stock? 
// The query should return the Name, ProductNo, and the MaxDays each product has been in stock, with a default
// value of 'N/A' if no inventory data is available. The product name should order the results.
Products
	.Select(p => new {
		Name = p.ProductName,
		ProductNo = p.ProductLabel,
		MaxDays = p.Inventories.Count() == 0 ?
				  "N/A" : p.Inventories.Max(inventory => inventory.MaxDayInStock).ToString()
	})
	.OrderBy(anon => anon.Name)
	.Dump();


// Question 9: Average()
// Context: The inventory management team at Contoso Corporation is analyzing their products to ensure that safety stock levels, 
// on-order quantities, and on-hand quantities are being appropriately maintained. They want to evaluate the average quantities 
// for each product across all stores to identify any discrepancies or potential issues in inventory management. 
// This data will help them optimize inventory levels and prevent stockouts or overstock situations.
// Objective: How would you write a query to calculate each product's average safety stock quantity, average on-order quantity, 
// and average on-hand quantity? The query should return the Name of the product, AverageSafetyStockQuantity, AverageOnOrder, 
// and AverageOnHand, ordered by product name. Only include products where all three averages are greater than zero.
Products
	.Where(p => p.Inventories.Count() > 0)
	.Select(p => new {
		Name = p.ProductName,
		AverageSafetyStockQuantity = (int)p.Inventories.Average(inventory => inventory.SafetyStockQuantity),
		AverageOnOrder = (int)p.Inventories.Average(inventory => inventory.OnOrderQuantity),
		AverageOnHand = (int)p.Inventories.Average(inventory => inventory.OnHandQuantity)
	})
	.OrderBy(anon => anon.Name)
	.Dump();


// Question 10: Average()
// Context: The sales analysis team at Contoso Corporation is working to identify their most valuable customers based on 
// their purchasing habits. To do this, they need to calculate the average purchase amount for each customer. 
// This will help the team target high-value customers for special promotions and personalized marketing efforts. 
// The analysis should focus only on customers who have made at least one purchase.
// Objective: How would you write a query to calculate the average purchase amount for each customer? 
// The query should return the CustomerID, Name, and AveragePurchases for customers who have an average purchase 
// amount greater than zero, ordered by customer name.
Customers
	.Where(c => c.Invoices.Count() > 0)
	.Select(c => new {
		CustomerID = c.CustomerID,
		Name = c.FirstName + " " + c.LastName,
		AveragePurchases = c.Invoices.Average(invoice => invoice.TotalAmount)
	})
	.Where(anon => anon.AveragePurchases > 0)
	.OrderBy(anon => anon.Name)
	.Dump();











