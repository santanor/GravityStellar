using System.Collections;
using GravitySystem;
using SelectSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Models
{
    public class Dot : GravityReceiver
    {
        Vector2 _dDir;
        Vector2 _vDir;

        public Color IdleColor;

        [Tooltip("Horizontal force magnitude")]
        public float InitialOrbitDeltaForce;

        [Tooltip("Duration of the orbital kick")]
        public float InitialOrbitKickDuration = 1f;

        [Tooltip("Vertical force magnitude")] public float InitialOrbitVertialForce;

        public Light Light;
        public Star ParentStar;

        public Selectable Selectable;
        public Color SelectedColor;

        void Awake()
        {
            Assert.IsNotNull(Selectable);
            Assert.IsNotNull(Light);

            Selectable.OnSelectedStatusChanged += OnSelectedStatusChanged;
        }

        /// <summary>
        ///     Changes the color of the lights indicating whether an item it's selected or not
        /// </summary>
        /// <param name="oldstatus"></param>
        /// <param name="newstatus"></param>
        void OnSelectedStatusChanged( Selectable.StatusEnum oldstatus, Selectable.StatusEnum newstatus )
        {
            switch (newstatus)
            {
                case Selectable.StatusEnum.Selected:
                    Light.color = SelectedColor;
                    break;
                case Selectable.StatusEnum.Idle:
                    Light.color = IdleColor;
                    break;
                default:
                    break;
            }
        }

        protected override void OnDotTooClose()
        {
            base.OnDotTooClose();
        }

        /// <summary>
        ///     Gives the dot the initial kick to put it in orbit of the parent star
        /// </summary>
        public void InitialOrbitKick()
        {
            _vDir = ( Random.insideUnitCircle - (Vector2) ParentStar.transform.position ).normalized;
            _dDir = ( (Vector2) Vector3.Cross(Vector3.forward, _vDir) ).normalized;

            //Perform an initial kick to move it from (0,0,0)
            Rigidbody2D.AddForce(_vDir);
            Rigidbody2D.AddForce(_dDir);

            StartCoroutine(OrbitalKickRoutine());
        }

        /// <summary>
        ///     Performs the initial force to put the Dot in orbit
        /// </summary>
        /// <returns></returns>
        IEnumerator OrbitalKickRoutine()
        {
            var ticker = 0f;

            while (ticker < InitialOrbitKickDuration)
            {
                transform.LookAt(ParentStar.transform.position);

                //Get the new up direction vector
                _vDir = -transform.forward;
                _dDir = ( (Vector2) Vector3.Cross(Vector3.forward, _vDir) ).normalized;

                //Lerps the forces giving more force towards the vertical direction at the begining
                //And more horizontal force at the end
                var vLerped = Mathf.Lerp(InitialOrbitVertialForce, 0, ticker / InitialOrbitKickDuration);
                var dLerped = Mathf.Lerp(0, InitialOrbitDeltaForce, ticker / InitialOrbitKickDuration);
                Rigidbody2D.AddForce(_vDir * vLerped);
                Rigidbody2D.AddForce(_dDir * dLerped);

                ticker += Time.deltaTime;
                yield return null;
            }
        }
    }
}
