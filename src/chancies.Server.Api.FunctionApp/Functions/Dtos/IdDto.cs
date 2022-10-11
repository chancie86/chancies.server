using System;

namespace chancies.Server.Api.FunctionApp.Functions.Dtos
{
    public class IdDto
    {
        public IdDto(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
