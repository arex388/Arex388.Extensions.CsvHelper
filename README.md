# Arex388.Extensions.CsvHelper

This is a utility project to allow the use of CSV files as a backing store for a pseudo-database experience.

Sometimes you really do not need a database for data storage. Installing, configuring, running, and maintaining the database can be a hassle for simple projects. Take my blog [arex388.com](https://arex388.com) for example. It was using SQL Server 2019 behind the scenes, but it is a simple blog.

I decided to experiment and remove the dependency on SQL Server and Entity Framework Core and started using CSV files. I still needed a way to read and write to those CSVs, so I made this pseudo-DbContext to give me this functionality. It has made managing the blog much easier and I am going to be using it in a couple of other projects that do not need a real database.

Obviously if you are working on a real business project do not use this. Stick to the proper database setup. For simple projects however, this works fine.