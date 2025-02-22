using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lean.CodeGen.WebApi.Filters;

/// <summary>
/// 防止 CSRF 攻击的过滤器
/// </summary>
public class LeanAntiForgeryFilter : IAsyncAuthorizationFilter
{
  private readonly IAntiforgery _antiforgery;

  public LeanAntiForgeryFilter(IAntiforgery antiforgery)
  {
    _antiforgery = antiforgery;
  }

  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    try
    {
      await _antiforgery.ValidateRequestAsync(context.HttpContext);
    }
    catch (AntiforgeryValidationException)
    {
      context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
    }
  }
}