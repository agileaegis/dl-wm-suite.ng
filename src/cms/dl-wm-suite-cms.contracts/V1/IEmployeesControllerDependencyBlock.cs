using dl.wm.suite.cms.contracts.Employees;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface IEmployeesControllerDependencyBlock
    {
        ICreateEmployeeProcessor CreateEmployeeProcessor { get; }
        IInquiryEmployeeProcessor InquiryEmployeeProcessor { get; }
        IUpdateEmployeeProcessor UpdateEmployeeProcessor { get; }
        IInquiryAllEmployeesProcessor InquiryAllEmployeesProcessor { get; }
        IDeleteEmployeeProcessor DeleteEmployeeProcessor { get; }
    }
}