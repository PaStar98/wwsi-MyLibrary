using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyLibrary.Api.Data.Common;

public abstract class BaseModel : IEquatable<BaseModel>
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public bool Equals(BaseModel? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((BaseModel)obj);
    }
    
    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(BaseModel? left, BaseModel? right) => Equals(left, right);
    public static bool operator !=(BaseModel? left, BaseModel? right) => !Equals(left, right);
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever();
        }
    }
}