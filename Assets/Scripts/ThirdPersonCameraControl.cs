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

    bool look = true;
    
    void Start() {
        Obstruction = Target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate() {
        CamControl();
        ViewObstructed();
    }
    

    void CamControl() {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed + incrementX;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        if (look)
            mouseXInterpolated = mouseXInterpolated - (mouseXInterpolated - mouseX) * (4f * Time.deltaTime);
        else
            mouseXInterpolated = mouseXInterpolated - (mouseXInterpolated - mouseX);
        // mouseYInterpolated = mouseYInterpolated - (mouseXInterpolated - mouseY) * (4f * Time.deltaTime);
        // mouseYInterpolated = Mathf.Clamp(mouseYInterpolated, -35, 60);
        // mouseYInterpolated = 0 - (mouseYInterpolated - (mouseYInterpolated - Player.position.y) * (10f * Time.deltaTime));

        transform.LookAt(Target);


        Player.rotation = Quaternion.Euler(0, mouseX, 0);
        Target.rotation = Quaternion.Euler(0 - Player.position.y / 2, mouseXInterpolated, 0);

        // aaaaa how to do this
        /*if (look)
            transform.positions.Translate(transform.positions.x, transform.positions.y, (transform.position.z - 7f) * (4f * Time.deltaTime));
        else
            transform.positions.Translate(transform.positions.x, transform.positions.y, (transform.position.z - 0f) * (4f * Time.deltaTime));*/

        if (Input.GetKey(KeyCode.Mouse1)) {
            look = !look;
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
