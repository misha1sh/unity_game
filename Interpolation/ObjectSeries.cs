using System.Collections.Generic;

namespace Interpolation {
    public class ObjectSeries<T> {
        private List<T> data = new List<T>();
        private int cur = 0;
        
        public void AddData(T newData) {
            data.Add(newData);
        }

        public void RemoveOldAndMove() {
            data.RemoveAt(0);
            cur--;
        }

        // ......
        // -2 --- lastlast
        // -1 --- last
        // 0 --- next;
        // 1 --- nextnext
        // ......
        public object this[int index] => data[cur + index];
    }
}