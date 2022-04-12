# Help

To scaffold context from existing db:

```
dotnet ef dbcontext scaffold "Server=.;Database=StackOverflow2010;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models
```