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
// Context: The sales analytics team at Contoso Corporation is conducting a regional analysis to understand the financial 
// demographics of their customers.They aim to compare the average yearly income and customer distribution across different 
// countries or regions.This analysis will help the team identify key markets and tailor their sales strategies accordingly.
// The team needs a report that groups customers by their country or region, calculates the average yearly income for each group, 
// and counts the number of customers in each region.

// Objective: "How would you write a query to group customers by their country or region and calculate the average yearly income 
// and customer count for each group? The query should return the RegionCountryName, AverageYearlyIncome, and CustomerCount, 
// ordered by the country or region name."
Customers
	.GroupBy(customer => customer.Geography.RegionCountryName)
	.Select(group => new {
		RegionCountryName = group.Key,
		AverageYearlyIncome = group.Average(customer => customer.YearlyIncome),
		CustomerCount = group.Count()
	})
	.OrderBy(anon => anon.RegionCountryName)
	.Dump();


// Question 2: Group By (Complex)
// Context: The wealth management division at Contoso Corporation is targeting high-income customers for an exclusive premium service. 
// They need to identify customers who earn more than $199,000 per year and analyze their distribution across different countries 
// or regions. The division wants a detailed report that not only provides the average yearly income and customer count per region 
// but also lists the individual high-income customers in each region. This data will enable the team to tailor their marketing 
// strategies and offer personalized services to these high-net-worth individuals.

// Objective: "How would you write a query to filter customers who earn more than $199,000, group them by their country or region, 
// and calculate the average yearly income and customer count for each group? The query should also return a list of individual 
// customers with their CustomerID, FirstName, LastName, and YearlyIncome within each region, ordered by the region name."
Customers
	.Where(customer => customer.YearlyIncome > 199_000)
	.GroupBy(customer => customer.Geography.RegionCountryName)
	.Select(group => new
	{
		RegionCountryName = group.Key,
		AverageYearlyIncome = group.Average(customer => customer.YearlyIncome),
		CustomerCount = group.Count(),
		Customers = group.Select(customer => new {
						CustomerID = customer.CustomerID,
						FirstName = customer.FirstName,
						LastName = customer.LastName,
						YearlyIncome = customer.YearlyIncome
					})
					.ToList()
	})
	.OrderBy(anon => anon.RegionCountryName)
	.Dump();



