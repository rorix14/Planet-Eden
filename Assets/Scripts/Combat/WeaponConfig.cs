using RPG.Attributes;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private Sprite imageUI;
        [SerializeField] private Sprite selectedImage;
        [SerializeField] Weapon equipedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float damagePercentageBonus = 0f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] bool isRightHand = true;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";

        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;
        public Projectile HasProjectile => projectile;
        public float DamagePercentageBonus => damagePercentageBonus;
        public Sprite ImageUI => imageUI;
        public Sprite SelectedImage => selectedImage;
        public float TimeBetweenAttacks => timeBetweenAttacks;

        public Weapon Spawn(Transform rigthHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rigthHand, leftHand);

            Weapon weapon = null;

            if (equipedPrefab)
            {
                weapon = Instantiate(equipedPrefab, GetTransform(rigthHand, leftHand));
                weapon.gameObject.name = weaponName;
            }

            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride) animator.runtimeAnimatorController = animatorOverride;
            else if (overrideController) animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        
            return weapon;
        }

        private void DestroyOldWeapon(Transform rigthHand, Transform leftHand)
        {
            Transform oldWeapon = rigthHand.Find(weaponName) ? rigthHand.Find(weaponName) : leftHand.Find(weaponName);
            if (!oldWeapon) return;
            oldWeapon.name = "DESTOYING";
            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculateDamage)
        {
            Projectile projectileInstance = FindObjectOfType<ConsumablePool>().GetItemFromPool(projectile.ItemClass, projectile).GetComponent<Projectile>();
            projectileInstance.transform.position = GetTransform(rightHand, leftHand).position;
            projectileInstance.transform.rotation = Quaternion.identity;
            projectileInstance.SetTarget(target, instigator, calculateDamage);
            projectileInstance.gameObject.SetActive(true);
        }

        private Transform GetTransform(Transform rigthHand, Transform leftHand) => isRightHand ? rigthHand : leftHand;
    }
}
