using UnityEngine;

public class RopeController : MonoBehaviour
{
   private LineRenderer lr;
   private Transform playPos;
   private Transform objPos;

   private void Awake()
   {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
   }

   public void setUpLine(Transform pPlayPos, Transform pObjPos)
   {
        playPos = pPlayPos;
        objPos = pObjPos;
   }

   public void Update()
   {
        if (objPos != null){
            lr.SetPosition(0, playPos.position);
            lr.SetPosition(1, objPos.position);
        }
        else
        {
            lr.SetPosition(0, Vector3.zero);
            lr.SetPosition(1, Vector3.zero);
        }
   }
}
