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
// Context: "The marketing department is planning a special promotion for Secretary Week to show appreciation to customers who work in clerical roles. They want to target single customers with less than $51,000 annually. To personalize the promotion, they need a list of these customers, including their full names, yearly income, and the number of children they have. The results should be ordered alphabetically by the customers' last names."
// Question: "How would you filter the Customers table to retrieve the clerical staff who are single and earn less than $51,000, and return the results as an anonymous data set that includes their full name, yearly income, and number of children, ordered by last name?"
Customers
	.Where(customer => customer.Occupation == "Clerical" &&
					   customer.MaritalStatus == "S" &&
					   customer.YearlyIncome < (decimal)51_000)
	.OrderBy(customer => customer.LastName)
	.Select(customer => new {
		Name = customer.FirstName + " " + customer.LastName,
		Income = customer.YearlyIncome,
		Childrens = customer.TotalChildren
	})
	.Dump();


// Question 2: Where Clause with OrderBy and Anonymous Data Set
// Context: "As part of their Secretary Week campaign, the marketing department wants to understand where their clerical customers are to tailor regional promotions. They specifically target single customers who work in clerical roles and earn less than $51,000 annually. The marketing team must gather customer data, including their full name, yearly income, number of children, city, and country. The results should be ordered first by country and then by city."
// Question: "How would you filter the Customers table to retrieve clerical staff who are single and earn less than $51,000, and return the results as an anonymous data set that includes their full name, yearly income, number of children, city, and country, ordered by country and then by city?"
Customers
	.Where(customer => customer.Occupation.ToLower().Equals("clerical") &&
					   customer.MaritalStatus.ToLower().Equals("s") &&
					   customer.YearlyIncome < (decimal)51_000)
	.Select(customer => new
	{
		Name = customer.FirstName + " " + customer.LastName,
		Income = customer.YearlyIncome,
		Childrens = customer.TotalChildren,
		City = customer.Geography.CityName,
		Country = customer.Geography.RegionCountryName
	})
	.OrderBy(anon => anon.Country)
	.ThenBy(anon => anon.City)
	.Dump();



// Question 3: Where Clause with OrderBy and Anonymous Data Set
// Context: "The retail chain is concerned about the high number of product returns that occurred in February 2023. They want to analyze all return transactions where more than six items were returned. The company is particularly interested in understanding which products were involved, which stores processed these returns, and which customers were making these returns. The results should be organized by the date of the original transaction and further sorted by the invoice ID."
// Question: "How would you filter the InvoiceLines table to retrieve data on returns where more than six items were returned in February 2023, and return the results as an anonymous data set that includes the invoice date, customer name, invoice ID, store name, product ID, product name, quantity returned, and total return amount, ordered by invoice date and then by invoice ID?"
InvoiceLines
	.Where(line => line.ReturnQuantity > 6 &&
				   line.Invoice.DateKey.Year == 2023 &&
				   line.Invoice.DateKey.Month == 2)
	.OrderBy(line => line.Invoice.DateKey)
	.ThenBy(line => line.InvoiceID)
	.Select(line => new {
		InvoiceDate = line.Invoice.DateKey.ToShortDateString(),
		Customer = line.Invoice.Customer.FirstName + " " + line.Invoice.Customer.LastName,
		InvoiceID = line.InvoiceID,
		Store = line.Invoice.Store.StoreName,
		ProductID = line.Product.ProductID,
		Name = line.Product.ProductName,
		Qty = line.ReturnQuantity,
		TotalReturn = line.ReturnAmount
	})
	// !!!Crash in next line!!!
	//.OrderBy(anon => anon.InvoiceDate)
	//.ThenBy(anon => anon.InvoiceID)
	.Dump();









