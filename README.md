# SeaQRS

A lightweight CQRS library for .NET that provides strongly-typed commands and queries with delegate-based dependency injection.

## Installation

```
dotnet add package SeaQRS
```

## Concepts

SeaQRS has three building blocks:

| Type | Input | Output | Use for |
|---|---|---|---|
| `CommandBase<TResponse>` | none | `TResponse` | operations that return a value |
| `VoidCommandBase<TRequest>` | `TRequest` | none | fire-and-forget operations |
| `QueryBase<TRequest, TResponse>` | `TRequest` | `TResponse` | reads with a parameter |
| `QueryBase<TResponse>` | none | `TResponse` | parameterless reads |

## Usage

### Command

```csharp
public record CreateOrderResponse(Guid OrderId);

public class CreateOrderCommand : CommandBase<CreateOrderResponse>
{
    private readonly IOrderRepository _orders;

    public CreateOrderCommand(IOrderRepository orders)
    {
        _orders = orders;
    }

    public override async Task<CreateOrderResponse> Run()
    {
        var id = await _orders.CreateAsync();
        return new CreateOrderResponse(id);
    }
}
```

### Void Command

```csharp
public record DeleteUserRequest(Guid UserId);

public class DeleteUserCommand : VoidCommandBase<DeleteUserRequest>
{
    private readonly IUserRepository _users;

    public DeleteUserCommand(IUserRepository users)
    {
        _users = users;
    }

    public override async Task Run(DeleteUserRequest request)
    {
        await _users.DeleteAsync(request.UserId);
    }
}
```

### Query

```csharp
public record GetUserRequest(Guid UserId);
public record UserDto(Guid Id, string Name);

public class GetUserQuery : QueryBase<GetUserRequest, UserDto>
{
    private readonly IUserRepository _users;

    public GetUserQuery(IUserRepository users)
    {
        _users = users;
    }

    public override async Task<UserDto> Run(GetUserRequest request)
    {
        return await _users.GetAsync(request.UserId);
    }
}
```

## Registration

Register your handlers in `Program.cs` using the extension methods on `IServiceCollection`. You control the lifetime of each handler individually.

```csharp
services.AddCommandTransient<CreateOrderCommand, CreateOrderResponse>();
services.AddVoidCommandScoped<DeleteUserCommand, DeleteUserRequest>();
services.AddQueryScoped<GetUserQuery, GetUserRequest, UserDto>();
```

Each type has `Transient`, `Scoped`, and `Singleton` convenience methods, plus a lifetime overload if you need something dynamic:

```csharp
services.AddCommand<CreateOrderCommand, CreateOrderResponse>(ServiceLifetime.Transient);
services.AddVoidCommand<DeleteUserCommand, DeleteUserRequest>(ServiceLifetime.Scoped);
services.AddQuery<GetUserQuery, GetUserRequest, UserDto>(ServiceLifetime.Scoped);
```

## Injection

Handlers are injected as delegates, keeping your consumers decoupled from the handler implementations.

```csharp
public class OrdersController : ControllerBase
{
    private readonly Func<Task<CreateOrderResponse>> _createOrder;
    private readonly Func<GetUserRequest, Task<UserDto>> _getUser;
    private readonly Func<DeleteUserRequest, Task> _deleteUser;

    public OrdersController(
        Func<Task<CreateOrderResponse>> createOrder,
        Func<GetUserRequest, Task<UserDto>> getUser,
        Func<DeleteUserRequest, Task> deleteUser)
    {
        _createOrder = createOrder;
        _getUser = getUser;
        _deleteUser = deleteUser;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var result = await _createOrder();
        return Ok(result);
    }
}
```

## License

MIT
