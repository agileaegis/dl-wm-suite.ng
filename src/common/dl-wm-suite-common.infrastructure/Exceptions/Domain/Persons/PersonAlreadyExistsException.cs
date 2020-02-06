using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Persons
{
    public class PersonAlreadyExistsException : Exception
    {
        public string Email { get; }
        public string BrokenRules { get; }

        public PersonAlreadyExistsException(string email, string brokenRules)
        {
            Email = email;
            BrokenRules = brokenRules;
        }

        public override string Message => $" Person with email:{Email} already Exists!\n Additional info:{BrokenRules}";
    }
}
