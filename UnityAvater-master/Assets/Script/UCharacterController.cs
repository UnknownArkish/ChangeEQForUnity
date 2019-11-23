using UnityEngine;
using System.Collections;

public class UCharacterController
{
    /// <summary>
    /// GameObject reference
    /// </summary>
	public GameObject Instance = null;              // 人物角色实例（有两个子节点，一个是骨架、一个是Mesh）
    public GameObject WeaponInstance = null;

    /// <summary>
    /// The unique id in the scene
    /// </summary>
	public int index;

    /// <summary>
    /// Equipment informations
    /// 装备名字
    /// </summary>
	public string skeleton;
    public string equipment_head;
    public string equipment_chest;
    public string equipment_hand;
    public string equipment_feet;

    /// <summary>
    /// Other vars
    /// </summary>
	public bool rotate = false;

    private Animation animationController = null;
    public int animationState = 0;

    /// <summary>
    /// 把身体各部件组合（头，胸，手，脚）在一起，并将武器添加到挂点上。身体各部件组合在一起
    /// 包含了mesh和material合并功能。
    /// 每个身体部件都是单独的prefab，并带了SkinnedMeshRenderer
    /// </summary>
	public UCharacterController(int index, string skeleton, string weapon,
        string head, string chest, string hand, string feet, bool combine = false)
    {
        this.index = index;

        InitSkinnedMesh( skeleton, head, chest, hand, feet, combine);
        InitWeapon(weapon);

        // Only for display
        animationController = Instance.GetComponent<Animation>();
        PlayStand();
    }
    private void InitWeapon(string weapon)
    {
        // Create weapon
        Object res = Resources.Load("Prefab/" + weapon);
        WeaponInstance = GameObject.Instantiate(res) as GameObject;

        Transform[] transforms = Instance.GetComponentsInChildren<Transform>();
        foreach (Transform joint in transforms)
        {
            if (joint.name == "weapon_hand_r")
            {
                // find the joint (need the support of art designer)
                WeaponInstance.transform.parent = joint.gameObject.transform;
                break;
            }
        }

        // Init weapon relative informations
        WeaponInstance.transform.localScale = Vector3.one;
        WeaponInstance.transform.localPosition = Vector3.zero;
        WeaponInstance.transform.localRotation = Quaternion.identity;
    }

    private void InitSkinnedMesh(string skeleton,string head, string chest, string hand, string feet, bool combine = false)
    {
        this.skeleton = skeleton;
        Object instance = Resources.Load("Prefab/" + skeleton);
        this.Instance = GameObject.Instantiate(instance) as GameObject;

        this.equipment_head = head;
        this.equipment_chest = chest;
        this.equipment_hand = hand;
        this.equipment_feet = feet;

        ForceCombineSkinnedMesh(combine);
    }

    public void ChangeHeadEquipment(string equipment, bool combine = false)
    {
        ChangeEquipment(0, equipment, combine);
    }

    public void ChangeChestEquipment(string equipment, bool combine = false)
    {
        ChangeEquipment(1, equipment, combine);
    }

    public void ChangeHandEquipment(string equipment, bool combine = false)
    {
        ChangeEquipment(2, equipment, combine);
    }

    public void ChangeFeetEquipment(string equipment, bool combine = false)
    {
        ChangeEquipment(3, equipment, combine);
    }

    public void ChangeWeapon(string weapon)
    {
        Object res = Resources.Load("Prefab/" + weapon);
        GameObject oldWeapon = WeaponInstance;
        WeaponInstance = GameObject.Instantiate(res) as GameObject;
        WeaponInstance.transform.parent = oldWeapon.transform.parent;
        WeaponInstance.transform.localPosition = Vector3.zero;
        WeaponInstance.transform.localScale = Vector3.one;
        WeaponInstance.transform.localRotation = Quaternion.identity;

        GameObject.Destroy(oldWeapon);
    }

    public void ChangeEquipment(int index, string equipment, bool combine = false)
    {
        switch (index)
        {
            case 0:
                equipment_head = equipment;
                break;
            case 1:
                equipment_chest = equipment;
                break;
            case 2:
                equipment_hand = equipment;
                break;
            case 3:
                equipment_feet = equipment;
                break;
        }
        ForceCombineSkinnedMesh(combine);
    }

    private void ForceCombineSkinnedMesh( bool combine )
    {
        string[] equipments = new string[4];
        equipments[0] = equipment_head;
        equipments[1] = equipment_chest;
        equipments[2] = equipment_hand;
        equipments[3] = equipment_feet;

        Object res = null;
        SkinnedMeshRenderer[] meshes = new SkinnedMeshRenderer[4];
        GameObject[] objects = new GameObject[4];
        for (int i = 0; i < equipments.Length; i++)
        {
            res = Resources.Load("Prefab/" + equipments[i]);
            objects[i] = GameObject.Instantiate(res) as GameObject;
            meshes[i] = objects[i].GetComponentInChildren<SkinnedMeshRenderer>();
        }

        App.Game.CharacterMgr.CombineSkinnedMgr.CombineSkinnedMesh(
            Instance, meshes, combine);

        for (int i = 0; i < objects.Length; i++)
        {
            GameObject.DestroyImmediate(objects[i].gameObject);
        }
    }

    public void PlayStand()
    {
        animationController.wrapMode = WrapMode.Loop;
        animationController.Play("breath");
        animationState = 0;
    }

    public void PlayAttack()
    {
        animationController.wrapMode = WrapMode.Once;
        animationController.PlayQueued("attack1");
        animationController.PlayQueued("attack2");
        animationController.PlayQueued("attack3");
        animationController.PlayQueued("attack4");
        animationState = 1;
    }

    // Update is called once per frame
    public void Update()
    {
        if (animationState == 1)
        {
            if (!animationController.isPlaying)
            {
                PlayAttack();
            }
        }
        if (rotate)
        {
            Instance.transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
        }
    }
}