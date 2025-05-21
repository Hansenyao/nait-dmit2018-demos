<Query Kind="Program">
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

void Main()
{
	GetEmployeeReview("al", 30).Dump();

	CodeBehind_GetProductsByCategory("Music").Dump();
}

// You can define other methods, fields, classes and namespaces here


// Question 1: Strongly Typed Queries
// Context: "The HR department at Contoso Corporation is conducting a customized salary review, allowing them to target specific groups of employees based on both their last name and their hourly wage. This approach helps HR quickly identify employees who might be underpaid. For this task, they need to generate a list of employees whose last names match a given search term and whose base rate is below a specified threshold. The HR team will provide these parameters to ensure flexibility in their search. The list should include the employee's full name, department, and an income category indicating whether their salary requires a review. The results should be ordered alphabetically by last name."
// Objective: "Create a method that retrieves employee records based on a search for last names and a base rate threshold. The method should take two parameters—lastName and baseRate—and return a strongly typed list of EmployeeView objects, containing the employee's full name, department, and income category, ordered by last name."
public List<EmployeeView> GetEmployeeReview(String lastName, decimal baseRate) 
{
	List<EmployeeView> employees =  Employees
										.Where(employee => employee.LastName.Contains(lastName))
										.OrderBy(employee => employee.LastName)
										.Select(employee => new EmployeeView {
										 	FullName = employee.FirstName + " " + employee.LastName,
											Department = employee.DepartmentName,
											IncomeCategory = employee.BaseRate < baseRate ? "Required Review" : "No Required Review"
										})
										.ToList();
	return employees;

}

public class EmployeeView
{
	public string FullName { get; set; }
	public string Department { get; set; }
	public string IncomeCategory { get; set; }
}

public List<ProductColorProcessView> CodeBehind_GetProductsByCategory(String categoryName) 
{
	// TODO: 
	// 1. Data validation
	// 2. Business rules
	// 3. Others checking
	return System_GetProductsByCategory(categoryName);	
}

// Question 2: Strongly Typed Queries
// Context: "The production team at Contoso Corporation needs to review their product line to determine which products require additional color processing. The team is particularly interested in products within specific categories, which they will specify as needed. For this task, the production team will provide a category name to search for products within that category. They need to identify whether each product's color requires additional processing, with black and white colors not needing further processing. The results should be organized by the product's style name to facilitate the review process."
// Objective: "Create a method that retrieves product records based on a category name search. The method should take a categoryName parameter and return a strongly typed list of ProductColorProcessView objects, containing the product name, color, and whether additional color processing is needed, ordered by the product's style name."
public List<ProductColorProcessView> System_GetProductsByCategory(String categoryName)
{
	List<ProductColorProcessView> products = Products
												.Where(product => product.ProductSubcategory.ProductCategory.ProductCategoryName.Contains(categoryName))
												.OrderBy(product => product.StyleName)
												.Select(product => new ProductColorProcessView
												{
													Name = product.ProductName,
													Color = product.ColorName,
													ColorProcessNeeded = (product.ColorName == "Black" || product.ColorName == "White") ? "No" : "Yes"
												})
												.ToList();
	return products;
}

public class ProductColorProcessView
{
	public String Name { get; set; }
	public String Color { get; set; }
	public String ColorProcessNeeded { get; set; }
}


