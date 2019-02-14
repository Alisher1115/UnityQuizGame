using UnityEngine;
using System.Collections.Generic;

// Очень простой класс пула объектов
public class SimpleObjectPool : MonoBehaviour
{
    // префаб, который этот пул объектов возвращает экземпляры
    public GameObject prefab;

    // коллекция неактивных в настоящий момент экземпляров префаба
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    // Возвращает экземпляр префаба
    public GameObject GetObject() 
    {
        GameObject spawnedGameObject;

        // если есть неактивный экземпляр префаба, готовый к возврату, вернуть
        if (inactiveInstances.Count > 0) 
        {
            // удаляем экземпляр из коллекции неактивных экземпляров
            spawnedGameObject = inactiveInstances.Pop();
        }
        // в противном случае создайте новый экземпляр
        else 
        {
            spawnedGameObject = (GameObject)GameObject.Instantiate(prefab);

            // добавляем компонент PooledObject в префаб, чтобы мы знали, что он получен из этого пула
            PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }

        // включить экземпляр
        spawnedGameObject.SetActive(true);

        // возвращаем ссылку на экземпляр
        return spawnedGameObject;
    }

    // Возвращаем экземпляр префаба в пул
    public void ReturnObject(GameObject toReturn) 
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        // если экземпляр пришел из этого пула, вернуть его в пул
        if(pooledObject != null && pooledObject.pool == this)
        {
            // отключаем экземпляр
            toReturn.SetActive(false);
            
            // добавляем экземпляр в коллекцию неактивных экземпляров
            inactiveInstances.Push(toReturn);
        }
        // иначе просто уничтожь
        else
        {
            Debug.LogWarning(toReturn.name + " был возвращен в пул, из которого он не появился! Уничтожаю.");
            Destroy(toReturn);
        }
    }
}

// компонент, который просто идентифицирует пул, из которого пришел GameObject
public class PooledObject : MonoBehaviour
{
    public SimpleObjectPool pool;
}