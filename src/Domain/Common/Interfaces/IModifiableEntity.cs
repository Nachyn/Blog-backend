using System;

namespace Domain.Common.Interfaces
{
    public interface IModifiableEntity
    {
        DateTime? LastModifiedUtc { get; set; }
    }
}