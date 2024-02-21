# Employee Hourly Salary API 💸

This project provides hourly salaries as an API by looking at the entry and exit hours of the employees. It is designed with the awareness that the hourly wage of each employee and unit is different. Employee and date-based queries can be made with employeeId, startDate, endDate parameters.

## Technologies Used

**Languages:** C#, SQL

**Frameworks:** .Net

**Environments:** MSSQL, Visual Studio

**Testing:** Postman

## API Usage

#### GET Item

```http
  GET /api/EHSA/employeesalary?{employeeId}&{startDate}&{endDate}
```

| Parameter | Type | Description                      |
| :-------- | :------- | :-------------------------------- |
| `employeeId`      | `int` |  Employee ID **(Necessary)**|
| `startDate`      | `date` | Work Start Time **(Necessary)** |
| `endDate`      | `date` | Work End Time **(Necessary)**|

+ #### employeesalary(employeeId, startDate, endDate)
Employee Id, start and end date are sent to the *stored procedure*. The number of hours worked in the selected interval is calculated. The salary is obtained by multiplying the calculated total hours by the hourly working wage of the employee. All this information is transferred as API.

If `employeeId=0`, hourly salaries of **all employees** are given.

#### Example:

```http
  GET /api/EHSA/employeesalary?employeeId=1208&startDate=2024-01-01&endDate=2024-01-31
```

```json
[{
    "EmployeeId": 1208,
    "EmployeeName": "Yigit Kalay",
    "DepartmentName": "Software Development",
    "HourlySalary": 36.48,
    "TotalWorkHours": 46,
    "TotalSalary": 1678.08
}]
```

## Run On Your Computer

Here is what you need to do to run this project on your own computer:
+ Download the project to your computer by cloning it. (git clone https://github.com/yigitkalay/EmployeeHourlySalaryAPI)
+ Enter the ProjectEHSA folder and create your ConnectionString.txt file in the folder as shown.
+ Create SQL tables and stored procedures in your database as shown.
+ Everything is done. Now all you have to do is run the project and sit back 🥳

## ConnectionString.txt File

ConnectionString.txt file is created in order to store the connection string safely and easy to use. The database path is written in a single line.

#### Example:
```c#
Server=myServerAddress; Database=myDataBase; User Id=myUsername; Password=myPassword;
```

## SQL Tables And Stored Procedures

Database operations are done by creating these 4 tables and 1 stored procedure. 

#### *Employee* Table
```sql
CREATE TABLE [dbo].[tb_EHSA_Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Surname] [nvarchar](50) NULL,
	[Age] [int] NULL,
	[WorkStartDate] [date] NULL,
	[WorkEndDate] [date] NULL,
 CONSTRAINT [PK_tb_EHSA_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
```

#### *Departmants* Table
```sql
CREATE TABLE [dbo].[tb_EHSA_Departments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nvarchar](50) NULL,
 CONSTRAINT [PK_tb_EHSA_Departments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
```


#### *Employee Work Time* Table
```sql
CREATE TABLE [dbo].[tb_EHSA_EmployeeWorkTime](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[WorkStartTime] [datetime] NULL,
	[WorkEndTime] [datetime] NULL,
 CONSTRAINT [PK_tb_EHSA_EmployeeWorkTime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
```


#### *Hourly Salary* Table
```sql
CREATE TABLE [dbo].[tb_EHSA_HourlySalaryRate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[DepartmentId] [int] NULL,
	[HourlySalary] [float] NULL,
 CONSTRAINT [PK_tb_EHSA_HourlySalaryRate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
```

#### *Employee Salary* Stored Procedure
```sql
CREATE PROCEDURE [dbo].[sp_EHSA_EmployeeSalary]
    @EmployeeId INT,
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SELECT 
	E.Id AS EmployeeId,
    E.Name +' '+ E.Surname AS EmployeeName,
    D.DepartmentName,
    HS.HourlySalary,
    SUM(DATEDIFF(HOUR, EWT.WorkStartTime, EWT.WorkEndTime)) AS TotalWorkHours,
    SUM(DATEDIFF(HOUR, EWT.WorkStartTime, EWT.WorkEndTime)) * HS.HourlySalary AS TotalSalary
FROM 
    tb_EHSA_Employee AS E
INNER JOIN 
    tb_EHSA_EmployeeWorkTime AS EWT ON E.Id = EWT.EmployeeId
INNER JOIN 
    tb_EHSA_HourlySalaryRate AS HS ON E.Id = HS.EmployeeId
INNER JOIN 
    tb_EHSA_Departments AS D ON HS.DepartmentId = D.Id

WHERE 
    (@EmployeeId = 0 OR E.Id = @EmployeeId)
    AND EWT.WorkStartTime >= @StartDate
    AND EWT.WorkEndTime <= @EndDate
GROUP BY 
    E.Id, E.Name, E.Surname , D.DepartmentName, HS.HourlySalary
order by
	TotalSalary desc
END

```

## Lessons Learned

In this project, I learnt that working with the stored procedure is more effective and if any change is desired in the calculation, it will be sufficient and faster to edit the stored procedure instead of editing and re-publishing the codes.

## Feedback

If you have any feedback, please open an issue or contact me on linkedin. Good coding 👨🏻‍💻
