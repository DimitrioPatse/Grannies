using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AbillitySelector : MonoBehaviour,ISaveable
{
    [SerializeField] Transform[] buttonSpawnPositions;
    [SerializeField] Button abillityButtonPrefab;

    List<Button> abilButs = new List<Button>();
    Dictionary<AbillityClass, int> availableAbillities = null;
    AbillityTable abillityTable;
    public void Awake()
    {
        abillityTable = Resources.Load<AbillityTable>("AbillityTable");
        if (abillityTable == null)
        {
            Debug.LogError("Abillity selector couldn't find abillity table");
        }
        availableAbillities = abillityTable.GetMaxCntDictionary();
    }

    public bool headStarted { set; get; }

    /// <summary>
    /// Energopoiei to Panel toy Selector
    /// </summary>
    public void EnableAbillitySelector()
    {
        
        abilButs = new List<Button>();
        SpawnButtons();
    }

    /// <summary>
    /// Apnergopoiei to Panel toy Selector
    /// </summary>
    public void DisableAbillitySelector()
    {
        if (abilButs.Count > 0)
        {
            for (int i = 0; i < abilButs.Count; i++)
            {
                Destroy(abilButs[i].gameObject);
            }
        }
        abilButs.Clear();
    }

    /// <summary>
    /// Reduses abillity's max count and removes it from availables if 0.
    /// It is called from abillity button OnClick
    /// </summary>
    public void SetUseOfAbillity(AbillityClass abillity)
    {
        availableAbillities[abillity]--;
        if (availableAbillities[abillity] <= 0)
        {
            availableAbillities.Remove(abillity);
        }
    }

    /// <summary>
    /// Spawns 3 unique ability buttons in button locations
    /// </summary>
    void SpawnButtons()
    {
        AbillityClass[] abs = new AbillityClass[3];

        for (int i = 0; i < 3; i++)
        {
            Transform buttonLocation = buttonSpawnPositions[i];

            //Spawn first button with random ability
            if (i == 0)
            {
                //Check if players hp is less than 25% so to spawn HEAL Ability button
                Health hp = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
                if(hp.GetHealthPersentage() < 25)
                {
                    abs[i] = SpawnAbillityButton(buttonLocation, AbillityClass.Heal);
                }
                //Else spawn random ability
                else
                {
                    AbillityClass abillity = availableAbillities.ElementAt(Random.Range(0, availableAbillities.Count - 1)).Key;
                    abs[i] = SpawnAbillityButton(buttonLocation, abillity);
                }
            }

            //Spawn 2nd and 3rd ability button while checking if already spawned same abiliy
            if (i > 0)
            {
                //Amuntikos Programmatismos
                while(true)
                {
                    //Get Random Ability
                    AbillityClass abillity = availableAbillities.ElementAt(Random.Range(0, availableAbillities.Count - 1)).Key;

                    //Check if ability already spawn in button
                    if (abillity != abs[1] && abillity != abs[2])
                    {
                        //Spawn ability button
                        abs[i] = SpawnAbillityButton(buttonLocation, abillity);
                        break;
                    }
                }
            }  
        }
    }

    /// <summary>
    /// Instantiates a abillity button in the given transform with the transform's size
    /// </summary>
    AbillityClass SpawnAbillityButton(Transform buttonLocation, AbillityClass abillity)
    {
        //Instaciates Button Prefab
        Button but = Instantiate(abillityButtonPrefab, buttonLocation.position, Quaternion.identity, transform) as Button;
        RectTransform buttonRect = but.GetComponent<RectTransform>();
        buttonRect.sizeDelta = buttonLocation.GetComponent<RectTransform>().sizeDelta;

        //Set ability to button
        AbillityHolder holder = but.GetComponent<AbillityHolder>();
        holder.abillity = abillity;
        Text butText = but.GetComponentInChildren<Text>();
        butText.text = abillity.ToString();
        
        //Adds Button to list
        abilButs.Add(but);

        return abillity;
    }

    #region Save

    public object CaptureState()
    {
        return headStarted;
    }

    public void RestoreState(object state)
    {
        headStarted = (bool)state;
    }
    #endregion
}
