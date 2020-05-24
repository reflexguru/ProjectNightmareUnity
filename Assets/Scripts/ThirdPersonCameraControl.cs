using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    float rotationSpeed = 2;
    public Transform Target, Player;
    float mouseX, mouseY, mouseXInterpolated, mouseYInterpolated, incrementX;

    public Transform Obstruction;
    float zoomSpeed = 2f;
    int timer = 100;

    bool look = true;
    bool lookpress = false;
    bool ylook = false;

    public AudioSource asrc;
    public AudioClip camerain;
    public AudioClip cameraout;
    
    void Start() {
        Obstruction = Target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        asrc = GetComponent<AudioSource>();
    }

    private void LateUpdate() {
        CamControl();
        // ViewObstructed();
    }
    

    void CamControl() {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed + incrementX;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -60, 60);
        if (look)
            mouseXInterpolated = mouseXInterpolated - (mouseXInterpolated - mouseX) * (4f * Time.deltaTime);
        else
            mouseXInterpolated = mouseXInterpolated - (mouseXInterpolated - mouseX);
        // mouseYInterpolated = mouseYInterpolated - (mouseXInterpolated - mouseY) * (4f * Time.deltaTime);
        // mouseYInterpolated = Mathf.Clamp(mouseYInterpolated, -35, 60);
        // mouseYInterpolated = 0 - (mouseYInterpolated - (mouseYInterpolated - Player.position.y) * (10f * Time.deltaTime));

        transform.LookAt(Target);


        Player.rotation = Quaternion.Euler(0, mouseX, 0);
        if (!ylook)
            Target.rotation = Quaternion.Euler(0 - Player.position.y / 2, mouseXInterpolated, 0);
        else 
            Target.rotation = Quaternion.Euler(mouseY, mouseXInterpolated, 0);

        if (!look && timer >= 35) {
            ylook = true;
        }

        // placeholder without delta timing
        if (!look && timer < 35) {
            transform.Translate(0, 0, 0.2f);
            timer++;
        } else if (timer < 35) {
            transform.Translate(0, 0, -0.2f);
            timer++;
            ylook = false;
        }

        /*if (transform.position.z < 0f)
            transform.Translate(0, 0, 0.1f);*/

        if (Input.GetKey(KeyCode.Mouse1) && !lookpress) {
            if (look)
                asrc.PlayOneShot(camerain);
            else
                asrc.PlayOneShot(cameraout);
            look = !look;
            lookpress = true;
            timer = 0;
        }

        if (!Input.GetKey(KeyCode.Mouse1) && lookpress) {
            lookpress = false;
        }

        // Player.rotation = Player.rotation - (pRotation - Player.rotation) * 0.1;
        // Target.rotation = Target.rotation - (tRotation - Target.rotation) * 0.1;
    }

    private void Update() {
        incrementX = 0;

        if (Input.GetKey("a")) {
            incrementX -= 145 * Time.deltaTime;
        }

        if (Input.GetKey("d")) {
            incrementX += 145 * Time.deltaTime;
        }
    }
    

    void ViewObstructed() {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, 4.5f)) {
            if (hit.collider.gameObject.tag != "Player") {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                
                if (Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f)
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
            } else {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Vector3.Distance(transform.position, Target.position) < 4.5f)
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
            }
        }
    }
}
