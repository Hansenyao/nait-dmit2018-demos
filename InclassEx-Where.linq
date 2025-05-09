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

// Question 1: Single Where Clause, All Fields
// Context: "We need to identify all employees hired after January 1, 2022, to ensure they are included in our new training program."
// Question: "How would you filter the Employee table to retrieve these employees?"
Employees
	.Where(e => e.HireDate > new DateOnly(2022,01,01))
	.Dump();

// Question 2: Single Where Clause, All Fields
// Context: "Our inventory team wants to find all products that have been available for sale since July 1, 2019, to ensure they are properly stocked."
// Question: "How would you filter the Product table to retrieve these products?"
Products
	.Where(product => product.AvailableForSaleDate.Value.Date >= new DateTime(2019, 07, 01))
	.Dump();

// Question 3: Multiple Where Clauses
// Context: "To update our customer database, we need to pull the email addresses of all customers with a yearly income between $60,000 & $61,000."
// Question: "How would you filter the Customer table and retrieve only the email addresses of these customers?"
Customers
	.Where(customer => customer.YearlyIncome >= 60_000 && customer.YearlyIncome <= 61_000)
	.Select(customer => customer.EmailAddress)
	.Dump();

// Question 4: Filtering using Contain
// Context: "The marketing department needs a list of all promotions focused on North America to prepare for the upcoming sale."
// Question: "How would you filter the Promotion table to retrieve the promotion information?"
Promotions
	.Where(promotion => promotion.PromotionName.Contains("North America"))
	.Dump();






