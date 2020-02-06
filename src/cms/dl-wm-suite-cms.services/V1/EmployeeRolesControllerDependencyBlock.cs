using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
    public class EmployeeRolesControllerDependencyBlock : IEmployeeRolesControllerDependencyBlock
    {
        public EmployeeRolesControllerDependencyBlock(ICreateEmployeeRoleProcessor createEmployeeRoleProcessor,
                                                        IInquiryEmployeeRoleProcessor inquiryEmployeeRoleProcessor,
                                                        IUpdateEmployeeRoleProcessor updateEmployeeRoleProcessor,
                                                        IInquiryAllEmployeeRolesProcessor allEmployeeRoleProcessor,
                                                        IDeleteEmployeeRoleProcessor deleteEmployeeRoleProcessor)

        {
            CreateEmployeeRoleProcessor = createEmployeeRoleProcessor;
            InquiryEmployeeRoleProcessor = inquiryEmployeeRoleProcessor;
            UpdateEmployeeRoleProcessor = updateEmployeeRoleProcessor;
            InquiryAllEmployeeRolesProcessor = allEmployeeRoleProcessor;
            DeleteEmployeeRoleProcessor = deleteEmployeeRoleProcessor;
        }

        public ICreateEmployeeRoleProcessor CreateEmployeeRoleProcessor { get; private set; }
        public IInquiryEmployeeRoleProcessor InquiryEmployeeRoleProcessor { get; private set; }
        public IUpdateEmployeeRoleProcessor UpdateEmployeeRoleProcessor { get; private set; }
        public IInquiryAllEmployeeRolesProcessor InquiryAllEmployeeRolesProcessor { get; private set; }
        public IDeleteEmployeeRoleProcessor DeleteEmployeeRoleProcessor { get; private set; }
    }
}