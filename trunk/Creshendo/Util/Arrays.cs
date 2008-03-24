using System;
using System.Collections;

namespace Creshendo.Util
{
    public class Arrays
    {
        public static bool equalsX(object[] facts, object[] objectses)
        {
            if(facts.Length == objectses.Length)
            {
                for(int i = 0; i < facts.Length; i++)
                {
                    if (facts[i] == null && objectses[i] == null)
                    {
                        
                    }
                    else if (facts[i] != null && objectses[i] != null)
                    {
                        if(facts[i].Equals(objectses[i]) == false)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool equals(object[] arr1, object[] arr2)
        {
            if (arr1 == null || arr2 == null)
                throw new NullReferenceException("Arrays must not be null");
            if (arr1.Length == arr2.Length)
            {
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (arr1[i] != arr2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static void sort(object[] sorted, IComparer comparator)
        {
            Array.Sort(sorted, comparator);
        }
    }
}