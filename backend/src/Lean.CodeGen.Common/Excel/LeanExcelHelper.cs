using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Lean.CodeGen.Common.Excel;

/// <summary>
/// Excel导入导出帮助类
/// </summary>
public static class LeanExcelHelper
{
  static LeanExcelHelper()
  {
    // 设置EPPlus许可证
    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
  }

  /// <summary>
  /// 导出Excel
  /// </summary>
  /// <typeparam name="T">数据类型</typeparam>
  /// <param name="data">数据列表</param>
  /// <param name="sheetName">工作表名称</param>
  /// <param name="exportFields">导出字段列表，为空则导出所有字段</param>
  /// <returns>Excel文件字节数组</returns>
  public static byte[] Export<T>(List<T> data, string sheetName = "Sheet1", List<string>? exportFields = null)
  {
    using var package = new ExcelPackage();
    var worksheet = package.Workbook.Worksheets.Add(sheetName);

    // 获取要导出的属性
    var properties = typeof(T).GetProperties()
        .Where(p => exportFields == null || exportFields.Contains(p.Name))
        .Where(p => p.GetCustomAttribute<ExcelIgnoreAttribute>() == null)
        .ToList();

    // 写入表头
    for (var i = 0; i < properties.Count; i++)
    {
      var property = properties[i];
      var displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
          ?? property.GetCustomAttribute<DisplayAttribute>()?.Name
          ?? property.Name;

      worksheet.Cells[1, i + 1].Value = displayName;
      worksheet.Cells[1, i + 1].Style.Font.Bold = true;
      worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
      worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
    }

    // 写入数据
    for (var i = 0; i < data.Count; i++)
    {
      var item = data[i];
      for (var j = 0; j < properties.Count; j++)
      {
        var property = properties[j];
        var value = property.GetValue(item);
        worksheet.Cells[i + 2, j + 1].Value = value;
      }
    }

    // 自动调整列宽
    worksheet.Cells.AutoFitColumns();

    return package.GetAsByteArray();
  }

  /// <summary>
  /// 导入Excel
  /// </summary>
  /// <typeparam name="T">数据类型</typeparam>
  /// <param name="fileBytes">Excel文件字节数组</param>
  /// <param name="validateData">数据验证委托</param>
  /// <returns>导入结果</returns>
  public static LeanExcelImportResult<T> Import<T>(byte[] fileBytes, Func<T, (bool IsValid, string? ErrorMessage)>? validateData = null) where T : new()
  {
    var result = new LeanExcelImportResult<T>();

    using var package = new ExcelPackage(new MemoryStream(fileBytes));
    var worksheet = package.Workbook.Worksheets[0];

    // 获取属性映射
    var properties = typeof(T).GetProperties()
        .Where(p => p.GetCustomAttribute<ExcelIgnoreAttribute>() == null)
        .ToDictionary(
            p => p.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                ?? p.GetCustomAttribute<DisplayAttribute>()?.Name
                ?? p.Name,
            p => p);

    // 验证表头
    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
    foreach (var cell in headerRow)
    {
      var header = cell.Text.Trim();
      if (!properties.ContainsKey(header))
      {
        result.ErrorMessage = $"表头 {header} 不存在";
        return result;
      }
    }

    // 读取数据
    for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
    {
      try
      {
        var item = new T();
        var hasValue = false;

        for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
          var header = worksheet.Cells[1, col].Text.Trim();
          var property = properties[header];
          var cell = worksheet.Cells[row, col];

          if (cell.Value != null)
          {
            hasValue = true;
            var value = Convert.ChangeType(cell.Value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            property.SetValue(item, value);
          }
        }

        // 如果该行没有任何数据，则跳过
        if (!hasValue)
        {
          continue;
        }

        // 验证数据
        var validationContext = new ValidationContext(item);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(item, validationContext, validationResults, true))
        {
          result.Errors.Add(new LeanExcelImportError
          {
            RowIndex = row,
            ErrorMessage = string.Join("; ", validationResults.Select(r => r.ErrorMessage))
          });
          continue;
        }

        // 自定义验证
        if (validateData != null)
        {
          var (isValid, errorMessage) = validateData(item);
          if (!isValid)
          {
            result.Errors.Add(new LeanExcelImportError
            {
              RowIndex = row,
              ErrorMessage = errorMessage
            });
            continue;
          }
        }

        result.Data.Add(item);
      }
      catch (Exception ex)
      {
        result.Errors.Add(new LeanExcelImportError
        {
          RowIndex = row,
          ErrorMessage = ex.Message
        });
      }
    }

    return result;
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <typeparam name="T">数据类型</typeparam>
  /// <returns>模板文件字节数组</returns>
  public static byte[] GetImportTemplate<T>()
  {
    using var package = new ExcelPackage();
    var worksheet = package.Workbook.Worksheets.Add("导入模板");

    // 获取属性
    var properties = typeof(T).GetProperties()
        .Where(p => p.GetCustomAttribute<ExcelIgnoreAttribute>() == null)
        .ToList();

    // 写入表头
    for (var i = 0; i < properties.Count; i++)
    {
      var property = properties[i];
      var displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
          ?? property.GetCustomAttribute<DisplayAttribute>()?.Name
          ?? property.Name;
      var required = property.GetCustomAttribute<RequiredAttribute>() != null;
      var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description
          ?? property.GetCustomAttribute<DisplayAttribute>()?.Description;

      worksheet.Cells[1, i + 1].Value = displayName + (required ? " *" : "");
      worksheet.Cells[1, i + 1].Style.Font.Bold = true;
      worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
      worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

      if (!string.IsNullOrEmpty(description))
      {
        worksheet.Cells[1, i + 1].AddComment(description, "说明");
      }
    }

    // 自动调整列宽
    worksheet.Cells.AutoFitColumns();

    return package.GetAsByteArray();
  }
}