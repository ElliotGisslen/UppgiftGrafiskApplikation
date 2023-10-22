using var httpClient = new HttpClient();
var result = await httpClient.PostAsync("", null!);