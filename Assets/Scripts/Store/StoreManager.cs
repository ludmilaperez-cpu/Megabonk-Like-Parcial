using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [Serializable]
    public class StorePage
    {
        public string name;
        public Button tabButton;
        public GameObject pageRoot;
    }

    [SerializeField] List<StorePage> pages = new List<StorePage>();
    [SerializeField] int startPage = 0;
    [SerializeField] Color selectedColor = Color.white;
    [SerializeField] Color unselectedColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    void Start()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            int index = i;
            pages[i].tabButton.onClick.AddListener(() => ShowPage(index));
        }
        ShowPage(startPage);
    }

    public void ShowPage(int index)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            bool selected = i == index;
            pages[i].pageRoot.SetActive(selected);
            pages[i].tabButton.image.color = selected ? selectedColor : unselectedColor;
        }
    }
}
