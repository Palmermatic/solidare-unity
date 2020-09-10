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
        public GameObject CashLabel;

        /// <summary>
        /// The private var to hold the value
        /// </summary>
        private int cash;
        /// <summary>
        /// The public var to watch for changes
        /// </summary>
        public int Cash { get { return cash; } set { if (cash != value) { cash = value; OnPropertyChanged(ResourceType.Cash); } } }

        /// <summary>
        /// Declares the parameter types for methods that PropertyChanged can call
        /// </summary>
        protected delegate void ResourceChangedEventHandler(object sender, ResourceType type);
        /// <summary>
        /// List of methods that get called upon value change
        /// </summary>
        protected virtual event ResourceChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets called when the value changes
        /// </summary>
        protected void OnPropertyChanged(ResourceType type)
        {
            PropertyChanged?.Invoke(this, type);
        }

        /// <summary>
        /// The observing method
        /// </summary>
        private void UpdateTextLabel(object sender, ResourceType type)
        {
            CashLabel.GetComponent<TextMeshProUGUI>().text = "Cash: " + cash;
        }

        void Awake()
        {
            PropertyChanged += UpdateTextLabel; // wire up the event or 'subscribe'
            Cash = 100;
        }


    }
}
