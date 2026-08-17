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
| `CommandBase<TRequest, TResponse>` | `TRequest` | `TResponse` | state changes that return a result |
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

### Command with a response

Use this when the caller needs something back from the operation, such as a
generated identifier.

```csharp
public record CreateUserRequest(string Name);

public class CreateUserCommand : CommandBase<CreateUserRequest, Guid>
{
    private readonly IUserRepository _users;

    public CreateUserCommand(IUserRepository users)
    {
        _users = users;
    }

    public override async Task<Guid> Run(CreateUserRequest request)
    {
        return await _users.CreateAsync(request.Name);
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
services.AddCommandScoped<CreateUserCommand, CreateUserRequest, Guid>();
services.AddQueryScoped<GetUserQuery, GetUserRequest, UserDto>();
```

Each type has `Transient`, `Scoped`, and `Singleton` convenience methods, plus a lifetime overload if you need something dynamic:

```csharp
services.AddCommand<DeleteUserCommand, DeleteUserRequest>(ServiceLifetime.Scoped);
services.AddCommand<CreateUserCommand, CreateUserRequest, Guid>(ServiceLifetime.Scoped);
services.AddQuery<GetUserQuery, GetUserRequest, UserDto>(ServiceLifetime.Scoped);
```

## Injection

Handlers are registered and injected by their interface, keeping your consumers
decoupled from the handler implementations.

```csharp
public class UsersController : ControllerBase
{
    private readonly IQuery<GetUserRequest, UserDto> _getUser;
    private readonly ICommand<DeleteUserRequest> _deleteUser;
    private readonly ICommand<CreateUserRequest, Guid> _createUser;

    public UsersController(
        IQuery<GetUserRequest, UserDto> getUser,
        ICommand<DeleteUserRequest> deleteUser,
        ICommand<CreateUserRequest, Guid> createUser)
    {
        _getUser = getUser;
        _deleteUser = deleteUser;
        _createUser = createUser;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _getUser.Run(new GetUserRequest(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        var id = await _createUser.Run(new CreateUserRequest(name));
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteUser.Run(new DeleteUserRequest(id));
        return NoContent();
    }
}
```

Each base class also defines an implicit conversion to the matching `Func<>`
delegate, which is useful when passing a handler to code that expects a plain
function:

```csharp
Func<CreateUserRequest, Task<Guid>> create = new CreateUserCommand(users);
```

## License

MIT
