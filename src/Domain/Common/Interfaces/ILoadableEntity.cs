using System;

namespace Domain.Common.Interfaces
{
    public interface ILoadableEntity
    {
        DateTime LoadedUtc { get; set; }
    }
}