# SeaQRS

A lightweight CQRS library for .NET that provides strongly-typed commands and queries with delegate-based dependency injection.

## Installation

```
dotnet add package SeaQRS
```

## Concepts

SeaQRS has two building blocks:

| Type | Input | Output | Use for |
|---|---|---|---|
| `CommandBase<TRequest>` | `TRequest` | none | operations that change state |
| `QueryBase<TRequest, TResponse>` | `TRequest` | `TResponse` | reads with a parameter |
| `QueryBase<TResponse>` | none | `TResponse` | parameterless reads |

## Usage

### Command

```csharp
public record DeleteUserRequest(Guid UserId);

public class DeleteUserCommand : CommandBase<DeleteUserRequest>
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
services.AddCommandScoped<DeleteUserCommand, DeleteUserRequest>();
services.AddQueryScoped<GetUserQuery, GetUserRequest, UserDto>();
```

Each type has `Transient`, `Scoped`, and `Singleton` convenience methods, plus a lifetime overload if you need something dynamic:

```csharp
services.AddCommand<DeleteUserCommand, DeleteUserRequest>(ServiceLifetime.Scoped);
services.AddQuery<GetUserQuery, GetUserRequest, UserDto>(ServiceLifetime.Scoped);
```

## Injection

Handlers are injected as delegates, keeping your consumers decoupled from the handler implementations.

```csharp
public class UsersController : ControllerBase
{
    private readonly Func<GetUserRequest, Task<UserDto>> _getUser;
    private readonly Func<DeleteUserRequest, Task> _deleteUser;

    public UsersController(
        Func<GetUserRequest, Task<UserDto>> getUser,
        Func<DeleteUserRequest, Task> deleteUser)
    {
        _getUser = getUser;
        _deleteUser = deleteUser;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _getUser(new GetUserRequest(id));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteUser(new DeleteUserRequest(id));
        return NoContent();
    }
}
```

## License

MIT
