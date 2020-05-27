using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : ScriptableObject
{
    public enum SlotType { empty, ground, underground, occupied}

    public SlotType slotType;
}
