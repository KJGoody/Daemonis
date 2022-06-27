using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadManager
{
    // JsonUtility를 사용하여, 변환된 JSON(string)을 BinaryFormatter를 이용해 바이너리 형태로 변환하고, 파일에 저장한다.
    // 파일을 불러오는 방법: BinaryFormatter를 사용해 JSON으로 변환하고, JsonUtility를 사용해 Data로 변환한다.

    public static bool FileExists(string _fileName)
    {
        string path = Application.persistentDataPath + "/" + _fileName;
        return File.Exists(path);
    }

    public static void DataSave<T>(T data, string _fileName)
    {
        try
        {
            string json = JsonUtility.ToJson(data);

            if (json.Equals("{}"))
            {
                Debug.Log("" + "json null");
                return;
            }
            string path = Application.persistentDataPath + "/" + _fileName;

            FileStream fileStream = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(fileStream, json);
            fileStream.Close();
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("The file was not found:" + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("The directory was not found: " + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be opened:" + e.Message);
        }
    }

    public static T DataLoad<T>(string _fileName)
    {
        string path = Application.persistentDataPath + "/" + _fileName;

        try
        {
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();

                string json = formatter.Deserialize(stream) as string;

                T data = JsonUtility.FromJson<T>(json);

                stream.Close();

                Debug.Log(json);
                return data;
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("The file was not found:" + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("The directory was not found: " + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be opened:" + e.Message);
        }
        return default;
    }
}
