using MediatR;
using Together.Shared.ValueObjects;

namespace Together.Shared.Mediator;

public interface IBaseRequest : IRequest<BaseResponse>;

public interface IBaseRequest<TResponse> : IRequest<BaseResponse<TResponse>>;