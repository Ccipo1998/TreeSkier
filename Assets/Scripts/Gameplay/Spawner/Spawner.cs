using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    public Transform LeftSpawn;
    public Transform RightSpawn;

    public Transform LeftSnow;
    public Transform RightSnow;

    public Transform FogPoint;

    [SerializeField]
    private int _NumSpawn = 3;
    [SerializeField]
    private int _MaxNumSpawn;
    [SerializeField]
    private int _MinNumSpawn;

    [SerializeField]
    private float _HeightOfWindowSnow;
    [SerializeField]
    private float _HeightOfWindow;

    [SerializeField]
    private List<TypeScriptableObject> _ListOfTypeObject;

    [SerializeField]
    private float _OffsetWidth;
    [SerializeField]
    private float _OffsetHeight;
    [SerializeField]
    private float _OffsetSnow;
    [SerializeField]
    private float _newPosToSum;

    [SerializeField, Range(0f,1f)]
    private float _probSpawnEachWindow;

    [SerializeField]
    private idContainer _IdSpawnContainerPoolManager;

    [SerializeField]
    private idContainer _IdSnowflakesPoolManager;

    [SerializeField]
    private idContainer _IdFogPoolManager;

    [Header("Probabilities")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _InitialProbabilityRampL;
    [SerializeField]
    [Range(0f, 1f)]
    private float _InitialProbabilityRampM;
    [SerializeField]
    [Range(0f, 1f)]
    private float _InitialProbabilityRampS;

    [Header("Intercept X when L probability is bigger")]
    [SerializeField]
    private float _InterceptX;

    private List<PoolableObject> _poolableObjectsPool;
    private Dictionary<TypeScriptableObject, float> _DictProb;
    private Dictionary<string, PoolManager> _DictPoolManagers;

    public int GetNumSpawn()
    {
        return _NumSpawn;
    }

    public int GetMaxNumSpawn()
    {
        return _MaxNumSpawn;
    }

    public float GetWindowHeight()
    {
        return _HeightOfWindow;
    }

    public float GetMinWindowHeight()
    {
        return _OffsetHeight;
    }

    public void SetUp()
    {
        _poolableObjectsPool = new List<PoolableObject>();
        _DictPoolManagers = new Dictionary<string, PoolManager>();
        _DictProb = new Dictionary<TypeScriptableObject, float>();
        int dim = _ListOfTypeObject.Count;
        for (int i = 0; i < dim; i++)
        {
            TypeScriptableObject typeObject = _ListOfTypeObject[i];

            _DictProb.Add(typeObject, 1 - typeObject.Rarity);

            foreach (idContainer id in typeObject.IdPoolManagers)
            {
                _DictPoolManagers.Add(id.Id, PoolingSystem.Instance.GetPoolManager(id));
            }
            
        }
        
        NormalizeDictonary();
    }
    public void SetSpawn(int value) 
    {
        if (value > _MaxNumSpawn)
        {
            _NumSpawn = _MaxNumSpawn;
            return;
        }

        if (value < _MinNumSpawn)
        {
            _NumSpawn = _MinNumSpawn;
            return;
        }

        if ((RightSpawn.position.x - LeftSpawn.position.x) / value < _OffsetWidth) return;

        _NumSpawn = value;
    }
    public void SetProbSpawnEachWindow(float value)
    {
        if (value > 1.0f) _probSpawnEachWindow = 1.0f;
        if (value < 0.0f) _probSpawnEachWindow = 0.0f;
        _probSpawnEachWindow = value;
    }
    public void DecreaseHeightOfWindow(float decrease)
    {
        _HeightOfWindow -= decrease;

        if (_HeightOfWindow < _OffsetHeight*2)
            _HeightOfWindow = _OffsetHeight*2;
    }
    public void Spawn(int index = -1) {
        float x1 = LeftSpawn.position.x;
        float y1 = LeftSpawn.position.y;
        float x2 = RightSpawn.position.x;
        float y2 = RightSpawn.position.y;

        float squareWidth = (x2 - x1)/_NumSpawn;
        float m = (y1 - y2) / (x1 - x2);
        float q = ((x1 * y2) - (x2 * y1)) / (x1 - x2);

        int emptyWindow;
        if (index == -1) emptyWindow = Random.Range(0, _NumSpawn);
        else emptyWindow = index;

        List<SlopeObject> newlist = new List<SlopeObject>();

        int randomLeftOrRight= Random.Range(0, 2);
        for (int i=0; i<=_NumSpawn-1; ++i)
        {
            //rectangle empty selectable by a new method, create a SpawnContainer  
            if (emptyWindow!=i)
            {
                if (Random.value <= _probSpawnEachWindow) 
                {
                    TypeScriptableObject keySelected = SelectARandomName();
                    SlopeObject obj = TakeObjectFromPool(keySelected);
                    ChangeProbabilitys(keySelected);
                    obj.transform.position = RandomPosForGameObject(m * (x1 + squareWidth * i) + q, m * (x1 + squareWidth * i) + q - _HeightOfWindow, x1 + squareWidth * i, x1 + squareWidth * (i + 1), _OffsetHeight, _OffsetWidth, randomLeftOrRight);
                    newlist.Add(obj);
                    _poolableObjectsPool.Add(obj);
                }
            }
            
        }

        PoolManager poolManager = PoolingSystem.Instance.GetPoolManager(_IdSpawnContainerPoolManager);
        SpawnContainer spawnContainer = poolManager.RequestPooledObject<SpawnContainer>();
        spawnContainer.transform.position = new UnityEngine.Vector3(LeftSpawn.position.x, LeftSpawn.position .y -_HeightOfWindow, LeftSpawn.position.z);
        spawnContainer.SetSpawnedObjects(newlist);
    }
    private SlopeObject TakeObjectFromPool(TypeScriptableObject keyselected)
    {

        string IdSelected = keyselected.IdPoolManagers[0].Id;
        
        if (keyselected.MyID.Id == "Ramp")
        {
            float difficulty = LevelSystem.Instance.GetDifficulty();
            float InterceptY = _InterceptX / 2;
            float probToAddToM = difficulty / 2;
            float probToAddToL = (InterceptY / Mathf.Exp(_InterceptX)) * Mathf.Exp(difficulty);

            float _probabilityL = _InitialProbabilityRampL + probToAddToL;
            float _probabilityM = _InitialProbabilityRampM + probToAddToM;
            float _probabilityS = _InitialProbabilityRampS;
            float total = _probabilityL + _probabilityM;

            _probabilityL = _probabilityL / total;
            _probabilityM = _probabilityM / total;
            _probabilityS = _probabilityS / total;

            float random = Random.value;
            if (random < _probabilityS)
            {
                IdSelected = keyselected.IdPoolManagers[0].Id;
            }
            else if (random < _probabilityM + _probabilityS)
            {
                IdSelected = keyselected.IdPoolManagers[1].Id;
            }
            else
            {
                IdSelected = keyselected.IdPoolManagers[2].Id;
            }
        }

        PoolManager poolManager = _DictPoolManagers[IdSelected];
        SlopeObject obj = null;
        
        switch (IdSelected)
        {
            case "Coin":
                obj = poolManager.RequestPooledObject<Coin>();
                break;

            case "Bomb":
                obj = poolManager.RequestPooledObject<Bomb>();
                break;

            case "Heart":
                obj = poolManager.RequestPooledObject<Heart>();
                break;

            case "Tree1":
                obj = poolManager.RequestPooledObject<Tree>();
                break;

            case "Tree2":
                obj = poolManager.RequestPooledObject<Tree>();
                break;

            case "Tree3":
                obj = poolManager.RequestPooledObject<Tree>();
                break;

            case "X2":
                obj = poolManager.RequestPooledObject<X2>();
                break;

            case "RampS":
                obj = poolManager.RequestPooledObject<Ramp>();
                break;

            case "RampM":
                obj = poolManager.RequestPooledObject<Ramp>();
                break;

            case "RampL":
                obj = poolManager.RequestPooledObject<Ramp>();
                break;
        }
        return obj;
    }
    private TypeScriptableObject SelectARandomName()
    {
        float random = Random.value;
        float cumulative = 0.0f;
        foreach (TypeScriptableObject key in _DictProb.Keys)
        {
            cumulative += _DictProb[key];
            if (random < cumulative)
            {
                return key;
            }
        }
        return null;
    }
    private void ChangeProbabilitys(TypeScriptableObject keySelected)
    {
        float probToSubtract = _DictProb[keySelected] * keySelected.Rarity;
        _DictProb[keySelected] = _DictProb[keySelected] - probToSubtract;
        foreach (TypeScriptableObject key in _DictProb.Keys.ToList())
        {
            if (keySelected != key)
            {
                _DictProb[key] = _DictProb[key] + (1-key.Rarity) * probToSubtract;
            }
        }
        NormalizeDictonary();
    }
    private UnityEngine.Vector2 RandomPosForGameObject(float UpY, float DownY,float leftX, float rightX,float heightOfTheSprite, float widthOfTheSprite, int LeftOrRight) //LeftOrRight 0 left, 1 right
    {
        UnityEngine.Vector2 pos = new UnityEngine.Vector2();

        if (UpY - heightOfTheSprite > DownY + heightOfTheSprite)
        {
            pos.y = Random.Range(UpY - heightOfTheSprite, DownY + heightOfTheSprite);
        }
        else
        {
            pos.y = UpY - heightOfTheSprite;
        }

        if (LeftOrRight == 0)
        {
            if (leftX < rightX - widthOfTheSprite)
            {
                pos.x = Random.Range(leftX, rightX - widthOfTheSprite); //pos.x = Random.Range(leftX + widthOfTheSprite, rightX - widthOfTheSprite);
            }
            else
            {
                pos.x = leftX;
            }
        }
        else
        {
            if (leftX + widthOfTheSprite < rightX)
            {
                pos.x = Random.Range(leftX + widthOfTheSprite, rightX); //pos.x = Random.Range(leftX + widthOfTheSprite, rightX - widthOfTheSprite);
            }
            else
            {
                pos.x = rightX;
            }
        }
        
        return pos;
    }
    private void NormalizeDictonary()
    {
        float total = SumValues();
        foreach (TypeScriptableObject key in _DictProb.Keys.ToList())
        {
            _DictProb[key] /= total;
        }
    }
    private float SumValues()
    {
        float total = 0;
        foreach (float values in _DictProb.Values)
        {
            total += values;
        }
        return total;
    }

    public List<Snowflake> SpawnSnowFlakes(int numberSnowflakes)
    {
        float x1 = LeftSnow.position.x;
        float y1 = LeftSnow.position.y;
        float x2 = RightSnow.position.x;
        float y2 = RightSnow.position.y;

        float squareWidth = (x2 - x1) / numberSnowflakes;
        float squareHeight = _HeightOfWindowSnow / numberSnowflakes;

        PoolManager poolManager = PoolingSystem.Instance.GetPoolManager(_IdSnowflakesPoolManager);
        List<Snowflake> listOfSnow = new List<Snowflake>();
        List<int> numberList = Enumerable.Range(1, numberSnowflakes * numberSnowflakes).ToList();
        System.Random rnd = new System.Random();
        numberList = numberList.OrderBy(_ => rnd.Next()).ToList<int>();
        
        for (int i = 0; i < numberSnowflakes; ++i)
        {
            Snowflake snowflake = poolManager.RequestPooledObject<Snowflake>();
            int numberPosition = numberList[0] - 1;
            numberList.RemoveAt(0);
            snowflake.transform.position = RandomPosForGameObject(y1 + squareHeight + squareHeight * (numberPosition / numberSnowflakes), y1 + squareHeight * (numberPosition / numberSnowflakes), x1 + squareWidth * (numberPosition % numberSnowflakes), x1 + squareWidth + squareWidth * (numberPosition % numberSnowflakes), _OffsetSnow, _OffsetSnow, 1);
            listOfSnow.Add(snowflake);
        }
        return listOfSnow;
    }

    [ContextMenu("SpawnFog")]
    public PoolableObject SpawnFog()
    {
        PoolManager poolManager = PoolingSystem.Instance.GetPoolManager(_IdFogPoolManager);
        
        PoolableObject fog = poolManager.RequestPooledObject<PoolableObject>();
        
        fog.transform.position = FogPoint.position;

        return fog;
    }
}
