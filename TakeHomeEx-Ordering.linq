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

// Question 1: Single Where Clause, Ordered by Invoice Date
// Context: "We need to review all invoices created after November 2023 to ensure they were processed correctly."
// Question: "How would you filter the Invoice table to retrieve these invoices, ordered by invoice date?"
Invoices
	.Where(invoice => invoice.DateKey.Date >= new DateTime(2023, 12, 01))
	.OrderBy(invoice => invoice.DateKey)
	.Dump();

// Question 2: Single Where Clause, Ordered by Geography
// Context: "Our regional analysis team needs to focus on all sales territories located in Canada for a new market expansion project."
// Question: "How would you filter the Geography table to retrieve all records where the country is Canada, ordered by geography?"
Geographies
	.Where(geography => geography.RegionCountryName == "Canada")
	.OrderBy(geography => geography.GeographyType)
	.Dump();

// Question 3: Multiple Field Selection
// Context: "After reviewing the previous data output, we noticed records with GeographyType labeled as 'Country/Region.' For our detailed analysis, we only want to focus on cities located in Canada."
// Question: "How would you filter the Geography table to retrieve records where the Type 'City' is ordered by province and then city?"
Geographies
	.Where(geography => geography.GeographyType == "City" && geography.RegionCountryName == "Canada")
	.OrderBy(geography => geography.StateProvinceName)
	.ThenBy(geography => geography.CityName)
	.Dump();

// Question 4: Filtering using Contains, Ordered by Selling Area Size, Employee Count from largest to smallest and finally Store Name
// Context: "There has been some confusion with store names that include the term 'No.' in them, which might indicate store numbers or branches. We need to identify all stores with 'No.' in their names to review their details and address any inconsistencies."
// Question: "How would you filter the Store table to retrieve all records where the StoreName contains 'No.', ordered by selling area size and then employee count descending and then store name?"
Stores
	.Where(store => store.StoreName.Contains("No."))
	.OrderBy(store => store.SellingAreaSize)
	.ThenByDescending(store => store.EmployeeCount)
	.ThenBy(store => store.StoreName)
	.Dump();