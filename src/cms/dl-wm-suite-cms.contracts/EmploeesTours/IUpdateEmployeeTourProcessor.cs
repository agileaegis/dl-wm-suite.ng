using System;
using dl.wm.suite.common.dtos.Vms.EmployeesTours;

namespace dl.wm.suite.cms.contracts.EmploeesTours
{
    public interface IUpdateEmployeeTourProcessor
    {
        EmployeeTourUiModel UpdateEmployeeTour(Guid idEmployeeTour, EmployeeTourUiModel updatedEmployeeTour);
    }
}