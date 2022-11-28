using CorgiShop.Pipeline.Abstractions;
using MediatR;
using System.Text.Json.Serialization;

namespace CorgiShop.Pipeline.Base.Requests;

public enum DeleteMode
{
    Hard,
    Soft
}

public record DeleteCommand<TDto> : IRequest 
    where TDto : class, IDtoEntity
{
    public int Id { get; set; }

    [JsonIgnore]
    public DeleteMode Mode { get; set; } = DeleteMode.Soft;

    public DeleteCommand(int id)
    {
        Id = id;
    }


}
