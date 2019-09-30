# Report gas consumption daily
#### Donalam Project
## Description
The project is about reporting each day at a set hour and to a set list of peaople via mail (customizable), 
the consumption of gas for the furnaces. I done this automatically by reading index of gas 
each day from the PLC, save it to SQL server and report the consumption of gas via mail.

# Features
* Reading signal from PLC
* Save to SQL Server
* Send mail automatically
* JavaScript - Use AJAX Calls
* JavaSCript - Pass parameters via JSON from server to view
* Asp.Net Core App

# Nuget Sources
* - S7netplus - to write to Siemens Plc