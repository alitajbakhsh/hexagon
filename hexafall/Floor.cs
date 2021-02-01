using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Floor : MonoBehaviour
{

	Animator Hex1Anim, Hex2Anim, Hex3Anim;
	public GameObject hexpref;
	public static int Xrow;
	public static int Yrow;
	int index1, index2, index3;
	public int ChildNumber = 1;
	int Xnum, Ynum;
	private float Xdist = 0.9f;
	private float Ydist = 0.77f;
	MeshRenderer sprite;
	public static Color[] generatedcolor;
	float num = 0.01f;
	public float coloroffset;
	bool LeftCheck = true;
	bool GenerateOk = false;
	
	public Color LeftColor;
	private Transform LeftObject;

	Color RandomClr;
	public Transform Column;
	Transform FirstHex, SecoundHex, ThirdHex;
	RaycastHit HitGet;
	bool RightColumn, DownRow;
	Transform OddOrEvenCheck;
	bool PlayAnim = false;
	Vector3 pos1, pos2, pos3;
	
	private Transform column1index, column2index, column3index;
	bool FirstTurn = false;
	bool Clickable = true;



	void Start()
	{

		GenerateColor();
		GenerateGrid();

	}


	void Update()
	{
		if (Input.GetMouseButtonDown(0) && Clickable)
		{
			PlayAnim = false;
			ShowSelected();
			RaycastHit hit;
			Ray rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(rayCast, out hit, Mathf.Infinity))
			{
				if (hit.collider.CompareTag("Hex"))
				{
					FirstHex = null;
					SecoundHex = null;
					ThirdHex = null;
					FirstHex = hit.transform.GetComponent<Transform>();
					HitGet = hit;

					if (hit.point.x <= FirstHex.position.x)
					{
						if (hit.point.y < FirstHex.position.y)
						{

							RightColumn = false;
							DownRow = true;
							GetNeighbour();
						}
						else
						{
							RightColumn = false;
							DownRow = false;
							GetNeighbour();
						}

					}
					else if (hit.point.x > FirstHex.position.x)
					{
						if (hit.point.y < FirstHex.position.y)
						{

							RightColumn = true;
							DownRow = true;
							GetNeighbour();
						}
						else
						{
							RightColumn = true;
							DownRow = false;
							GetNeighbour();
						}

					}

				}
			}
		}
		if (ThirdHex != null && FirstTurn)
		{
			Turn1();
		}
	}
	 

	private void GenerateGrid()
	{
		for (int i = 0; i < Xrow; i++)
		{
			Transform Columns = (Transform)Instantiate(Column, transform.position, Quaternion.identity);
			Columns.name = i.ToString();
			Columns.SetParent(this.transform);
		}

		for (int x = 0; x < Xrow; x++)
		{
			for (int y = 0; y < Yrow; y++)
			{
				float Xcalculator = x * Xdist;
				if (y % 2 == 1)
				{
					Xcalculator += Xdist / 2f;
				}

				RandomColor();

				if (GenerateOk)
				{
					GameObject NameGenerator = (GameObject)Instantiate(hexpref, new Vector2(Xcalculator, y * Ydist), hexpref.transform.rotation);
					NameGenerator.name = x + "_" + y;
					NameGenerator.transform.SetParent(this.transform.GetChild(x));
					GenerateOk = false;
					Xnum = x;
					Ynum = y;
					sprite = NameGenerator.GetComponent<MeshRenderer>();
					sprite.material.color = RandomClr;
				}
			}
		}
	}

	private void GenerateColor()
	{
		
		coloroffset = 1f / generatedcolor.Length;
		for (int i = 0; i < generatedcolor.Length; i++)
		{
			Color gen = Color.HSVToRGB(num, 1, 1);
			generatedcolor[i] = gen;
			num += coloroffset;
		}
	}

	private void RandomColor()
	{
		if (ChildNumber >= Yrow && Ynum < Yrow - 1)
		{
			Transform parrent = this.transform.GetChild(Xnum - 1);
			LeftObject = parrent.GetChild(Ynum + 1);
			LeftColor = LeftObject.GetComponent<MeshRenderer>().material.color;
			RandomClr = generatedcolor[Random.Range(0, generatedcolor.Length)];

			if (RandomClr == LeftColor)
			{
				while (RandomClr == LeftColor)
				{
					RandomClr = generatedcolor[Random.Range(0, generatedcolor.Length)];
				}

				GenerateOk = true;
				ChildNumber++;
			}

			else if (RandomClr != LeftColor)
			{
				GenerateOk = true;
				ChildNumber++;
			}
		}

		else
		{
			RandomClr = generatedcolor[Random.Range(0, generatedcolor.Length)];
			GenerateOk = true;
			ChildNumber++; ;
		}

	}

	void GetNeighbour()
	{
		int ColumnIndex = FirstHex.parent.GetSiblingIndex();
		int RowIndex = FirstHex.GetSiblingIndex();


		if (RightColumn)
		{
			if (ColumnIndex != Xrow - 1)
			{
				Transform NeighbourRight = this.transform.GetChild(ColumnIndex + 1);
				if (RowIndex % 2 != 0)
				{
					OddOrEvenCheck = NeighbourRight;
				}
				else if (RowIndex % 2 == 0)
				{
					OddOrEvenCheck = FirstHex.parent;
				}
				if (DownRow && RowIndex != 0)
				{
					SecoundHex = OddOrEvenCheck.GetChild(RowIndex - 1);
					ThirdHex = NeighbourRight.GetChild(RowIndex);
				}
				else if (DownRow && RowIndex == 0)
				{
					SecoundHex = OddOrEvenCheck.GetChild(RowIndex + 1);
					ThirdHex = NeighbourRight.GetChild(RowIndex);
				}
				else if (!DownRow && RowIndex != Yrow - 1)
				{
					SecoundHex = OddOrEvenCheck.GetChild(RowIndex + 1);
					ThirdHex = NeighbourRight.GetChild(RowIndex);
				}
				else if (!DownRow && RowIndex == Yrow - 1)
				{
					SecoundHex = OddOrEvenCheck.GetChild(RowIndex - 1);
					ThirdHex = NeighbourRight.GetChild(RowIndex);
				}
				PlayAnim = true;
				ShowSelected();
				pos1 = FirstHex.transform.position;
				pos2 = SecoundHex.transform.position;
				pos3 = ThirdHex.transform.position;
				index1 = FirstHex.GetSiblingIndex();
				index2 = SecoundHex.GetSiblingIndex();
				index3 = ThirdHex.GetSiblingIndex();
				column1index = FirstHex.parent;
				column2index = SecoundHex.parent;
				column3index = ThirdHex.parent;
			}
			else if (ColumnIndex == Xrow - 1)
			{
				PlayAnim = true;
				RightColumn = false;
				GetNeighbour();
			}

		}
		else if (!RightColumn)
		{

			if (ColumnIndex != 0)
			{
				Transform NeighbourLeft = this.transform.GetChild(ColumnIndex - 1);
				if (RowIndex % 2 != 0)
				{
					OddOrEvenCheck = FirstHex.parent;
				}
				else if (RowIndex % 2 == 0)
				{
					OddOrEvenCheck = NeighbourLeft;
				}

				if (DownRow && RowIndex != 0)
				{
					SecoundHex = OddOrEvenCheck.GetChild(RowIndex - 1);
					ThirdHex = NeighbourLeft.GetChild(RowIndex);
				}
				else if (DownRow && RowIndex == 0)
				{
					SecoundHex = OddOrEvenCheck.GetChild(RowIndex + 1);
					ThirdHex = NeighbourLeft.GetChild(RowIndex);
				}
				else if (!DownRow && RowIndex != Yrow - 1)
				{
					SecoundHex = OddOrEvenCheck.GetChild(RowIndex + 1);
					ThirdHex = NeighbourLeft.GetChild(RowIndex);
				}
				else if (!DownRow && RowIndex == Yrow - 1)
				{
					SecoundHex = OddOrEvenCheck.GetChild(RowIndex - 1);
					ThirdHex = NeighbourLeft.GetChild(RowIndex);
				}
				PlayAnim = true;
				ShowSelected();
				pos1 = FirstHex.transform.position;
				pos2 = SecoundHex.transform.position;
				pos3 = ThirdHex.transform.position;
				index1 = FirstHex.GetSiblingIndex();
				index2 = SecoundHex.GetSiblingIndex();
				index3 = ThirdHex.GetSiblingIndex();
				column1index = FirstHex.parent;
				column2index = SecoundHex.parent;
				column3index = ThirdHex.parent;
			}
			else if (ColumnIndex == 0)
			{
				PlayAnim = true;
				RightColumn = true;
				GetNeighbour();
			}

		}
	}
	void ShowSelected()
	{
		if (ThirdHex && FirstHex && SecoundHex != null)
		{
			Hex1Anim = FirstHex.GetComponent<Animator>();
			Hex2Anim = SecoundHex.GetComponent<Animator>();
			Hex3Anim = ThirdHex.GetComponent<Animator>();
			if (PlayAnim)
			{
				Hex1Anim.SetBool("select", true);
				Hex2Anim.SetBool("select", true);
				Hex3Anim.SetBool("select", true);
			}
			else if (!PlayAnim)
			{
				Hex1Anim.SetBool("select", false);
				Hex2Anim.SetBool("select", false);
				Hex3Anim.SetBool("select", false);
			}
		
		}

	}
	public void RotateClockWise()
	{

		if (ThirdHex != null && Clickable)
		{
			FirstTurn = true;

		}

	}

	void Turn1()
	{
			Clickable = false;
			FirstHex.position = Vector3.Lerp(FirstHex.position, pos2, Time.deltaTime * 15);
			FirstHex.parent = column2index;
			FirstHex.SetSiblingIndex(index2);
			SecoundHex.position = Vector3.Lerp(SecoundHex.position, pos3, Time.deltaTime * 15);
			SecoundHex.parent = column3index;
			SecoundHex.SetSiblingIndex(index3);
			ThirdHex.position = Vector3.Lerp(ThirdHex.position, pos1, Time.deltaTime * 15);
			ThirdHex.parent = column1index;
			ThirdHex.SetSiblingIndex(index1);
			Invoke("turn2", 0.2f);
	}
	void turn2()
	{

		FirstTurn = false;
			ThirdHex.position = Vector3.Lerp(ThirdHex.position, pos2, Time.deltaTime * 15);
			ThirdHex.parent = column2index;
			ThirdHex.SetSiblingIndex(index2);
			FirstHex.position = Vector3.Lerp(FirstHex.position, pos3, Time.deltaTime * 15);
			FirstHex.parent = column3index;
			FirstHex.SetSiblingIndex(index3);
			SecoundHex.position = Vector3.Lerp(SecoundHex.position, pos1, Time.deltaTime * 15);
			SecoundHex.parent = column1index;
			SecoundHex.SetSiblingIndex(index1);
			Invoke("turn3", 0.2f);
	}
	void turn3()
	{
	
			SecoundHex.position = Vector3.Lerp(SecoundHex.position, pos2, Time.deltaTime * 15);
			SecoundHex.parent = column2index;
			SecoundHex.SetSiblingIndex(index2);
			ThirdHex.position = Vector3.Lerp(ThirdHex.position, pos3, Time.deltaTime * 15);
			ThirdHex.parent = column3index;
			ThirdHex.SetSiblingIndex(index3);
			FirstHex.position = Vector3.Lerp(FirstHex.position, pos1, Time.deltaTime * 15);
			FirstHex.parent = column1index;
			FirstHex.SetSiblingIndex(index1);
			Invoke("SetClickability", 0.2f);

	}

	void SetClickability()
	{
		Clickable = true;
	}
	public void ReturnToMenu()
    {
		SceneManager.LoadScene(0);
	}
}
	