```markdown
# High-Performance Search Algorithm Benchmark Suite (.NET 8)

An empirical performance analysis and verification platform built in C# (.NET 8) comparing classic logarithmic search algorithms against hardware-accelerated SIMD vectorization techniques over large-scale datasets ($N = 10^7$).

## Overview

This repository provides a framework for evaluating search latency, throughput, memory consumption, and functional correctness. It benchmarks a baseline **Classic Iterative Binary Search** against a vectorized **SIMD AVX2 S-Tree Search** utilizing 256-bit SIMD registers, cache-line alignment, and hardware prefetching.

Designed following **Clean Architecture** and **SOLID** principles, the suite isolates benchmarking pipelines, profiling mechanisms, memory managers, and algorithm contracts.

---

## Repository Architecture

```text
daa-coursework/
├── Core/
│   ├── Abstractions/       # Contracts (ISearchAlgorithm, IPerformanceBenchmarkEngine, etc.)
│   └── Models/             # Domain DTOs (BenchmarkReport, PerformanceMetrics)
├── Algorithms/             # ClassicBinarySearch & SimdSTreeSearch implementations
├── Services/
│   ├── Data/               # High-throughput sorted dataset generators
│   ├── Verification/       # Functional equivalence testing engine
│   ├── Performance/        # High-resolution time measurement engine
│   └── Profiling/          # Native memory & GC allocation profiler
├── Reporting/              # Console report formatting and presentation
└── Pipeline/               # Benchmark execution orchestrator

```

---

## Evaluated Algorithms

| Algorithm | Theoretical Time | Theoretical Space | Primary Optimization Techniques |
| --- | --- | --- | --- |
| **Classic Binary Search** | $O(\log_2 N)$ | $O(1)$ | Direct pointer arithmetic, JIT loop unrolling |
| **SIMD AVX2 S-Tree Search** | $O(\log_9 N)$ | $O(N)$ Pre-processed | 256-bit AVX2 vectors, `Sse.Prefetch0`, 64-byte memory alignment (`NativeMemory.AlignedAlloc`) |

---

## Empirical Results

### Environment Setup

* **Runtime:** .NET 8.0 (Release Configuration, x64)
* **Dataset Size:** 10,000,000 sorted 32-bit integers
* **Verification Workload:** 100,000 randomized queries
* **Benchmark Iterations:** 2,000,000 queries

### Benchmark Output
```text
##################################################################################################
               ALGORITHMIC BENCHMARK & COMPLEXITY SUITE | MAHDI LAKI
               Workload Scale: 10,000,000 Keys (Int32 Array)
##################################################################################################

[*] EQUIVALENCE VERIFICATION CHECK
    Outcome: SUCCESS (Identical results over 100,000 queries)

[*] BENCHMARK METRICS (TIME & MEMORY)
+---------------------------+-------------------+-------------------+-------------------+----------------------+
| Target Algorithm          | Latency (ns)      | Rate (ops/sec)    | Duration (ms)     | Memory Allocation    |
+---------------------------+-------------------+-------------------+-------------------+----------------------+
| Classic Binary Search     |            421.74 |         2,371,145 |            843.47 |     Zero Alloc (0 B) |
| SIMD AVX2 S-Tree Search   |            116.42 |         8,589,604 |            232.84 |             38.15 MB |
+---------------------------+-------------------+-------------------+-------------------+----------------------+

[*] ANALYSIS: SIMD AVX2 S-Tree Search performs 3.62x faster compared to Classic Binary Search.

##################################################################################################
```
### Technical Analysis

* **Performance Gains:** The SIMD AVX2 S-Tree Search achieved a **3.62x throughput increase** over Classic Binary Search, reducing average query latency from 421.74 ns to 116.42 ns per operation.
* **Space-Time Trade-off:** To enable 8-way parallel integer comparisons per CPU cycle, the SIMD implementation restructures the dataset into a cache-friendly 9-ary tree. This requires 38.15 MB of unmanaged aligned memory ($O(N)$ space), optimizing for bandwidth-heavy search workloads at the cost of pre-processing storage.

---

## Building & Execution

### Prerequisites

* .NET 8.0 SDK or higher
* x86-64 processor supporting AVX2 and SSE instruction sets

### Commands

Compilation in `Release` configuration is required to allow JIT vectorization and SIMD instruction emission:

```bash
# Clone repository
git clone https://github.com/mlaki20788/algo-benchmark-platform.git
cd algo-benchmark-platform

# Build executable
dotnet build -c Release

# Execute benchmark suite
dotnet run -c Release --no-build

```

---

```

```
