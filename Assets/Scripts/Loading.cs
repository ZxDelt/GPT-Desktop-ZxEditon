using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject load;

    public void On()
    {
        load.SetActive(true);
    }

    public void Off()
    {
        load.SetActive(false);
    }
}