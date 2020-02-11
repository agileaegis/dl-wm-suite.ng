using System;
using System.Collections.Generic;
using System.Text;

namespace dl.wm.suite.common.infrastructure.Exceptions.Repositories.Containers
{
  public class MultipleContainersForAnIdException : Exception
  {
    private Guid _containerId;

    public MultipleContainersForAnIdException(Guid id)
    {
      this._containerId = id;
    }

    public override string Message => $"Multiple Containers found for: {_containerId}";
  }
}
