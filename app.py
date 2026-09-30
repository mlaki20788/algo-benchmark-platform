import time
import tracemalloc
import copy

def run_benchmark(func, test_data):
    """اندازه‌گیری زمان و حافظه مصرفی"""
    data_copy = copy.deepcopy(test_data)
    
    tracemalloc.start()
    t_start = time.perf_counter()
    
    result = func(data_copy)
    
    t_end = time.perf_counter()
    _, peak_mem = tracemalloc.get_traced_memory()
    tracemalloc.stop()
    
    return {
        "result": result,
        "time_ms": (t_end - t_start) * 1000,
        "mem_kb": peak_mem / 1024
    }

# الگوریتم اول: مرتب‌سازی حبابی
def bubble_sort(arr):
    n = len(arr)
    for i in range(n):
        for j in range(0, n - i - 1):
            if arr[j] > arr[j + 1]:
                arr[j], arr[j + 1] = arr[j + 1], arr[j]
    return arr

# الگوریتم دوم: مرتب‌سازی سریع
def quick_sort(arr):
    if len(arr) <= 1:
        return arr
    pivot = arr[len(arr) // 2]
    left = [x for x in arr if x < pivot]
    middle = [x for x in arr if x == pivot]
    right = [x for x in arr if x > pivot]
    return quick_sort(left) + middle + quick_sort(right)

def main():
    print("=" * 50)
    print("      ALGORITHM PERFORMANCE COMPARATOR        ")
    print("=" * 50)
    
    test_input = [64, 34, 25, 12, 22, 11, 90, 88, 76, 54, 42, 99, 1]
    print(f"\n[+] Input size: {len(test_input)} elements")
    
    # تست اجرا و منابع
    res_a = run_benchmark(bubble_sort, test_input)
    res_b = run_benchmark(quick_sort, test_input)
    
    # چک کردن هم‌ارزی خروجی
    is_same = res_a["result"] == res_b["result"]
    print(f"[+] Output equivalence check: {'PASSED (Identical results)' if is_same else 'FAILED'}")
    
    print("\n--- Benchmark Results ---")
    print(f"Algorithm 1 (Bubble Sort):")
    print(f"  - Execution Time : {res_a['time_ms']:.4f} ms")
    print(f"  - Peak Memory    : {res_a['mem_kb']:.2f} KB")
    
    print(f"\nAlgorithm 2 (Quick Sort):")
    print(f"  - Execution Time : {res_b['time_ms']:.4f} ms")
    print(f"  - Peak Memory    : {res_b['mem_kb']:.2f} KB")
    print("-" * 50)

if __name__ == "__main__":
    main()
