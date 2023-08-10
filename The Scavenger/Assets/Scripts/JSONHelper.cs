using Leguar.TotalJSON;

namespace Scavenger
{
    public static class JSONHelper
    {
        // TODO add docs
        public static JSON Combine(JSON[] jsonsToCombine)
        {
            JSON json = new JSON();

            foreach (JSON jsonToCombine in jsonsToCombine)
            {
                foreach (string key in jsonToCombine.Keys)
                {
                    json[key] = jsonToCombine[key];
                }
            }
            
            return json;
        }

        public static JSON GetJSONOrNull(JSON json)
        {
            if (json == null || json.Count == 0)
            {
                return null;
            }
            return json;
        }

        public static JSON GetJSONOrEmpty(JSON json)
        {
            if (json == null)
            {
                return new JSON();
            }
            return json;
        }

        public static void TryAdd(JSON json, string key, JValue value)
        {
            if (value == null)
            {
                return;
            }

            if (value is JSON && (value as JSON).Count == 0)
            {
                return;
            }

            json.Add(key, value);
        }

        public static JSON Copy(JSON json)
        {
            return new JSON(json.AsDictionary());
        }
    }
}
