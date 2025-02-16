namespace Core
{
    public static class Helper
    {
        // this partition is used for the QuickSort function but takes in an array of objects and an array of floats as distances to be sorted upon
        static int PartitionWithDistances<T>(T[] arr, float[] distances, int low, int high)
        {
            // Choose the pivot as the last element in the distance array, this can be an issue if the array is already sorted but we will assume it isnt
            var pivot = distances[high];
            
            //get the smallest element
            var i = low - 1;

            for (var j = low; j <= high - 1; j++)
            {
                // Compare distance with the pivot
                if (!(distances[j] < pivot)) continue;
                i++;

                // Swap elements in both the distance array and the array of objects
                SwapWithDistances(arr, distances, i, j);
            }

            // Move the pivot element to its correct position
            SwapWithDistances(arr, distances, i + 1, high);

            return i + 1;
        }

        // Swap function to swap elements in both arrays
        static void SwapWithDistances<T>(T[] arr, float[] distances, int i, int j)
        {
            // Swap elements in the object array
            (arr[i], arr[j]) = (arr[j], arr[i]);
            // Swap corresponding distances
            (distances[i], distances[j]) = (distances[j], distances[i]);
        }
       
        // QuickSort function to sort the array of objects based on a distances array
        public static void QuickSortWithDistances<T>(T[] arr, float[] distances, int low, int high)
        {
            if (low >= high) return;
            // Partition the array and get the pivot index
            var pi = PartitionWithDistances(arr, distances, low, high);

            // Recursively sort elements before and after the partition
            QuickSortWithDistances(arr, distances, low, pi - 1);
            QuickSortWithDistances(arr, distances, pi + 1, high);
        }


    }
}