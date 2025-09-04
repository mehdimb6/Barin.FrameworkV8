using Stimulsoft.Report;

namespace Barin.Framework.Utilities;

public class ReportHelper
{
    public static ReportHelper Instance => new ReportHelper();

    public void GetPdf(string reportPath, string businessObjectName, object businessObject, string outfilePath)
    {
        using (StiReport report = new StiReport())
        {
            report.Load(reportPath);
            report.RegBusinessObject(businessObjectName, businessObject);
            report.Render();
            report.ExportDocument(StiExportFormat.Pdf, outfilePath);
        }
    }
}
