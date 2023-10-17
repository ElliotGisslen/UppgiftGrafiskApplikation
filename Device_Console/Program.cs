using var httpClient = new HttpClient();
var result = await httpClient.GetAsync("https://iotdevicefunctions.azurewebsites.net/api/devices?code=MZv5RErPGwp2TqZ5XmclwFQvIONtXQHmsrmT5-udfs7fAzFu0iXOgw==");

var content = await result.Content.ReadAsStringAsync();
