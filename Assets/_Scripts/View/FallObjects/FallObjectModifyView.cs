using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectModifyView : MonoBehaviour, IFallingObject
    {
        private void Start()
        {
            InitModify();
        }

        public void ApplyMod()
        {
            Debug.Log("Изменить поведение игрока");
        }

        public void DropObject()
        {
            gameObject.SetActive(false);
        }

        private void InitModify()
        {

        }
    }
}
