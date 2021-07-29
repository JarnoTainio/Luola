using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
  public GameObject newGameContainer;
  public GameObject profileContainer;
  public GameObject loadingObject;
  public GameObject deleteDialog;
  public TextObject profileButton;

  public Image fadeInImage;
  public float fadeDuration = .5f;

  public TMP_InputField inputField;

  public int selectedIndex;
  public int lastIndex;
  public ProfileBox profileBox;
  public ProfileBox[] profiles;

  private void Start()
  {
    List<SaveData> saveData = new List<SaveData>();
    bool saveExists = false;
    for(int i = 0; i < profiles.Length; i++)
    {
      SaveData save = SaveManager.LoadSave(i);
      if (save != null){
        saveExists = true;
      }
      saveData.Add(save);
      profiles[i].Show(this, save, i);
    }
    if (saveExists){
      profileButton.SetText("profileMenu");
    }
    lastIndex = DataManager.instance.gameSettings.lastIndex;
    profileBox.Show(this, lastIndex >= 0 && lastIndex < saveData.Count ? saveData[lastIndex] : null, lastIndex, true);
  }

  public void DeleteGame()
  {
    SaveManager.DeleteSave(selectedIndex);
    profiles[selectedIndex].Show(this, null, selectedIndex);
  }

  public void StartGame(ProfileBox box){
    StartGame(box.index);
  }

  public void StartGame(int index)
  {
    selectedIndex = index;
    if (SaveManager.SaveExists(index))
    {
      loadingObject.SetActive(true);
      DataManager.instance.saveData = SaveManager.LoadSave(index);
      StartCoroutine(FadeToGame(true));
    }
    else
    {
      selectedIndex = index;
      profileContainer.SetActive(false);
      newGameContainer.SetActive(true);
      EventSystem.current.SetSelectedGameObject(inputField.gameObject);
    }
  }

  public void SelectForDelete(int index){
    selectedIndex = index;
    deleteDialog.SetActive(true);
  }

  public void CreateProfile()
  {
    loadingObject.SetActive(true);
    string name = inputField.text;
    if (name.Length == 0){
      return;
    }
    SaveData data = new SaveData();
    data.NewProfile(name);
    DataManager.instance.saveData = data;
    SaveManager.SaveGame(selectedIndex, data);
    StartCoroutine(FadeToGame(false));
  }

  public IEnumerator FadeToGame(bool load)
  {
    DataManager.instance.gameSettings.lastIndex = selectedIndex;
    SaveManager.SaveSettings(DataManager.instance.gameSettings);
    DataManager.instance.index = selectedIndex;

    fadeInImage.gameObject.SetActive(true);
    float f = 0f;
    while (f < fadeDuration)
    {
      f += Time.deltaTime;
      fadeInImage.color = new Color(0, 0, 0, f / fadeDuration);
      yield return null;
    }

    if (load)
    {
      DataManager.instance.LoadGame();
    }
    else
    {
      DataManager.instance.ToCharacterSelection();
    }
  }

}
