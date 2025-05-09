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

// For Database Contoso.bacpac

// Question 1: Single Where Clause, Ordered by Last name
// Context: "We need to identify all employees hired after January 1, 2022, to ensure they are included in our new training program."
// Question: "How would you filter the Employee table to retrieve these employees, ordered by last name?"
Employees
	.Where(e => e.HireDate > new DateOnly(2022, 01, 01))
	.OrderBy(e => e.LastName)
	.Dump();
	
	
// Question 2: Single Where Clause, Ordered by Product Label Descending
// Context: "Our inventory team wants to find all products that have been available for sale since July 1, 2019, to ensure they are properly stocked."
// Question: "How would you filter the Product table to retrieve these products, ordered by product label descending?"
Products
	.Where(product => product.AvailableForSaleDate.Value.Date > new DateTime(2019,07,01))
	.OrderByDescending(product => product.ProductLabel)
	.Dump();

// Question 3: Multiple Where Clauses, Ordered by Email Address
// Context: "To update our customer database, we need to pull the email addresses of all customers with a yearly income between $60,000 and $61,000." NOTE: Order by must follow the where clause but before the select.
// Question: "How would you filter the Customer table and retrieve only the email addresses of these customers, ordered by email address?"
Customers
	.Where(customer => customer.YearlyIncome >= 60_000 && customer.YearlyIncome <= 61_000)
	.OrderBy(customer => customer.EmailAddress)
	.Select(customer => customer.EmailAddress)
	.Dump();

// Question 4: Filtering using Contains, Ordered by Promotion Name and Start Date
// Context: "The marketing department needs a list of all promotions focused on North America to prepare for the upcoming sale."
// Question: "How would you filter the Promotion table to retrieve the promotion information, ordered by promotion name?"
Promotions
	.Where(promotion => promotion.PromotionName.Contains("North America"))
	.OrderBy(promotion => promotion.PromotionName)
	.ThenByDescending(promotion => promotion.PromotionLabel)
	.Dump();
	
	




