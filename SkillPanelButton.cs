using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanelButton : MonoBehaviour
{
    public Skill skill;
    [SerializeField] string title;
    [SerializeField] string description;

    public string GetTitle()
    {
        return title;
    }
    public string GetDescription()
    {
        return description;
    }
}
