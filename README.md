# Cqrs wit Event Sourcing

As event store / snapshoting => NEventStore (in my case I'm using SQL Server as the DB)

As state store => SQL Server

---------------------------------------------------------------------------------------

How to use it?

Just change the connection strings in the BankAccount.Client project (web.config). 

Build the solution (enable restoring of the nuget packages in visual studio).

That's it!
