using Creshendo.Util.Collections;

namespace Test.Creshendo.Support
{
    public class HashMapBenchmark
    {
        public GenericHashMap<object, object> createHashMap(int count)
        {
            GenericHashMap<object, object> map = new GenericHashMap<object, object>();
            for (int idx = 0; idx < count; idx++)
            {
                map.Put(count.ToString(), count + "value");
            }
            return map;
        }

        public GenericHashMap<string, string> createGenericHashMap(int count)
        {
            GenericHashMap<string, string> map = new GenericHashMap<string, string>();
            for (int idx = 0; idx < count; idx++)
            {
                map.Put(count.ToString(), count + "value");
            }
            return map;
        }
    }
}