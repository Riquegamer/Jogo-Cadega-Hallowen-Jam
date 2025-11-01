using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Dialogue
{
        public string characterName;
        [TextArea(5, 10)]
        public string text;

}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogos/DialogueData", order = 1)]
public class DialogueData : ScriptableObject
{
    public List<Dialogue> talkScript;
}
