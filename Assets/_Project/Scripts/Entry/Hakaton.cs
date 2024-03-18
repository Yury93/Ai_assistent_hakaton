using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hakaton  
{
    [field: SerializeField] public List<HakatonEntity> entities = new List<HakatonEntity> ();
}
[Serializable]
public class HakatonEntity
{
    [field: SerializeField] public int Id { get; set; }
    [field: SerializeField] public string TypeConfig { get; set; }
    [field: SerializeField] public string Voice { get; set; }
}
