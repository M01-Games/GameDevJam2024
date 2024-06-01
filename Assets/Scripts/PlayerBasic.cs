using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBasic : MonoBehaviour
{
        #region Variable
        //Variables (public can be changed in unity / private can ONLY be changed in script)
    public float minimumX = -60f; //The distance the camera can turn down
    public float maximumX = 60f; //The distance the camera can turn up
    public float camturnSpeed = 15f; //The senitivty of the camera
    public Camera cam; //The Camera
    public float rotX = 0f; //The amount the player looks left or right
    public float rotY = 0f; //The amount the player looks up or down
    public float walkSpeed = 5f; //this is the speed that the player will move at
    public float jumpHeight = 1f; //this is the height the player can jump up to
    public float gravity = -9.81f; //this is the gravity of the map
    private CharacterController controller; //the main charecter controlle
    private Vector3 velocity; //the speed the controller will move at
    private Vector3 moveDirection; //the direction the controller will move towards
    public float playerHealth = 100f; //This is the players health when they start the game
    public int range = 5; //The range that the player can interact with objects
    public GameObject firePoint; //Where the starting point of the ray is
        #endregion

        #region Methods
    void Start()
    {
        controller = GetComponent<CharacterController>(); //this gets the main charecter controller so that this script can access its values and movement
    }
    void Update()
    {
        CamLook(); //the camera movement task
        Walking(); //this runs movement task
        if(Input.GetKeyDown(KeyCode.Space) && controller.isGrounded) //Checks if the player pressed space if so this part runs
        {
            Debug.Log("Jumped"); //Tells the system to display the text in "..."
            Jump(); //This runs the jumping task
        }
        if(playerHealth <= 0) //This checks if the player's health equals 0 if so then this part runs
        {
            Debug.Log("PlayerDEAD"); //Tells the system to display the text in "..."
            Killplayer(); //This activates the players death
        }
        //Interact(); //Runs the interaction    
    }
    void CamLook () //the camera movement task
    {
        rotX += Input.GetAxis("Mouse Y") * camturnSpeed; //Input from mouse fore camera movement
        rotY += Input.GetAxis("Mouse X") * camturnSpeed; //Input from mouse fore camera movement
        rotX = Mathf.Clamp(rotX,minimumX,maximumX); //Calculations
        transform.localEulerAngles = new  Vector3 (0,rotY,0); //Task Executed (localEuleAngels-theses are the rotaion lines)
        cam.transform.localEulerAngles = new  Vector3 (-rotX,0,0); //sets the camera to the new position of the mouse
    }
        void Walking() //the movement task
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); //takes in the values of left and right movement with A , D
        float moveVertical = Input.GetAxisRaw("Vertical");   //takes in the values of up and backwords movement with W , S
        moveDirection = (moveHorizontal * transform.right + moveVertical * transform.forward); //this creates a new direction with all the new values the player has changed
        controller.Move(moveDirection * walkSpeed * Time.deltaTime);  //this moves the main charetect by using its controller and seting its new values to the new direction
        velocity.y += gravity * Time.deltaTime; //this keeps the player grounded while moving
        controller.Move(velocity * Time.deltaTime); //this sets the speed which the charecter will move to its new direction at
    }
    void Jump() //the jumping task
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //this makes the player jump to the certain height at the certain speed set by the script and then it pulls them back down with the gravity
    }
    void Killplayer() //This kills the player
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //This resets the whole scene
    }
    /*
    void Interact() //The interaction
    {
        RaycastHit hit; //Creates a storage of all the objects the player has hit with their ray
        Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit, range); //Creates the ray with a starting point, a direction, a way for the ray to be stopped and a range for the ray
        if(Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit, range)) //Checks if the ray has collided with somthing if so
        {
            Debug.Log(hit.transform.name); //Tells the system to display the objects name
            Interactables medkit = hit.transform.GetComponent<Interactables>(); //Creates a variable for the heal script within this script
            if(medkit != null) //Checks if the heal script is on the object if so
            {
                if(Input.GetKeyDown(KeyCode.F)) //Checks if the player clicked F if so
                {
                    medkit.Heal(); //Runs the medkit script
                }
            }
        }
    }
    */
        #endregion
}