using System;
using Newtonsoft.Json;

namespace Droid_Rest1
{
    public class SimpleList
    {
        public int id { get; set; }
        public string name { get; set; }
        public int ownerId { get; set; }

        public override string ToString()
        {
            return string.Format(
                "id: {0}\nname: {1}\nownerId: {2}",
                id, name, ownerId);
        }
    }
}