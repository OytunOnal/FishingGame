using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeaImage : MonoBehaviour
{
    [SerializeField] private Image _seaImage;

    [SerializeField] private List<SeaImageObject> _seaSprites;

    public void EnvironmentSet(string environmentName)
    {
        foreach (SeaImageObject seaImageObject in _seaSprites)
        {
            if (seaImageObject.EnvironmentName.Equals(environmentName))
            {
                _seaImage.sprite = seaImageObject.SeaSprite;
                break;
            }
        }
    }

    [Serializable]
    public class SeaImageObject
    {
        public string EnvironmentName;
        public Sprite SeaSprite;
    }
}
