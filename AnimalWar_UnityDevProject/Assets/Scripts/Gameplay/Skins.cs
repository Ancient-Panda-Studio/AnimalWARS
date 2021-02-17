using UnityEngine;

public class Skins : MonoBehaviour
{
    private static Transform _staticTransform;
    private static Sprite _sprite;

    private void Start()
    {
        SetTransfrom(transform);
    }

    public void SetTransfrom(Transform _transform)
    {
        _staticTransform = _transform;
    }

    public static void PopulateSkinScroller(string _name, string _price)
    {
        var skin = Instantiate(Resources.Load("Prefabs/SkinPrefab") as GameObject, _staticTransform, true);
        Debug.Log(skin);
        skin.transform.localScale = Vector3.one;
        skin.transform.localPosition = Vector3.zero;
        var script = skin.GetComponent<SkinScript>();
        script.price.text = _price;
        script.skinName.text = _name;
        script.sprite.sprite = _sprite;
    }

    public static void PopulateSkinImage(Sprite _sprite)
    {
        Skins._sprite = _sprite;
    }
}