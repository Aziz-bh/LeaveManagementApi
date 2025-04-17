.NET 8 Web API for managing employee leave requests.

***Features***
Filter Leave Requests: By employee, type, status, and date range.

Pagination & Sorting: Easily navigate through data.

Summary Reports: Annual, sick leaves per employee.

Admin Approval: Approve pending leave requests.

Business Rules:

No overlapping leaves.

Max 20 annual leave days per year.

Sick leave requires a reason.

***Setup Instructions***
Clone the Repository:

git clone https://github.com/yourusername/leave-management-api.git
cd leave-management-api

Build the Project:
dotnet build

Run the Application:
dotnet run --project Presentation
Access the API: Navigate to http://localhost:7036/swagger .