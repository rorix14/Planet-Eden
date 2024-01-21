using UnityEngine;

namespace RPG.Core
{
    public class CamaraFacing : MonoBehaviour
    {
        [SerializeField] private bool freezeXZRotation = false;

        void LateUpdate()
        {
            if(!freezeXZRotation) transform.forward = Camera.main.transform.forward;
            else transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
