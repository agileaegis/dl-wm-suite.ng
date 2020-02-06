using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Common
{
    [Serializable]
    public class RootObjectNotFoundException : Exception
    {
        public RootObjectNotFoundException(string message) : base(message)
        {
        }
    }
}