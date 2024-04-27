/*
This class is responsible for setting the values in a dropdown menu based on the selected Enum type.

Each new Enum must be added in the local enum EnumSelector
The names of the enum fields are fetched and added as dropdown options. 

If the Enum is not selected, it logs an error in the console.
*/

using System;
using Enums;
using TMPro;
using UnityEngine;

public class SetValueInDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private EnumSelector enumToUse;
    
    private string[] _enumNames;
    
    private enum EnumSelector
    {
        CardTypeEnum,
        GameModeEnum,
        MenuListEnum,
        SecretRoleEnum
    }

    private void Awake()
    {
        switch (enumToUse)
        {
            case EnumSelector.CardTypeEnum:
                _enumNames = System.Enum.GetNames(typeof(CardTypeEnum));
                break;
            case EnumSelector.GameModeEnum:
                _enumNames = System.Enum.GetNames(typeof(GameModeEnum));
                break;
            case EnumSelector.MenuListEnum:
                _enumNames = System.Enum.GetNames(typeof(AdminMenuListEnum));
                break;
            case EnumSelector.SecretRoleEnum:
                _enumNames = System.Enum.GetNames(typeof(SecretRoleEnum));
                break;
            default:
                Debug.LogError("Enum not selected");
                break;
        }

        SetDropdownValue();
    }
    
    private void SetDropdownValue()
    {
        // Clear any existing options in the dropdown
        dropdown.ClearOptions();
        
        // Add each enum value to the dropdown
        foreach (string enumName in _enumNames) { dropdown.options.Add(new TMP_Dropdown.OptionData(enumName)); }
        
        // Refresh the dropdown to display the new options
        dropdown.RefreshShownValue();
    }

    public void RemoveDropdownValue(string textToRemove)
    {
        int indexToRemove = dropdown.options.FindIndex(option => option.text == textToRemove);

        if (indexToRemove != -1)
        {
            dropdown.options.RemoveAt(indexToRemove);
            dropdown.RefreshShownValue();
        }
        else
        {
            throw new Exception($"{textToRemove} no not exist");
        }
    }
    
    public void AddDropdownValue(string textToAdd)
    {
        TMP_Dropdown.OptionData newOptionData = new TMP_Dropdown.OptionData(textToAdd);
        dropdown.options.Add(newOptionData);
        dropdown.RefreshShownValue();
    }
    
    public bool IsValueInDropdown(string textToFind)
    {
        int indexToFind = dropdown.options.FindIndex(option => option.text == textToFind);

        return indexToFind != -1;
    }
}