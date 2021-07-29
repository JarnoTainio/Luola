using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EscapeButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI costText;
    private bool isActive = false;
    public bool canEscape = false;
    private int cost;

    public void UpdateState(GameManager gameManager){
        if (isActive == false && gameManager.combatManager.monsterTemplate.escapeOnTurn <= gameManager.roundNumber){
            gameObject.SetActive(true);
            isActive = true;
            cost = gameManager.combatManager.monsterTemplate.escapeCost;
            costText.text = cost.ToString();
        }
    }

    public void EnergyModified(int current){
        if (isActive){
            button.enabled = current >= cost;
            canEscape = current >= cost;
        }
    }

}
