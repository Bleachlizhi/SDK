using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyToolsSetting
{
    public class SortManage
    {
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[] Bubble_sort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }
        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[] Insertion_sort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (array[j - 1] > array[j])
                    {
                        int temp = array[j - 1];
                        array[j - 1] = array[j];
                        array[j] = temp;
                    }
                }
            }
            return array;
        }
        /// <summary>
        /// 希尔排序
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[] Shell_sort(int[] array)
        {
            int inc;
            for (inc = 1; inc <= array.Length / 9; inc = 3 * inc + 1) ;
            for (; inc > 0; inc /= 3)
            {
                for (int i = inc + 1; i <= array.Length; i += inc)
                {
                    int t = array[i - 1];
                    int j = i;
                    while ((j > inc) && (array[j - inc - 1] > t))
                    {
                        array[j - 1] = array[j - inc - 1];
                        j -= inc;
                    }
                    array[j - 1] = t;
                }
            }
            return array;
        }
        /// <summary>
        /// 选择排序
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[] Selection_sort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[min])
                        min = j;
                }
                int temp = array[min];
                array[min] = array[i];
                array[i] = temp;
            }
            return array;
        }
    }

}