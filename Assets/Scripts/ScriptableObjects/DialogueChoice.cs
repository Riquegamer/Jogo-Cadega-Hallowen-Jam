using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Choice
{
    public string optionText;
    public DialogueData consequencia;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogos/DialogueChoice", order = 2)]
public class DialogueChoice : ScriptableObject
{
    public List<Choice> Escolhas;
}
