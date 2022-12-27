# HotDesk

## Project description:
Project aims to create RESTfull API using ASP.NET web API. API should enable making actions related to the automatic booking of desks in the office via the online system. The project has the following requirements:
Administration:
- Manage locations (add/remove, can't remove if desk exists in location)
- Manage desk in locations (add/remove if no reservation/make unavailable)
Employees
- Determine which desks are available to book or unavailable.
- Filter desks based on location
- Book a desk for the day.
- Allow reserving a desk for multiple days but now more than a week.
- Allow to change desk, but not later than the 24h before reservation.
- Administrators can see who reserves a desk in location, where Employees can see only that specific desk is unavailable.

## Data base diagram:
![UmlDiagram](https://user-images.githubusercontent.com/66009631/209720614-4ec1c51b-3e91-4f8d-ba04-da8cda89a223.png)

## Most important libraries used in project
- AutoMapper.Extensions.Microsoft.DependendencyInjection v12.0.0
- FluentValidation.AspNetCore v11.2.2
- Microsoft.AspNetCore.Authentication.JwtBearer v6.0.1
- Microsoft.EntityFrameworkCore v7.0.1
- Microsoft.EntityFrameworkCore.SqlServer v7.0.1
- Microsoft.EntityFrameworkCore.Tools v7.0.1
- Moq v4.18.3
- xunit v2.4.2
- xunit.runner.visualstudio v2.4.3
- AutoFixture v4.17.0
- FluentAssertions v6.8.0

## Additionals information
- Project is using seeder to fill database with initial data
- Project is using Microsoft SQL Server database


