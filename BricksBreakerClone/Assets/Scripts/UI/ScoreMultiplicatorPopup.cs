using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMultiplicatorPopup : MonoBehaviour
{
    [SerializeField]
    private Button multiplier_1;
    [SerializeField]
    private Button multiplier_3;
    [SerializeField]
    private Button multiplier_5;

    public event Action<int> MultiplicatorSelected;

    private void Awake()
    {
        multiplier_1.onClick.AddListener(() => SelectMultiplicator(1));
        multiplier_3.onClick.AddListener(() => SelectMultiplicator(3));
        multiplier_5.onClick.AddListener(() => SelectMultiplicator(5));
    }
    
    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void SelectMultiplicator(int mult)
    {
        MultiplicatorSelected?.Invoke(mult);
    }

    private void Unsubscribe()
    {
        multiplier_1.onClick.RemoveAllListeners();
        multiplier_3.onClick.RemoveAllListeners();
        multiplier_5.onClick.RemoveAllListeners();
    }
    
}
