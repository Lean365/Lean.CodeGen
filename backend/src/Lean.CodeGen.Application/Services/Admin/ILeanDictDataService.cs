using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 字典数据服务接口
/// </summary>
public interface ILeanDictDataService
{
  /// <summary>
  /// 创建字典数据
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanCreateDictDataDto input);

  /// <summary>
  /// 更新字典数据
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanUpdateDictDataDto input);

  /// <summary>
  /// 删除字典数据
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除字典数据
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取字典数据信息
  /// </summary>
  Task<LeanApiResult<LeanDictDataDto>> GetAsync(long id);

  /// <summary>
  /// 分页查询字典数据
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanDictDataDto>>> GetPageAsync(LeanQueryDictDataDto input);

  /// <summary>
  /// 修改字典数据状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanChangeDictDataStatusDto input);

  /// <summary>
  /// 根据字典类型编码获取字典数据列表
  /// </summary>
  Task<LeanApiResult<List<LeanDictDataDto>>> GetListByTypeCodeAsync(string typeCode);

  /// <summary>
  /// 导出字典数据
  /// </summary>
  Task<byte[]> ExportAsync(LeanQueryDictDataDto input);

  /// <summary>
  /// 导入字典数据
  /// </summary>
  Task<LeanExcelImportResult<LeanDictDataImportDto>> ImportAsync(byte[] file);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  Task<byte[]> GetImportTemplateAsync();
}