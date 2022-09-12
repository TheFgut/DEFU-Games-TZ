using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Material playerMaterial;
    public GameObject animObj;
    Vector3 idleRotation;
    Vector3 needRot;

    public Map map;
    public static Player instance;
    [SerializeField]
    private playerSettings parameters;
    [System.Serializable]
    class playerSettings
    {
        public int power;
        public int maxPower;


        public float speed;

        public float manouvreSpeed;

        public GameObject waterFallParticles;
        public GameObject DeadParticles = null;
    }

    CharacterController charC;
    void Awake()
    {
        idleRotation = animObj.transform.rotation.eulerAngles;
        instance = this;
        charC = GetComponent<CharacterController>();
        score = 0;
    }
    void Start()
    {
        playerColor.Init(this);
    }
    void Update()
    {
        float speed = parameters.speed * Time.deltaTime;
        Vector3 moveVect = manouvreVect + new Vector3(0, 0, speed);

        float rotation = Vector3.SignedAngle(new Vector3(0,0,1), moveVect,new Vector3(0,1,0));
        needRot = idleRotation + new Vector3(0, rotation, 0);
        animObj.transform.rotation = Quaternion.Lerp(animObj.transform.rotation,Quaternion.Euler(needRot),Time.deltaTime * 3);
        charC.Move(moveVect);
        manouvreVect = new Vector3();

        //fall Check
        RaycastHit hit;
        Physics.Raycast(transform.position,new Vector3(0,-1,0),out hit,50,LayerMask.GetMask("Default", "Water"));
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            StartCoroutine(WaterFallAnim(moveVect));
            this.enabled = false;
        }
    }

    IEnumerator WaterFallAnim(Vector3 dir)
    {
        charC.enabled = false;
        float fallSpeed = 0.1f;
        float rotSpeed = 1;
        Vector3 fallVect = new Vector3(0, 0, 0);
        bool particlesSpawned = false;
        GameObject particleSys = null;
        do
        {
            fallVect += new Vector3(0, -fallSpeed, 0) * Time.deltaTime;
            dir -= dir * (Time.deltaTime/5);
            Vector3 flyVect = dir + fallVect;
            transform.position += flyVect;
            animObj.transform.Rotate(-Quaternion.FromToRotation(new Vector3(0,1,0), flyVect).eulerAngles * rotSpeed * Time.deltaTime);
            if (particlesSpawned == false && transform.position.y < 0)
            {
                particleSys = Instantiate(parameters.waterFallParticles);
                particleSys.transform.position = transform.position;
                particlesSpawned = true;
            }
            yield return new WaitForEndOfFrame();
        } while (transform.position.y > -charC.height - 0.3f);
        yield return new WaitForSeconds(1.5f);
        GameInterfaces.instance.Death();
        Destroy(particleSys);

    }

    Vector3 manouvreVect;
    public void Manouvre(float dir)
    {
        float power = dir;
        if (Mathf.Abs(power) > parameters.manouvreSpeed)
        {
            power = parameters.manouvreSpeed * Mathf.Sign(power);
        }
        power *= Time.fixedDeltaTime;
        manouvreVect = new Vector3(power, 0, 0);
    }

    public PlayerColorModule playerColor = new PlayerColorModule();
    public class PlayerColorModule
    {
        GameColor PlayerColor;
        Player playerScr;
        public void Init(Player player)
        {
            playerScr = player;
            SetColor(Map.instance.ColorsModule.GetRandom());
        }

        public void SetColor(GameColor color)
        {
            PlayerColor = color;
            playerScr.playerMaterial.color = PlayerColor.color;
        }
        public Color GetColor()
        {
            return PlayerColor.color;
        }
        public bool Check(GameColor col)
        {
            if (col.ID == PlayerColor.ID)
            {
                return true;
            }
            return false;
        }
    }

    int HP = 0;
    public static int score = 0;

    static GameObject deadPInstance;
    public void Die()
    {
        gameObject.SetActive(false);
        deadPInstance = Instantiate(parameters.DeadParticles);
        ParticleSystem.MainModule particlSys = deadPInstance.GetComponent<ParticleSystem>().main;
        particlSys.startColor = playerColor.GetColor();
        deadPInstance.transform.position = transform.position;
        GameInterfaces.instance.Death();
    }
    public void GotPotion(GameColor color)
    {
        if (playerColor.Check(color))
        {
            HP++;
            score++;
        }
        else
        {
            HP--;
        }


        if (HP < 0)
        {
            Die();
        }
        if (HP > 10)
        {
            HP = 10;
        }
        transform.localScale = new Vector3(1, 1, 1) * (1 + HP / 20);
    }
}
