namespace Lean.CodeGen.Domain.Interfaces.Entities;

/// <summary>
/// 软删除接口
/// </summary>
public interface ILeanSoftDelete
{
  /// <summary>
  /// 是否已删除
  /// </summary>
  int IsDeleted { get; set; }
}