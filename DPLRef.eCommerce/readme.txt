###############################################################################

Don't Panic Labs - Reference Implemenation 

Created on 2016-09-25 by Doug Durham & Chad Michel

Contributors
	Doug Durham
	Chad Michel
	Andy Unterseher
	Branden Barber
	Ceren Kaplan
	Zach Lannin

https://dontpaniclabs.com/education

###############################################################################

Goal
	Solution is to be used as a reference for demoing pattterns and practices
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
enivironment variables thru windows configuration. If you specify environment
variables through windows configuration you will need to restart Visaul Studio
after setting the environment variables.

Environment Variables Database
	Note, if use must specify a eCommerceDatabase environment variable or a 
	eCommerceDatabaseSqlite environment variable. If you do not specify either
	the system will run run. The eCommerceDatabase takes priority over 
	eCommerceDatabaseSqlite. 

eCommerceDatabase:			connection string for SQLExpress / SQLServer
Sample Value:				"Server=.\SqlExpress; Database=DPLRef.eCommerce; Trusted_connection=true"

eCommerceDatabaseSqlite:	connection string for Sqlite
Sample Value:				Data Source=c:\temp\mydb.sqlite

eCommerceQueuePath			local directory path used for async queuing
Sample Value:				c:\temp\eCommerceQueuePath

###############################################################################

Mini Assignment

Want to do something inside of the reference implementation? There is an easy
assignment for you.

1. Get the reference implementation building and running.
2. Make sure all unit tests pass before you start.
