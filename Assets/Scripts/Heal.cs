namespace VRTK.Examples
{
    using UnityEngine;

    public class Heal : MonoBehaviour
    {
        public VRTK_ControllerEvents leftController;
        public VRTK_ControllerEvents rightController;
        private Player playerScript;
        private Potion potion;

        private void Start()
        {
            playerScript = GetComponent<Player>();
            potion = GetComponent<Potion>();
        }
        protected bool state;

        protected virtual void OnEnable()
        {
            state = false;
            RegisterEvents(leftController);
            RegisterEvents(rightController);
        }

        protected virtual void RegisterEvents(VRTK_ControllerEvents events)
        {
            if (events != null)
            {
                events.ButtonTwoPressed += ButtonTwoPressed;
            }
        }

        protected virtual void ButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
        {
            state = !state;
            if (potion.GetPotionCount() > 0)
            {
                FindObjectOfType<AudioManager>().Play("Drinking");
                Debug.Log("heal");
                playerScript.playerHealth += 30f;
                potion.RemovePotion();
            }
        }
    }
}