#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    JObject resultSet = JObject.FromObject(new { rows = new JArray() });
    string[] csvLines = ((string)data["csv"]).Split(new char [] {'\n', '\r'});
    var headers = csvLines[0].Split(',').ToList<string>();

    foreach (var line in csvLines.Skip(1))
    {
        if(line == null || line == "")
            continue;
        var lineObject = new JObject();
        var lineAttr = line.Split(',');
        for (int x = 0; x < headers.Count; x++)
        {
            lineObject[headers[x]] = lineAttr[x];
        }
        ((JArray)resultSet["rows"]).Add(lineObject);
    }

    return req.CreateResponse(HttpStatusCode.OK, resultSet);
}
