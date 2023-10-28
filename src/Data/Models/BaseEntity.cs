using System;

namespace YallaMasar.Data.Models;

public abstract class BaseLangEntity : BaseEntity<int>
{
    public string Language { get; set; }
}

public abstract class BaseEntity : BaseEntity<Guid>
{
}

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreateBy { get; set; }

    public DateTimeOffset ModifiedOn { get; set; }

    public string ModifiedBy { get; set; }

    public DateTimeOffset? DeleteDate { get; set; }
}