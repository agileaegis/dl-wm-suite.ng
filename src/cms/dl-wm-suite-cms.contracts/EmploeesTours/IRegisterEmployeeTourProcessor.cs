using System;
using dl.wm.suite.common.dtos.Vms.EmployeesTours;

namespace dl.wm.suite.cms.contracts.EmploeesTours
{
    public interface IRegisterEmployeeTourProcessor
    {
        EmployeeTourUiModel RegisterEmployeeTour(Guid employeeIdForRegistration, Guid tourIdForRegistration);
    }
}