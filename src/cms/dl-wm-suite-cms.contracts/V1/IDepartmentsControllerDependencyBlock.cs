using dl.wm.suite.cms.contracts.Employees.Departments;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface IDepartmentsControllerDependencyBlock
    {
        ICreateDepartmentProcessor CreateDepartmentProcessor { get; }
        IInquiryDepartmentProcessor InquiryDepartmentProcessor { get; }
        IUpdateDepartmentProcessor UpdateDepartmentProcessor { get; }
        IInquiryAllDepartmentsProcessor InquiryAllDepartmentsProcessor { get; }
        IDeleteDepartmentProcessor DeleteDepartmentProcessor { get; }
    }
}