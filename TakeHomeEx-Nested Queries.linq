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
	CodeBehind_GetManagerAndSubordinates().Dump();
	
	CodeBehind_GetInventoryOnHandByCity("Seattle", 200).Dump();
}

// Question 1: 
// Context: The human resources department at Contoso Corporation is working on a comprehensive organizational structure review.
// The goal is to map out the reporting hierarchy within the company, focusing on managers and their direct subordinates.
// This review will help the HR team better understand team dynamics, identify gaps in management, and ensure that all employees 
// are properly aligned with their roles and responsibilities.

// To achieve this, the HR team needs to create a detailed report that lists each manager alongside their respective subordinates.
// For each manager, the report should include their first name, last name, and job title, as well as a list of their direct 
// reports (subordinates). Each subordinate’s entry should include their full name and job title. The report should be organized 
// alphabetically by the manager's last name, and within each manager’s section, the subordinates should also be listed alphabetically by last name.

// Objective: "How would you create a strongly typed query that retrieves all managers and their direct subordinates as strongly 
// typed lists? The query should return a list of ManagerView objects, where each manager includes their first name, last name, 
// position, and a list of SubordinateView objects. Each SubordinateView should include the subordinate’s full name and job title.
// The results should be ordered alphabetically by the manager's last name, and the subordinates should also be ordered alphabetically
// by last name within each manager’s list."
	
public List<ManagerView> CodeBehind_GetManagerAndSubordinates()
{
	// TODO: Add any business rules here!!!
	
	return System_GetManagerAndSubordinates();
}

public List<ManagerView> System_GetManagerAndSubordinates()
{
	return Employees
				.Where(manager => manager.Title.Contains("Manager"))
				.OrderBy(manager => manager.LastName)
				.Select(manager => new ManagerView {
					FirstName = manager.FirstName,
					LastName = manager.LastName,
					Position = manager.Title,
					//Employees = Employees
					//				.Where(employee => employee.ParentEmployeeID == manager.EmployeeID)
					//				.OrderBy(employee => employee.LastName)
					//				.Select(employee => new SubordinateView {
					//					FullName = employee.FirstName + " " + employee.LastName,
					//					Title = employee.Title
					//				})
					//				.ToList()
					Employees = manager.Children
									.OrderBy(sub => sub.LastName)
									.Select(sub => new SubordinateView
									{
										FullName = sub.FirstName + " " + sub.LastName,
										Title = sub.Title
									})
									.ToList()
				})
				.ToList();
}

public class ManagerView
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Position { get; set; }
	public List<SubordinateView> Employees { get; set; }
}

public class SubordinateView
{
	public string FullName { get; set; }
	public string Title { get; set; }
};



// Question 2: 
// Context: The inventory management team at Contoso Corporation is conducting a detailed analysis of product availability 
// across their stores, with a specific focus on the city of Seattle. The team needs to identify stores within Seattle that 
// have significant stock levels, specifically for products with an on-hand quantity of 200 units or more. To ensure the accuracy 
// and effectiveness of their analysis, they require the data to be organized using strongly typed lists. This approach will help 
// them manage the inventory data with greater type safety and clarity, making it easier to strategize for inventory distribution, 
// marketing efforts, and potential reordering needs.

// The team needs a hierarchical list where each store in Seattle is represented by a strongly typed object (StoreSummaryView), 
// and each associated inventory item within the store is represented by another strongly typed object (InventoryView). The results 
// should be ordered alphabetically by product name within each store, ensuring the data is easy to review and analyze.

// Objective: "How would you create a strongly typed query that retrieves all stores located in Seattle and lists their inventory 
// for products with an on-hand quantity of 200 or more as strongly typed lists? The query should return a list of StoreSummaryView 
// objects, where each store includes its name, city, and a list of InventoryView objects. Each InventoryView should include the 
// product name, price, cost, margin, and on-hand quantity. The results should be ordered alphabetically by product name within
// each store."
public List<StoreSummaryView> CodeBehind_GetInventoryOnHandByCity(string city, int onHandQty)
{
	// TODO: Add any business rules here!!!
	
	return System_GetInventoryOnHandByCity(city, onHandQty);
}

public List<StoreSummaryView> System_GetInventoryOnHandByCity(string city, int onHandQty)
{
	return Stores
				.Where(store => store.Geography.CityName == city)
				.Select(store => new StoreSummaryView {
					StoreName = store.StoreName,
					City = store.Geography.CityName,
					Inventory = store.Inventories
											.Where(inventory => inventory.OnHandQuantity >= onHandQty)
											.OrderBy(inventory => inventory.Product.ProductName)
											.Select(inventory => new InventoryView {
												Name = inventory.Product.ProductName,
												Price = inventory.Product.UnitPrice ?? 0,
												Cost = inventory.UnitCost,
												Margin = inventory.Product.UnitPrice - inventory.Product.UnitCost ?? 0,
												OnHand = inventory.OnHandQuantity
											})
											.ToList()
				})
				.ToList();
}

public class StoreSummaryView
{
	public string StoreName { get; set; }
	public string City { get; set; }
	public List<InventoryView> Inventory { get; set; }
}

public class InventoryView 
{
	public string Name { get; set; }
	public decimal Price { get; set; }
	public decimal Cost { get; set; }
	public decimal Margin { get; set; }
	public int OnHand { get; set; }
}



