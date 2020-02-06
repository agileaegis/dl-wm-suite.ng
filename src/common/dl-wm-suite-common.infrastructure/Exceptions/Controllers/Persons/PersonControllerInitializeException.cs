using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Controllers.Persons
{
    public class PersonControllerInitializeException : Exception
    {
        public PersonControllerInitializeException()
        {
        }

        public override string Message => $" Customer Controller initialization failed!";
    }
}
