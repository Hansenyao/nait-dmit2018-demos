<Query Kind="Program">
  <Connection>
    <ID>46397a74-becc-4122-90c5-2c366cf1c703</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Server>DESKTOP-GGJISR9\SQLEXPRESS</Server>
    <Database>OLTP-DMIT2018</Database>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
  <NuGetReference>BYSResults</NuGetReference>
  <Namespace>BYSResults</Namespace>
</Query>

using BYSResults;

void Main()
{
	
}


// You can define other methods, fields, classes and namespaces here
public class CodeBehind(TypedDataContext context)
{
	#region Supporting Members
	// For read-only property
	//public List<string> ErrorDetails { get { return errorDetails;}}
	public List<string> ErrorDetails => errorDetails;

	private readonly CustomerService Service = new CustomerService(context);
	#endregion

	#region CodeBehind Variables

	private List<string> errorDetails = new List<string>();
	private string errorMessage = string.Empty;

	private CustomerEditView customer = new CustomerEditView();
	public List<CustomerEditView> Customers = new List<UserQuery.CustomerEditView>();

	#endregion

	public void AddEditCustomer(CustomerEditView customer)
	{
		errorDetails.Clear();
		
		try
		{
			var result = Service.AddEditCustomer(customer);
			if (result.IsSuccess)
			{
				customer = result.Value;
			}
			else 
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			errorMessage = GetInnerMostException(ex).Message;
		}
	}
}


public class CustomerService
{
	private readonly TypedDataContext _hogWildContext;
	
	
	public CustomerService(TypedDataContext context)
	{
		_hogWildContext = context ?? throw new ArgumentException(nameof(context));
	}
	
	public Result<CustomerEditView> AddEditCustomer(CustomerEditView editCustomer)
	{
		
		var result = new Result<CustomerEditView>();
		
		#region Data Validation
		// rule: customer cannot be null
		if (editCustomer == null)
		{
			result.AddError(new Error("Missing Customer", "No customer was supply"));
			return result;
		}
		
		// rule: FirtName, LastName, Phone number and email are required (not empty)
		if (string.IsNullOrWhiteSpace(editCustomer.FirstName))
		{
			result.AddError(new Error("Missing Information", "First name is required"));
		}
		if (string.IsNullOrWhiteSpace(editCustomer.LastName))
		{
			result.AddError(new Error("Missing Information", "Last name is required"));
		}
		if (string.IsNullOrWhiteSpace(editCustomer.Phone))
		{
			result.AddError(new Error("Missing Information", "Phone number is required"));
		}
		if (string.IsNullOrWhiteSpace(editCustomer.Email))
		{
			result.AddError(new Error("Missing Information", "Email is required"));
		}
		if (result.Errors.Count > 0)
		{
			return result;
		}
		#endregion
		
		#region Business Rules
		// rule: FirtName, LastName and Phone number cannot be duplicated (found more than once)
		if (editCustomer.CustomerID == 0) 
		{
			bool customerExist = _hogWildContext.Customers.Any(x => 
										x.FirstName.ToUpper() == editCustomer.FirstName.ToUpper() &&
										x.LastName.ToUpper() == editCustomer.LastName.ToUpper() &&
										x.Phone.ToUpper() == editCustomer.Phone.ToUpper()
									);
			if (customerExist)
			{
				result.AddError(new Error("Existing Customer Data", 
										  "Customer already exist in the database and cannot be enter again"));
			}
		}
		if (result.IsFailure) 
		{
			return result;
		}
		#endregion
		
		Customer customer = _hogWildContext.Customers
									.Where(x => x.CustomerID == editCustomer.CustomerID)
									.Select(x => x)
									.FirstOrDefault();
		// 
		if (customer == null)
		{
			customer = new Customer();
		}
		customer.FirstName = editCustomer.FirstName;
		customer.LastName = editCustomer.LastName;
		customer.Address1 = editCustomer.Address1;
		customer.Address2 = editCustomer.Address2;
		customer.City = editCustomer.City;
		customer.ProvStateID = editCustomer.ProvStateID;
		customer.CountryID = editCustomer.CountryID;
		customer.PostalCode = editCustomer.PostalCode;
		customer.Email = editCustomer.Email;
		customer.StatusID = editCustomer.StatusID;
		customer.RemoveFromViewFlag = editCustomer.RemoveFromViewFlag;
		
		if (customer.CustomerID == 0) 
		{
			_hogWildContext.Customers.Add(customer);
		}
		else {
			_hogWildContext.Customers.Update(customer);
		}
		
		try 
		{
			_hogWildContext.SaveChanges();
		}
		catch(Exception ex)
		{
			_hogWildContext.ChangeTracker.Clear();
			result.AddError(new Error("Error Saving Changes", 
							GetInnerMostException(ex).Message));
			return result;
		}


		return GetCustomer(customer.CustomerID);
	}
	
	public Result<CustomerEditView> GetCustomer(int customerID)
	{
		return null;
	}
}

// This VM will be used to retrieve information
//	for a particular customer from the DB
//	for the purposes of editing the info
//	for that customer. It will also be 
//	used to collect information for new
//	customers from the UI so that an insert
//	may be performed against to DB.
public class CustomerEditView
{
	public int CustomerID { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Address1 { get; set; }
	public string Address2 { get; set; }
	public string City { get; set; }
	public int ProvStateID { get; set; }
	public int CountryID { get; set; }
	public string PostalCode { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
	public int StatusID { get; set; }
	public bool RemoveFromViewFlag { get; set; }
}


#region Support Method
public static List<string> GetErrorMessages(List<Error> errorList)
{
	List<string> errorMessages = new List<string>();

	foreach (Error error in errorList)
	{
		errorMessages.Add(error.ToString());
	}

	return errorMessages;
}

// This method has been created to extract the root cause of a received
// 		exception. 
public static Exception GetInnerMostException(Exception ex)
{
	// While the current exception has an InnerException
	while (ex.InnerException != null)
	{
		// Make the InnerException the current Exception
		ex = ex.InnerException;
	}

	// When we reach this point we have the InnerMostException.  Return
	//		it to the calling scope.
	return ex;
}

#endregion
