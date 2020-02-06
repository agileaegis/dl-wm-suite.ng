using dl.wm.suite.cms.contracts.Employees.Departments;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
    public class DepartmentsControllerDependencyBlock : IDepartmentsControllerDependencyBlock
    {
        public DepartmentsControllerDependencyBlock(ICreateDepartmentProcessor createDepartmentProcessor,
                                                        IInquiryDepartmentProcessor inquiryDepartmentProcessor,
                                                        IUpdateDepartmentProcessor updateDepartmentProcessor,
                                                        IInquiryAllDepartmentsProcessor allDepartmentProcessor,
                                                        IDeleteDepartmentProcessor deleteDepartmentProcessor)

        {
            CreateDepartmentProcessor = createDepartmentProcessor;
            InquiryDepartmentProcessor = inquiryDepartmentProcessor;
            UpdateDepartmentProcessor = updateDepartmentProcessor;
            InquiryAllDepartmentsProcessor = allDepartmentProcessor;
            DeleteDepartmentProcessor = deleteDepartmentProcessor;
        }

        public ICreateDepartmentProcessor CreateDepartmentProcessor { get; private set; }
        public IInquiryDepartmentProcessor InquiryDepartmentProcessor { get; private set; }
        public IUpdateDepartmentProcessor UpdateDepartmentProcessor { get; private set; }
        public IInquiryAllDepartmentsProcessor InquiryAllDepartmentsProcessor { get; private set; }
        public IDeleteDepartmentProcessor DeleteDepartmentProcessor { get; private set; }
    }
}