namespace JewelDesign.Rendering.Candidate;

/// <summary>
/// Authored value translation of the pinned Microsoft CRT100 width8 qsort control flow.
/// Preserves the cutoff8 short-sort tie order, median partition, equal-run handling and callback sequence.
/// Callers needing transactional mutation sort an owned copy and commit only after success.
/// The list length and comparison policy must remain stable during this operation.
/// </summary>
public static class Rhino5Crt100QSort
{
    public static void Sort<T>(List<T> items, Comparison<T> compare)
    {
        ArgumentNullException.ThrowIfNull(items); ArgumentNullException.ThrowIfNull(compare);
        if (items.Count < 2) return;
        Span<int> lows = stackalloc int[62], highs = stackalloc int[62];
        int stack = 0, low = 0, high = items.Count - 1;
        while (true)
        {
            int size = high - low + 1;
            if (size <= 8)
            {
                ShortSort(items, low, high, compare);
                if (stack == 0) return;
                stack--; low = lows[stack]; high = highs[stack];
                continue;
            }
            int middle = low + size / 2;
            if (compare(items[low], items[middle]) > 0) Swap(items, low, middle);
            if (compare(items[low], items[high]) > 0) Swap(items, low, high);
            if (compare(items[middle], items[high]) > 0) Swap(items, middle, high);
            int left = low, right = high;
            while (true)
            {
                if (middle > left)
                {
                    do { left++; } while (left < middle && compare(items[left], items[middle]) <= 0);
                }
                if (middle <= left)
                {
                    do { left++; } while (left <= high && compare(items[left], items[middle]) <= 0);
                }
                do { right--; } while (right > middle && compare(items[right], items[middle]) > 0);
                if (left > right) break;
                Swap(items, left, right);
                if (middle == right) middle = left;
            }
            right++;
            if (middle < right)
            {
                do { right--; } while (right > middle && compare(items[right], items[middle]) == 0);
            }
            if (middle >= right)
            {
                do { right--; } while (right > low && compare(items[right], items[middle]) == 0);
            }
            // Native qsort pushes the larger range, then immediately processes the smaller.
            if (right - low >= high - left)
            {
                if (low < right) { Push(lows, highs, ref stack, low, right); }
                if (left < high) { low = left; continue; }
            }
            else
            {
                if (left < high) { Push(lows, highs, ref stack, left, high); }
                if (low < right) { high = right; continue; }
            }
            if (stack == 0) return;
            stack--; low = lows[stack]; high = highs[stack];
        }
    }

    private static void ShortSort<T>(List<T> items, int low, int high, Comparison<T> compare)
    {
        while (high > low)
        {
            int maximum = low;
            for (int i = low + 1; i <= high; i++) if (compare(items[i], items[maximum]) > 0) maximum = i;
            Swap(items, maximum, high); high--;
        }
    }
    private static void Swap<T>(List<T> items, int first, int second)
    {
        if (first == second) return;
        (items[first], items[second]) = (items[second], items[first]);
    }
    private static void Push(Span<int> lows, Span<int> highs, ref int stack, int low, int high)
    {
        if ((uint)stack >= (uint)lows.Length) throw new InvalidDataException("Recorded native sort stack bound exceeded.");
        lows[stack] = low; highs[stack] = high; stack++;
    }
}
