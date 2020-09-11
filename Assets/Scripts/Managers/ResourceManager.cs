using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{

    public class ResourceManager : Singleton<ResourceManager>
    {


        public GameObject ResourceLabelPrefab, ResourceIconPrefab;
        public Dictionary<string, Resource> Resources = new Dictionary<string, Resource>();


        void Awake()
        {
            var container = GameObject.Find("ResourcesContainer");

            var icon = Instantiate(ResourceIconPrefab, container.transform).GetComponent<Image>();
            Resources.Add("Cash", Instantiate(ResourceLabelPrefab, container.transform).GetComponent<Resource>());
            Resources["Cash"].Icon = icon;

            icon = Instantiate(ResourceIconPrefab, container.transform).GetComponent<Image>();
            Resources.Add("Lives", Instantiate(ResourceLabelPrefab, container.transform).GetComponent<Resource>());
            Resources["Lives"].Icon = icon;

            icon = Instantiate(ResourceIconPrefab, container.transform).GetComponent<Image>();
            Resources.Add("Attack", Instantiate(ResourceLabelPrefab, container.transform).GetComponent<Resource>());
            Resources["Attack"].Icon = icon;

            icon = Instantiate(ResourceIconPrefab, container.transform).GetComponent<Image>();
            Resources.Add("Defense", Instantiate(ResourceLabelPrefab, container.transform).GetComponent<Resource>());
            Resources["Defense"].Icon = icon;


            Resources["Cash"].NewResource("Cash", 100, "ace_of_diamonds");
            Resources["Lives"].NewResource("Lives", 3, "ace_of_hearts");
            Resources["Attack"].NewResource("Attack", 3, "ace_of_clubs");
            Resources["Defense"].NewResource("Defense", 3, "ace_of_spades");

        }
    }
}
