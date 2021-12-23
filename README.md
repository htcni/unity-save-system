# Unity Save System
A simple unity save system to save data to a file. It store data in key-value format, similar to PlayerPerfs but a bit more flexible way.

- [Usage](#usage)
  - [Dependencies](#dependencies)
- [Saving data](#saving-data)
  - [Saving simple data](#saving-simple-data)
  - [Retrieve data](#retrieve-data)
  - [Get save dump](#get-save-dump)
 -  [Deleting a value](#deleting-a-value)
 -  [Reset data](#reset-data)
- [Saving Complex data](#saving-complex-data)
  -  [Tips](#tips)



### Usage

import namespace

`using BigFrogTools;`

### Dependencies

Requires newtonsoft json.
Get it [here](https://github.com/jilleJr/Newtonsoft.Json-for-Unity).


### Saving data

### Saving simple data
Simple data can be saved very easily using a key-value pair format.

```c#
SaveSystem.SetData("health", 100);
SaveSystem.SetData("levelCompleted", false);
SaveSystem.SetData("distance": 15.55f)
```
`Note: All the values get converted into string.`

### Retrieve data
```c#
SaveSystem.GetData("health");
SaveSystem.GetData("distance")
```
Since all the values get saved as string. You need to parse it into proper data type.
```c#
int.parse(SaveSystem.GetData("health"));
float.parse(SaveSystem.GetData("distance"));
bool.parse(SaveSystem.GetData("levelCompleted"));

```

### Get save dump
```c#
SaveSystem.GetSaveDump()
{"localData":{"health":"100","levelCompleted":"false","distance":"15.55"}}
```

### Deleting a value
```c#
SaveSystem.RemoveData("health")
```

### Reset data
It removes all the data from saved file
```c#
SaveSystem.ResetSaveData();
```

### Saving complex data
Since everything is serialized as string we can store any type of data. But we have to do some black magic while parsing it.

#### Store an array
```c#
int[] someData = new int[5] { 15, 6, 7, 9, 2 };
SaveSystem.SetData("someData", someData);;
string tmpArr = SaveSystem.GetData("someData");
int[] newArr;
int[] myArr = SaveSystem.ConvertValue(tmpArr, newArr);
//float[] myArr = SaveSystem.ConvertValue(tmpArr, someData);
```
After it we loop through the elements.

#### Convert Data 
We can also parse our primitive data with this method.
```c#
int tempHealth;
int health = SaveSystem.ConvertValue(SaveSystem.GetData("health"), tempHealth)
```
### Tips
For more complex data I would suggest to store it in class then serialize it and save it to file. Since this serialization will keep data type intact and maintainable;

```c#
public class User {
    public string username;
    public int points;
    public int xp;
}

User u = new User();
string userData = JsonConvert.SerializeObject(u);
SaveSystem.SetData("user", userData);

// Get data

u = JsonConvert.DeserializeObject<User>(SaveSystem.GetData("user"));
// Access
//u.points
```
We can store multiple serialize class and access it with a key. Since it's valid JSON we can sync this data with our server.




