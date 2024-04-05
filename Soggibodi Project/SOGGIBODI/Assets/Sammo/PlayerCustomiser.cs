using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomiser : MonoBehaviour
{
    [SerializeField] GameObject playerEntryPrefab;

    public List<GameObject> HealthBars = new List<GameObject>();
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(GameObject g in HealthBars)
        {
            g.SetActive(false);
        }
        for(int i = 0; i < PlayerConfigurationManager.instance.playerConfigs.Count; i++)
        {
            GameObject item = Instantiate(playerEntryPrefab, this.transform);
            PlayerModals pm = item.GetComponent<PlayerModals>();
            pm.playerIndex = i;
            pm.textMeshProUGUI.SetText($"Player {pm.playerIndex + 1}");

            PlayerConfigurationManager.instance.playerConfigs[i].GetComponent<PlayerInformation>().playerInfo.playerControls.lWeapon = PlayerConfigurationManager.instance.GetComponent<PlayerChoices>().weapons[0];
            PlayerConfigurationManager.instance.playerConfigs[i].GetComponent<PlayerInformation>().playerInfo.playerControls.rWeapon = PlayerConfigurationManager.instance.GetComponent<PlayerChoices>().weapons[0];

            HealthBars[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
