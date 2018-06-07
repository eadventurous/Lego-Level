﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LevelBuilder : MonoBehaviour
{

    public List<Structure> Structures;

    public Texture2D LegoBlocks;

    [Serializable]
    public struct ColoredBlock
    {
        public Color Color;
        public string Name;
    }

    [Serializable]
    public struct NamedPrefab
    {
        public string Name;
        public GameObject gameObject;
    }

    public List<ColoredBlock> ColorBlockNames;

    public List<NamedPrefab> NamedPrefabs;

    public Vector2Int GridNumber;

    // Use this for initialization

    public void BuildLevel()
    {
        var colorBlockNames = new Dictionary<Color, string>();
        var namedPrefabs = new Dictionary<string, GameObject>();

        foreach (ColoredBlock coloredBlock in ColorBlockNames)
            colorBlockNames.Add(coloredBlock.Color, coloredBlock.Name);
        foreach (NamedPrefab namedPrefab in NamedPrefabs)
            namedPrefabs.Add(namedPrefab.Name, namedPrefab.gameObject);

        var factory = new StructureRecognizerFactory(Structures, new GridRecognizerFactory(GridNumber, colorBlockNames));
        foreach (var item in factory.GetObject().Recognize(LegoBlocks))
        {
            var sceneObject = Instantiate(namedPrefabs[item.Name]);
            Vector3 translate = sceneObject.GetComponent<Collider>().bounds.extents - sceneObject.GetComponent<Collider>().bounds.center;
            sceneObject.transform.position = new Vector3(item.Position.x + translate.x, translate.y, item.Position.y + translate.z);
        }

    }

    void Start()
    {
        BuildLevel();
    }
}
