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
// Context: "The HR department at Contoso Corporation is in the middle of their annual salary review process. This year, they are focusing on employees who might be earning below industry standards, particularly those with a base rate of less than $30 per hour. The department aims to ensure fair compensation and recognize areas where pay adjustments may be necessary. To start the review, HR wants to compile a list of employees who might require a salary review. The list should include each employee's full name and department, and categorize them based on whether they require a review or not. The results should be sorted alphabetically by last name to make it easier for the HR team to navigate through the data."
// Question: "How would you filter the Employees table to retrieve those with a base rate of less than $30, and return the results as an anonymous data set that includes their full name, department, and whether they require a salary review, ordered by last name?"

Employees
	.OrderBy(employee => employee.LastName)
	.Select(employee => new {
		Fullname = employee.FirstName + " " + employee.LastName,
		Department = employee.DepartmentName,
		IncomeCategory = employee.BaseRate < 30 ? "Requirde Review" : "No Review Required"
	})
	.Dump();

// Question 2: Ternary Operator
// Context: "The product development team at Contoso Corporation is preparing to optimize the production process for their 'Music, Movies, and Audio Books' category. As part of this effort, they need to identify which products in this category require additional color processing. Products in black or white are typically ready for packaging without further processing, while other colors may need additional steps. To streamline their workflow, the team wants to compile a list of all products in this category, sorted by their style name. This list should include the product name, color, and an indicator of whether additional color processing is required."
// Question: "How would you filter the Products table to retrieve items in the 'Music, Movies and Audio Books' category, and return the results as an anonymous data set that includes the product name, color, and whether color processing is needed, ordered by style name?"
Products
	.Where(product => product.ProductSubcategory.ProductCategory.ProductCategoryName == "Music, Movies and Audio Books")
	.OrderBy(product => product.StyleName)
	.Select(product => new {
		ProductName = product.ProductName,
		Color = product.ColorName,
		ColorProcessNeeded = (product.ColorName == "Black" || product.ColorName == "White") ? "No" : "Yes"
	})
	.Dump();



