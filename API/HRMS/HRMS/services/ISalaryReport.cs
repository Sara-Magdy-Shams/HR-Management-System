using HRMS.DataTypes;

namespace HRMS.services
{
    public interface ISalaryReport
    {
        public Task<List<Invoice>> Report(int month, int year);
        public Task <SalaryDetails> SingleEmployeeReport(int month, int year, string empId);

    }
}
