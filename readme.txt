###############################################################################

Don't Panic Labs - Reference Implementation 

Created on 2016-09-25 by Doug Durham & Chad Michel

Contributors
	Doug Durham
	Chad Michel
	Andy Unterseher
	Branden Barber
	Ceren Kaplan
	Zach Lannin
	Jesse Temple
	
https://dontpaniclabs.com/education

###############################################################################

Goal
	Solution is to be used as a reference for demoing patterns and practices
	used by us at Don't Panic Labs.

.NET CORE Version
	This version uses .NET Core 2, instead of the original .NET Framework used
	by the first version we created.

SQL Server or Sqlite
	This version supports two data stores, both SQL Server and Sqlite.


###############################################################################

Configuration - Handled through environment variables.

Visual Studio makes it easy to specify the environment variables per project.
You can do this on the property page for the project. You can also specify 
environment variables through windows configuration. If you specify environment
variables through windows configuration you will need to restart Visual Studio
after setting the environment variables.

Environment Variables Database
	Note, you can configure the database using the eCommerceDatabase environment 
	variable or a the eCommerceDatabaseSqlite environment variable. If you do 
	not specify either the system will run run. The eCommerceDatabase takes 
	priority over eCommerceDatabaseSqlite. 

Mac Notes
	To set your environment variable on a Mac, you need to update your 
	.bash_profile file. You will need to add an export line like below.
	export eCommerceDatabaseSqlite="Data Source=~/ecommerce.sqlite"

eCommerceDatabase:			connection string for SQLExpress / SQLServer
Sample Value:				"Server=.\SqlExpress; Database=DPLRef.eCommerce; Trusted_connection=true"

eCommerceDatabaseSqlite:	connection string for Sqlite
Sample Value:				Data Source=c:\temp\mydb.sqlite

eCommerceQueuePath			local directory path used for async queuing
Sample Value:				c:\temp\eCommerceQueuePath

###############################################################################

Mini Assignment - If you want to dive in...

Want to do something inside of the reference implementation? There is an easy
assignment to get you started.

1. 	Get the reference implementation building and running.
2. 	Make sure all unit tests pass before you start.
3. 	In the 3rdParty folder there is a USATaxer library project that contains 
	a very simple tax calculation library. Build this project. That will create
	a USATaxer.dll.
4.	Copy that dll to the root of the eCommerce solution. Add a reference to it
	from the Accessors project.
5. 	Create a a new accessor SalesTaxRuleAccessor (ISalesTaxRuleAccessor)
6. 	Update the AccessorFactory to support the SalesTaxRuleAccessor service.
7. 	Write some unit tests for the new SalesTaxRuleAccessor service.
8. 	Edit the TaxCalculationEngine to use the new SalsTaxRuleAccessor service.
9. 	Some existing unit tests will break because sales tax values will change.
	Update those unit tests.
10. Profit :)

###############################################################################

Key Concepts

###############################################################################
