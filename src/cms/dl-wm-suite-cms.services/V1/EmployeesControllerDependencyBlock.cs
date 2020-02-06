using dl.wm.suite.cms.contracts.Employees;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
    public class EmployeesControllerDependencyBlock : IEmployeesControllerDependencyBlock
    {
        public EmployeesControllerDependencyBlock(ICreateEmployeeProcessor createEmployeeProcessor,
                                                        IInquiryEmployeeProcessor inquiryEmployeeProcessor,
                                                        IUpdateEmployeeProcessor updateEmployeeProcessor,
                                                        IInquiryAllEmployeesProcessor allEmployeeProcessor,
                                                        IDeleteEmployeeProcessor deleteEmployeeProcessor)

        {
            CreateEmployeeProcessor = createEmployeeProcessor;
            InquiryEmployeeProcessor = inquiryEmployeeProcessor;
            UpdateEmployeeProcessor = updateEmployeeProcessor;
            InquiryAllEmployeesProcessor = allEmployeeProcessor;
            DeleteEmployeeProcessor = deleteEmployeeProcessor;
        }

        public ICreateEmployeeProcessor CreateEmployeeProcessor { get; private set; }
        public IInquiryEmployeeProcessor InquiryEmployeeProcessor { get; private set; }
        public IUpdateEmployeeProcessor UpdateEmployeeProcessor { get; private set; }
        public IInquiryAllEmployeesProcessor InquiryAllEmployeesProcessor { get; private set; }
        public IDeleteEmployeeProcessor DeleteEmployeeProcessor { get; private set; }
    }
}