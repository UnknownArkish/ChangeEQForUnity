using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 比较高层级的人物管理系统
/// 负责人物的创建、以及对其的管理（删除等）
/// </summary>
public class UCharacterManager  {
	private UCombineSkinnedMgr skinnedMgr = null;
    // 合成系统实例
    public UCombineSkinnedMgr CombineSkinnedMgr { get{ return skinnedMgr; } }

	private int characterIndex = 0;
	private Dictionary<int, UCharacterController> characterDic = 
        new Dictionary<int, UCharacterController>();

	public UCharacterManager () 
    {

		skinnedMgr = new UCombineSkinnedMgr ();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skeleton"> 骨骼 </param>
    /// <param name="weapon"> 武器 </param>
    /// <param name="head"> 头部 </param>
    /// <param name="chest"> 胸部 </param>
    /// <param name="hand"> 手部 </param>
    /// <param name="feet"> 脚部 </param>
    /// <param name="combine"></param>
    /// <returns></returns>
	public UCharacterController Generatecharacter (
        string skeleton, 
        string weapon, string head, string chest, string hand, string feet, 
        bool combine = false)
	{

		UCharacterController instance = new UCharacterController (characterIndex,
            skeleton,weapon,head,chest,hand,feet,combine);

		characterDic.Add(characterIndex,instance);
		characterIndex++;

		return instance;
	}

	public void Removecharacter (CharacterController character)
	{

	}

	public void Update () 
    {
		foreach(UCharacterController character in characterDic.Values)
		{
			character.Update();
		}
	}
}
