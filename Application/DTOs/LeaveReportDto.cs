using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class LeaveReportDto
{
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int TotalLeaves { get; set; }
    public int AnnualLeaves { get; set; }
    public int SickLeaves { get; set; }
}
