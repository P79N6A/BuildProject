using UnityEngine;
using UnityEngine.UI;

namespace Sakura
{
    public interface IFindComponent
    {
        Text getText(string name, GameObject parent = null);
        RawImage getRawImage(string name, GameObject parent = null);
        Button getButton(string name, GameObject parent = null);
        Image getImage(string name, GameObject parent = null);
        GameObject getGameObject(string name, GameObject parent = null);
        T getComponent<T>(string path = "", GameObject go = null) where T:Component;
    }
}