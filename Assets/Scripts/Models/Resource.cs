using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models
{
    public class Resource : MonoBehaviour
    {
        protected delegate void ResourceChangedEventHandler(object sender, string propertyName);
        protected virtual event ResourceChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, propertyName);
        }

        private int _value;
        public int Value
        {
            get { return _value; }
            set { if (_value != value) { _value = value; OnPropertyChanged("Value"); } }
        }

        public string Name;
        public Image Icon;

        private void UpdateTextLabel(object sender, string propertyName)
        {
            GetComponent<TextMeshProUGUI>().text = Name + ": " + Value;
        }

        private void Awake()
        {
            PropertyChanged += UpdateTextLabel;
        }

        public void NewResource(string name, int val, string spriteName)
        {
            this.name = name;
            Name = name;
            Value = val;
            Icon.sprite = Resources.Load<Sprite>(spriteName);
            Icon.name = name + "Icon";
        }
    }
}
