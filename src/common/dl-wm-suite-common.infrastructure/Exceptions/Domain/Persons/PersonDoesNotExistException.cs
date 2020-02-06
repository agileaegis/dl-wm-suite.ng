using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Persons
{
    public class PersonDoesNotExistException : Exception
    {
        public Guid PersonId { get; }

        public PersonDoesNotExistException(Guid personId)
        {
            personId = personId;
        }

        public override string Message => $"Person with Id: {PersonId}  doesn't exists!";
    }
}
