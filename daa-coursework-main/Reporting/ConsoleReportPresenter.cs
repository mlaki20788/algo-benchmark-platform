using SearchBenchmark.Core.Abstractions;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Reporting;

public sealed class ConsoleReportPresenter : IReportPresenter
{
    public void RenderReport(BenchmarkReport report)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("##################################################################################################");
        Console.WriteLine($"               ALGORITHMIC BENCHMARK & COMPLEXITY SUITE | MAHDI LAKI");
        Console.WriteLine($"               Workload Scale: {report.DatasetSize:N0} Keys (Int32 Array)");
        Console.WriteLine("##################################################################################################");
        Console.ResetColor();

        // 1. اعتبارسنجی هم‌ارزی نتایج
        Console.WriteLine("\n[*] EQUIVALENCE VERIFICATION CHECK");
        Console.Write("    Outcome: ");
        if (report.Verification.IsEquivalent)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"SUCCESS (Identical results over {report.Verification.TotalQueries:N0} queries)");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"FAILED - Detected {report.Verification.Mismatches} divergent answers!");
        }
        Console.ResetColor();

        // 2. ماتریس مقایسه زمان و حافظه
        Console.WriteLine("\n[*] BENCHMARK METRICS (TIME & MEMORY)");
        Console.WriteLine("+---------------------------+-------------------+-------------------+-------------------+----------------------+");
        Console.WriteLine("| Target Algorithm          | Latency (ns)      | Rate (ops/sec)    | Duration (ms)     | Memory Allocation    |");
        Console.WriteLine("+---------------------------+-------------------+-------------------+-------------------+----------------------+");

        foreach (var audit in report.AlgorithmReports)
        {
            Console.WriteLine($"| {audit.AlgorithmName,-25} | {audit.Performance.NanosecondsPerOp,17:N2} | {audit.Performance.OperationsPerSecond,17:N0} | {audit.Performance.TotalTimeMs,17:N2} | {FormatBytes(audit.Memory.AuxiliaryAllocatedBytes),20} |");
        }
        Console.WriteLine("+---------------------------+-------------------+-------------------+-------------------+----------------------+");

        // محاسبه شتاب و نتیجه‌گیری
        if (report.AlgorithmReports.Count >= 2)
        {
            double t1 = report.AlgorithmReports[0].Performance.TotalTimeMs;
            double t2 = report.AlgorithmReports[1].Performance.TotalTimeMs;
            double factor = t1 / t2;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[*] ANALYSIS: {report.AlgorithmReports[1].AlgorithmName} performs {factor:F2}x faster compared to {report.AlgorithmReports[0].AlgorithmName}.");
            Console.ResetColor();
        }

        Console.WriteLine("\n##################################################################################################\n");
    }

    private static string FormatBytes(long memoryBytes)
    {
        if (memoryBytes == 0) return "Zero Alloc (0 B)";
        if (memoryBytes < 1024) return $"{memoryBytes} B";
        if (memoryBytes < 1024 * 1024) return $"{(memoryBytes / 1024.0):F2} KB";
        return $"{(memoryBytes / (1024.0 * 1024.0)):F2} MB";
    }
}