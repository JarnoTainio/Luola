using System.IO;
using UnityEngine;

public class SaveManager
{

  public static bool SaveExists(int index)
  {
    try
    {
      return File.Exists(ProfilePath(index));
    }catch (IOException e)
    {
      Debug.LogError(e);
      return false;
    }
  }

  public static SaveData LoadSave(int index)
  {
    try
    {
      string path = ProfilePath(index);
      System.Xml.Serialization.XmlSerializer reader =  new System.Xml.Serialization.XmlSerializer(typeof(SaveData));
      StreamReader file = new System.IO.StreamReader(path);
      SaveData saveData = (SaveData)reader.Deserialize(file);
      file.Close();
      return saveData;
    }
    catch (IOException e)
    {
      Debug.LogError(e);
      return null;
    }
  }

  public static void SaveGame(int index, SaveData data)
  {
    try
    {
      var writer = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));
      var wfile = new System.IO.StreamWriter(ProfilePath(index));
      writer.Serialize(wfile, data);
      wfile.Close();
    }
    catch (IOException e)
    {
      Debug.LogError(e);
    }
  }

  public static void SaveSettings(GameSettings data)
  {
    try
    {
      var writer = new System.Xml.Serialization.XmlSerializer(typeof(GameSettings));
      var wfile = new System.IO.StreamWriter(SettingsPath());
      writer.Serialize(wfile, data);
      wfile.Close();
    }
    catch (IOException e)
    {
      Debug.LogError(e);
    }
  }

  public static GameSettings LoadSettings()
  {
    try
    {
      string path = SettingsPath();
      System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(GameSettings));
      StreamReader file = new System.IO.StreamReader(path);
      GameSettings settings = (GameSettings)reader.Deserialize(file);
      file.Close();
      return settings;
    }
    catch (IOException e)
    {
      Debug.LogError(e);
      return new GameSettings();
    }
  }


  public static void DeleteSave(int index)
  {
    try
    {
      File.Delete(ProfilePath(index));
    }
    catch (IOException e)
    {
      Debug.LogError(e);
    }
  }

  private static string ProfilePath(int index)
  {
    return $"{Application.persistentDataPath}/profile_{index}.save";
  }

  private static string SettingsPath()
  {
    return $"{Application.persistentDataPath}/settings.save";
  }

}
