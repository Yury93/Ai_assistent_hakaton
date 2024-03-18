using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigsContainer : MonoBehaviour
{
  public Hakaton Hakaton { get; private set; }
    public static ConfigsContainer Instance;
    public void Init()
    {
        Instance = this;
    }
    public void CreateConfigHakaton(Hakaton hakaton)
    {
        Hakaton = new Hakaton();
        Hakaton.entities = hakaton.entities;
    }
}
