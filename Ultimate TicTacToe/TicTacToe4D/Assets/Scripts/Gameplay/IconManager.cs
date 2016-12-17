using UnityEngine;
using System.Collections;

public class IconManager : MonoBehaviour
{
	public Sprite [] IconList;

	void Start ()
	{
		Init();
	}

	public void Init()
	{
		IconList = new Sprite[(int)Defines.ICONS.TOTAL];

		IconList[(int)Defines.ICONS.EMPTY]		= Resources.Load<Sprite>("Icons/Empty") as Sprite;
		IconList[(int)Defines.ICONS.HIGHLIGHT]	= Resources.Load<Sprite>("Icons/Highlight") as Sprite;
		IconList[(int)Defines.ICONS.LOCKED]		= Resources.Load<Sprite>("Icons/Locked") as Sprite;

		IconList[(int)Defines.ICONS.CIRCLE]		= Resources.Load<Sprite>("Icons/Circle") as Sprite;
		IconList[(int)Defines.ICONS.CROSS]		= Resources.Load<Sprite>("Icons/Cross") as Sprite;

		IconList[(int)Defines.ICONS.SPADE]		= Resources.Load<Sprite>("Icons/Spade") as Sprite;
		IconList[(int)Defines.ICONS.HEART]		= Resources.Load<Sprite>("Icons/Heart") as Sprite;
		IconList[(int)Defines.ICONS.CLUB]		= Resources.Load<Sprite>("Icons/Club") as Sprite;
		IconList[(int)Defines.ICONS.DIAMOND]	= Resources.Load<Sprite>("Icons/Diamond") as Sprite;

		IconList[(int)Defines.ICONS.TREBLE]		= Resources.Load<Sprite>("Icons/Treble") as Sprite;
	}

	public Sprite GetIcon(Defines.ICONS currIcon)
	{
		return IconList[(int)currIcon];
	}

	public Sprite GetIcon(int currIcon)
	{
		return IconList[currIcon];
	}

	void Update ()
	{
	}
}

