using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class SpawnDino : MonoBehaviour
{
    private GameObject TRex;
    private Animator trexAnim;
    private GameObject Triceratops;
    private Animator tricAnim;

    private ARPlaneManager arPlaneManager;
    private ARPlane arPlane;

    [SerializeField] private Camera arCamera;

    private Vector2 touchPosition = default;

    private bool isEnableToSpawn = false;
    private bool isCanSetupPlane = false;

    [SerializeField] private Button ScanningButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string[] buttonTexts;

    [SerializeField] private GameObject TRexPrefab;
    [SerializeField] private GameObject TriceratopsPrefab;
    [SerializeField] private GameObject EnvPrefab;

    [SerializeField] private DinoText dialogueSystem;

    void Awake() 
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        if(ScanningButton != null)
        {
            buttonText.text = buttonTexts[0];
            ScanningButton.onClick.AddListener(placeEnvironement);
            ScanningButton.interactable = false;
        }
    }

    void Start() 
    {
        StartCoroutine(CheckPlaneExist());
    }
    private void placeEnvironement()
    {
        StartCoroutine(PlaceEnv());
    }
    
    private void PlaceDino()
    {
        StopAllCoroutines();
        StartCoroutine(DinoCreation());
    }

    IEnumerator CheckPlaneExist()
    {
        while(GameObject.FindGameObjectWithTag("Floor") == null)
        {
            yield return null;
        }

        arPlane = GameObject.FindGameObjectWithTag("Floor").GetComponent<ARPlane>();
        yield return null;

        StartCoroutine(CheckPlaneSize());
        yield break;
    }

    IEnumerator CheckPlaneSize()
    {
        while(arPlane.extents.y < 5f && arPlane.extents.x < 5f)
        {
            yield return null;
        }

        buttonText.text = buttonTexts[1];
        ScanningButton.interactable = true;
        yield break;
    }

    IEnumerator PlaceEnv()
    {
        arPlane.GetComponent<LineRenderer>().enabled = false;
        arPlane.GetComponent<MeshRenderer>().enabled = false;
        arPlaneManager.enabled = false;
        yield return null;

        EnvPrefab.SetActive(true);
        yield return null;

        buttonText.text = buttonTexts[2];
        ScanningButton.onClick.RemoveListener(placeEnvironement);
        ScanningButton.onClick.AddListener(PlaceDino);
        yield break;
    }

    private IEnumerator WalkTowardsPlayer(GameObject walkingDino, Animator dinoAnimator, Vector3 position, float speed)
    {
        yield return new WaitForSeconds(2f);
        
        position.y = walkingDino.transform.position.y;
        float startTime = Time.time;
        float length = Vector3.Distance(walkingDino.transform.position, position);

        dinoAnimator.SetBool("isWalking", true);
        while(walkingDino.transform.position.z > (position.z + .02f))
        {
            float distCovered = (Time.time - startTime) * speed;
            float fraction = distCovered / length;
            walkingDino.transform.position = Vector3.Lerp(walkingDino.transform.position, position, fraction);
            yield return null;
        }

        dinoAnimator.SetBool("isWalking", false);
        yield break;
    }

    private IEnumerator DinoCreation()
    {
        isEnableToSpawn = false;
        ScanningButton.gameObject.SetActive(false);

        yield return null;

        //TRex creation

        TRex = Instantiate(TRexPrefab, new Vector3(arPlane.center.x + 2.1f, 2f, arPlane.center.z + 4.5f), Quaternion.Euler(0, -139,0)) as GameObject;
        trexAnim = TRex.GetComponent<Animator>();

        yield return StartCoroutine(WalkTowardsPlayer(TRex, trexAnim, new Vector3(.68f, 0f, 4.185f), .04f));

        yield return new WaitForSeconds(.5f);

        //Triceratops creation

        Triceratops = Instantiate(TriceratopsPrefab, new Vector3(arPlane.center.x - 2.05f, 2f, arPlane.center.z + 4.5f), Quaternion.Euler(0, 139, 0)) as GameObject;
        tricAnim = Triceratops.GetComponent<Animator>();

        yield return StartCoroutine(WalkTowardsPlayer(Triceratops, tricAnim, new Vector3(-.5f, 0f, 4.185f), .01f));

        yield return new WaitForSeconds(.5f);

        dialogueSystem.StartDialogue();

        yield break;
    }

    public void HeToldNO()
    {
        StartCoroutine(NOAnswerCase());
    }

    private IEnumerator NOAnswerCase()
    {
        trexAnim.SetBool("isAttacking", true);

        while(TRex.transform.position.x > .35f) //one step forward because attack animation has step movement
        {
            TRex.transform.Translate(Vector3.forward * Time.deltaTime);
            yield return null;
        }    

        yield return new WaitForSeconds(1.52f);
        trexAnim.SetBool("isAttacking", false);

        tricAnim.SetBool("isDead", true);
        yield return new WaitForSeconds(2.1f);
        trexAnim.SetBool("isEating", true);
    }

    public void HeToldYes()
    {
        trexAnim.SetBool("isRoaring", true);
    }
}