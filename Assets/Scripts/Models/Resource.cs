using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public enum ResourceType { Cash, Lives }

    public class Resource : MonoBehaviour
    {
        public string Name;
        public ResourceType Type;
        public int Value;
        public Sprite Icon;

        private void Awake()
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = Name + ": " + Value;
        }
    }

}
